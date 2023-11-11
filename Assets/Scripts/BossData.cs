using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 各ステージのBossのパラメータデータ用のScriptableObject
/// </summary>
[CreateAssetMenu(menuName = "MyScriptable/Create BossData")]

public class BossData : ScriptableObject
{
    [SerializeField, Tooltip("攻撃方法が変化する距離")]
    float _attackChangeDistance = 0f;
    public float AttackChangeDistance => _attackChangeDistance;

    [SerializeField, Tooltip("追跡と攻撃のボーダー距離")]
    float _borderDistance = 0f;
    public float BorderDistance => _borderDistance;

    [SerializeField, Tooltip("移動速度")]
    float _moveSpeed = 0f;
    public float MoveSpeed => _moveSpeed;

    [SerializeField, Tooltip("デフォルトHP")]
    float _hp = 0f;
    public float Hp => _hp;

    [SerializeField, Tooltip("対応するAnimationOverrideController")]
    Animator _bossAniCom = default;
    public Animator BossAniCon => _bossAniCom;

    [SerializeField, Tooltip("SEのリスト")]
    List<AudioClip> _seList = default;
    public List<AudioClip> SEList => _seList;

}