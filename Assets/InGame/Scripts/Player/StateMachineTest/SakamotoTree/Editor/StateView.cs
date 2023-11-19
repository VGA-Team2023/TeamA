//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using System;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor;
public class StateView : UnityEditor.Experimental.GraphView.Node
{
    public Action<StateView> OnNodeSelected;
    public Node Node;
    public Port Input;
    public Port Output;

    public StateView(Node node) : base("Assets/InGame/Scripts/Player/StateMachineTest/SakamotoTree/Editor/NodeView.uxml")
    {
        Node = node;
        viewDataKey = node.Guid;

        style.left = node.Position.x;
        style.top = node.Position.y;
        style.width = node.NodeScale.x;
        style.height = node.NodeScale.y;
        node.TitleName = node.name;

        capabilities |= Capabilities.Resizable;
        Debug.Log(IsResizable());

        if (Node.NodeScale == Vector3.zero)
        {
            // Node.NodeScale = Vector3.one;
        }

        CreateInputPorts();
        CreateOutputPorts();
        SetUpNodeDesign();
        this.AddManipulator(new ContextualMenuManipulator(BuildContextualMenu));

        Label titleLabel = this.Q<Label>("title-label");
        titleLabel.bindingPath = "TitleName";
        titleLabel.Bind(new SerializedObject(node));

        Label descriptionLabel = this.Q<Label>("description");
        descriptionLabel.bindingPath = "Description";
        descriptionLabel.Bind(new SerializedObject(node));
    }

    /// <summary>
    ///Input用のPortを作成する
    /// </summary>
    private void CreateInputPorts()
    {
        if (Node is ActionNode)
        {
            Input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if (Node is ConditionNode)
        {
            Input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if (Node is DecoratorNode)
        {
            Input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
        }

        if (Input != null)
        {
            Input.portName = "";
            Input.portColor = Color.red;
            Input.style.flexDirection = FlexDirection.Column;
            inputContainer.Add(Input);
        }
    }

    /// <summary>
    /// Output用のPortを作成する
    /// </summary>
    private void CreateOutputPorts()
    {
        //それぞれのOutputPortを作成
        if (Node is ConditionNode)
        {
            Output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
        }
        else if (Node is RootNode)
        {
            Output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
        }
        else if (Node is DecoratorNode)
        {
            Output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
        }

        if (Output != null)
        {
            Output.portName = "";
            Output.style.flexDirection = FlexDirection.ColumnReverse;
            outputContainer.Add(Output);
        }
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        Undo.RecordObject(Node, "Behaviour Tree (Set Position");
        Node.Position.x = newPos.xMin;
        Node.Position.y = newPos.yMin;
        EditorUtility.SetDirty(Node);
    }

    private void SetUpNodeDesign()
    {
        if (Node is ActionNode)
        {
            AddToClassList("action");
        }
        else if (Node is ConditionNode)
        {
            AddToClassList("composite");
        }
        else if (Node is DecoratorNode)
        {
            AddToClassList("decorator");
        }
        else if (Node is RootNode)
        {
            AddToClassList("root");
        }
        else if (Node is StickyNoteNode)
        {
            AddToClassList("notePad");
        }
        else if (Node is GroupNode)
        {
            AddToClassList("group");
            this.layer = -1000;
        }
    }

    public override void OnSelected()
    {
        base.OnSelected();
        if (OnNodeSelected != null)
        {
            OnNodeSelected.Invoke(this);
        }
    }

    public void SortChildren()
    {
        ConditionNode condition = Node as ConditionNode;
        if (condition)
        {
            condition.NodeChildren.Sort(SortChildren);
        }
    }

    private int SortChildren(Node left, Node right)
    {
        return left.Position.x < right.Position.x ? -1 : 1;
    }

    public void UpdateState()
    {
        RemoveFromClassList("running");
        RemoveFromClassList("failure");
        RemoveFromClassList("success");

        switch (Node.CurrentState)
        {
            case Node.State.Running:
                if (Node.Started)
                {
                    AddToClassList("running");
                }
                break;
            case Node.State.Failure:
                AddToClassList("failure");
                break;
            case Node.State.Success:
                AddToClassList("success");
                break;
        }
    }

    public void UpdateScele(NodeView node)
    {
        Node.NodeScale = new Vector2(node.style.width.value.value, node.style.height.value.value);
    }
}
