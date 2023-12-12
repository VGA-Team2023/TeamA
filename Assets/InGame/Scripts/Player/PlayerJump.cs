//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class PlayerJump : IPlayerState
{
    [SerializeField] private Rigidbody2D _rb;
    [Header("ジャンプの強さ")]
    [SerializeField] private float _jumpPower;
    [Header("着地判定の大きさ")]
    [SerializeField] private Vector2 size;
    [SerializeField] private LayerMask _groundLayer;

    private PlayerEnvroment _env;
    private bool _isTwoJumps;
    private bool _isGround;
    Collider2D[] _buffer = new Collider2D[10];

    public void SetUp(PlayerEnvroment env, CancellationToken token)
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
        if (_env.PlayerState.HasFlag(PlayerStateType.Damage) ||
            _env.PlayerState.HasFlag(PlayerStateType.Inoperable)) return;

        var col = Physics2D.OverlapBoxNonAlloc(_env.PlayerTransform.position, size, 0, _buffer, _groundLayer);
        Debug.DrawRay(_env.PlayerTransform.position, size);

        if (0 < col)
        {
            if (_isGround) return;
            _env.PlayerAnim.JumpAnim(false);
            //CriAudioManager.Instance.SE.Play("CueSheet_0", "SE_player_landing");
            _isGround = true;
            _isTwoJumps = true;
        }
        else if(col == 0)
        {
            _env.PlayerAnim.JumpAnim(true);
            _isGround = false;
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
            _rb.velocity = Vector3.zero;
            _rb.AddForce(Vector3.up * _jumpPower, ForceMode2D.Impulse);
        }
    }

    public void Dispose()
    {
        InputProvider.Instance.LiftEnterInput(InputProvider.InputType.Jump, Jump);
    }
}
