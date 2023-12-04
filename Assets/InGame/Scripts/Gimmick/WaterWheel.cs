using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//水車の回し方やその他攻撃取得方法が決まっていないため仮
[RequireComponent(typeof(Rigidbody2D))]
public class WaterWheel : MonoBehaviour
{
    [SerializeField, Tooltip("仮の攻撃タグ")]
    string _tagName = string.Empty;

    /// <summary>水車が起動したかどうか</summary>
    bool _isWaterWheel = false;

    /// <summary>水車が起動しているか</summary>
    public bool IsWaterWheel => _isWaterWheel;

    void Start()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //攻撃に当たったら起動
        if (collision.gameObject.tag == _tagName)
        {
            _isWaterWheel = true;
        }
    }
}
