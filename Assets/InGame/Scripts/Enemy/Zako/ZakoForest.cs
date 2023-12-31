//日本語対応
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ZakoForest : ZakoBase
{
    [SerializeField, Tooltip("攻撃(仮)")]
    BoxCollider2D _attack = default;
    [SerializeField, Tooltip("仮のタグ")]
    string _tagName = string.Empty;
    bool _isStart = false;

    public override void Attack()
    {
        //攻撃するときの音や攻撃
        EnemyAnimator.SetTrigger("ShortRangeAttack");
    }

    public override void Exit()
    {
        //死んだときのアニメーションやエフェクト,死んだ個体の処理
        EnemyAnimator.SetBool("Move", false);
        EnemyAnimator.SetBool("Die", true);
    }
    protected override void OnBecameVisible() 
    {
        base.OnBecameVisible();
        _isStart = true;
    }


    protected override void OnBecameInvisible()
    {
        base.OnBecameInvisible();
        _isStart = false;
    } 
    public override bool Wait()
    {
        return _isStart;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerHp>(out var pHp))
        //|| collision.gameObject.tag == _tagName)
        {
            //Debug.Log("マンドラゴラの攻撃成功");
            Vector2 knockBackDir = pHp.transform.position - transform.position;
            pHp.ApplyDamage(EnemyDataSource.AttackValue, knockBackDir.normalized).Forget();
        }
    }
}
