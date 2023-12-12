using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

/// <summary> �`�F�b�N�|�C���g��ʉ߂���Ƃ��̏��� </summary>
public class CheckPoint : MonoBehaviour
{
    [SerializeField] private float _healHpSize = 0f;
    [SerializeField] private SpriteRenderer _heartSprite;
    [Tooltip("ハートをフェードする時間")] float _fadeTime = 1f;

    private string _sceneName;

    private void Start()
    {
        _sceneName = SceneManager.GetActiveScene().name;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerHp>(out var playerHp))
        {
            var playerRoot = collision.GetComponent<IPlayerRoot>();
            playerRoot.SeachState<PlayerAttack>().RestoreWater();

            _heartSprite.transform.DOScale(0f, _fadeTime)
                .SetEase(Ease.InBack)
                .SetLink(gameObject);
            playerHp.ApplyHeal(_healHpSize); //Player��HP���񕜂���
            RestartController.Instance.SetRestartPos(transform, _sceneName); //���X�^�[�g�̍��W��ς���
#if UNITY_EDITOR
            Debug.Log($"���X�^�[�g���W��{gameObject.transform.position}�ɕύX�B");
#endif
        }
    }
}