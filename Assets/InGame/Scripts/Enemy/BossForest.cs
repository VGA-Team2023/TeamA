//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossForest : BossBase
{
    public override void BattleStart()
    {
        if (CurrentBossState == BossState.Await)
        {
            //バトル開始時の演出：未定
            //アニメーションイベント：BossStateをInGameに変える
        }
    }

    public override void BattleEnd()
    {
        //アニメーション：姿が変わる
        //アニメーションイベント：シーン遷移
    }

    public override void ShortRangeAttack()
    {
        //アニメーション
        //アニメーションイベント：コライダーの変化、プレイヤーへのダメージ、SE
    }


    public override void LongRangeAttack()
    {
        //アニメーション
        //アニメーションイベント：コライダーの変化、プレイヤーへのダメージ、SE
    }

}
