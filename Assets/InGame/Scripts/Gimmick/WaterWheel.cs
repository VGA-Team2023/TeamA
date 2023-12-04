using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//���Ԃ̉񂵕��₻�̑��U���擾���@�����܂��Ă��Ȃ����߉�
[RequireComponent(typeof(Rigidbody2D))]
public class WaterWheel : MonoBehaviour
{
    [SerializeField, Tooltip("���̍U���^�O")]
    string _tagName = string.Empty;

    /// <summary>���Ԃ��N���������ǂ���</summary>
    bool _isWaterWheel = false;

    /// <summary>���Ԃ��N�����Ă��邩</summary>
    public bool IsWaterWheel => _isWaterWheel;

    void Start()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�U���ɓ���������N��
        if (collision.gameObject.tag == _tagName)
        {
            _isWaterWheel = true;
        }
    }
}
