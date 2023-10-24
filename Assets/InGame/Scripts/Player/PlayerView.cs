//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Image _currentWaterImage;
    [SerializeField] private Image _currentHpImage;

    private float _maxWater;
    private float _currentWater;
    private float _maxHp;
    private float _currentHp;

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

    public void SetHpView(float num)
    {
        _currentHp = num;
        AdjustmentHpBar();
    }

    public void SetMaxHpView(float num) 
    {
        _maxHp = num;
    }


    private void AdjustmentHpBar() 
    {
        _currentHpImage.DOFillAmount(_currentHp / _maxHp, 0.5f);
    }

    private void AdjustmentWaterBar()
    {
        Debug.Log(_currentWater / _maxWater);
        _currentWaterImage.DOFillAmount(_currentWater / _maxWater, 0.5f);
    }

}