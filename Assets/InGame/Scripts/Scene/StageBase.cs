//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Threading;
using System;
using Cinemachine;

public abstract class StageBase : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private RestartController _restartController;
    [SerializeField] private string _cueName;
    [SerializeField] Transform _playerInsPos;
    [SerializeField] PolygonCollider2D _polygonCollider;
    [SerializeField] bool _isCine;

    private IDisposable _onDeadDisposable;

    private void Awake()
    {
        new GameManager();
        PlayerIns();
        CriAudioManager.Instance.BGM.Play("CueSheet_0", _cueName);
    }

    private void PlayerIns()
    {

        var playerIns = Instantiate(_playerPrefab, _playerInsPos);
        var root = playerIns.GetComponentInChildren<IPlayerRoot>();
        GameManager.Instance.PlayerRoot = root;
        root.SetUp();
        playerIns.GetComponentInChildren<CinemachineVirtualCamera>().enabled = _isCine;
        var cine = playerIns.GetComponentInChildren<CinemachineConfiner2D>();
        if (cine) 
        {
            cine.m_BoundingShape2D = _polygonCollider;
        }
        _onDeadDisposable = playerIns.GetComponentInChildren<IHealth>().OnDead.Subscribe(_ => PlayerDead());
    }

    /// <summary>
    /// Playerが死んだときの流れ
    /// </summary>
    public void PlayerDead() 
    {
        _onDeadDisposable?.Dispose();
        var obj = _restartController.Restart();
        _onDeadDisposable = obj.GetComponentInChildren<IHealth>().OnDead.Subscribe(_ => PlayerDead());
    }

    private void OnDestroy()
    {
       _onDeadDisposable?.Dispose();
    }
}