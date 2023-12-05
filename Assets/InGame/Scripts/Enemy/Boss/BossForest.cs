//日本語対応
using UnityEngine;


/// <summary>
/// 森林ステージのボスのクラス。BossBaseを継承。
/// </summary>

public class BossForest : BossBase
{
    DamageBossToPlayer _damageBossToPlayer;

    //あとでStart消す
    protected override void Start()
    {
        base.Start();
        //バトル開始時の演出：
        _damageBossToPlayer = GetComponent<DamageBossToPlayer>();
        BossAnimator.SetTrigger("BattleStart");
        Debug.Log("ボス戦闘開始");

    }

    public override void BattleStart()
    {
        if (CurrentBossState == BossState.Await)
        {
            //バトル開始時の演出：
            _damageBossToPlayer = GetComponent<DamageBossToPlayer>();
            BossAnimator.SetTrigger("BattleStart");
            Debug.Log("ボス戦闘開始");
            //アニメーションイベント：BossStateをInGameに変える
        }
    }

    public override void ShortRangeAttack()
    {
        if (base.CurrentPolygonCollider2D != null)
        {
            _damageBossToPlayer.CurrentAttackType = DamageBossToPlayer.AttackType.ShortRangeAttack;
            base.CurrentPolygonCollider2D.isTrigger = true;
            //アニメーション
            BossAnimator.SetTrigger("ShortRangeAttack");
        }
    }

    public override void LongRangeAttack()
    {
        if (base.CurrentPolygonCollider2D != null)
        {
            _damageBossToPlayer.CurrentAttackType = DamageBossToPlayer.AttackType.LongRangeAttack;
            base.CurrentPolygonCollider2D.isTrigger = true;
            //アニメーション
            BossAnimator.SetTrigger("LongRangeAttack");
        }
    }

}
