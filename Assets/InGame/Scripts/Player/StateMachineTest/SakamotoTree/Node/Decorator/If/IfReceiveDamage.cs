using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IfReceiveDamage : DecoratorNode
{
    [Header("次の攻撃を食らうようになるまでの無敵時間")]
    [SerializeField] private float _invincibleTime;
    [NonSerialized] private float _saveDamage;
    [NonSerialized]private float _saveTime;
    protected override void OnExit(Environment env)
    {
        
    }

    protected override void OnStart(Environment env)
    {
        
    }

    protected override State OnUpdate(Environment env)
    {
       return ReceiveDamageCheck(env);
    }

    /// <summary>
    /// ダメージを受けたかどうかチェックする
    /// </summary>
    /// <param name="env"></param>
    /// <returns></returns>
    private State ReceiveDamageCheck(Environment env) 
    {
        if (env.ActorStatus.CurrentHp.Value != _saveDamage && Time.time - _saveTime > _invincibleTime)
        {
            _saveTime = Time.time;
            env.MySelfAnim.Play("Damage");
            _saveDamage = env.ActorStatus.CurrentHp.Value;
            return Child.update(env);
           // return State.Success;
        }
        else 
        {
            _saveDamage = env.ActorStatus.CurrentHp.Value;
        }

        return State.Failure;
    }
}
