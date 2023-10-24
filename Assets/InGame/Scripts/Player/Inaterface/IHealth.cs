//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    public void ApplyDamage(float damageNum);
    public void ApplyHeal(float healNum);
}
