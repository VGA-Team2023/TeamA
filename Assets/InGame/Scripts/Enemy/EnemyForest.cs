//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyForest : EnemyBase
{
    [SerializeField, Tooltip("攻撃(仮)")]
    BoxCollider2D _attack = default;
    [SerializeField, Tooltip("仮のタグ")]
    string _tagName = string.Empty;

    float _time = 999;
    public override void Attack()
    {
        //攻撃するときの音や攻撃
        _time += Time.deltaTime;
        if (_time > EnemyDataSource.AttackInterval && _attack.enabled == false)
        {
            Debug.Log("マンドラゴラの攻撃");
            _attack.enabled = true;
        }
        if (_time > EnemyDataSource.AttackValue + 0.5f)
        {
            _attack.enabled = false;
            _time = 0f;
        }
    }

    public override void Damaged()
    {
        //自分がダメージを食らうときの音やエフェクト
        Debug.Log("マンドラゴラがプレイヤーに攻撃されてる");
    }

    public override void Die()
    {
        //死んだときのアニメーションやエフェクト,死んだ個体の処理
        Debug.Log("マンドラゴラ死にました");
    }

    //アニメーションが間に合わないため仮
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerHp>(out var pHp)
            || collision.gameObject.tag == _tagName)
        {
            Debug.Log("マンドラゴラの攻撃成功");
            //pHp.ApplyDamage(EnemyDataSource.AttackValue);
        }
    }
}
