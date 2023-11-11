using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ギミック：毒の挙動 </summary>
public class Poison : MonoBehaviour
{
    Collider2D _collider = default;
    Animator _poisonAnim = default;
    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = false;
        _poisonAnim = GetComponent<Animator>();
    }

    /// <summary> Playerから攻撃を受けたときに呼ばれる </summary>
    public void Detoxification()
    {
        //毒が消えて通れるようになる。Triggerはアニメーションでオンにしてます
        _poisonAnim.SetBool("IsAttacked", true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Playerが当たったときにダメージ与えるとか
    }

}
