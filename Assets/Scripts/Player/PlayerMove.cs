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

    public void SetUp(PlayerEnvroment env)
    {
        
    }

    public void Update()
    {
       
    }

    public void FixedUpdate()
    {
        var dir = InputProvider.Instance.MoveDir;
        _rb.velocity = new Vector2(dir.x * _speed, _rb.velocity.y);
    }

    public void Dispose()
    {
        
    }
}
