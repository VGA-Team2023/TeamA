//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cysharp.Threading.Tasks;
using System;
using UniRx;

/// <summary>
/// Boss戦前のAnimationを再生するためのクラス
/// </summary>
public class BossStartAnimEvent : MonoBehaviour
{
    public IObservable<Unit> OnEndAnim => _onEndAnim;

    [Tooltip("Boss戦前のAnimation")]
    [SerializeField] private PlayableDirector _timeLine;
    [SerializeField] private FadeScript _fadeScript;
    private Subject<Unit> _onEndAnim = new Subject<Unit>(); 

    void Start()
    {
        StartAnim().Forget();
    }

    void Update()
    {
        
    }

    public async UniTask StartAnim() 
    {
        _timeLine.Play();
        await UniTask.WaitUntil
            (() => _timeLine.state == PlayState.Paused, cancellationToken: this.GetCancellationTokenOnDestroy());
        await _fadeScript.FadeOut();
        _onEndAnim.OnNext(new Unit());
    }

    private void OnDestroy()
    {
        _onEndAnim?.Dispose();
    }
}
