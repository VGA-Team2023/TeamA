//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

[Serializable]
public class PlayerEnvroment
{
    public PlayerEnvroment(Transform playerTransform, PlayerAnimation playerAnimation) 
    {
        _playerAnimation = playerAnimation;
        _playerTransform = playerTransform;
    }

    public PlayerStateType PlayerState => _playerState;
    public Transform PlayerTransform => _playerTransform;
    public PlayerAnimation PlayerAnim => _playerAnimation;

    [Tooltip("向いている方向")]
    [NonSerialized] public Vector2 LastDir;

    private PlayerStateType _playerState;
    private PlayerAnimation _playerAnimation;
    private Transform _playerTransform;

    /// <summary>
    /// 状態を追加する
    /// </summary>
    /// <param name="state">追加する状態</param>
    public void AddState(PlayerStateType state)
    {
        _playerState |= state;
    }

    /// <summary>
    /// 状態を削除する
    /// </summary>
    /// <param name="state">削除する状態</param>
    public void RemoveState(PlayerStateType state)
    {
        _playerState &= ~state;
    }
}
