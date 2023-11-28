using UnityEngine;

/// <summary> ギミック：枯葉 </summary>
public class DeadLeaves : WaterGimmickBase
{
    BoxCollider2D _defaultCollider = default;
    Animator _deadLeavesAnim = default;
    private void Start()
    {
        _defaultCollider = GetComponent<BoxCollider2D>();
        _deadLeavesAnim = GetComponent<Animator>();
    }

    public override void WeightActive()
    {
        _deadLeavesAnim.SetBool("IsWeightActive", true);
    }

    /// <summary>
    /// 葉が開いたあとの形に合わせてコライダーをつける
    /// アニメーションイベントから呼ぶ 
    /// </summary>
    public void SetNewCollider()
    {
        gameObject.AddComponent<PolygonCollider2D>();   //とりあえずポリゴンにしてます。仕様や不具合に合わせて変更
    }

    /// <summary> 
    /// 通れないようにつけていたデカいコライダーを破棄する。
    /// アニメーションイベントから呼ぶ 
    /// </summary>
    public void DestroyDefaultCollider()
    {
        Destroy(_defaultCollider);
    }

}
