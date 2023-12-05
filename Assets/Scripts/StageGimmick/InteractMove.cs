using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

//プレイヤーが一定範囲内にいるときにインタラクトボタンが押されたときの処理
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
            Debug.Log("インターアクト");
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<PlayerHp>(out var playerHp))
        {
            _trigger = true;
            Debug.Log("プレイヤーが範囲内にいます");
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent<PlayerHp>(out var playerHp))
        {
            _trigger = false;
            Debug.Log("プレイヤーが範囲外です");
        }
    }
}
