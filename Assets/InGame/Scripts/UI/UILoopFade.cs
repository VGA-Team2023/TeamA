using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UILoopFade : MonoBehaviour
{
    private Image _image;

    [SerializeField] private float _duration = 1.0f;
    [SerializeField] private float _delayTime = 1.0f;

    private void Start()
    {
        _image = GetComponent<Image>();

        StartFadeLoop();
    }

    private async void StartFadeLoop()
    {
        while (true)
        {
            await FadeImage(0, 1);

            // 一定時間待機
            await UniTask.Delay((int)(_delayTime * 1000));

            await FadeImage(1, 0);

            await UniTask.Delay((int)(_delayTime * 1000));
        }
    }

    private async UniTask FadeImage(float startAlpha, float endAlpha)
    {
        await _image.DOFade(endAlpha, _duration)
            .SetEase(Ease.Linear)
            .AsyncWaitForCompletion();
    }
}