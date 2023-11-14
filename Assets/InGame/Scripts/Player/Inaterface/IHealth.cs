//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Cysharp.Threading.Tasks;

public interface IHealth
{
    public IObservable<Unit> OnDead { get; }
    public UniTask ApplyDamage(float damageNum, Vector2 attackDir);
    public void ApplyHeal(float healNum);
}
