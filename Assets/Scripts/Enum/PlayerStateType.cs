//日本語対応
using System;

[Flags]
public enum PlayerStateType
{
    Run = 1,
    Attack = 2,
    Damage = 4,
    Jump = 8,
}
