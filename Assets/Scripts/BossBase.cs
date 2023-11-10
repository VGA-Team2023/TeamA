//日本語対応
using Action2D;
using UnityEngine;

/// <summary>  敵（ボス）の共通処理を持つ基底クラス  </summary>
public abstract class BossBase : MonoBehaviour
{
    [SerializeField, Tooltip("対応するBossData")] BossData _bossDataSource = default;
    public BossData BossDataSource => _bossDataSource;

    [SerializeField, Tooltip("SEを鳴らすAudioSource")] AudioSource _seAudioSource = default;
    public AudioSource SEAudioSource => _seAudioSource;

    Rigidbody2D _rigidbody2 = default;


    private void Awake()
    {
        _rigidbody2 = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        Vector2 pPos = GameManager.Instance.PlayerEnvroment.PlayerTransform.position;   //Playerの座標
        float distance = MeasureDistance(pPos); //Playerとの距離を測る

        if (distance > _bossDataSource.BorderDistance) //距離が離れてたら
        {
            MoveToPlayer(pPos);
        }
        else
        {
            //攻撃する
            Attack(distance);
        }
    }


    /// <summary> Playerとの距離を計算するメソッド </summary>
    /// <returns>Playerとの距離</returns>
    private float MeasureDistance(Vector2 playerPos)
    {
        float distance = Vector2.Distance(this.transform.position, playerPos);   //Playerとの距離
        return distance;
    }

    /// <summary> Playerに近づく </summary>
    /// <param name="playerPos">Playerの座標</param>
    private void MoveToPlayer(Vector2 playerPos)
    {
        //方向転換
        if (this.transform.position.x < playerPos.x && transform.eulerAngles.y < 180f)    //右に向く  
        {
            transform.eulerAngles = new Vector3(0, 180f, 0);
        }
        else if (this.transform.position.x >= playerPos.x && transform.eulerAngles.y >= 180f)    //左に向く
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        //移動
        Vector2 moveDir = new Vector2(playerPos.x, transform.position.y);
        _rigidbody2.AddForce(moveDir * BossDataSource.MoveSpeed);

    }

    /// <summary> 戦闘開始時の演出 </summary>
    public abstract void BattleStart();

    /// <summary> 戦闘終了時の演出 </summary>
    public abstract void BattleEnd();

    /// <summary> 近距離攻撃 </summary>
    public abstract void CloseRangeAttack();

    /// <summary> 遠距離攻撃 </summary>
    public abstract void LongRangeAttack();

    /// <summary> 被ダメージ </summary>
    public abstract void Damaged();

    /// <summary> 攻撃。Playerとの距離で種類が変わる </summary>
    /// <param name="distance"></param>
    public void Attack(float distance)
    {
        if (distance > _bossDataSource.AttackChangeDistance) LongRangeAttack();  //遠距離攻撃
        else CloseRangeAttack();  //近距離攻撃
    }

}
