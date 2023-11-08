//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class MainStageScene : MonoBehaviour
{
    [SerializeField] private GameObject _playerObj;
    [SerializeField] private RestartController _restartController;

    private void Awake()
    {
        _playerObj.GetComponent<IHealth>().OnDead.Subscribe(_ => _restartController.Restart()).AddTo(gameObject);
    }
}