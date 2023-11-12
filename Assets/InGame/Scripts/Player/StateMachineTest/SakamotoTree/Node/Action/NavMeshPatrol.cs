using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
using Random = UnityEngine.Random;

/// <summary>
/// ŽüˆÍ‚ðœpœj‚³‚¹‚éNodeClass
/// </summary>
public class NavMeshPatrol : ActionNode
{
    [Header("œpœj‚·‚é”ÍˆÍ")]
    [SerializeField] private float _patrolRange = 0;
    [Header("–Ú“I’n‚É’…‚¢‚½‚Æ‚«‚ÉŽ~‚Ü‚éŽžŠÔ")]
    [SerializeField] private float _goalStopTime = 0;
    [SerializeField] private float _speed;
    [NonSerialized] private Vector3 _startPosition;
    [NonSerialized] private Vector3 _goalPosition;
    [NonSerialized] private float _countTime;

    protected override void OnExit(Environment env)
    {
        env.MySelfAnim.SetBool("Move", false);
    }

    protected override void OnStart(Environment env)
    {
        _startPosition = env.MySelf.transform.position;
        env.MySelfAnim.SetBool("Move", true);
        env.NavMesh.speed = _speed;
        SelectPosition();
    }

    protected override State OnUpdate(Environment env)
    {
        if (env.MySelf.transform.position.x == _goalPosition.x && env.MySelf.transform.position.z == _goalPosition.z
            && _countTime < _goalStopTime) 
        {
            //–Ú“I’n‚É’…‚¢‚½‚çŽw’è‚µ‚½•b”Ž~‚Ü‚é
            _countTime += Time.deltaTime;
            env.MySelfAnim.SetBool("Move", false);
        }
        else if (env.MySelf.transform.position.x == _goalPosition.x && env.MySelf.transform.position.z == _goalPosition.z) 
        {
            //–Ú“I’n•ÏX
            env.MySelfAnim.SetBool("Move", true);
            SelectPosition();
            _countTime = 0;
        }
       
        env.NavMesh.SetDestination(_goalPosition);
        return State.Running;
    }

    public void SelectPosition() 
    {
        _goalPosition.x = Random.Range(_startPosition.x - _patrolRange, _startPosition.x + _patrolRange);
        _goalPosition.y = _startPosition.y;
        _goalPosition.z = Random.Range(_startPosition.z - _patrolRange, _startPosition.z + _patrolRange);
    }
}