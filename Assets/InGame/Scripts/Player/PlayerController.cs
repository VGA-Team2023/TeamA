//日本語対応
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UniRx;

/// <summary>
/// PlayerのRootクラス
/// </summary>
public class PlayerController : MonoBehaviour
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
    }
    #endregion

    private void BindView()
    {
        _playerHp.CurrentHp.Subscribe(_playerView.SetHpView).AddTo(this);
        _playerHp.MaxHp.Subscribe(_playerView.SetMaxHpView).AddTo(this);

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

    void Update()
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
