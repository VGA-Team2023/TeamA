//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public interface IHealth
{
    public IObservable<Unit> OnDead { get; }
    public void ApplyDamage(float damageNum);
    public void ApplyHeal(float healNum);
}
