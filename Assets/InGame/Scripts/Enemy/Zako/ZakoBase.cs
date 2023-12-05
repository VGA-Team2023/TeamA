//日本語対応
using Cysharp.Threading.Tasks;
using UnityEngine;

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

    Rigidbody2D _rb = default;
    [SerializeField, Tooltip("ステート")]
    ZakoState _state = ZakoState.Wander;
    [Tooltip("エネミーのスピード")]
    float _moveSpeed = 0f;
    [Tooltip("いまのHP ")]
    float _enemyHp = 0f;
    [Tooltip("始まりの位置")]
    Vector2 _pos = default;
    [Tooltip("右向きか")]
    bool _isRight = true;
    [Tooltip("時間はかる")]
    float _time = 999;
    float _idleTime = 0f;
    int _seNum = -1;
    bool _isAttack = false;
    enum ZakoState
    {
        /// <summary>巡回</summary>
        Wander,
        /// <summary>攻撃</summary>
        Attack,
        /// <summary>何してもいい</summary>
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

        //体力ある時
        if (_enemyHp > 0)
        {
            Vector2 pPos = GManager != null ? GManager.PlayerEnvroment.PlayerTransform.position : _pPos.transform.position;

            //距離が離れているとき巡回させる
            if (Distance > _enemyDate.LookDistance)
            {
                if (_seNum == -1)
                {
                    Debug.Log("流した");
                    _seNum = CriAudioManager.Instance.SE.Play("CueSheet_0", "Enemy_FS_ZAKO");
                }

                Wander();
                _time = _enemyDate.AttackInterval;
            }

            //見つけられる距離かつプレイヤーとの距離が近い場合,攻撃
            else if (transform.position.x - pPos.x < _enemyDate.AttackDistance && Distance < _enemyDate.AttackDistance)
            {
                if (_seNum != -1)
                {
                    Debug.Log("止めた");
                    CriAudioManager.Instance.SE.Stop(_seNum + 1);
                    _seNum = -1;
                }

                _time += Time.deltaTime;
                //攻撃するときは移動を中止
                _rb.velocity = Vector2.zero;
                _enemyAnim.SetBool("Move", false);
                //プレイヤーに向く
                LookAtPlayer(pPos);
                if (_time > _enemyDate.AttackInterval && !_isAttack) { Attack(); _time = 0f; _state = ZakoState.Idle; _isAttack = true; }
            }

            //距離が離れていて見つけているとき,プレイヤーに近づく
            else
            {
                if (_state == ZakoState.Wander) MoveToPlayer(pPos);
            }

            if (_state == ZakoState.Idle)
            {
                _idleTime += Time.deltaTime;

                if (_isAttack)
                {
                    if (StopEnemy(_idleTime, _enemyDate.StopTime + 1))
                    {
                        _idleTime = 0f;
                        _isAttack = false;
                        _state = ZakoState.Wander;
                    }
                }
                else
                {
                    Debug.Log("攻撃で止まる");
                    if (StopEnemy(_idleTime, 1f))
                    {
                        _idleTime = 0f;
                        _state = ZakoState.Wander;
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

    bool StopEnemy(float now, float t)
    {
        return now >= t;
    }

    /// <summary>巡回中の動き</summary>
    void Wander()
    {
        Debug.Log("巡回");
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
        Debug.Log(_isAttack);
    }

    #region enemy実装

    public override void Move()
    {
        _enemyAnim.SetBool("Move", true);

        float horizontalInput = _isRight ? 1 : -1;
        Vector2 moveDir = new Vector2(horizontalInput, 0).normalized;
        _rb.velocity = new Vector2(moveDir.x * _moveSpeed, _rb.velocity.y);
    }
    public override void Damaged()
    {
        if (_enemyHp > 0)
        {
            _rb.velocity = Vector2.zero;
            _state = ZakoState.Idle;
            _idleTime = 0;
            _enemyHp--;
            //自分がダメージを食らうときの音やエフェクト
            _enemyAnim.SetTrigger("Damage");
            Vector2 dir = transform.position - GManager.PlayerEnvroment.PlayerTransform.position;
            dir.y = 3;
            dir.Normalize();
            _rb.AddForce(dir * _enemyDate.Knockback, ForceMode2D.Impulse);
        }

        Debug.Log("プレイヤーに攻撃されてる");
    }

    public override void Exit()
    {
        Debug.Log("エネミー退場");
        _enemyAnim.SetBool("Die", true);
    }
    #endregion

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
}
