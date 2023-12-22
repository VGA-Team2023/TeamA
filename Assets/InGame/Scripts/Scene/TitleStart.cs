using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleStart : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Image _fadePanel;
    [SerializeField] private float _duration = 1f;
    [SerializeField, SceneName()] private string _sceneName;

    private bool _isStart = false;

    private void Start()
    {
        InputProvider.Instance.SetEnterInput(InputProvider.InputType.Jump, OnStartGame);
    }

    public async void OnStartGame()
    {
        if (_isStart)
        {
            return;
        }

        _isStart = true;
        _animator.Play("TitleClick");
        CriAudioManager.Instance.SE.Play("CueSheet_0", "SE_UI_startbotann");
        await FadeOut();
        LoadScene(_sceneName);
    }

    private async UniTask FadeOut()
    {
        _fadePanel.gameObject.SetActive(true);

        await _fadePanel.DOFade(1f, _duration)
            .SetEase(Ease.Linear)
            .AsyncWaitForCompletion();
    }

    private void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}