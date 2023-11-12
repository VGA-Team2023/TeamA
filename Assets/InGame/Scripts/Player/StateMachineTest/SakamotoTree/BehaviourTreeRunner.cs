using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.IO;
using System;

public class BehaviourTreeRunner : MonoBehaviour, IDamageble
{
    public ActorStatus ActorStatus => _actorStatus;
    public BehaviourTree CloneBehaviourTree => _cloneBehaviour;

    [SerializeField] private BehaviourTree _behaviour;
    [SerializeField] private Animator _conditionAnim;
    private Environment _env = new();
    private BehaviourTree _cloneBehaviour;
    private ActorStatus _actorStatus = new();
    private void Start()
    {
        _cloneBehaviour = BehaviorLoadManager.CloneBehaviorTree(_behaviour, gameObject.name);
        EnvSetUp();
    }

    private void Update()
    {
        _cloneBehaviour.update(_env);
    }

    /// <summary>
    /// behaviorTreeÇ…ìnÇ∑èÓïÒÇSetÇ∑ÇÈ
    /// </summary>
    public void EnvSetUp()
    {
        _env.MySelf = this.gameObject;
        _env.MySelfAnim = GetComponent<Animator>();
        _env.MySelfRb = GetComponent<Rigidbody>();
        _env.NavMesh = GetComponent<NavMeshAgent>();
        _env.ConditionAnim = _conditionAnim;
        //_env.Target = ActorGenerator.PlayerObj;
        _env.ActorStatus = _actorStatus;
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _cloneBehaviour.Nodes.Count; i++) 
        {
            _cloneBehaviour.Nodes[i].Cancel();
        }
    }

    public void ReceiveDamage(float damage, Vector3 myselfPosition)
    {
        _actorStatus.ReceiveDamage(damage);
    }
}
