//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using UniRx;
using System.Threading;

public class PlayerAttack : IPlayerState, IPlayerAttack
{
    public IReadOnlyReactiveProperty<float> CurrentWaterNum => _currentWaterNum;
    public IReadOnlyReactiveProperty<float> MaxWaterNum => _maxWaterNum;

    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private Transform _eimPos;
    [Header("最初の最大の水の量")]
    [SerializeField] private float _firstMaxWater;
    [Header("１発の水の消費量")]
    [SerializeField] private float _waterConsumption;
    [Header("連射のレート")]
    [SerializeField] private float _waterRate;
    private readonly ReactiveProperty<float> _currentWaterNum = new ReactiveProperty<float>();
    private readonly ReactiveProperty<float> _maxWaterNum = new ReactiveProperty<float>();

    private PlayerEnvroment _env;
    private bool _isAttack;


    public void SetUp(PlayerEnvroment env, CancellationToken token)
    {
        _env = env;
        _maxWaterNum.Value = _firstMaxWater;
        _currentWaterNum.Value = _firstMaxWater;
        InputProvider.Instance.SetEnterInputAsync(InputProvider.InputType.Attack, Attack);
    }

    public void Update()
    {

    }

    public void FixedUpdate()
    {

    }

    private async UniTaskVoid Attack()
    {
        if (_env.PlayerState.HasFlag(PlayerStateType.Damage) ||
            _env.PlayerState.HasFlag(PlayerStateType.Inoperable)) return;

        _env.AddState(PlayerStateType.Attack);

        while (InputProvider.Instance.GetStayInput(InputProvider.InputType.Attack) && 0 < _currentWaterNum.Value 
               && !_isAttack)
        {
            _isAttack = true;
            await UniTask.WaitForSeconds(_waterRate);
            _currentWaterNum.Value -= _waterConsumption;
            _env.PlayerAnim.AttackAnim(true);

            var bulletCs = UnityEngine.Object.
                Instantiate(_bullet, _muzzle.transform.position,
                Quaternion.FromToRotation(Vector2.left, _env.PlayerTransform.transform.position - _eimPos.transform.position)
                .normalized).GetComponent<TestBullet>();

            bulletCs.SetShotDirection((_eimPos.transform.position - _env.PlayerTransform.transform.position).normalized);
            _isAttack = false;
            CriAudioManager.Instance.SE.Play("CueSheet_0", "SE_player_attack");
        }
        
        
        _env.RemoveState(PlayerStateType.Attack);
        _env.PlayerAnim.AttackAnim(false);
    }

    private void CancelAttak() 
    {
        _env.PlayerAnim.AttackAnim(false);
        _env.RemoveState(PlayerStateType.Attack);
    }

    public void Dispose()
    {
        InputProvider.Instance.LiftEnterInputAsync(InputProvider.InputType.Attack, Attack);
        _maxWaterNum.Dispose();
        _currentWaterNum.Dispose();
    }
}
