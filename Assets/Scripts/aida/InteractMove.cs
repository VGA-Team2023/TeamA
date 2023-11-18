using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

//プレイヤーが一定範囲内にいるときにインタラクトボタンが押されたときの処理
public class InteractMove : MonoBehaviour
{
    [SerializeField, Range(0, 100)]
    private int width = 0;
    [SerializeField,Range(0,100)]
    private int height = 0;
    [SerializeField] private PlayerEnvroment _env;
    Collider2D col;

    void Start()
    {
        col = GetComponent<Collider2D>();
        col.offset = new Vector2(width, height);
    }

    void Update()
    {
        float dx = _env.PlayerTransform.position.x - col.offset.x;
        float dy = _env.PlayerTransform.position.y - col.offset.y;
        float d = Mathf.Sqrt(dx * dx + dy * dy);
        if(d <= _env.PlayerTransform.position.x + col.offset.x)
        {
            if (InputProvider.Instance.GetStayInput(InputProvider.InputType.Interact))
            {
                Debug.Log("InterAct");
                //何らかのプレイヤーのアニメーション
            }
        }
    }
}
