//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMove : IPlayerState
{
    [SerializeField] private Rigidbody2D _rb;
    [Header("Playerのスピード")]
    [SerializeField] private float _walkSpeed;
    [Header("ダッシュスピード")]
    [SerializeField] private float _dashSpeed;

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
        if (InputProvider.Instance.GetStayInput(InputProvider.InputType.Dash))
        {
            Run();
        }
        else 
        {
            Walk();
        }
       
    }

    private void Run()
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

        _rb.velocity = new Vector2(_dir.x * _dashSpeed, _rb.velocity.y);
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

        _rb.velocity = new Vector2(_dir.x * _walkSpeed, _rb.velocity.y);
    }


    private void MoveDirSprite()
    {
        if (_dir.x < -0.5f) 
        {
            _env.PlayerTransform.rotation = new Quaternion(0, 180, 0, 0);
        }
        else if(_dir.x > 0.5f)
        {
            _env.PlayerTransform.rotation = new Quaternion(0, 0, 0, 0);
        }
        
    }

    public void Dispose()
    {
        
    }
}