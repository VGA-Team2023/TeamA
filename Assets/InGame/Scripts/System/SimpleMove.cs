//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SimpleMove : MonoBehaviour
{
    [SerializeField] private float _fallnum;
    [SerializeField] private float _fallSpeed;
    [SerializeField] private float _interval;
    // Start is called before the first frame update
    void Start()
    {
        var sequence = DOTween.Sequence();
        sequence.AppendInterval(_interval)
                .Append(transform.DOMoveY(transform.position.y + _fallnum, _fallSpeed))
                .AppendInterval(_interval)
                .Append(transform.DOMoveY(transform.position.y, _fallSpeed))
                .AppendInterval(_interval)
                .SetLoops(-1)
                .SetLink(gameObject);

        sequence.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
