//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public interface IPlayerAttack
{
    public IReadOnlyReactiveProperty<float> CurrentWaterNum { get; }
    public IReadOnlyReactiveProperty<float> MaxWaterNum { get; }
}
