using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//制限時間付きの足場の処理。一定時間が過ぎると足場が崩れる
public class TimeLimitGround : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer beforeImage = null;
    [SerializeField]
    private SpriteRenderer afterImage = null;
    private SpriteRenderer currentImage = null;
    //足場が崩れるまでの時間
    [SerializeField]
    private float timeLimit = 5f;
    //プレイヤーが足場に乗っている時間
    private float totalTime = 0f;
    private int flag = 0;
    private Collider2D col;
    private PlayerEnvroment _env;

    private void Start()
    {
        currentImage = GetComponent<SpriteRenderer>();
        currentImage.color = beforeImage.color;
        col = GetComponent<Collider2D>();
    }

    public void SetUp(PlayerEnvroment env, CancellationToken token)
    {
        _env = env;
    }

    private void Update()
    {
        Debug.DrawLine(transform.position + Vector3.up * 0.5f, transform.position + Vector3.up * -0.2f, Color.red);
        if (_env.PlayerTransform.position.y == gameObject.transform.position.y + 0.5f)
        {
            flag = 1;
            Debug.Log("Col");
        }

        if (flag != 0)
        {
            ReceiveForce();

            if (totalTime >= timeLimit)
            {
                totalTime = 0f;
                col.gameObject.SetActive(false);
                currentImage.color = afterImage.color;
                Debug.Log("Corpse");
            }
        }
        else
        {
            totalTime = 0f;
        }
    }

    public void ReceiveForce()
    {
        totalTime += Time.deltaTime;
    }

}
