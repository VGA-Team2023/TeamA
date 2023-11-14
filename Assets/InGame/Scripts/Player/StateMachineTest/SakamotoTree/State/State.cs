//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

public abstract class StateBase : ScriptableObject
{
    public enum State
    {
        Running,
        Failure,
        Success
    }
    [NonSerialized] private State sendState = State.Failure;
    [NonSerialized] public State CurrentState = State.Running;
    [NonSerialized] public bool Started = false;
    public string Guid;
    [Header("Nodeの名前")]
    public string TitleName;
    public Vector2 Position;
    public Vector3 NodeScale = new Vector3(100, 100, 100);
    [NonSerialized] protected CancellationTokenSource _token;
    [Header("説明")]
    [TextArea] public string Description;

    public StateBase()
    {
        _token = new CancellationTokenSource();
    }

    public State update(Environment env)
    {
        if (!Started)
        {
            OnStart(env);
            Started = true;
        }

        CurrentState = OnUpdate(env);

        if (CurrentState == State.Failure || CurrentState == State.Success)
        {
            OnExit(env);
            Started = false;
        }

        return CurrentState;
    }

    protected abstract void OnStart(Environment env);
    protected abstract void OnExit(Environment env);
    protected abstract State OnUpdate(Environment env);

    public void Cancel()
    {
        _token.Cancel();
    }

    public StateBase Clone()
    {
        StateBase state = Instantiate(this);
        return state;
    }
}
