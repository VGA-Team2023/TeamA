using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public static class BehaviorLoadManager
{
    private static bool _isDebugBool = false;
    private static int _nodeCount = 0;
    private static BehaviourTree _cloneBehaviour;


    private static void Init()
    {
        _nodeCount = 0;
        _cloneBehaviour = null;
        _isDebugBool = false;
    }

    /// <summary>
    /// behaviorTreeをクローンする
    /// メモ：2人の敵が同じScriptableObjを参照していた場合Dataが共有してしまうのでクローン処理を行っている
    /// </summary>
    public static BehaviourTree CloneBehaviorTree(BehaviourTree behaviour, string objName)
    {
        Init();
        //_cloneBehaviour = ScriptableObject.CreateInstance<BehaviourTree>();
        _cloneBehaviour = ScriptableObject.Instantiate(behaviour);
        _cloneBehaviour.Nodes.Clear();
        _cloneBehaviour.RootNode = CloneNode(behaviour.RootNode);
        return _cloneBehaviour;
    }

    /// <summary>
    /// NodeをCloneする
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private static Node CloneNode(Node node)
    {
        if (!node) return null;

        if (node is RootNode)
        {
            RootNode rootNode = node as RootNode;
            var cloneRootNode = Clone(rootNode);
            cloneRootNode.Child = CloneNode(rootNode.Child);
            node = cloneRootNode;
            _cloneBehaviour.Nodes.Add(cloneRootNode.Child);
        }
        else if (node is ActionNode)
        {
            ActionNode actionNode = node as ActionNode;
            var cloneActionNode = Clone(actionNode);
            _cloneBehaviour.Nodes.Add(cloneActionNode);
            return cloneActionNode;
        }
        else if (node is ConditionNode)
        {
            ConditionNode conditionNode = node as ConditionNode;
            ConditionNode cloneCondiitonNode = Clone(conditionNode);
            _cloneBehaviour.Nodes.Add(cloneCondiitonNode);
            List<Node> nodeChildren = new();
            for (int i = 0; i < conditionNode.NodeChildren.Count; i++)
            {
                nodeChildren.Add(CloneNode(conditionNode.NodeChildren[i]));
            }

            if (cloneCondiitonNode.NodeChildren != null)
            {
                cloneCondiitonNode.NodeChildren = nodeChildren;
            }
            return cloneCondiitonNode;
        }
        else if (node is DecoratorNode)
        {
            DecoratorNode decoratorNode = node as DecoratorNode;
            DecoratorNode cloneDecoratorNode = Clone(decoratorNode);
            _cloneBehaviour.Nodes.Add(cloneDecoratorNode);
            if (decoratorNode.Child)
            {
                cloneDecoratorNode.Child = CloneNode(decoratorNode.Child);
            }
            return cloneDecoratorNode;
        }

        return node;
    }

    private static T Clone<T>(T So) where T : ScriptableObject
    {
        So = ScriptableObject.Instantiate(So);
        return So;
    }
}
