//日本語対応
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]

/// <summary>  敵（ボス）の共通処理を持つ基底クラス  </summary>
public abstract class BossBase : EnemyBase
{
    [SerializeField, Tooltip("対応するBossData")] BossData _bossDataSource = default;
    public BossData BossDataSource => _bossDataSource;

    Rigidbody2D _rigidbody2d = default;

    [Tooltip("Animator。任意のAnimationControllerを割り当てる用")] Animator _bossAnimator = default;
    public Animator BossAnimator => _bossAnimator;

    [SerializeField, Tooltip("画面に映ってるかどうか")] bool _onScreen = false;
    public bool OnScreen => _onScreen;

    CriAudioManager _criAudioManager = default;

    [SerializeField, Tooltip("現在のHP")] float _currentHp = 0f;

    [Tooltip("SEを対応させるDictionary")] Dictionary<string, string> _sePairDic = new Dictionary<string, string>();

    /// <summary> ボスの状態を管理するEnum </summary>
    public enum BossState
    {
        Await,  // 戦闘前、待機中
        InBattle,   //戦闘中
        OutBattle, //戦闘終了
    }

    [SerializeField, Tooltip("現在のボスの状態")] BossState _currentbossState = BossState.Await;
    public BossState CurrentBossState => _currentbossState;

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _bossAnimator = GetComponent<Animator>();
        _bossAnimator.runtimeAnimatorController = _bossDataSource.BossAniCon;   //Bossの種類に合わせて指定のコントローラーを割り当てる
        _criAudioManager = CriAudioManager.Instance;
        _currentHp = _bossDataSource.DefaultHp;
    }

    protected override void Start()
    {
        base.Start();
        ///SEの名前とFileを対応させる
        for (int i = 0; i < _bossDataSource.SeTypeList.Count; i++)
        {
            _sePairDic.Add(_bossDataSource.SeTypeList[i], _bossDataSource.SeFileList[i]);
        }
    }

    protected override void Update()
    {
        base.Update();
        if (_currentbossState == BossState.InBattle)
        {
            if (GManager != null)
            {
                if (Distance > _bossDataSource.BorderDistance) //距離が離れてたら
                {
                    _bossAnimator.SetBool("Move", true);
                    Move(); //Playerに近づく
                }
                else
                {
                    _bossAnimator.SetBool("Move", false);
                    Attack();   //攻撃する
                }
            }
        }
    }

    /// <summary> 画面に映ったとき</summary>
    private void OnBecameVisible() => _onScreen = true;

    /// <summary> 画面から外れたとき</summary>
    private void OnBecameInvisible() => _onScreen = false;

    /// <summary> Playerに近づく </summary>
    public override void Move()
    {
        Vector2 pPos = GManager.PlayerEnvroment.PlayerTransform.position;
        //方向転換
        if (this.transform.position.x < pPos.x && transform.eulerAngles.y < 180f)    //右に向く  
        {
            transform.eulerAngles = new Vector3(0, 180f, 0);
        }
        else if (this.transform.position.x >= pPos.x && transform.eulerAngles.y >= 180f)    //左に向く
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        //移動
        Vector2 moveDir = new Vector2(pPos.x, transform.position.y).normalized;
        _rigidbody2d.velocity = moveDir * BossDataSource.MoveSpeed;
    }

    /// <summary> 戦闘終了時の演出 。HPが0になったら呼ばれる</summary>
    public override void Exit()
    {
        _criAudioManager.SE.Play("CueSheet_0", _sePairDic["ShortRangeAttack"]);
        _bossAnimator.SetTrigger("BattleEnd");
    }

    /// <summary> 被ダメージ処理。水から呼ばれる</summary>
    public override void Damaged()
    {
        if (_currentbossState == BossState.InBattle)
        {
            _criAudioManager.SE.Play("CueSheet_0", _sePairDic["BossDamaged"]);
            _bossAnimator.SetTrigger("Damaged");    //被ダメージアニメーション

            //ダメージ計算
            if (_currentHp > 0)
            {
                _currentHp -= _bossDataSource.ReceiveDamageSize;
                Debug.Log(this.gameObject.name + "の残りHP：" + _currentHp.ToString("0000000"));
            }

            if (_currentHp <= 0)  //撃破
            {
                _bossAnimator.SetBool("Move", false);
                Debug.Log("ボスを撃破した！");
                Exit();    //戦闘終了演出
            }

        }
    }

    /// <summary> 攻撃。Playerとの距離で種類が変わる </summary>
    public override void Attack()
    {
        _rigidbody2d.velocity = Vector2.zero;
        if (Distance > _bossDataSource.AttackChangeDistance)
        {
            _criAudioManager.SE.Play("CueSheet_0", _sePairDic["LongRangeAttack"]);
            LongRangeAttack();  //遠距離攻撃
        }
        else
        {
            _criAudioManager.SE.Play("CueSheet_0", _sePairDic["ShortRangeAttack"]);
            ShortRangeAttack();  //近距離攻撃
        }
    }

    /// <summary> 戦闘開始時の演出。システムから呼ばれる </summary>
    public abstract void BattleStart();

    /// <summary> 近距離攻撃。Attackメソッドから呼ばれる </summary>
    public abstract void ShortRangeAttack();

    /// <summary> 遠距離攻撃。 Attackメソッドから呼ばれる</summary>
    public abstract void LongRangeAttack();

    /// <summary> Player接触時にダメージを与える </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerHp>(out var playerHp))
        {
            Vector2 _knockBackDir = playerHp.transform.position - transform.position;
            playerHp.ApplyDamage(_bossDataSource.TouchedDamageSize, _knockBackDir.normalized).Forget();
        }
    }

    /// <summary> 近距離攻撃と遠距離攻撃のボーダーをギズモで描画 </summary>
    private void OnDrawGizmos()
    {
        //近接攻撃の範囲描画
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, _bossDataSource.AttackChangeDistance);
        //遠距離攻撃の範囲
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, _bossDataSource.BorderDistance);
    }

    //*****以下はアニメーションイベントから呼ぶ用のメソッド*****

    /// <summary> バトル終了時の（元の姿に戻る）SEを鳴らす。アニメーションイベントから呼ぶ  </summary>
    public void BattleEndSE()
    {
        _criAudioManager.SE.Play("CueSheet_0", _sePairDic["BattleEnd"]);
    }

    /// <summary> イベントシーンへ遷移する。アニメーションイベントから呼ぶ  </summary>
    public void ChangeScene()
    {
        //イベントシーンへ遷移
        //SceneManager.LoadScene(_bossDataSource.SceneName);  //フェード等の演出周りはα後に追加する
    }

    /// <summary> ボスの状態（BossState）を変える。アニメーションイベントから呼ぶ </summary>
    public void ChangeBossState()
    {
        BossState _oldState = _currentbossState;
        if (_oldState == BossState.Await) _currentbossState = BossState.InBattle;
        else if (_oldState == BossState.InBattle) _currentbossState = BossState.OutBattle;
        Debug.Log($"ボスのステートが変更されました{_oldState} -> {_currentbossState}");
    }

    ///// <summary> スプライト切り替わるたびにコライダー生成し直す </summary>
    //public void SetNewCollider()
    //{
    //    if (TryGetComponent<PolygonCollider2D>(out var oldCol))
    //    {
    //        gameObject.AddComponent<PolygonCollider2D>();
    //        Destroy(oldCol);
    //    }
    //}
}