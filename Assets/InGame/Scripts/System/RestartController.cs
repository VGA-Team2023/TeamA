using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

/// <summary> ���X�^�[�g���̏��� </summary>
public class RestartController : MonoBehaviour
{
    public static RestartController Instance;

    [SerializeField] private PlayerController _playerController = default;

    private Transform _restartPos = default;
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
    }

    private void Start()
    {
        _restartPos = (GameManager.Instance.GetRespawnTransform().position != Vector3.zero) ? GameManager.Instance.GetRespawnTransform() : _restartPos;
        _sceneName = GameManager.Instance.SceneName;

#if UNITY_EDITOR
        Debug.Log($"GameManagerから読み込んだRespawnTransform:{_restartPos}");
        Debug.Log($"GameManagerから読み込んだSceneName:{_sceneName}");
#endif

        if (_sceneName == SceneManager.GetActiveScene().name)
        {
            _playerController.transform.position = _restartPos.position;
        }
    }

    /// <summary> Player�����S������Ă΂�� </summary>
    public GameObject Restart()
    {
        if (_sceneName != SceneManager.GetActiveScene().name)
        {
            SceneManager.LoadScene(_sceneName);
        }

        //Destroy(_playerObj);
        //_playerObj = Instantiate(_playerPrefab, _restartPos.position, _restartPos.rotation); //Player�̃X�|�[��
        _playerController.transform.position = _restartPos.position;
        return _playerController.gameObject;
    }

    /// <summary> �`�F�b�N�|�C���g��ʉ߂����Ƃ��ɌĂ΂�� </summary>
    /// <param name="pos">�`�F�b�N�|�C���g�̍��W</param>
    public void SetRestartPos(Transform pos, string scene)
    {
        _restartPos = pos;
        _sceneName = scene;
        GameManager.Instance.SetSaveData(new Vector2(_restartPos.position.x, _restartPos.position.y), _sceneName);
    }

    public void SceneTest()
    {
        SceneManager.LoadScene("CheckPontTestScene2");
    }
}