//日本語対応

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ウツボカズラ,重り
public class WaterGimmick : WaterGimmickBase
{
    [SerializeField] private BoxCollider2D _defaltCollider;
    [SerializeField] private BoxCollider2D _changeCollider;

    Animator _weightAnim = default;

    private void Start()
    {
        _defaltCollider.enabled = true;
        _defaltCollider.isTrigger = true;

        _changeCollider.enabled = false;
        _changeCollider.isTrigger = false;
    }

    public override void WeightActive()
    {
        Debug.Log("一定数の水が当たった！");

        //作動するときのアニメーション
        _weightAnim = gameObject.GetComponent<Animator>();
        _weightAnim.SetBool("IsWeightActive", true);    

        _defaltCollider.enabled = false;
        _changeCollider.enabled = true;
    }
}
