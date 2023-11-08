//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

/// <summary>
/// PlayerのHPクラス
/// </summary>
public class PlayerHp : MonoBehaviour, IHealth 
{
    public IReactiveProperty<float> MaxHp => _maxHp;
    public IReactiveProperty<float> CurrentHp => _currentHp;
    public IObservable<Unit> OnDead => _onDead;

    [Header("最大のHp")]
    [SerializeField] private float _defaultMaxHp;

    private Subject<Unit> _onDead = new Subject<Unit>();
    private ReactiveProperty<float> _maxHp = new ReactiveProperty<float>();
    private ReactiveProperty<float> _currentHp = new ReactiveProperty<float>();

    public void SetUp()
    {
        _maxHp.Value = _defaultMaxHp;
        _currentHp.Value = _defaultMaxHp;
    }

    public void ApplyDamage(float damageNum)
    {
        _currentHp.Value -= damageNum;
        if (_currentHp.Value < 1) 
        {
            _onDead.OnNext(Unit.Default);
        }

        Debug.Log(_currentHp.Value);
    }

    public void ApplyHeal(float healNum)
    {
        _currentHp.Value += healNum;
    }

    private void OnDestroy()
    {
        _maxHp.Dispose();
        _currentHp.Dispose();
        _onDead.Dispose();
    }
}
