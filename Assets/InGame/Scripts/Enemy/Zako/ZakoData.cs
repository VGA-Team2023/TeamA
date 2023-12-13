//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ボスと同じところ
[CreateAssetMenu(menuName = "MyScriptable/Create EnemyData")]

public class ZakoData : ScriptableObject
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

    [SerializeField, Tooltip("攻撃後とどまる時間")]
    float _stopTime = 1.5f;
    public float StopTime => _stopTime;

    [SerializeField, Tooltip("ノックバックの威力")]
    float _knockback = 3f;
    public float Knockback => _knockback;

    [SerializeField, Tooltip("HP")]
    float _hp = 0f;
    public float Hp => _hp;

    [Header("1,足音 2,攻撃 3,被ダメ の順番にコピペ")]
    [SerializeField, Tooltip("SEのリスト,1,足音 2,攻撃 3,被ダメ")]
    List<string> _seList = default;
    public List<string> SEList => _seList;

    [SerializeField, Tooltip("AnimationOverrideController")]
    AnimatorOverrideController _enemyAnimCom = default;
    public AnimatorOverrideController EnemyAnimCon => _enemyAnimCom;
}
