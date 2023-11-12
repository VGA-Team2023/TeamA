using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// BehaviourTreeNode内でデータを共有するためのクラス
/// </summary>
public class Environment
{
    public ActorStateType ActorStateType => _actorStateType;
    public GameObject MySelf;
    public Rigidbody MySelfRb;
    public Animator MySelfAnim;
    public Animator ConditionAnim;
    public Transform BulletInsPos;
    public NavMeshAgent NavMesh;
    public GameObject Target;
    public ActorStatus ActorStatus;

    private ActorStateType _actorStateType;

    public void AddState(ActorStateType state)
    {
        _actorStateType |= state;
    }
    public void RemoveState(ActorStateType state)
    {
        _actorStateType &= ~state;
    }
}

[System.Serializable]
[System.Flags]
public enum ActorStateType
{
    None = 1,
    FollowingPlayer = 2,
    Attack = 4,
}
