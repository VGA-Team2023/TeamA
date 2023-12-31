using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class ForestBossAnimController : MonoBehaviour
{
    [Tooltip("ボスの名前ロゴ絵")]
    [SerializeField] private GameObject _bossName = null;
    [Tooltip("ロゴ絵を表示する時間")]
    [SerializeField] private float _activeTime = 3f;
    [Tooltip("z軸を回転させる値")]
    [SerializeField] private float _valueRotat = 0.5f;
    private bool _finish = false;
    private Subject<Unit> _onParticleEnd = new Subject<Unit>();
    public IObservable<Unit> OnParticleEnd => _onParticleEnd;

    private void Start()
    {
        CriAudioManager.Instance.BGM.Play("CueSheet_0", "Boss_BGM_OP");
    }

    private void Update()
    {
        if (!_finish)
        {
            gameObject.transform.Rotate(0f, 0f, _valueRotat);
        }
    }

    private void OnParticleSystemStopped()
    {
        _finish = true;

        if (_bossName != null) { StartCoroutine(OnBossNameImage(_activeTime, _bossName)); }
        _onParticleEnd.OnNext(new Unit());
    }

    private IEnumerator OnBossNameImage(float time, GameObject img)
    {
        img.SetActive(true); 
        yield return new WaitForSeconds(time);
        img.SetActive(false);
        CriAudioManager.Instance.BGM.StopAll();
    }

    private void OnDestroy()
    {
        _onParticleEnd?.Dispose();
    }
}
