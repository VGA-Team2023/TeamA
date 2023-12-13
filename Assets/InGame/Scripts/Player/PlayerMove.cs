//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class PlayerMove : IPlayerState
{
    [SerializeField] private Rigidbody2D _rb;
    [Header("Playerのスピード")]
    [SerializeField] private float _walkSpeed;
    [Header("ダッシュスピード")]
    [SerializeField] private float _dashSpeed;

    private Vector3 _dir;
    private PlayerEnvroment _env;
    private int _walkSE = -1;

    public void SetUp(PlayerEnvroment env, CancellationToken token)
    {
        _env = env;
    }

    public void Update()
    {
        _dir = InputProvider.Instance.MoveDir;
        MoveDirSprite();

        if (_dir == Vector3.zero) 
        {
            _env.PlayerAnim.WalkAnim(false);
            _env.PlayerAnim.RunAnim(false);
        }
    }

    public void FixedUpdate()
    {
        if (_env.PlayerState.HasFlag(PlayerStateType.Damage) ||
            _env.PlayerState.HasFlag(PlayerStateType.Inoperable)) return;

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
            _env.RemoveState(PlayerStateType.Run);
        }
        else
        {
            _env.RemoveState(PlayerStateType.Run);
            _env.AddState(PlayerStateType.Walk);
            _env.LastDir = _dir;
            _env.PlayerAnim.RunAnim(true);
            _env.PlayerAnim.WalkAnim(false);
        }

        _rb.velocity = new Vector2(_dir.x * _dashSpeed, _rb.velocity.y);
    }

    private void Walk()
    {
        if (_dir == Vector3.zero)
        {
            _env.RemoveState(PlayerStateType.Walk);
            if (_walkSE != -1) 
            {
                Debug.Log("止まった");
                //CriAudioManager.Instance.SE.Stop(_walkSE);
                _walkSE = -1;
            }
        }
        else
        {
            if (_walkSE == -1) 
            {
                Debug.Log("ある");
                //_walkSE = CriAudioManager.Instance.SE.Play("CueSheet_0", "SE_player_FS_1");
            }
            _env.RemoveState(PlayerStateType.Run);
            _env.AddState(PlayerStateType.Walk);
            _env.LastDir = _dir;
            _env.PlayerAnim.WalkAnim(true);
            _env.PlayerAnim.RunAnim(false);
        }

        _rb.velocity = new Vector2(_dir.x * _walkSpeed, _rb.velocity.y);
    }


    private void MoveDirSprite()
    {
        if (_dir.x < -0.5f)
        {
            _env.PlayerTransform.rotation = new Quaternion(0, 180, 0, 0);
        }
        else if (_dir.x > 0.5f)
        {
            _env.PlayerTransform.rotation = new Quaternion(0, 0, 0, 0);
        }

    }

    public void Dispose()
    {

    }
}
