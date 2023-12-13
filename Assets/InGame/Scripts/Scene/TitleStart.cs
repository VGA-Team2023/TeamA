using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleStart : MonoBehaviour
{
    [SerializeField] private Image _fadePanel;
    [SerializeField] private float _duration = 1f;
    [SerializeField,SceneName()] private string _sceneName;
    
    public async void OnStartGame()
    {
        CriAudioManager.Instance.SE.Play("CueSheet_0","SE_UI_startbotann");
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
