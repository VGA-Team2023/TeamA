//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using UniRx;
using Cysharp.Threading.Tasks;

public class ForestBossScene : MonoBehaviour
{
    [SerializeField] private FadeScript _fadeScript;
    [SerializeField] private BossBase _bossBase;
    [SerializeField] private BossStartAnimEvent _bossStartAnimEvent;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    void Start()
    {
        _bossStartAnimEvent.OnEndAnim.Subscribe(_ => StartBattle().Forget()).AddTo(this);   
    }

    void Update()
    {
        
    }

    public async UniTask StartBattle()
    {
        _bossBase.BattleStart();
        await _fadeScript.FadeIn();
    }

    
}
