using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class ForestBossAnimController : MonoBehaviour
{
    [Tooltip("�{�X�̖��O���S�G")]
    [SerializeField] private GameObject _bossName = null;
    [Tooltip("���S�G��\�����鎞��")]
    [SerializeField] private float _activeTime = 3f;
    [Tooltip("z������]������l")]
    [SerializeField] private float _valueRotat = 0.5f;
    private bool _finish = false;
    private Subject<Unit> _onParticleEnd = new Subject<Unit>();
    public IObservable<Unit> OnParticleEnd => _onParticleEnd;

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
    }

    private IEnumerator OnBossNameImage(float time, GameObject img)
    {
        img.SetActive(true); 
        yield return new WaitForSeconds(time);
        img.SetActive(false);
        _onParticleEnd.OnNext(new Unit());
    }

    private void OnDestroy()
    {
        _onParticleEnd?.Dispose();
    }
}
