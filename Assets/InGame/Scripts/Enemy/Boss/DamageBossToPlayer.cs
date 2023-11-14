//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Playerにダメージを与える。攻撃の蔦やヘドロに直接アタッチする  </summary>
public class DamageBossToPlayer : MonoBehaviour
{
    float _damageSize = 0f;


    /// <summary> 攻撃の種類を指定するEnum </summary>
    enum AttackType
    {
        ShortRangeAttack,
        LongRangeAttack,
    }

    [SerializeField, Tooltip("この攻撃の種類")] AttackType _attackType = AttackType.ShortRangeAttack;

    private void Awake()
    {
        BossData _bossData = GetComponentInParent<BossBase>().BossDataSource;
        if (_bossData)
        {
            //攻撃の種類に合わせて与えるダメージ数を取得する
            _damageSize = 
                (_attackType == AttackType.ShortRangeAttack) ? _bossData.ShortAttackDamageSize : _bossData.LongAttackDamageSize;
        }
    }

    /// <summary> Playerに攻撃が当たったときにダメージを与える </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)     //アニメーション納品後、不都合あればOnCollisionEnter2Dに変える可能性あり
    {
        if (collision.gameObject.TryGetComponent<PlayerHp>(out var playerHp))
        {
            playerHp.ApplyDamage(_damageSize);
        }

    }
}


