//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using UniRx;

public class PlayerAttack : IPlayerState, IPlayerAttack
{
    public IReadOnlyReactiveProperty<float> CurrentWaterNum => _currentWaterNum;
    public IReadOnlyReactiveProperty<float> MaxWaterNum => _maxWaterNum;

    [SerializeField] private Transform _attackPos;
    [SerializeField] private Collider2D _attackCollider;
    [Header("最初の最大の水の量")]
    [SerializeField] private float _firstMaxWater;
    [Header("１秒間の水の消費量")]
    [SerializeField] private float _waterConsumption;

    private readonly ReactiveProperty<float> _currentWaterNum = new ReactiveProperty<float>();
    private readonly ReactiveProperty<float> _maxWaterNum = new ReactiveProperty<float>();

    private PlayerEnvroment _env;


    public void SetUp(PlayerEnvroment env)
    {
        _env = env;
        _maxWaterNum.Value = _firstMaxWater;
        _currentWaterNum.Value = _firstMaxWater;
    }

    public void Update()
    {

    }

    public void FixedUpdate()
    {
        if (InputProvider.Instance.GetStayInput(InputProvider.InputType.Attack))
        {
            Attack();
        }
        else
        {
            CancelAttak();
        }
    }

    private void Attack()
    {
        _currentWaterNum.Value -= Time.deltaTime * _waterConsumption;
        _env.PlayerAnim.AttackAnim(true);
        _env.AddState(PlayerStateType.Attack);
    }

    private void CancelAttak() 
    {
        _env.PlayerAnim.AttackAnim(false);
        _env.RemoveState(PlayerStateType.Attack);
    }

    public void Dispose()
    {
        _maxWaterNum.Dispose();
        _currentWaterNum.Dispose();
    }
}
