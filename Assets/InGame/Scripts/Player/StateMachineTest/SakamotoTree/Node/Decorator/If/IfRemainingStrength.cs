using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfRemainingStrength : DecoratorNode
{
    [Header("残り体力")]
    [SerializeField] private float _remainingStrength;
    [Header("設定した残り体力より下だった場合")]
    [SerializeField] private State _lowStrengthState;
    [Header("設定した残り体力より上だった場合")]
    [SerializeField] private State _highStrengthState;
    protected override void OnExit(Environment env)
    {

    }

    protected override void OnStart(Environment env)
    {
        
    }

    protected override State OnUpdate(Environment env)
    {
        //体力が設定された値以下だった場合
        if (env.ActorStatus.CurrentHp.Value <= _remainingStrength) 
        {
            return _lowStrengthState;
        }
        return _highStrengthState;
    }
}
