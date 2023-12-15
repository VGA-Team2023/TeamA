//日本語対応
using UnityEngine;

public class ZakoHornet : ZakoBase
{
    [SerializeField, Tooltip("高さ")]
    float _hight = 3f;
    [SerializeField, Tooltip("強さ")]
    float _power = 3f;
    bool _isStart = false;
    Vector2 _dis;
    public override void Attack()
    {
        _dis = GManager.PlayerEnvroment.PlayerTransform.position - transform.position;
        Rb.AddForce(_dis.normalized * _power, ForceMode2D.Impulse);
    }

    public override bool Wait()
    {
        //出てくるときに当たり判定の追加
        if (EnemyDataSource.LookDistance > Distance)
        {
            Debug.Log("開始");
            GetComponent<PolygonCollider2D>().enabled = true; ;
            Rb.AddForce(Vector2.up * _power, ForceMode2D.Impulse);
        }
        return EnemyDataSource.LookDistance > Distance;
    }

    public override void Exit()
    {
        //死んだときのアニメーションやエフェクト,死んだ個体の処理
    }
}
