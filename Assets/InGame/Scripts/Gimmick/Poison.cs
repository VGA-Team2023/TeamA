using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary> ギミック：毒の挙動 </summary>
public class Poison : WaterGimmickBase
{
    [SerializeField, Tooltip("Playerに与えるダメージ")]
    float _damageSizeToPlayer = 0f;
    Collider2D _collider = default;
    Animator _poisonAnim = default;
    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = false;
        _poisonAnim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Playerが当たったときにダメージ与える
        if (collision.gameObject.TryGetComponent<PlayerHp>(out var playerHp))
        {
            playerHp.ApplyDamage(_damageSizeToPlayer, Vector2.zero).Forget();
        }
    }

    public override void WeightActive()
    {
        //毒が消えて通れるようになる
        _poisonAnim.SetBool("IsAttacked", true);
    }
}
