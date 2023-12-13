//日本語対応
using Cysharp.Threading.Tasks;
using UnityEngine;
using System;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public abstract class ZakoBase : EnemyBase
{
    [SerializeField, Tooltip("雑魚敵のデータ")]
    ZakoData _enemyDate = default;
    public ZakoData EnemyDataSource => _enemyDate;

    [SerializeField, Tooltip("雑魚敵のアニメーター")]
    Animator _enemyAnim = default;
    public Animator EnemyAnimator => _enemyAnim;
    [SerializeField]
    GameObject _pPos = default;
    public GameObject Pos => _pPos;

    Rigidbody2D _rb = default;
    public Rigidbody2D Rb => _rb;
    [Tooltip("始まりの位置")]
    Vector2 _pos = default;
    public Vector2 StartPos => _pos;

    [SerializeField, Tooltip("ステート")]
    ZakoState _state = ZakoState.Wait;
    [Tooltip("エネミーのスピード")]
    float _moveSpeed = 0f;
    [Tooltip("いまのHP ")]
    float _enemyHp = 0f;
    [Tooltip("右向きか")]
    bool _isRight = true;
    [SerializeField, Tooltip("時間はかる")]
    float _time = 999;
    float _idleTime = 0f;
    int _seNum = -1;
    bool _isAttack = false;
    enum ZakoState
    {
        /// <summary>最初の待機</summary>
        Wait,
        /// <summary>巡回(動き)</summary>
        Wander,
        /// <summary>攻撃</summary>
        Attack,
        /// <summary>動きを止める</summary>
        Idle,
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _enemyAnim = GetComponent<Animator>();
        _enemyAnim.runtimeAnimatorController = _enemyDate.EnemyAnimCon;
        _pos = gameObject.transform.position;
        _moveSpeed = _enemyDate.MoveSpeed;
        _enemyHp = _enemyDate.Hp;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        Debug.Log(_state);

        //体力がある
        if (_enemyHp > 0)
        {
            Vector2 pPos = GManager != null ? GManager.PlayerEnvroment.PlayerTransform.position : _pPos.transform.position;

            //プレイヤーに姿を見せる演出
            if (_state == ZakoState.Wait && Wait())
            {
                _enemyAnim.SetBool("Start", true);
                StateCheng(ZakoState.Wander);
            }
            //巡回
            else if (_state == ZakoState.Wander)
            {

                if (Distance < _enemyDate.AttackDistance)
                {
                    //攻撃するときは足音を止める
                    if (_seNum != -1)
                    {
                        Debug.Log("止めた");
                        CriAudioManager.Instance.SE.Stop(_seNum + 1);
                        _seNum = -1;
                    }
                    StateCheng(ZakoState.Attack);
                }
                else if (Distance < _enemyDate.LookDistance)
                {
                    Debug.Log("近づく");
                    MoveToPlayer(pPos);
                }
                else
                {
                    //足音がループのため
                    if (_seNum == -1)
                    {
                        Debug.Log("流した");
                        _seNum = CriAudioManager.Instance.SE.Play("CueSheet_0", "Enemy_FS_ZAKO");
                    }
                    Wander();
                    //巡回後すぐ攻撃に移るため
                    _time = EnemyDataSource.AttackInterval;
                }
            }



            //    //距離が離れている
            //    if (Distance > _enemyDate.LookDistance)
            //    {
            //        //足音がループのため
            //        if (_seNum == -1)
            //        {
            //            Debug.Log("流した");
            //            _seNum = CriAudioManager.Instance.SE.Play("CueSheet_0", "Enemy_FS_ZAKO");
            //        }
            //        Wander();
            //        //巡回後すぐ攻撃に移るため
            //        _time = EnemyDataSource.AttackInterval;
            //    }
            //    //攻撃
            //    else if (/*transform.position.x - pPos.x < _enemyDate.AttackDistance && */Distance < _enemyDate.AttackDistance)
            //    {
            //        //攻撃するときは足音を止める
            //        if (_seNum != -1)
            //        {
            //            Debug.Log("止めた");
            //            CriAudioManager.Instance.SE.Stop(_seNum + 1);
            //            _seNum = -1;
            //        }
            //        StateCheng(ZakoState.Attack);
            //    }
            //    //距離が近いため近づく
            //    else
            //    {
            //        Debug.Log("近づく");
            //        MoveToPlayer(pPos);
            //    }
            //}

            //攻撃
            else if (_state == ZakoState.Attack)
            {
                _time += Time.deltaTime;
                //攻撃するときは移動を中止
                _rb.velocity = Vector2.zero;
                _enemyAnim.SetBool("Move", false);
                //プレイヤーに向く
                LookAtPlayer(pPos);
                //攻撃できるか
                if (_time > _enemyDate.AttackInterval && !_isAttack)
                {
                    StateCheng(ZakoState.Idle);
                    Attack();
                    _time = 0f;
                    _isAttack = true;
                }
            }

            //待機
            if (_state == ZakoState.Idle)
            {
                _idleTime += Time.deltaTime;

                //攻撃後
                if (_isAttack)
                {
                    //止まり終えたら巡回に戻る
                    if (StopEnemy(_idleTime, _enemyDate.StopTime + 1))
                    {
                        _idleTime = 0f;
                        _isAttack = false;
                        StateCheng(ZakoState.Wander);
                        Debug.Log("攻撃で止まる");
                    }
                }
                else
                {
                    Debug.Log("攻撃で止まる");
                    if (StopEnemy(_idleTime, 1f))
                    {
                        _idleTime = 0f;
                        StateCheng(ZakoState.Wander);
                    }
                }
            }
        }

        //死亡
        else if (_enemyHp <= 0)
        {
            Exit();
        }
    }

    //ステートの変更
    void StateCheng(ZakoState state)
    {
        _state = state;
    }

    /// <summary>時間が超えたのか</summary>
    /// <param name="now">経過時間</param>
    /// <param name="t">制限時間</param>
    /// <returns>第一引数が越えたら</returns>
    bool StopEnemy(float now, float t)
    {
        return now >= t;
    }

    /// <summary>巡回中の動き</summary>
    void Wander()
    {
        Move();

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
        Move();

    }

    /// <summary>プレイヤーに向く</summary>
    /// <param name="playerPos">Playerの座標</param>
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

    /// <summary>アニメーションイベント</summary>
    public void AttackEnd()
    {
        _isAttack = true;
    }

    #region enemy実装

    public override void Move()
    {
        _enemyAnim.SetBool("Move", true);

        float horizontalInput = _isRight ? 1 : -1;
        Vector2 moveDir = new Vector2(horizontalInput, 0).normalized;
        _rb.velocity = new Vector2(moveDir.x * _moveSpeed, 0);
    }
    public override void Damaged()
    {
        if (_enemyHp > 0)
        {
            _rb.velocity = Vector2.zero;
            StateCheng(ZakoState.Idle);
            _idleTime = 0;
            _enemyHp--;
            //自分がダメージを食らうときの音やエフェクト
            _enemyAnim.SetTrigger("Damage");
            //ノックバック
            Vector2 dir = transform.position - GManager.PlayerEnvroment.PlayerTransform.position;
            dir.y = 3;
            dir.Normalize();
            _rb.AddForce(dir.normalized * _enemyDate.Knockback, ForceMode2D.Impulse);
        }

        Debug.Log("プレイヤーに攻撃されてる");
    }

    public override void Exit()
    {
        Debug.Log("エネミー退場");
        _enemyAnim.SetBool("Die", true);
    }
    #endregion

    //アニメーションイベント死んだら消す処理
    public void Die()
    {
        gameObject.SetActive(false);
    }

    //自分自身に当たった時
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerHp>(out var pHp))
        {
            Debug.Log("Playerが当たってきた");
            Vector2 knockBackDir = pHp.transform.position - transform.position;
            pHp.ApplyDamage(1, knockBackDir.normalized).Forget();
        }
    }

    /// <summary>敵の動きはじめ</summary>
    /// <returns>開始の条件</returns>
    public abstract bool Wait();

    private void OnDrawGizmos()
    {
        //攻撃の範囲
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, _enemyDate.AttackDistance);
        //接近と巡回のボーダー
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, _enemyDate.LookDistance);
    }

}
