using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> チェックポイントを通過するときの処理 </summary>
public class CheckPoint : MonoBehaviour
{
    [SerializeField]RestartController _setPos = default; //一旦SerializeFieldにしてます。多数置くのでシングルトンにしたらインスタンスで探したい

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //PlayerのHPを回復する
        Debug.Log("HP回復");
        //リスタートの座標を変える
        Debug.Log($"リスタート座標が{ gameObject.transform.position}に変更されました。");
        _setPos.SetRestartPos(gameObject.transform);
    }
}
