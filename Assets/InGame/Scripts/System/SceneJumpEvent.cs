//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class SceneJumpEvent : MonoBehaviour
{
    [SceneName]
    [SerializeField] private string _nextScene;
    [SerializeField] private FadeScript _fadeScript;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<IPlayerRoot>(out var playerRoot))
        {
            playerRoot.SeachState<IPlayerSceneMove>().SceneMoveStart();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IPlayerRoot>(out var playerRoot))
        {
            var sceneMoveCs = playerRoot.SeachState<IPlayerSceneMove>();
            sceneMoveCs.SceneMoveStart();
            NextSceneLoad().Forget();
        }
    }

    public async UniTask NextSceneLoad()
    {
        await _fadeScript.FadeOut();
        SceneManager.LoadScene("ForestSceneBoss");
    }
}
