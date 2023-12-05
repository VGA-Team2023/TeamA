//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 森林ステージのボスのクラス。BossBaseを継承。
/// </summary>

public class BossForest : BossBase
{
    DamageBossToPlayer _damageBossToPlayer;

    //一時的に
     protected override  void  Start()
    {
        base.Start();
        _damageBossToPlayer = GetComponent<DamageBossToPlayer>();
        BossAnimator.SetTrigger("BattleStart");
    }

    public override void BattleStart()
    {
        if (CurrentBossState == BossState.Await)
        {

            //バトル開始時の演出：未定 ←一旦アニメーションで作ってるけど、おそらくTimeLineになる
            _damageBossToPlayer = GetComponent<DamageBossToPlayer>();
            BossAnimator.SetTrigger("BattleStart");
            Debug.Log("ボス戦闘開始");
            //アニメーションイベント：BossStateをInGameに変える
        }
    }


    public override void ShortRangeAttack()
    {
        _damageBossToPlayer.CurrentAttackType = DamageBossToPlayer.AttackType.ShortRangeAttack;
        base.CurrentPolygonCollider2D.isTrigger = true;
        //アニメーション
        BossAnimator.SetTrigger("ShortRangeAttack");
        //アニメーションイベント：コライダーの変化、プレイヤーへのダメージ、SE   ←納品後に大幅調整
    }


    public override void LongRangeAttack()
    {
        _damageBossToPlayer.CurrentAttackType = DamageBossToPlayer.AttackType.LongRangeAttack;
        base.CurrentPolygonCollider2D.isTrigger = true;
        //アニメーション
        BossAnimator.SetTrigger("LongRangeAttack");
        //アニメーションイベント：コライダーの変化、プレイヤーへのダメージ、SE   ←納品後に大幅調整
    }

}
