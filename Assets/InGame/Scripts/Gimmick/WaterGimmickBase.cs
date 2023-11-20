using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//水が当たると作動するギミックの基底クラス
[RequireComponent(typeof(Animator))]
public abstract class WaterGimmickBase : MonoBehaviour, IReceiveWater
{
    [SerializeField, Tooltip("重りが作動する水の量")]
    float _maxWeight = 10f;
    public float MaxWeight => _maxWeight;

    [SerializeField, Tooltip("現在の水の量/確認用")]
    float _nowWeight = 0f;
    public float NowWeight => _nowWeight;

    //水が当たったらカウントを増やしていく
    //一定数たまったら呼ぶ
    public void ReceiveWater()
    {
        _nowWeight++;
        if (_maxWeight <= _nowWeight)
        {
            WeightActive();
        }
    }

    /// <summary>水が一定数たまった時</summary>
    public abstract void WeightActive();
}
