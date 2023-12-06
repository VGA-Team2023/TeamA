//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadLeavesWall : WaterGimmickBase
{
    Animator _deadLeavesAnim = default;
    private void Start()
    {
        _deadLeavesAnim = GetComponent<Animator>();
    }
    public override void WeightActive()
    {
        _deadLeavesAnim.SetBool("IsWeightActive", true);
    }
}
