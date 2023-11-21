using UnityEngine;

/// <summary> Enemy（ザコとボス）共通の基底クラス </summary>
public abstract class EnemyBase : MonoBehaviour, IReceiveWater
{
    [SerializeField, Tooltip("Playerとの距離")]
    float _distance = 0f;
    public float Distance => _distance;
    [Tooltip("GameManagerのインスタンス")]
    GameManager _gm = default;
    public GameManager GManager => _gm;

    private void Update()
    {
        _gm = GameManager.Instance;
        if (_gm != null) _distance = MeasureDistance();
    }

    /// <summary> 水があたったときに呼ばれる。 </summary>
    public void ReceiveWater()
    {
        Damaged();
    }

    /// <summary> 攻撃時に呼ぶメソッド </summary>
    public abstract void Attack();
    /// <summary> 移動時に呼ぶメソッド </summary>
    public abstract void Move();
    /// <summary> 被ダメ時に呼ぶメソッド </summary>
    public abstract void Damaged();
    /// <summary> 退場時に呼ぶメソッド </summary>
    public abstract void Exit();

    /// <summary> Playerとの距離を測る </summary>
    /// <returns>Playerとの距離</returns>
    public float MeasureDistance()
    {
        Vector2 pPos = _gm.PlayerEnvroment.PlayerTransform.position;   //Playerの座標
        float distance = Vector2.Distance(this.transform.position, pPos);   //Playerとの距離
        return distance;
    }

}
