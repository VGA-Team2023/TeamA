using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{
    public static GameManager Instance => _instance;

    public PlayerEnvroment PlayerEnvroment { get; set; }
    public IPlayerRoot PlayerRoot { get; set; }

    private Vector2 _respawnTransform;
    private string _sceneName;
    private static GameManager _instance;

    public Vector2 RespawnTransform => _respawnTransform;
    public string SceneName => _sceneName;

    public GameManager()
    {
        Debug.Log("‚«‚½");
        if (_instance == null) 
        {
            _instance = this;
        }
        _sceneName = SceneManager.GetActiveScene().name;
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