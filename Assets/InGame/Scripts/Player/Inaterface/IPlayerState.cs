//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public interface IPlayerState
{
    public void SetUp(PlayerEnvroment env, CancellationToken token);
    public void Update();
    public void FixedUpdate();
    public void Dispose();
}
