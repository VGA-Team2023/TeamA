//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Image _currentWaterImage;
    [SerializeField] private Transform _activeHpInsPos;
    [SerializeField] private Transform _withoutHpInsPos;
    [SerializeField] private GameObject _activeHpObj;
    [SerializeField] private GameObject _withoutHpObj;

    private List<Image> _currentHpImage = new List<Image>();
    private List<Image> _maxHpImage = new List<Image>();
    private float _maxWater;
    private float _currentWater;
    private float _maxHp;
    private int _currentHp;

    public void SetCurrentWater(float num)
    {
        _currentWater = num;
        AdjustmentWaterBar();
        //Debug.Log($"現在の水の残量は{num}");
    }

    public void SetMaxWater(float num)
    {
        _maxWater = num;
    }

    public void SetHpView(float num)
    {
        _currentHp = (int)num;
        for (int i = 0; i < _currentHp; i++) 
        {
            _currentHpImage[i].enabled = true;
        }

        for (int i = _currentHp; i < _currentHpImage.Count; i++) 
        {
            _currentHpImage[i].enabled = false;
        }
    }

    public void SetMaxHpView(float num) 
    {
        num = (int)num;

        //減った時
        if (num < _maxHp) 
        {
            //画像を更新
            for(int i = 0; i < num; i++)
            {
                _maxHpImage[i].enabled = false;
                _currentHpImage[i].enabled = false;
            }
        }

        //増えたとき
        if (_maxHp < num) 
        {
            //アクティブにするインスタンスがないとき
            if (_maxHpImage.Count < num) 
            {
                var sum = num - _maxHpImage.Count;

                //増えた数分プールに増やす
                for (int i = 0; i < sum; i++) 
                {
                    _maxHpImage.Add(Instantiate(_withoutHpObj, _withoutHpInsPos).GetComponent<Image>());
                    _currentHpImage.Add(Instantiate(_activeHpObj, _activeHpInsPos).GetComponent<Image>());
                }
            }

            for (int i = 0; i < num; i++) 
            {
                _maxHpImage[i].enabled = true;
                _currentHpImage[i].enabled = true;
            }
        }

        _maxHp = num;
    }

    private void AdjustmentWaterBar()
    {
        _currentWaterImage.DOFillAmount(_currentWater / _maxWater, 0.5f);
    }

}