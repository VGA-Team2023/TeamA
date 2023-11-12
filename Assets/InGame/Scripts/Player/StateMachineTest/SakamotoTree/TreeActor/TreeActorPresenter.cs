using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TreeActorPresenter : MonoBehaviour
{
    [SerializeField] private BehaviourTreeRunner _treeRunner;
    [SerializeField] private TreeActorView _treeActorView;

    void Start()
    {
        _treeRunner.ActorStatus.MaxHp.Subscribe(_treeActorView.SetMaxHp).AddTo(this);
        _treeRunner.ActorStatus.CurrentHp.Subscribe(_treeActorView.SetHpCurrent).AddTo(this);
    }
}
