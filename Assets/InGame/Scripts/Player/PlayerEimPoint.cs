//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class PlayerEimPoint : IPlayerState
{
    [SerializeField] private Rigidbody2D _rb;
    [Header("感度")]
    [SerializeField] private float _eimSpeed;

    private Vector2 _dir;
    private PlayerEnvroment _env;

    public void SetUp(PlayerEnvroment env, CancellationToken token)
    {
        _env = env;
    }

    public void Update()
    {
        _dir = InputProvider.Instance.EimDir;
    }

    public void FixedUpdate()
    {
        _rb.velocity = _dir * _eimSpeed;
    }

    public void Dispose()
    {
      
    }
}
