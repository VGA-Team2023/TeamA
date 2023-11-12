using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageble
{
    void ReceiveDamage(float damage, Vector3 myselfPosition);
}