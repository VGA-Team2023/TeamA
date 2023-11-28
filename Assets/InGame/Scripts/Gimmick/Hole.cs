//日本語対応
using UnityEngine;

/// <summary> 落下判定を取りたいところに設置する </summary>
public class Hole : MonoBehaviour
{
    [SerializeField, Tooltip("このシーンのStageBase")] StageBase _stageBase;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<PlayerHp>(out var playerHp))
        {
            _stageBase.PlayerDead();
        }
    }
}
