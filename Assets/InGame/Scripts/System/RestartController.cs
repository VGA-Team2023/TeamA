using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

/// <summary> ���X�^�[�g���̏��� </summary>
public class RestartController : MonoBehaviour
{
    public static RestartController Instance;

    [SerializeField] private IPlayerRoot _playerController = default;

    private IHealth _playerHealth;
    private Transform _restartPos;
    private string _sceneName = default;
    public Transform ReStartPos => _restartPos;
    public string SceneName => _sceneName;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }

        Instance = this;
        _restartPos = transform;
    }

    private void Start()
    {
        _restartPos = (GameManager.Instance.GetRespawnTransform().position != Vector3.zero) ? GameManager.Instance.GetRespawnTransform() : _restartPos;
        _sceneName = GameManager.Instance.SceneName;
        _playerController = GameManager.Instance.PlayerRoot;
        _playerHealth = _playerController.PlayerHp;

#if UNITY_EDITOR
        Debug.Log($"GameManagerから読み込んだRespawnTransform:{_restartPos}");
        Debug.Log($"GameManagerから読み込んだSceneName:{_sceneName}");
#endif

        if (_sceneName == SceneManager.GetActiveScene().name)
        {
            //_playerController.Envroment.PlayerTransform.position = _restartPos.position;
        }
    }

    /// <summary> Player�����S������Ă΂�� </summary>
    public GameObject Restart()
    {
        if (_sceneName != SceneManager.GetActiveScene().name)
        {
            SceneManager.LoadScene(_sceneName);
        }

        _playerController.Envroment.PlayerTransform.position = _restartPos.position;
        _playerHealth.ApplyHeal(5);
        _playerController.SeachState<IPlayerAttack>()?.RestoreWater();
        return _playerController.Envroment.PlayerTransform.gameObject;
    }

    /// <summary> �`�F�b�N�|�C���g��ʉ߂����Ƃ��ɌĂ΂�� </summary>
    /// <param name="pos">�`�F�b�N�|�C���g�̍��W</param>
    public void SetRestartPos(Transform pos, string scene)
    {
        _restartPos = pos;
        _sceneName = scene;
        GameManager.Instance.SetSaveData(new Vector2(_restartPos.position.x, _restartPos.position.y), _sceneName);
    }
}