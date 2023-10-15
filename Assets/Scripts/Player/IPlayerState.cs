//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    public void SetUp(PlayerEnvroment env);
    public void Update();
    public void FixedUpdate();
    public void Dispose();
}
