using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 各ステージのBossのパラメータデータ用のScriptableObject
/// </summary>
[CreateAssetMenu(menuName = "MyScriptable/Create BossData")]

public class BossData : ScriptableObject
{
    [Header("数値：移動周り")]
    [SerializeField, Tooltip("攻撃方法が変化する距離")]
    float _attackChangeDistance = 0f;
    public float AttackChangeDistance => _attackChangeDistance;

    [SerializeField, Tooltip("追跡と攻撃のボーダー距離")]
    float _borderDistance = 0f;
    public float BorderDistance => _borderDistance;

    [SerializeField, Tooltip("移動速度")]
    float _moveSpeed = 0f;
    public float MoveSpeed => _moveSpeed;

    [Header("数値：HP・ダメージ周り")]
    [SerializeField, Tooltip("デフォルトHP")]
    float _dafaultHp = 0f;
    public float DefaultHp => _dafaultHp;

    /// <summary> このプロパティは、プレイヤー側から指定する場合は削除する </summary>
    [SerializeField, Tooltip("プレイヤーから受けるダメージの大きさ")]
    float _receiveDamageSize = 0f;
    public float ReceiveDamageSize => _receiveDamageSize;

    [SerializeField, Tooltip("近距離攻撃時にプレイヤーへ与えるダメージの大きさ")]
    float _shortAttackDamageSize = 0f;
    public float ShortAttackDamageSize => _shortAttackDamageSize;

    [SerializeField, Tooltip("遠距離攻撃時にプレイヤーへ与えるダメージの大きさ")]
    float _longAttackDamageSize = 0f;
    public float LongAttackDamageSize => _longAttackDamageSize;

    [Header("アニメーション・演出周り")]
    [SerializeField, Tooltip("対応するAnimationOverrideController")]
    AnimatorOverrideController _bossAniCom = default;
    public AnimatorOverrideController BossAniCon => _bossAniCom;

    [SerializeField, Tooltip("戦闘開始時の演出用AnimationClip")]
    AnimationClip _battleStartClip = default;
    public AnimationClip BattleStartClip => _battleStartClip;

    [SerializeField, Tooltip("戦闘終了時の演出用AnimationClip")]
    AnimationClip _battleEndClip = default;
    public AnimationClip BattleEndClip => _battleEndClip;

    [SerializeField, Tooltip("撃退後の移動先シーン名")]
    string _sceneName = "";
    public string SceneName => _sceneName;

    [Header("SEの対応" +
        "※インデックスを対応させてください。")]
    [Header("SEの種類")]
    [SerializeField] List<string> _seTypeList = default;
    public List<string> SeTypeList => _seTypeList;

    [Header("ファイル名" +
        "※CRIのAtomBrowserからファイル名をコピペする")]
    [SerializeField, Tooltip("SEのリスト")]
    List<string> _seFileList = default;
    public List <string> SeFileList => _seFileList;

}