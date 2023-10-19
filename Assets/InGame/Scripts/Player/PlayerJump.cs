//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : IPlayerState
{
    [SerializeField] private Rigidbody2D _rb;
    [Header("ジャンプの強さ")]
    [SerializeField] private float _jumpPower;
    [Header("着地判定の大きさ")]
    [SerializeField] private Vector2 size;
    [TagName]
    [SerializeField] private string _groundTag;

    private PlayerEnvroment _env;
    private bool _isTwoJumps;
    private bool _isGround;
    Collider2D[] _buffer = new Collider2D[10];

    public void SetUp(PlayerEnvroment env)
    {
        InputProvider.Instance.SetEnterInput(InputProvider.InputType.Jump, Jump);
        _env = env;
    }

    public void Update()
    {
        GroundCheck();
    }

    public void FixedUpdate()
    {

    }

    private void GroundCheck()
    {
        var col = Physics2D.OverlapBoxNonAlloc(_env.PlayerTransform.position, size, 0, _buffer);
        Debug.DrawRay(_env.PlayerTransform.position, size);

        for (int i = 0; i < col; i++)
        {
            _isGround = false;
            if (_buffer[i].CompareTag(_groundTag))
            {
                _isGround = true;
                _isTwoJumps = true;
                break;
            }
        }
    }

    private void Jump()
    {
        if (_isGround)
        {
            _rb.AddForce(Vector3.up * _jumpPower, ForceMode2D.Impulse);
        }
        else if (_isTwoJumps) 
        {
            _isTwoJumps = false;
            _rb.AddForce(Vector3.up * _jumpPower, ForceMode2D.Impulse);
        }
    }

    public void Dispose()
    {

    }
}
