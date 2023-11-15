//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Threading;
using System;

public abstract class StageBase : MonoBehaviour
{
    [SerializeField] private GameObject _playerObj;
    [SerializeField] private RestartController _restartController;
    [SerializeField] private string _cueName;

    private IDisposable _onDeadDisposable;

    private void Awake()
    {
        _onDeadDisposable = _playerObj.GetComponent<IHealth>().OnDead.Subscribe(_ => PlayerDead());
        CriAudioManager.Instance.BGM.Play("CueSheet_0", _cueName);
    }

    /// <summary>
    /// Playerが死んだときの流れ
    /// </summary>
    private void PlayerDead() 
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