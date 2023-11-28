using UnityEngine;

/// <summary> ギミック：枯葉 </summary>
public class DeadLeaves : WaterGimmickBase
{
    [SerializeField, Tooltip("デフォルトのコライダー")] Collider2D _defaultCollider = default;
    [SerializeField, Tooltip("新しいコライダー")] Collider2D _newCollider = default;
    Animator _deadLeavesAnim = default;
    private void Start()
    {
        _deadLeavesAnim = GetComponent<Animator>();
        _newCollider.enabled = false;   
    }

    public override void WeightActive()
    {
        ChangeCollider();
        _deadLeavesAnim.SetBool("IsWeightActive", true);
    }

    /// <summary> 
    /// コライダーを書き換える
    /// </summary>
    public void ChangeCollider()
    {
        _defaultCollider.isTrigger = true;
        _newCollider.enabled=true;
    }

}
