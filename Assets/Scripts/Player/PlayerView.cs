//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Image _currentWaterImage;

    private float _maxWater;
    private float _currentWater;

    public void SetCurrentWater(float num)
    {
        _currentWater = num;
        AdjustmentWaterBar();
        Debug.Log($"現在の水の残量は{num}");
    }

    public void SetMaxWater(float num)
    {
        _maxWater = num;
    }

    private void AdjustmentWaterBar()
    {
        _currentWaterImage.DOFillAmount(_currentWater / _maxWater, 0.5f);
    }

}