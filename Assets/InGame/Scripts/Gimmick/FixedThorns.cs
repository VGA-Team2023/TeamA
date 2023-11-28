//日本語対応
using Cysharp.Threading.Tasks;
using UnityEngine;

public class FixedThorns : MonoBehaviour
{
    [SerializeField, Tooltip("Playerに与えるダメージの大きさ")] float _damageSize = 0f;
    [SerializeField, Tooltip("このステージのStageBase派生クラス")]StageBase _stageBase = default;

    /// <summary> Playerにダメージを与えてリスポーンさせる </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerHp>(out var playerHp))
        {
            playerHp.ApplyDamage(_damageSize, Vector2.zero).Forget();
            _stageBase.PlayerDead();
        }
    }
}
