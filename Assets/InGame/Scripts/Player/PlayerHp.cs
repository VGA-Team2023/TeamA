//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Cysharp.Threading.Tasks;

/// <summary>
/// PlayerのHPクラス
/// </summary>
public class PlayerHp : MonoBehaviour, IHealth 
{
    public IReadOnlyReactiveProperty<float> MaxHp => _maxHp;
    public IReadOnlyReactiveProperty<float> CurrentHp => _currentHp;
    public IObservable<Unit> OnDead => _onDead;

    [Header("最大のHp")]
    [SerializeField] private float _defaultMaxHp;

    private Subject<Unit> _onDead = new Subject<Unit>();
    private ReactiveProperty<float> _maxHp = new ReactiveProperty<float>();
    private ReactiveProperty<float> _currentHp = new ReactiveProperty<float>();
    private PlayerEnvroment _env;
    private PlayerKnockback _knockback;

    public void SetUp(PlayerEnvroment env, PlayerKnockback knockback)
    {
        _env = env;
        _maxHp.Value = _defaultMaxHp;
        _currentHp.Value = _defaultMaxHp;
        _knockback = knockback;
    }

    public async UniTask ApplyDamage(float damageNum, Vector2 attackDir)
    {
        if (_env.PlayerState.HasFlag(PlayerStateType.Damage) ||
            _env.PlayerState.HasFlag(PlayerStateType.Inoperable)||
            _env.PlayerState.HasFlag(PlayerStateType.Invincible)) return;

        _currentHp.Value -= damageNum;
        await _knockback.Knockback(attackDir);
        if (_currentHp.Value < 1) 
        {
            _onDead.OnNext(Unit.Default);
        }
    }

    public void ApplyHeal(float healNum)
    {
        if (_env.PlayerState.HasFlag(PlayerStateType.Damage) ||
            _env.PlayerState.HasFlag(PlayerStateType.Inoperable)) return;

        //上限以上回復しないように
        if (_currentHp.Value + healNum > _maxHp.Value) 
        {
            healNum = _maxHp.Value - _currentHp.Value;
        }

        _currentHp.Value += healNum;
    }

    private void OnDestroy()
    {
        _maxHp.Dispose();
        _currentHp.Dispose();
        _onDead.Dispose();
    }
}
