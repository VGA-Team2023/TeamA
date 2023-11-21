//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//足場の上に薄くコライダー(IsTriggerオン)を乗せてください
//そのためボックスコライダーは"2つ"使います
//右、上から動き始めます

//プレイヤーのRigidbodyをKinematicにしているため動作不良があったら変えます

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class MoveFloor : WaterGimmickBase
{
    [SerializeField, Tooltip("動く幅")]
    float _moveDis = 5f;

    [SerializeField, Tooltip("動くスピード")]
    float _moveSpeed = 5f;

    [Header("動かす方向の指定(横ならFreezePositionX1つを選択)")]
    [SerializeField, Tooltip("方向")]
    RigidbodyConstraints2D _constraints = RigidbodyConstraints2D.FreezePositionX;

    /// <summary>作動したか</summary>
    bool _isActive = false;

    Rigidbody2D _rb = default;

    Vector2 _pos = default;

    int _dir = 1;
    private void Update()
    {
        if (_isActive)
        {
            //横移動
            if (_constraints == RigidbodyConstraints2D.FreezePositionX && _isActive)
            {
                HorizontalMove();
            }
            //縦移動
            else if (_constraints == RigidbodyConstraints2D.FreezePositionY)
            {
                VerticalMove();
            }
        }

    }

    /// <summary>横移動</summary>
    void HorizontalMove()
    {
        //右に動いていた時
        if (_pos.x + _moveDis <= transform.position.x && _dir == 1)
        {
            _dir = -1;
        }
        //左に動いていた時
        else if (_pos.x >= transform.position.x && _dir == -1)
        {
            _dir = 1;
        }

        Vector2 moveDir = new Vector2(_dir, 0).normalized;
        _rb.velocity = new Vector2(moveDir.x * _moveSpeed, _rb.velocity.y);
    }

    /// <summary>縦移動</summary>
    void VerticalMove()
    {
        //上に動いていた時
        if (_pos.y + _moveDis <= transform.position.y && _dir == 1)
        {
            _dir = -1;
        }
        //下に動いていた時
        else if (_pos.y >= transform.position.y && _dir == -1)
        {
            _dir = 1;
        }

        Vector2 moveDir = new Vector2(0, _dir).normalized;
        _rb.velocity = new Vector2(_rb.velocity.x, moveDir.y * _moveSpeed);
    }

    //Playerが乗ってきたとき一緒に動かすために子オブジェクトに指定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            player.gameObject.transform.parent = transform;
        }
    }

    //Playerが降りたとき子オブジェクトを解除
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            player.gameObject.transform.parent = null;
        }
    }

    public override void WeightActive()
    {
        //最初の場所を記憶
        _pos = transform.position;
        _isActive = true;

        _rb = GetComponent<Rigidbody2D>();
        //フリーズを解く
        _rb.constraints &= ~_constraints;
    }
}
