using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using System.Threading;

public class FadeScript : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [AnimationParameter]
    [SerializeField] private string _fadeOut;
    [AnimationParameter]
    [SerializeField] private string _fadeIn;
    [SerializeField] private Image _fadeImage;
    [SerializeField] private Color _fadeColor;
    private CancellationToken _cancellationToken;
    private bool _isFadeIn = false;

    private void Awake()
    {
        _cancellationToken = this.GetCancellationTokenOnDestroy();
    }

    private void Start()
    {
        if (!_isFadeIn) 
        {
            //FadeIn().Forget();
        }
    }

    public async UniTask FadeOut() 
    {
        _fadeImage.enabled = true;
        _anim.SetTrigger(_fadeOut);
        await UniTask.Delay(2);
        await UniTask.WaitUntil(() => _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f
                                , cancellationToken: _cancellationToken);
        _fadeImage.color = _fadeColor;
    }

    public async UniTask FadeIn() 
    {
        _anim.SetTrigger(_fadeIn);
        await UniTask.WaitUntil(() => _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f
                                , cancellationToken: _cancellationToken);
       // _fadeImage.enabled = false;
    }
}
