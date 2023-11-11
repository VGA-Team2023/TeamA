//日本語対応
using System.Collections;
using System.Collections.Generic;
using Action2D;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField, Tooltip("雑魚敵のデータ")]
    EnemyDate _enemyDate = default;
    public EnemyDate EnemyDataSource => _enemyDate;

    Rigidbody2D _rb = default;

    Vector2 _pos = default;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _pos = gameObject.transform.position;
    }

    private void Update()
    {
        //Playerとの距離計算
        Vector2 pPos = GameManager.Instance.PlayerEnvroment.PlayerTransform.position;
        float distance = Vector2.Distance(this.transform.position, pPos);

        //距離が離れてたら
        if (distance > _enemyDate.LookDistance)
        {
            //巡回
            Move();
        }
        else
        {
            //近くまで移動
            MoveToPlayer(pPos);
        }
    }

    /// <summary>巡回中の動き</summary>
    void Move()
    {
        {

        }
    }

    /// <summary> Playerに近づく </summary>
    /// <param name="playerPos">Playerの座標</param>
    private void MoveToPlayer(Vector2 playerPos)
    {
        //方向転換
        if (transform.position.x < playerPos.x && transform.eulerAngles.y < 180f)    //右に向く  
        {
            transform.eulerAngles = new Vector3(0, 180f, 0);
        }
        else if (transform.position.x >= playerPos.x && transform.eulerAngles.y >= 180f)    //左に向く
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        //移動
        Vector2 moveDir = new Vector2(playerPos.x - transform.position.x, transform.position.y);
        _rb.velocity = moveDir.normalized * _enemyDate.MoveSpeed;

        //攻撃
        Attack();
    }

    /// <summary>攻撃</summary>
    public abstract void Attack();

    /// <summary> 被ダメージ </summary>
    public abstract void Damaged();

    /// <summary>死亡時</summary>
    public abstract void Die();
}
