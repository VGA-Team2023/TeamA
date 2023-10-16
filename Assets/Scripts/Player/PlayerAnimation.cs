//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;

[Serializable]
public class PlayerAnimation
{
    [SerializeField] private Animator _anim;
    private CancellationToken _token;

    public void SetUp(CancellationToken token) 
    {
        _token = token;
    }

    public async UniTask AttackAnim() 
    {
        _anim.SetTrigger("Attack");
        await UniTask.WaitUntil(() => _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.98, cancellationToken: _token);
    }

    public void AttackAnim(bool isAttack) 
    {
        _anim.SetBool("Attack", isAttack);
    }
}
