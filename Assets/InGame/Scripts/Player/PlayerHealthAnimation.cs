//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerHealthAnimation : MonoBehaviour
{
    [SerializeField] private Vector3 _uiScale;
    void Start()
    {
        transform.DOScale(_uiScale, 2f)
            .SetEase(Ease.OutBounce)
            .SetLink(gameObject);
    }

    void Update()
    {
        
    }
}
