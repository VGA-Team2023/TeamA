using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//とりあえず当たったら重りが起動します
//水車,ウツボカズラ,地底の樽のために基底クラス
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PolygonCollider2D))]
public abstract class WeightBase : MonoBehaviour
{
    [SerializeField, Tooltip("仮の攻撃タグ")]
    string _tagName = string.Empty;

    [SerializeField, Tooltip("重りが作動する水の量")]
    float _maxWeight = 10f;

    [SerializeField, Tooltip("現在の水の量/確認用")]
    float _nowWeight = 0f;

    /// <summary>重りが起動したかどうか</summary>
    bool _isWeight = false;

    private void Awake()
    {
        GetComponent<PolygonCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //攻撃に当たったら起動
        if (collision.gameObject.tag == _tagName && !_isWeight)
        {
            _nowWeight++;
            if (_nowWeight >= _maxWeight)
            {
                _isWeight = true;

                //起動
                WeightActive();
            }
        }
    }

    /// <summary>水が一定数たまった時</summary>
    public abstract void WeightActive();
}
