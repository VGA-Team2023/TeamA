using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownNode : ActionNode
{
    [SerializeField] private GameObject _enemyDownPrefab;
    private Animator _animator;
    protected override void OnExit(Environment env)
    {
        
    }

    protected override void OnStart(Environment env)
    {
        var enemyObj = Object.Instantiate(_enemyDownPrefab, env.MySelf.transform.position, env.MySelf.transform.rotation);
        Destroy(enemyObj, 2f);
        _animator = enemyObj.GetComponent<Animator>();
    }

    protected override State OnUpdate(Environment env)
    {
        Destroy(env.MySelf, 2f);
        env.MySelf.SetActive(false);
        return State.Running;
    }
}
