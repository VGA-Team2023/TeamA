//日本語対応
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System;

/// <summary>
/// 次のシーンに移動する際の処理を書くクラス
/// </summary>
[Serializable]
public class PlayerSceneMove : IPlayerSceneMove, IPlayerState
{

    public void SetUp(PlayerEnvroment env, CancellationToken token)
    {

    }

    public void Update()
    {

    }

    public void FixedUpdate()
    {

    }

    public void Dispose()
    {
       
    }

    public void SceneMoveStart()
    {
        Debug.Log("呼ばれた");
    }
}
