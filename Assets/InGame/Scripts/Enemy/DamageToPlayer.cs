//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Playerにダメージを与える。攻撃の蔦やヘドロに直接アタッチする  </summary>
public class DamageToPlayer : MonoBehaviour
{
    float _damageSize = 0f;

    enum AttackType
    {
        CloseAttack,
        LongAttack,
    }

   [SerializeField] AttackType _attackType = AttackType.CloseAttack;

    private void Awake()
    {
        BossData _bossData = GetComponentInChildren<BossBase>().BossDataSource;
    }

    private void OnTriggerEnter2D(Collider2D collision)     //アニメーション納品後、不都合あればOnCollisionEnter2Dに変える
    {
        if (collision.gameObject.TryGetComponent<PlayerHp>(out var playerHp))
        {

        }

    }
}


