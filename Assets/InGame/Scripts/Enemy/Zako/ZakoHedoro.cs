//日本語対応
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ZakoHedoro : ZakoBase
{
    [SerializeField, Tooltip("ヘドロ弾のプレハブ")]
    GameObject _bullet = default;
    [SerializeField, Tooltip("出す位置")]
    Transform _bulletPos = default;
    public override void Attack()
    {
        //攻撃するときの音や攻撃
        Debug.Log("ヘドロの攻撃");
        EnemyAnimator.SetTrigger("ShortRangeAttack");
    }

    public override bool Wait()
    {
        //出てくるときに当たり判定の追加
        if (EnemyDataSource.LookDistance > Distance)
        { GetComponent<PolygonCollider2D>().enabled = true; }
        return EnemyDataSource.LookDistance > Distance;
    }

    public override void Exit()
    {
        //死んだときのアニメーションやエフェクト,死んだ個体の処理
    }

    //アニメーションイベントで弾を出す
    public void Bullet()
    {
        Instantiate(_bullet, _bulletPos.position, Quaternion.identity, gameObject.transform);
    }
}
