//日本語対応
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// PlayerのStateをコントロールするクラス
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeReference, SubclassSelector]
    private List<IPlayerState> _playerStateList = new List<IPlayerState>();
    [SerializeField] private PlayerAnimation _playerAnim;
    [SerializeField] private PlayerEnvroment _playerEnvroment;

    private CancellationToken _token;

    void Start()
    {
        _token = this.GetCancellationTokenOnDestroy();
        _playerAnim.SetUp(_token);
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
        _playerEnvroment.PlayerAnim = _playerAnim;
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _playerStateList.Count; i++)
        {
            _playerStateList[i].Dispose();
        }
    }
}
