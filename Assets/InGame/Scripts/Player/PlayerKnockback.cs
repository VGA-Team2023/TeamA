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
    [Header("動けない時間")]
    [SerializeField] private float _stuckTime;
    [Header("無敵時間")]
    [SerializeField] private float _invincibleTime;

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
        Invincible().Forget();
        _env.PlayerAnim.DamageAnim(_stuckTime);
        _env.AddState(PlayerStateType.Damage);
        _rb.velocity = Vector2.zero;
        _rb.AddForce(dir * _knockBackSpeed, ForceMode2D.Impulse);
        await UniTask.WaitForSeconds(_stuckTime);
        _env.RemoveState(PlayerStateType.Damage);
    }

    private async UniTask Invincible()
    {
        _env.AddState(PlayerStateType.Invincible);
        await _env.PlayerAnim.InvincibleTimeAnim(_invincibleTime / 3);
        _env.RemoveState(PlayerStateType.Invincible);
    }
}
