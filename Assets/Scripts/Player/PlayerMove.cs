//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMove : IPlayerState
{
    [SerializeField] private Rigidbody2D _rb;
    [Header("Playerのスピード")]
    [SerializeField] private float _speed;

    private Vector3 _dir;
    private PlayerEnvroment _env;

    public void SetUp(PlayerEnvroment env)
    {
        _env = env;
    }

    public void Update()
    {
        _dir = InputProvider.Instance.MoveDir;
        MoveDirSprite();
    }

    public void FixedUpdate()
    {
        Walk();
    }

    private void MoveDirSprite()
    {
        if (_dir.x == -1) 
        {
            _env.PlayerTransform.rotation = new Quaternion(0, 180, 0, 0);
        }
        else if(_dir.x == 1)
        {
            _env.PlayerTransform.rotation = new Quaternion(0, 0, 0, 0);
        }
        
    }

    private void Run() 
    {

    }

    private void Walk() 
    {
        if (_dir == Vector3.zero)
        {
            _env.RemoveState(PlayerStateType.Walk);
        }
        else
        {
            _env.RemoveState(PlayerStateType.Run);
            _env.AddState(PlayerStateType.Walk);
            _env.LastDir = _dir;
        }

        _rb.velocity = new Vector2(_dir.x * _speed, _rb.velocity.y);
    }

    public void Dispose()
    {
        
    }
}
