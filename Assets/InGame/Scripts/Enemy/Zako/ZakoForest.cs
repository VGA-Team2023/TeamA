//日本語対応
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ZakoForest : ZakoBase
{
    [SerializeField, Tooltip("攻撃(仮)")]
    BoxCollider2D _attack = default;
    [SerializeField, Tooltip("仮のタグ")]
    string _tagName = string.Empty;

    public override void Attack()
    {
        //攻撃するときの音や攻撃
        Debug.Log("マンドラゴラの攻撃");
        EnemyAnimator.SetTrigger("ShortRangeAttack");
    }

    //public override void Damaged()
    //{
    //    //自分がダメージを食らうときの音やエフェクト
    //    EnemyAnimator.SetTrigger("Damage");
    //    Debug.Log("マンドラゴラがプレイヤーに攻撃されてる");
    //}

    public override void Die()
    {
        //死んだときのアニメーションやエフェクト,死んだ個体の処理
        Debug.Log("マンドラゴラ死にました");
        EnemyAnimator.SetBool("Die", true);
    }

    public void ForestDie()
    {
        gameObject.SetActive(false);
    }

    //アニメーションが間に合わないため仮
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerHp>(out var pHp)
            || collision.gameObject.tag == _tagName)
        {
            Debug.Log("マンドラゴラの攻撃成功");
            Vector2 knockBackDir = pHp.transform.position - transform.position;
            pHp.ApplyDamage(EnemyDataSource.AttackValue, knockBackDir.normalized).Forget();
        }
    }
}
