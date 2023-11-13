//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestWeight : WeightBase
{
    Animator _weightAnim = default;
    public override void WeightActive()
    {
        Debug.Log("ウツボカズラが閉じた！");
        //今のところアニメーションできそうにないので手動で
        PolygonCollider2D polCollider2D = gameObject.GetComponent<PolygonCollider2D>();
        polCollider2D.isTrigger = false;

        //ウツボカズラが閉じるアニメーション
        _weightAnim = gameObject.GetComponent<Animator>();
        _weightAnim.SetBool("IsWeightActive", true);
    }
}
