//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerHp : MonoBehaviour, IHealth 
{
    public IReactiveProperty<float> MaxHp => _maxHp;
    public IReactiveProperty<float> CurrentHp => _currentHp;

    [Header("最大のHp")]
    [SerializeField] private float _defaultMaxHp;

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
        Debug.Log(_currentHp.Value);
    }

    public void ApplyHeal(float healNum)
    {
        _currentHp.Value += healNum;
    }
}
