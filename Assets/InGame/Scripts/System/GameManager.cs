using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PlayerEnvroment PlayerEnvroment { get; set; }

    private Vector2 _respawnTransform;
    private string _sceneName;

    public Vector2 RespawnTransform => _respawnTransform;
    public string SceneName => _sceneName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Transform GetRespawnTransform()
    {
        GameObject respawnObject = new GameObject("RespawnObject");
        respawnObject.transform.position = new Vector3(_respawnTransform.x, _respawnTransform.y, 0f);

        return respawnObject.transform;
    }

    public void SetSaveData(Vector2 pos, string name)
    {
        _respawnTransform = pos;
        _sceneName = name;
    }
}