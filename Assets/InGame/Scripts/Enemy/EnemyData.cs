//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ボスと同じところ
[CreateAssetMenu(menuName = "MyScriptable/Create EnemyData")]

public class EnemyData : ScriptableObject
{
    [SerializeField, Tooltip("巡回する距離")]
    float _moveDistance = 0f;
    public float MoveDistance => _moveDistance;

    [SerializeField, Tooltip("プレイヤーを見つける距離")]
    float _lookDistance = 0f;
    public float LookDistance => _lookDistance;

    [SerializeField, Tooltip("攻撃距離")]
    float _attackDistance = 0f;

    [SerializeField, Tooltip("攻撃間隔(秒)")]
    float _attackInterval = 0f;
    public float AttackInterval => _attackInterval;
    public float AttackDistance => _attackDistance;

    [SerializeField, Tooltip("攻撃力")]
    float _attackValue = 0f;
    public float AttackValue => _attackValue;

    [SerializeField, Tooltip("移動速度")]
    float _moveSpeed = 0f;
    public float MoveSpeed => _moveSpeed;

    [SerializeField, Tooltip("HP")]
    float _hp = 0f;
    public float Hp => _hp;

    [SerializeField, Tooltip("SEのリスト")]
    List<AudioClip> _seList = default;
    public List<AudioClip> SEList => _seList;

    [SerializeField, Tooltip("AnimationOverrideController")]
    Animator _enemyAnimCom = default;
    public Animator EnemyAnimCon => _enemyAnimCom;
}
