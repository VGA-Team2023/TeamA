//日本語対応
using System;

[Flags]
public enum PlayerStateType
{
    Walk = 1,
    Run = 2,
    Attack = 4,
    Damage = 8,
    Jump = 16,
    Inoperable = 32,
}
