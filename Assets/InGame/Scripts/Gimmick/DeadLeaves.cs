using UnityEngine;

/// <summary> �M�~�b�N�F�͗t </summary>
public class DeadLeaves : WaterGimmickBase
{
    [SerializeField, Tooltip("�f�t�H���g�̃R���C�_�[")] Collider2D _defaultCollider = default;
    [SerializeField, Tooltip("�V�����R���C�_�[")] Collider2D _newCollider = default;
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
    /// �R���C�_�[������������
    /// </summary>
    public void ChangeCollider()
    {
        _defaultCollider.isTrigger = true;
        _newCollider.enabled=true;
    }

}
