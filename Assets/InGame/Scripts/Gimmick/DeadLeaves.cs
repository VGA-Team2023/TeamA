using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ギミック：枯葉 </summary>
public class DeadLeaves : MonoBehaviour
{
    [SerializeField] Sprite _leafSprite;
    [SerializeField] SpriteRenderer _deadLeafRenderer;
    [SerializeField] LayerMask _waterLayer;
    //Animationがないのでテスト用本来は SetNewCollider
    [SerializeField] Collider2D _leafColliderMock;

    BoxCollider2D _defaultCollider = default;
    Animator _deadLeavesAnim = default;
    private void Start()
    {
        _defaultCollider = GetComponent<BoxCollider2D>();
        _deadLeavesAnim = GetComponent<Animator>();
    }

    /// <summary> Playerから攻撃を受けたときに呼ばれる </summary>
    public void Attacked()
    {
        //枯葉が開くアニメーション再生
        //_deadLeavesAnim.SetBool("IsAttacked", true);
        _deadLeafRenderer.sprite = _leafSprite;
    }


    /// <summary> 
    /// 通れないようにつけていたデカいコライダーを破棄する。
    /// アニメーションイベントから呼ぶ 
    /// </summary>
    public void DestroyDefaultCollider()
    {
        Destroy(_defaultCollider);
    }

    /// <summary>
    /// 葉が開いたあとの形に合わせてコライダーをつける
    /// アニメーションイベントから呼ぶ 
    /// </summary>
    public void SetNewCollider()
    {
        gameObject.AddComponent<PolygonCollider2D>();   //とりあえずポリゴンにしてます。仕様や不具合に合わせて変更
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("BulletWater")) 
        {
            Attacked();
            _leafColliderMock.enabled = true;
        }
    }
}
