//日本語対応
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class PlayerKnockback : IPlayerState
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Vector2 _knockBackDirection;
    [SerializeField] private float _knockBackSpeed;

    private PlayerEnvroment _env;

    public void SetUp(PlayerEnvroment env, CancellationToken token)
    {
        _env = env;
    }

    public void Update()
    {
        
    }

    public void FixedUpdate()
    {
        
    }

    public void Dispose()
    {
       
    }

    public async UniTask Knockback(Vector2 dir) 
    {
        _env.AddState(PlayerStateType.Damage);
        _rb.velocity = Vector2.zero;
        _rb.AddForce(dir * _knockBackSpeed, ForceMode2D.Impulse);
        await _env.PlayerAnim.KnockBackAnim();
        _env.RemoveState(PlayerStateType.Damage);
    }
}
