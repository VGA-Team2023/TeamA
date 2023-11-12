using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Selector : ConditionNode
{
    [NonSerialized] int _count = 0;
    protected override void OnExit(Environment env)
    {

    }

    protected override void OnStart(Environment env)
    {

    }

    protected override State OnUpdate(Environment env)
    {
        _count = 0;
        State childState = NodeChildren[_count % NodeChildren.Count].update(env);
        while (childState == State.Failure)
        {
            if (childState == State.Running) return State.Running;
            if (childState == State.Success)
            {
                _count = 0;
                return State.Success;
            }
            if (childState == State.Failure)
            {
                _count++;
                childState = NodeChildren[_count % NodeChildren.Count].update(env);
            }

            if (NodeChildren.Count == _count) 
            {
                return State.Failure;
            }
        }

        return State.Failure;
    }
}
