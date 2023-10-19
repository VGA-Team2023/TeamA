using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private CriAudioType _criAudioType;
    [SerializeField] private string _cueSheetName = "";
    [SerializeField] private string _cueName = "";

    private CriAudioManager _criAudioManager;

    private void Start()
    {
        if (CriAudioManager.Instance != null)
        {
            _criAudioManager = CriAudioManager.Instance;
        }
    }

    public void PlaySound()
    {
        _criAudioManager.PlaySE(_cueSheetName, _cueName);
    }

    public void StopSound()
    {
    }
}