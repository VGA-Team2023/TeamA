//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestStage : StageBase
{
    void Start()
    {
        CriAudioManager.Instance.BGM.Play("CueSheet_0", _cueName);
    }

    void Update()
    {
        
    }
}
