using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

/// <summary> �`�F�b�N�|�C���g��ʉ߂���Ƃ��̏��� </summary>
public class CheckPoint : MonoBehaviour
{
    [SerializeField] private float _healHpSize = 0f;

    private string _sceneName;

    private void Start()
    {
        _sceneName = SceneManager.GetActiveScene().name;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerHp>(out var playerHp))
        {
            playerHp.ApplyHeal(_healHpSize); //Player��HP���񕜂���
            RestartController.Instance.SetRestartPos(transform, _sceneName); //���X�^�[�g�̍��W��ς���
#if UNITY_EDITOR
            Debug.Log($"���X�^�[�g���W��{gameObject.transform.position}�ɕύX�B");
#endif
        }
    }
}