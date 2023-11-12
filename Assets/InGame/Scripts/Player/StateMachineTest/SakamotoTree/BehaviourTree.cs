using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;


[Serializable][CreateAssetMenu()]
public class BehaviourTree : ScriptableObject
{
    public Node RootNode;
    private Node.State _treeState = Node.State.Running;
    [SerializeField] public List<Node> Nodes = new List<Node>();
    private Stack<Node> _nodeStack = new Stack<Node>();

    public Node.State update(Environment env) 
    {
        return RootNode.update(env);
    }

#if UNITY_EDITOR
    public Node CreateNode(Type type) 
    {
        Node node = CreateInstance(type) as Node;
        node.name = type.Name;
        node.Guid = GUID.Generate().ToString();
        Undo.RecordObject(this, "Behaviour Tree (CreateNode)");
        Nodes.Add(node);

        AssetDatabase.AddObjectToAsset(node, this);
        Undo.RegisterCreatedObjectUndo(node, "Behaviour Tree (CreateNode)");
        AssetDatabase.SaveAssets();
        return node;
    }

    public void DeleteNode(Node node) 
    {
        Undo.RecordObject(this, "Behaviour Tree (DeleteNode)");
        Nodes.Remove(node);
        _nodeStack.Push(node);
        //AssetDatabase.RemoveObjectFromAsset(node);
        Undo.DestroyObjectImmediate(node);
        AssetDatabase.SaveAssets();
    }

    /// <summary>
    /// ノードつないだ時に参照を渡す処理
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="child"></param>
    public void AddChild(Node parent, Node child) 
    {
        ConditionNode conditionNode = parent as ConditionNode;
        if (conditionNode) 
        {
            Undo.RecordObject(conditionNode, "Behaviour Tree (AddChild)");
            conditionNode.NodeChildren.Add(child);
            EditorUtility.SetDirty(conditionNode);
        }

        RootNode rootNode = parent as RootNode;
        if (rootNode) 
        {
            Undo.RecordObject(rootNode, "Behaviour Tree (AddChild)");
            rootNode.Child = child;
            EditorUtility.SetDirty(rootNode);
        }

        DecoratorNode decoratorNode = parent as DecoratorNode;
        if (decoratorNode) 
        {
            Undo.RecordObject(decoratorNode, "Behaviour Tree (AddChild)");
            decoratorNode.Child = child;
            EditorUtility.SetDirty(decoratorNode);
        }
    }

    public void RemoveChild(Node parent, Node child) 
    {
        ConditionNode conditionNode = parent as ConditionNode;
        if (conditionNode)
        {
            Undo.RecordObject(conditionNode, "Behaviour Tree (RemoveChild)");
            conditionNode.NodeChildren.Remove(child);
            EditorUtility.SetDirty(conditionNode);
        }


        RootNode rootNode = parent as RootNode;
        if (rootNode)
        {
            Undo.RecordObject(rootNode, "Behaviour Tree (RemoveChild)");
            rootNode.Child = null;
            EditorUtility.SetDirty(rootNode);
        }


        DecoratorNode decoratorNode = parent as DecoratorNode;
        if (decoratorNode)
        {
            Undo.RecordObject(decoratorNode, "Behaviour Tree (RemoveChild)");
            decoratorNode.Child = null;
            EditorUtility.SetDirty(decoratorNode);
        }
    }

    public List<Node> GetChildren(Node parent) 
    {
        //後々Nodeの種類を増やすことを考慮して作成
        List<Node> children = new();

        ConditionNode conditionNode = parent as ConditionNode;
        if (conditionNode)
        {
            return conditionNode.NodeChildren;
        }

        RootNode rootNode = parent as RootNode;
        if (rootNode && rootNode.Child != null)
        {
            children.Add(rootNode.Child);
        }

        DecoratorNode decoratorNode = parent as DecoratorNode;
        if (decoratorNode && decoratorNode.Child != null) 
        {
            children.Add(decoratorNode.Child);
        }

        return children;
    }
#endif

    //public BehaviourTree Clone() 
    //{
    //    BehaviourTree tree = Instantiate(this);
    //    tree.RootNode = tree.RootNode.Clone();
    //    tree
    //}
}
