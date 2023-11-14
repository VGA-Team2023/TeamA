//日本語対応
using UnityEngine;

/// <summary> ダメージ処理用のインターフェイス。Enemy（ザコとボス両方）で継承する。 </summary>
public abstract class IEnemyDamaged : MonoBehaviour
{
    /// <summary> 被ダメージ処理。水かプレイヤーから呼ばれる</summary>
    public abstract void Damaged();

}
