//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerのStateをコントロールするクラス
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeReference, SubclassSelector]
    private List<IPlayerState> _playerStateList = new List<IPlayerState>();
    [SerializeField] private PlayerEnvroment _playerEnvroment;

    void Start()
    {
        SetUpEnv();
        for (int i = 0; i < _playerStateList.Count; i++) 
        {
            _playerStateList[i].SetUp(_playerEnvroment);
        }
    }

    void Update()
    {
        for (int i = 0; i < _playerStateList.Count; i++)
        {
            _playerStateList[i].Update();
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < _playerStateList.Count; i++)
        {
            _playerStateList[i].FixedUpdate();
        }
    }

    private void SetUpEnv() 
    {
        _playerEnvroment.PlayerTransform = transform;
    }
}
