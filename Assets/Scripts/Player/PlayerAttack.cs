//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class PlayerAttack : IPlayerState
{
    [SerializeField] private Transform _attackPos;
    [SerializeField] private Collider2D _attackCollider;

    [Header("一秒あたりの水の消費量")]
    [SerializeField] private float _waterConsumption;
    private PlayerEnvroment _env;
    public void SetUp(PlayerEnvroment env)
    {
        InputProvider.Instance.SetEnterInputAsync(InputProvider.InputType.Attack, AttackAsync);
        _env = env;
    }

    public void Update()
    {
        if (InputProvider.Instance.GetStayInput(InputProvider.InputType.Attack)) 
        {

        }
    }

    public void FixedUpdate()
    {

    }
    private async UniTaskVoid AttackAsync()
    {
        if (_env.PlayerState.HasFlag(PlayerStateType.Attack)) return;

        _attackCollider.enabled = true;
        _env.AddState(PlayerStateType.Attack);
        await _env.PlayerAnim.AttackAnim();
        _attackCollider.enabled = false;
        _env.RemoveState(PlayerStateType.Attack);
    }

    public void Dispose()
    {

    }
}
