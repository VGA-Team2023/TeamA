//日本語対応
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UniRx;
using Action2D;

/// <summary>
/// PlayerのRootクラス
/// </summary>
public class PlayerController : MonoBehaviour, IPlayerRoot
{
    [SerializeReference, SubclassSelector]
    private List<IPlayerState> _playerStateList = new List<IPlayerState>();
    [SerializeField] private PlayerAnimation _playerAnim;
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private PlayerHp _playerHp;

    private PlayerEnvroment _playerEnvroment;
    private CancellationToken _token;

    void Start()
    {
        _token = this.GetCancellationTokenOnDestroy();
        SetUp();
        BindView();
    }

    #region SetUp
    public void SetUp()
    {
        _playerAnim.SetUp(_token);
        _playerHp.SetUp();
        SetUpEnv();
        SetUpState();
    }

    private void SetUpState()
    {
        for (int i = 0; i < _playerStateList.Count; i++)
        {
            _playerStateList[i].SetUp(_playerEnvroment, _token);
        }
    }

    private void SetUpEnv()
    {
        _playerEnvroment = new PlayerEnvroment(transform, _playerAnim);
        GameManager.Instance.PlayerEnvroment = _playerEnvroment;
    }
    #endregion

    /// <summary>
    /// ViewとModleの結び付け
    /// </summary>
    private void BindView()
    {
        _playerHp.MaxHp.Subscribe(_playerView.SetMaxHpView).AddTo(this);
        _playerHp.CurrentHp.Subscribe(_playerView.SetHpView).AddTo(this);

        for (int i = 0; i < _playerStateList.Count; i++)
        {
            if (_playerStateList[i] is IPlayerAttack)
            {
                var attack = _playerStateList[i] as IPlayerAttack;

                attack.MaxWaterNum.Subscribe(_playerView.SetMaxWater).AddTo(this);
                attack.CurrentWaterNum.Subscribe(_playerView.SetCurrentWater).AddTo(this);
            }
        }
    }

    /// <summary>
    /// PlayerStateを検索して返す
    /// </summary>
    /// <typeparam name="T">検索されたState</typeparam>
    /// <returns></returns>
    public T SeachState<T>() where T : class
    {
        for (int i = 0; i < _playerStateList.Count; i++) 
        {
            if (_playerStateList[i] is T) 
            {
                return _playerStateList[i] as T;
            }
        }
        Debug.LogError("指定されたステートが見つかりませんでした");
        return default;
    }

    private void Update()
    {
        for (int i = 0; i < _playerStateList.Count; i++)
        {
            _playerStateList[i].Update();
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < _playerStateList.Count; i++)
        {
            _playerStateList[i].FixedUpdate();
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _playerStateList.Count; i++)
        {
            _playerStateList[i].Dispose();
        }
    }
}
