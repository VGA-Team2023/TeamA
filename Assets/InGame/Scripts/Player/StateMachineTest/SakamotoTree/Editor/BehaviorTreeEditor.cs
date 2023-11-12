using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using System;


public class BehaviorTreeEditor : EditorWindow
{
    private BehaviourTreeView _treeView;
    private InspectorView _inspectorView;

    [MenuItem("BehaviorTreeEditor/ Editor")]
    public static void OpenWindow()
    {
        BehaviorTreeEditor wnd = GetWindow<BehaviorTreeEditor>();
        wnd.titleContent = new GUIContent("BehaviorTreeEditor");
    }

    [OnOpenAsset(0)]
    public static bool OnBaseGraphOpened(int instanceID, int line)
    {
        var asset = EditorUtility.InstanceIDToObject(instanceID) as BehaviourTree;

        if (asset == null) return false;

        var window = GetWindow<BehaviorTreeEditor>();
        window.titleContent = new GUIContent("BehaviorTreeEditor");
        BehaviourTree tree = Selection.activeObject as BehaviourTree;
        window.RefarenceSetView(tree);
        return true;
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>
            ("Assets/InGame/Scripts/Player/StateMachineTest/SakamotoTree/Editor/BehaviorTreeEditor.uxml");
        visualTree.CloneTree(root);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>
            ("Assets/InGame/Scripts/Player/StateMachineTest/SakamotoTree/Editor/BehaviorTreeEditor.uss");
        root.styleSheets.Add(styleSheet);

        _treeView = root.Q<BehaviourTreeView>();
        _treeView.SetEditorWindow(this);
        _treeView.OnNodeSelected = OnNodeSelectionChanged;
        _inspectorView = root.Q<InspectorView>();
    }

    private void OnNodeSelectionChanged(NodeView nodeView)
    {
        _inspectorView.UpdateSelection(nodeView);
    }

    private void OnSelectionChange()
    {
        BehaviourTree tree = Selection.activeObject as BehaviourTree;

        if (!tree)
        {
            if (Selection.activeGameObject)
            {
                if (Selection.activeGameObject.TryGetComponent<BehaviourTreeRunner>(out var runner)) 
                {
                    tree = runner.CloneBehaviourTree;
                }
            }
        }

        if (Application.isPlaying)
        {
            if (tree)
            {
                _treeView.PopulateView(tree);
            }
        }
        else
        {
            if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
            {
                _treeView.PopulateView(tree);
            }
        }

    }

    public void RefarenceSetView(BehaviourTree tree)
    {
        _treeView.PopulateView(tree);
    }

    private void OnInspectorUpdate()
    {
        _treeView?.UpdateNode();
    }
}