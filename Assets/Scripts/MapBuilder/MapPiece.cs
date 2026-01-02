using UnityEngine;
using System;

namespace MapBuilder
{
    [Serializable]
    public class MapPiece
    {
        [SerializeField] private Vector3Int _location;
        [SerializeField] private uint _orientation;
        [SerializeField] private Piece _piece;

        public Vector3Int location { get => _location; set => _location = value; }
        public uint orientation { get => _orientation; set => _orientation = value % 4; }
        public Piece piece { get => _piece; set => _piece = value; }

        public MapPiece(Vector3Int newLocation, Piece newPiece, uint newOrientation)
        {
            _location = newLocation;
            _piece = newPiece;
            _orientation = newOrientation;
        }

        public MapPiece(MapPiece otherPiece)
        {
            _location = new Vector3Int(otherPiece.location.x, otherPiece.location.y, otherPiece.location.z);
            _piece = otherPiece.piece;
            _orientation = otherPiece.orientation;
        }

        public MapPiece()
        {
            _location = new Vector3Int();
            _orientation = 0;
            _piece = new Piece();
        }

        // GetMortonCode and SplitBy3Bits21 are from: https://github.com/fwilliams/point-cloud-utils/blob/master/src/common/morton_code.cpp#L72
        public ulong GetMortonCode()
        {
            Debug.Assert(-(1 << 20) <= _location.x && _location.x < (1 << 20));
            Debug.Assert(-(1 << 20) <= _location.y && _location.y < (1 << 20));
            Debug.Assert(-(1 << 20) <= _location.z && _location.z < (1 << 20));

            int x = (int) (_location.x & 0x80000000) >> 11 | (_location.x & 0x0fffff);
            int y = (int) (_location.y & 0x80000000) >> 11 | (_location.y & 0x0fffff);
            int z = (int) (_location.z & 0x80000000) >> 11 | (_location.z & 0x0fffff);

            ulong data = SplitBy3Bits21(x) | SplitBy3Bits21(y) << 1 | SplitBy3Bits21(z) << 2;
            data = data ^ 0x7000000000000000;

            return data;
        }

        private ulong SplitBy3Bits21(int x)
        {
            ulong r = (ulong) x;

            r = (r | r << 32) & 0x1f00000000ffff;
            r = (r | r << 16) & 0x1f0000ff0000ff;
            r = (r | r << 8) & 0x100f00f00f00f00f;
            r = (r | r << 4) & 0x10c30c30c30c30c3;
            r = (r | r << 2) & 0x1249249249249249;

            return r;
        }
    }
}
