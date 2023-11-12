using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

public class BehaviourTreeView : GraphView
{
    public new class UxmlFactory : UxmlFactory<BehaviourTreeView, UxmlTraits> { }
    private BehaviourTree _tree;
    public Action<NodeView> OnNodeSelected;

    public BehaviourTreeView()
    {
        Insert(0, new GridBackground());

        //Editor上での基本的な操作を実装している
        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>
            ("Assets/InGame/Scripts/Player/StateMachineTest/SakamotoTree/Editor/BehaviorTreeEditor.uss");
        styleSheets.Add(styleSheet);

        Undo.undoRedoPerformed += OnUndoRedo;
    }

    private void OnUndoRedo() 
    {
        PopulateView(_tree);
        AssetDatabase.SaveAssets();
    }

    public void SetEditorWindow(EditorWindow editorWindow)
    {
        var menuWindowProvider = ScriptableObject.CreateInstance<SearchMenuWindowProvider>();
        menuWindowProvider.Init(this, editorWindow);
        nodeCreationRequest += context =>
        {
            Debug.Log("メニューを開く");
            SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), menuWindowProvider);
        };
    }

    private NodeView FindNodeView(Node node)
    {
        return GetNodeByGuid(node.Guid) as NodeView;
    }

    /// <summary>
    /// ノードの見た目を作るための関数
    /// </summary>
    /// <param name="tree"></param>
    internal void PopulateView(BehaviourTree tree)
    {
        this._tree = tree;
        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;
        if (tree.RootNode == null)
        {
            tree.RootNode = tree.CreateNode(typeof(RootNode)) as RootNode;
            EditorUtility.SetDirty(tree);
            AssetDatabase.SaveAssets();
        }
        //グラフにノードを表示する処理
        tree.Nodes.ForEach(n => CreateNodeView(n));
        //グラフにEdgeを表示する処理
        tree.Nodes.ForEach(n =>
        {
            var children = tree.GetChildren(n);
            children.ForEach(c =>
            {
                NodeView parentView = FindNodeView(n);
                NodeView childView = FindNodeView(c);

                Edge edge = parentView.Output.ConnectTo(childView.Input);
                AddElement(edge);
            });
        });
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        //入力ポートと入力ポートがつながらないようにしている
        return ports.ToList().Where(endPort =>
        endPort.direction != startPort.direction &&
        endPort.node != startPort.node).ToList();
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        if (graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(elem =>
            {
                NodeView nodeView = elem as NodeView;
                if (nodeView != null)
                {
                    _tree.DeleteNode(nodeView.Node);
                }

                Edge edge = elem as Edge;
                if (edge != null)
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;
                    _tree.RemoveChild(parentView.Node, childView.Node);
                }

            });
        }

        //ノードでつないだ線を保存できるように
        if (graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                Debug.Log("つないだ");
                NodeView parentView = edge.output.node as NodeView;
                NodeView childView = edge.input.node as NodeView;
                _tree.AddChild(parentView.Node, childView.Node);
            });
        }

        if(graphViewChange.movedElements != null) 
        {
            nodes.ForEach((n) =>
            {
                NodeView view = n as NodeView;
                view.SortChildren();
            });
        }
        return graphViewChange;
    }

    /// <summary>
    ///　右クリックでノードを作成できるように
    /// </summary>
    /// <param name="evt"></param>
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        Debug.Log(contentRect);
        base.BuildContextualMenu(evt);
    }

    public void CreateNode(Type type, Rect grid)
    {
        Node node = _tree.CreateNode(type);
        CreateNodeView(node, grid);
    }
    private void CreateNodeView(Node node)
    {
        NodeView nodeView = new NodeView(node);
        nodeView.OnNodeSelected = OnNodeSelected;
        AddElement(nodeView);
    }

    private void CreateNodeView(Node node, Rect rect)
    {
        NodeView nodeView = new NodeView(node);
        nodeView.SetPosition(rect);
        nodeView.OnNodeSelected = OnNodeSelected;
        AddElement(nodeView);
    }

    public void UpdateNode() 
    {
        nodes.ForEach(n =>
        {
            NodeView view = n as NodeView;
            view.UpdateState();
            view.UpdateScele(view);
        });
    }
}
