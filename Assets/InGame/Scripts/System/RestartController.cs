using UnityEngine;
using Action2D;

/// <summary> リスタート時の処理 </summary>
public class RestartController : MonoBehaviour
{
    [SerializeField, Tooltip("PlayerのPrefab")] PlayerController _pController = default;
    [SerializeField] GameObject _scenePlayer;
    [SerializeField] GameObject _playerPrefab;
    [Tooltip("リスタートする座標")]
    Transform _restartPos = default;
    public Transform ReStartPos => _restartPos;
    private GameObject _playerObj;

    private void Start()
    {
        _playerObj = _scenePlayer;
    }

    /// <summary> Playerが死亡したら呼ばれる </summary>
    public GameObject Restart()
    {
        //Playerのインスタンスがあったら消す。シングルトンとの兼ね合い
        Destroy(_playerObj);       
        _playerObj = Instantiate(_playerPrefab, _restartPos.position, _restartPos.rotation);   //Playerのスポーン
        _pController = _playerObj.GetComponentInChildren<PlayerController>();
        return _playerObj;
    }


    /// <summary> チェックポイントを通過したときに呼ばれる </summary>
    /// <param name="restartPos">チェックポイントの座標</param>
    public void SetRestartPos(Transform restartPos)
    {
        _restartPos = restartPos;
    }
}
