//日本語対応
using Action2D;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]

/// <summary>  敵（ボス）の共通処理を持つ基底クラス  </summary>
public abstract class BossBase : MonoBehaviour
{
    [SerializeField, Tooltip("対応するBossData")] BossData _bossDataSource = default;
    public BossData BossDataSource => _bossDataSource;

    Rigidbody2D _rigidbody2d = default;

    [Tooltip("Animator。任意のAnimationControllerを割り当てる用")] Animator _bossAnimator = default;
    public Animator BossAnimator => _bossAnimator;

    [SerializeField, Tooltip("画面に映ってるかどうか")] bool _onScreen = false;
    public bool OnScreen => _onScreen;

    CriAudioManager _criAudioManager = default;
    public CriAudioManager BossCriAudioManager => _criAudioManager;

    [Tooltip("現在のHP")] float _currentHp = 0f;

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
    private void Update()
    {
        if (_currentbossState == BossState.InBattle)
        {
            Vector2 pPos = GameManager.Instance.PlayerEnvroment.PlayerTransform.position;   //Playerの座標
            float distance = MeasureDistance(pPos); //Playerとの距離を測る
            if (distance > _bossDataSource.BorderDistance) //距離が離れてたら
            {
                _bossAnimator.SetBool("Walk", true);
                MoveToPlayer(pPos); //Playerに近づく
            }
            else
            {
                _bossAnimator.SetBool("Walk", false);
                Attack(distance);   //攻撃する
            }

        }
    }

    /// <summary> 画面に映ったとき</summary>
    private void OnBecameVisible() => _onScreen = true;

    /// <summary> 画面から外れたとき</summary>
    private void OnBecameInvisible() => _onScreen = false;

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
        _rigidbody2d.AddForce(moveDir * BossDataSource.MoveSpeed);

    }

    /// <summary> 戦闘開始時の演出。システムから呼ばれる </summary>
    public abstract void BattleStart();

    /// <summary> 戦闘終了時の演出 。HPが0になったら呼ばれる</summary>
    public abstract void BattleEnd();

    /// <summary> 近距離攻撃。Attackメソッドから呼ばれる </summary>
    public abstract void ShortRangeAttack();

    /// <summary> 遠距離攻撃。 Attackメソッドから呼ばれる</summary>
    public abstract void LongRangeAttack();

    /// <summary> 被ダメージ処理。水かプレイヤーから呼ばれる</summary>
    public void Damaged()
    {
        if (_currentbossState == BossState.InBattle)
        {
            _bossAnimator.SetTrigger("Damaged");    //被ダメージアニメーション

            //ダメージ計算
            if (_currentHp > 0)
            {
                _currentHp -= _bossDataSource.ReceiveDamageSize;
                Debug.Log("ボスの残りHP：" + _currentHp.ToString("0000000"));
            }

            if (_currentHp <= 0)  //撃破
            {
                ChangeBossState();
                Debug.Log("ボスを撃破した！");
                BattleEnd();    //戦闘終了演出
            }

        }
    }

    /// <summary> 攻撃。Playerとの距離で種類が変わる </summary>
    /// <param name="distance">Playerとの距離</param>
    public void Attack(float distance)
    {
        if (distance > _bossDataSource.AttackChangeDistance) LongRangeAttack();  //遠距離攻撃
        else ShortRangeAttack();  //近距離攻撃
    }

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

    /// <summary> 近距離攻撃時のSEを鳴らす。アニメーションイベントから呼ぶ  </summary>
    public void ShortRangeAttackSE()
    {
        Debug.Log("近距離攻撃時のSEを鳴らす");
        //BossCriAudioManager.PlaySE();     //SE納品後コメントイン
    }

    /// <summary> 遠距離攻撃時のSEを鳴らす。アニメーションイベントから呼ぶ  </summary>
    public void LongRangeAttackSE()
    {
        Debug.Log("遠距離攻撃時のSEを鳴らす");
        //BossCriAudioManager.PlaySE();     //SE納品後コメントイン
    }

    /// <summary> 被ダメージSEを鳴らす。アニメーションイベントから呼ぶ  </summary>
    public void DamagedSE()
    {
        Debug.Log("被ダメージ時のSEを鳴らす");
        //BossCriAudioManager.PlaySE();     //SE納品後コメントイン
    }

    /// <summary> バトル終了時の（元の姿に戻る）SEを鳴らす。アニメーションイベントから呼ぶ  </summary>
    public void BattleEndSE()
    {
        Debug.Log("撃退時のSEを鳴らす");
        //BossCriAudioManager.PlaySE();     //SE納品後コメントイン
    }

    /// <summary> イベントシーンへ遷移する。アニメーションイベントから呼ぶ  </summary>
    public void ChangeScene()
    {
        //イベントシーンへ遷移
        SceneManager.LoadScene(_bossDataSource.SceneName);  //フェード等の演出周りはα後に追加する
    }

    /// <summary> ボスの状態（BossState）を変える。アニメーションイベントから呼ぶ </summary>
    public void ChangeBossState()
    {
        BossState _oldState = _currentbossState;
        if (_oldState == BossState.Await) _currentbossState = BossState.InBattle;
        else if (_oldState == BossState.InBattle) _currentbossState = BossState.OutBattle;
        Debug.Log($"ボスのステートが変更されました{_oldState} -> {_currentbossState}");
    }
}