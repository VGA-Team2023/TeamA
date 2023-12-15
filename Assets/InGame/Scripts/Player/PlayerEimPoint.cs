//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class PlayerEimPoint : IPlayerState
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Transform _eimPos;
    [Header("感度")]
    [SerializeField] private float _eimSpeed;

    private Vector2 _dir;
    private PlayerEnvroment _env;
    private Vector3 _savePos;

    public void SetUp(PlayerEnvroment env, CancellationToken token)
    {
        _env = env;
        _savePos = _env.PlayerTransform.position;
    }

    public void Update()
    {
        if (_env.PlayerState.HasFlag(PlayerStateType.Damage) ||
            _env.PlayerState.HasFlag(PlayerStateType.Inoperable))
        {
            _dir = Vector2.zero;
            return;
        }

        Move();
        MovementRestrictions();
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    private void Move()
    {
        _dir = InputProvider.Instance.EimDir.normalized;


        _eimPos.position = _eimPos.transform.position + _env.PlayerTransform.position - _savePos;
        _savePos = _env.PlayerTransform.position;
    }

    /// <summary>
    /// EimPointの移動を制限する
    /// </summary>
    private void MovementRestrictions()
    {

        //左下
        Vector3 screenLeftBottom = Camera.main.ScreenToWorldPoint(Vector3.zero);
        //右上
        Vector3 screenRightTop = Camera.main.ScreenToWorldPoint(
        new Vector3(Screen.width, Screen.height, 0));

        _eimPos.transform.position = new Vector2(
            Mathf.Clamp(_eimPos.position.x, screenLeftBottom.x, screenRightTop.x),
            Mathf.Clamp(_eimPos.position.y, screenLeftBottom.y, screenRightTop.y));
    }

    public void FixedUpdate()
    {
        _rb.velocity = _dir * _eimSpeed;
    }

    public void Dispose()
    {
      
    }
}
