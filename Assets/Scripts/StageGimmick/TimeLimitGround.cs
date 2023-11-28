using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//制限時間付きの足場の処理。
public class TimeLimitGround : MonoBehaviour
{
    enum State 
    {
        Wait,
        Init,
        Corpse,
    }

    //足場が崩れるまでの時間
    [SerializeField] private float timeLimit = 5f;
    //プレイヤーが足場に乗っている時間
    private float totalTime = 0f;
    private State state = State.Wait;
    private Collider2D col;
    [SerializeField] private PlayerEnvroment _env;

    public void Start()
    {
        state = State.Wait;
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        switch (state) {
            case State.Wait:
                Debug.Log("Wait");
                break;
            case State.Init:
                Debug.Log("Init");
                if (ReceiveForce())
                {
                    totalTime = 0;
                    state = State.Corpse;
                }
                break;
            case State.Corpse:
                Debug.Log("Corpse");
                Corpse();
                break;
        }
    }

    private bool ReceiveForce()
    {
        totalTime += Time.deltaTime;
        if (timeLimit <= totalTime)
        {
            return true;
        }
        return false;
    }

   

    //アニメーションイベントから呼び出す
    private void Corpse()
    {
        Destroy(gameObject);
        Debug.Log("Corpse");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<PlayerHp>(out var playerHp))
        {
            state = State.Init;
            Debug.Log("Enter");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<PlayerHp>(out var playerHp) && col.enabled)
        {
            totalTime = 0;
            state = State.Wait;
            Debug.Log("Exit");
        }
    }
}
