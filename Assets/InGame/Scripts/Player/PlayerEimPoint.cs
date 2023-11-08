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
        _dir = InputProvider.Instance.EimDir;
        _eimPos.position = _eimPos.transform.position + _env.PlayerTransform.position - _savePos;
        _savePos = _env.PlayerTransform.position;
    }

    public void FixedUpdate()
    {
        _rb.velocity = _dir * _eimSpeed;
    }

    public void Dispose()
    {
      
    }
}
