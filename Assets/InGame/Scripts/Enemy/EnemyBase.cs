//日本語対応
using System.Collections;
using System.Collections.Generic;
using Action2D;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField, Tooltip("雑魚敵のデータ")]
    EnemyData _enemyDate = default;
    public EnemyData EnemyDataSource => _enemyDate;

    [SerializeField, Tooltip("プレイヤー仮(GameManagerができ次第削除)")]
    Transform _playerPos;

    Rigidbody2D _rb = default;
    [Tooltip("エネミーのスピード")]
    float _moveSpeed = 0f;
    [Tooltip("いまのHP ")]
    float _enemyHp = 0f;
    [Tooltip("始まりの位置")]
    Vector2 _pos = default;
    [Tooltip("右向きか")]
    bool _isRight = true;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _pos = gameObject.transform.position;
        _moveSpeed = _enemyDate.MoveSpeed;
        _enemyHp = _enemyDate.Hp;
    }

    private void Update()
    {
        //Playerとの距離計算
        //Vector2 pPos = GameManager.Instance.PlayerEnvroment.PlayerTransform.position;
        //GameManagerが使えるようになったらなくす
        Vector2 pPos = _playerPos.position;
        float distance = Vector2.Distance(this.transform.position, pPos);

        //距離が離れているとき巡回させる
        if (distance > _enemyDate.LookDistance)
        {
            Move();
        }

        //見つけられる距離かつプレイヤーとの距離が近い場合,攻撃
        else if (transform.position.x - pPos.x < _enemyDate.AttackDistance && distance < _enemyDate.AttackDistance)
        {
            //攻撃するときは移動を中止
            _rb.velocity = Vector2.zero;
            //プレイヤーに向く
            LookAtPlayer(pPos);
            Attack();
        }
        //距離が離れていて見つけているとき,プレイヤーに近づく
        else
        {
            MoveToPlayer(pPos);
        }

        //死亡
        if (_enemyHp <= 0)
        {
            Die();
        }
    }

    /// <summary>巡回中の動き</summary>
    void Move()
    {
        float horizontalInput = _isRight ? 1 : -1;
        Vector2 moveDir = new Vector2(horizontalInput, 0).normalized;
        _rb.velocity = new Vector2(moveDir.x * _moveSpeed, _rb.velocity.y);

        //反転
        if ((_isRight && transform.position.x > _pos.x + _enemyDate.MoveDistance) || (!_isRight && transform.position.x < _pos.x - _enemyDate.MoveDistance))
        {
            Flip();
        }
    }

    /// <summary> Playerに近づく </summary>
    /// <param name="playerPos">Playerの座標</param>
    private void MoveToPlayer(Vector2 playerPos)
    {
        LookAtPlayer(playerPos);
        //移動
        float horizontalInput = _isRight ? 1 : -1;
        Vector2 moveDir = new Vector2(horizontalInput, 0).normalized;
        _rb.velocity = new Vector2(moveDir.x * _moveSpeed, _rb.velocity.y);

    }

    private void LookAtPlayer(Vector2 playerPos)
    {
        //方向転換
        if (transform.position.x < playerPos.x && !_isRight)    //右に向く  
        {
            Flip();
        }
        else if (transform.position.x >= playerPos.x && _isRight)    //左に向く
        {
            Flip();
        }
    }

    /// <summary>向きの反転</summary>
    void Flip()
    {
        _isRight = !_isRight;

        Vector3 rotation = transform.eulerAngles;
        rotation.y += 180f;
        transform.eulerAngles = rotation;
    }

    //自分自身に当たった時
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerHp>(out var pHp))
        {
            Debug.Log("Playerが当たってきた");
            //pHp.ApplyDamage(EnemyDataSource.AttackValue);
        }
    }

    /// <summary>攻撃</summary>
    public abstract void Attack();

    /// <summary> 被ダメージ </summary>
    public abstract void Damaged();

    /// <summary>死亡時</summary>
    public abstract void Die();

}
