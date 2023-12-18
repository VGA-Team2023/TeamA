//日本語対応
using UnityEngine;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(Rigidbody2D))]
public class FallingSpike : MonoBehaviour
{
    [SerializeField, Tooltip("復活時間")]
    float _revivalTime = 3f;
    [SerializeField, Tooltip("攻撃力")]
    float _attackValue = 1f;
    [SerializeField, Tooltip("感知する距離(横の距離だけとする)")]
    float _distance = 3f;
    [SerializeField, Tooltip("落ちるスピード")]
    float _speed = 5f;

    /// <summary>当たったか<summary>
    bool _isHit = false;

    Rigidbody2D _rb = default;

    GameManager _gm = default;

    Vector3 _pos = default;

    float _t = 0f;

    Collider2D _col = default;

    SpriteRenderer _sprite = default;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _gm = GameManager.Instance;
        _pos = gameObject.transform.position;
        _col = gameObject.GetComponent<Collider2D>();
        _sprite = gameObject.GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        if (_gm != null)
        {
            Vector2 pPos = new Vector2(_gm.PlayerEnvroment.PlayerTransform.position.x, 0);
            Vector2 pos = new Vector2(transform.position.x, 0);

            //落とす
            if (Vector2.Distance(pos, pPos) <= _distance && !_isHit)
            {
                _rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
                _rb.velocity = Vector3.down * _speed;
            }
            //ナニカに当たったら
            else if (_isHit)
            {
                _col.enabled = false;
                _sprite.enabled = false;
                _t += Time.deltaTime;
                if (_t >= _revivalTime)
                {
                    //元に戻す
                    Set();
                }
            }
        }
    }

    void Set()
    {
        _t = 0f;
        _isHit = false;
        gameObject.transform.position = _pos;
        _rb.constraints |= RigidbodyConstraints2D.FreezePositionY;
        _col.enabled = true;
        _sprite.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerHp>(out var pHp))
        {
            Debug.Log("刺が当たった");
            Vector2 knockBackDir = pHp.transform.position - transform.position;
            pHp.ApplyDamage(_attackValue, knockBackDir.normalized).Forget();
            pHp.ApplyDamage(_attackValue, knockBackDir.normalized).Forget();
        }
        _isHit = true;
    }
}
