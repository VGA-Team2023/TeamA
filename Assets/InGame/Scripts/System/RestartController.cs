using UnityEngine;

/// <summary> リスタート時の処理 </summary>
public class RestartController : MonoBehaviour
{
    [SerializeField, Tooltip("PlayerのPrefab")] PlayerController _pController = default;
    [Tooltip("リスタートする座標")]
    Transform _restartPos = default;
    public Transform ReStartPos => _restartPos;

    /// <summary> Playerが死亡したら呼ばれる </summary>
    public void Restart()
    {
        //Playerのインスタンスがあったら消す。シングルトンとの兼ね合い
        Destroy(_pController.gameObject);       
        Instantiate(_pController.gameObject, _restartPos.position, _restartPos.rotation);   //Playerのスポーン
    }


    /// <summary> チェックポイントを通過したときに呼ばれる </summary>
    /// <param name="restartPos">チェックポイントの座標</param>
    public void SetRestartPos(Transform restartPos)
    {
        _restartPos = restartPos;
    }
}
