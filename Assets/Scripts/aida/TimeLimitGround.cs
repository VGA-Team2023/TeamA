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
        Reactive,
    }

    //崩れる前のスプライト
    [SerializeField] private SpriteRenderer beforeImage = null;
    //崩れた後のスプライト
    [SerializeField] private SpriteRenderer afterImage = null;
    //現在のスプライト
  　private SpriteRenderer currentImage = null;
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
        currentImage = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        currentImage.sprite = beforeImage.sprite;
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
                if (ReceiveForce())
                {
                    totalTime = 0;
                    state = State.Reactive;
                }
                break;
            case State.Reactive:
                Debug.Log("Reactive");
                Reactive();
                if (ReceiveForce())
                {
                    totalTime = 0;
                    state = State.Wait;
                }
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


    private void Reactive()
    {
        currentImage.sprite = beforeImage.sprite;
        col.enabled = true;
        Debug.Log("Reactive");
    }

    private void Corpse()
    {
        currentImage.sprite = afterImage.sprite;
        col.enabled = false;
        Debug.Log("Corpse");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            state = State.Init;
            Debug.Log("Enter");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Player" && col.enabled)
        {
            totalTime = 0;
            state = State.Wait;
            Debug.Log("Exit");
        }
    }
}
