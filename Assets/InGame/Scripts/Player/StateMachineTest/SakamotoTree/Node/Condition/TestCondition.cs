using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCondition : ConditionNode
{
    protected override void OnExit(Environment env)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnStart(Environment env)
    {
        throw new System.NotImplementedException();
    }

    protected override State OnUpdate(Environment env)
    {
        //if (env.mySelf.transform.position) 
        //{

        //}

        return State.Running;
    }
}
