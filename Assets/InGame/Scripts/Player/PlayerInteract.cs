//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : IPlayerState
{
    [Header("boxの大きさ")]
    [SerializeField] private float _boxScale;
    [Header("判定の距離")]
    [SerializeField] private float _maxDistance;
    private PlayerEnvroment _env;

    public void SetUp(PlayerEnvroment env)
    {
        _env = env;
        InputProvider.Instance.SetEnterInput(InputProvider.InputType.Interact, Interact);
    }

    public void Update()
    {
       
    }

    public void FixedUpdate()
    {
        
    }

    public void Interact() 
    {
        var isHit = Physics2D.BoxCast(_env.PlayerTransform.position, Vector2.one * _boxScale, 0, InputProvider.Instance.MoveDir);
        if (!isHit && isHit.collider.TryGetComponent<IInteractEvent>(out var interactEvent)) 
        {
            interactEvent.Execute();
        }
    }

    public void Dispose()
    {
        
    }

}
