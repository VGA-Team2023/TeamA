using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//崩れる足場を生成するコンポーネント
public class TimeGroundCreate : MonoBehaviour
{
    //足場のプレハブ
    [SerializeField] GameObject field;
    private TimeLimitGround tg;
    private float totalTime;
    private float timeInterval = 5f;

    void Start()
    {
        //足場を作る
        tg = Instantiate(field, transform.position, Quaternion.identity).GetComponent<TimeLimitGround>();
    }


    void Update()
    {
        //足場がなかったらすぐに生み出さず一定時間すぎたら足場を再生成する
        if (tg == null)
        {
            totalTime += Time.deltaTime;
            if(timeInterval <= totalTime)
            {
                totalTime = 0;
                tg = Instantiate(field, transform.position, Quaternion.identity).GetComponent<TimeLimitGround>();
            }
        }
    }
}
