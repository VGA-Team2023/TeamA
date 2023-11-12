using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ギミック：枯葉 </summary>
public class DeadLeaves : MonoBehaviour
{
    BoxCollider2D _defaultCollider = default;
    Animator _deadLeavesAnim = default;
    private void Start()
    {
        _defaultCollider = GetComponent<BoxCollider2D>();
        _deadLeavesAnim = GetComponent<Animator>();
    }

    /// <summary> Playerから攻撃を受けたときに呼ばれる </summary>
    public void Attacked()
    {
        //枯葉が開くアニメーション再生
        _deadLeavesAnim.SetBool("IsAttacked", true);
    }


    /// <summary> 
    /// 通れないようにつけていたデカいコライダーを破棄する。
    /// アニメーションイベントから呼ぶ 
    /// </summary>
    public void DestroyDefaultCollider()
    {
        Destroy(_defaultCollider);
    }

    /// <summary>
    /// 葉が開いたあとの形に合わせてコライダーをつける
    /// アニメーションイベントから呼ぶ 
    /// </summary>
    public void SetNewCollider()
    {
        gameObject.AddComponent<PolygonCollider2D>();   //とりあえずポリゴンにしてます。仕様や不具合に合わせて変更
    }
}
