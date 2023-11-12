//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossForest : BossBase
{
    public override void BattleStart()
    {
        //バトル開始時の演出：未定
    }

    public override void BattleEnd()
    {
        //バトル終了時の演出：姿が変わる　→　シーン遷移
    }


    public override void CloseRangeAttack()
    {
        //アニメーションイベント：コライダーの変化、プレイヤーへのダメージ
    }


    public override void LongRangeAttack()
    {

        //アニメーションイベント：コライダーの変化、プレイヤーへのダメージ
    }

    public override void Damaged()
    {

    }



}
