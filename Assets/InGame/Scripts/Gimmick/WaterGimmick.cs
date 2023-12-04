//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ウツボカズラ,重り
public class WaterGimmick : WaterGimmickBase
{
    Animator _weightAnim = default;
    public override void WeightActive()
    {
        Debug.Log("一定数の水が当たった！");

        //作動するときのアニメーション
        _weightAnim = gameObject.GetComponent<Animator>();
        _weightAnim.SetBool("IsWeightActive", true);
    }
}
