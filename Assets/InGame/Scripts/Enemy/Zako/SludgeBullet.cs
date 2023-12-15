//日本語対応 
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary> ヘドロが吐く弾のクラス </summary>
public class SludgeBullet : MonoBehaviour
{
    Rigidbody2D _rb = default;
    [SerializeField, Tooltip("弾の速度")] float _shootSpeed = 1.0f;
    [SerializeField, Tooltip("Playerに与えるダメージの大きさ")] float _damageSize = 1.0f;
    [Tooltip("ヘドロを飛ばすベクトル")] Vector2 _shootDir = default;
    GameManager _gm = default;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _gm = GameManager.Instance;
        _shootDir = (_gm.PlayerEnvroment.PlayerTransform.position.x < transform.position.x) ? Vector2.left : Vector2.right;
        _rb.velocity = _shootDir * _shootSpeed;
        transform.parent = null;
    }

    /// <summary> Playerに攻撃が当たったときにダメージを与える </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerHp>(out var playerHp))
        {
            Vector2 _knockBackDir = playerHp.transform.position - transform.position;
            playerHp.ApplyDamage(_damageSize, _knockBackDir.normalized).Forget();
        }
    }

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

}
