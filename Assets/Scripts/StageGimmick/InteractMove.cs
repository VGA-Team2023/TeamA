using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

//�v���C���[�����͈͓��ɂ���Ƃ��ɃC���^���N�g�{�^���������ꂽ�Ƃ��̏���
public class InteractMove : MonoBehaviour
{
    [SerializeField] private PlayerEnvroment _env;
    [SerializeField] private bool _trigger;
    Animator _interactAnim;

    void Start()
    {
        _trigger = false;
        _interactAnim = GetComponent<Animator>();
    }

    void Update()
    {
        if (_trigger && InputProvider.Instance.GetStayInput(InputProvider.InputType.Interact))
        {
            _interactAnim.SetBool("Interact", true);
            Debug.Log("�C���^�[�A�N�g");
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<PlayerHp>(out var playerHp))
        {
            _trigger = true;
            Debug.Log("�v���C���[���͈͓��ɂ��܂�");
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent<PlayerHp>(out var playerHp))
        {
            _trigger = false;
            Debug.Log("�v���C���[���͈͊O�ł�");
        }
    }
}
