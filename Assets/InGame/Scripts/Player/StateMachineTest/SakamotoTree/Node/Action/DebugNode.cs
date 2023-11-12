using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugNode : ActionNode
{
    [SerializeField] private string _logSt;
    protected override void OnExit(Environment env)
    {
        
    }

    protected override void OnStart(Environment env)
    {
       
    }

    protected override State OnUpdate(Environment env)
    {
        Debug.Log(_logSt);
        return State.Success;
    }
}
