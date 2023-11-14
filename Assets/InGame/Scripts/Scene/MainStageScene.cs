//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Threading;
using System;

public class MainStageScene : MonoBehaviour
{
    [SerializeField] private GameObject _playerObj;
    [SerializeField] private RestartController _restartController;

    private IDisposable _onDeadDisposable;

    private void Awake()
    {
        _onDeadDisposable = _playerObj.GetComponent<IHealth>().OnDead.Subscribe(_ => PlayerDead());
        CriAudioManager.Instance.BGM.Play("CueSheet_0", "BGM_stage1");
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