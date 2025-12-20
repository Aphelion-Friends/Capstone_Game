using UnityEngine;
using PurrNet;

public class SpawnPlayer : NetworkIdentity
{
    [SerializeField] NetworkIdentity playerPrefab;

    protected override void OnSpawned(bool asServer)
    {
        base.OnSpawned(asServer);


        if (!asServer)
        {
            NetworkIdentity player = Instantiate(playerPrefab, new Vector3(0, 5, 0), Quaternion.identity);
            player.GiveOwnership(localPlayer);
        }
    }
}
