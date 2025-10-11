using UnityEngine;
using PurrNet;

public class NetworkTest : NetworkIdentity
{
    [SerializeField] private Color _color;
    [SerializeField] private Renderer _renderer;

    protected override void OnSpawned()
    {
        base.OnSpawned();

        if(!isServer)
            return;

    }

    void Update()
    {
    }

    void OnJump()
    {
        _color = Random.ColorHSV();
        SetColor(_color);
    }

    [ObserversRpc(bufferLast:true)]
    void SetColor(Color color)
    {
        _renderer.material.color = color;
    }
}
