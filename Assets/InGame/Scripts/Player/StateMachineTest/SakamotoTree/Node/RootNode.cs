using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootNode : Node
{
    public Node Child;
    protected override void OnExit(Environment env)
    {
        
    }

    protected override void OnStart(Environment env)
    {
      
    }

    protected override State OnUpdate(Environment env)
    {
        return Child.update(env);
    }
}
