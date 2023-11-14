using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> チェックポイントを通過するときの処理 </summary>
public class CheckPoint : MonoBehaviour
{
    [SerializeField] RestartController _restartCon = default; //一旦SerializeFieldにしてます。多数置くのでシングルトンにしたらインスタンスで探したい
    [SerializeField, Tooltip("回復するHPの大きさ")] float _healHpSize = 0f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerHp>(out var playerHp))
        {
            playerHp.ApplyHeal(_healHpSize);    //PlayerのHPを回復する
            _restartCon.SetRestartPos(gameObject.transform);        //リスタートの座標を変える
            Debug.Log($"リスタート座標を{gameObject.transform.position}に変更。");
        }
    }
}
