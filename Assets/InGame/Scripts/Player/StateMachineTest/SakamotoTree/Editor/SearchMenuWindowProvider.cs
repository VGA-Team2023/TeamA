using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;

public class SearchMenuWindowProvider : ScriptableObject, ISearchWindowProvider
{
    private BehaviourTreeView _treeView;
    private EditorWindow _editorWindow;

    public void Init(BehaviourTreeView behaviourTreeView, EditorWindow editorWindow) 
    {
        _treeView = behaviourTreeView;
        _editorWindow = editorWindow;
    }

    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        var entries = new List<SearchTreeEntry>();
        entries.Add(new SearchTreeGroupEntry(new GUIContent("NodeSelect")));

        entries.Add(new SearchTreeGroupEntry(new GUIContent("ActionNode")) { level = 1 });
        var actionTypes = TypeCache.GetTypesDerivedFrom<ActionNode>();
        for(int i = 0; i < actionTypes.Count; i++) 
        {
            entries.Add(new SearchTreeEntry(new GUIContent(actionTypes[i].Name))
            { level = 2, userData = actionTypes[i]});
        }

        entries.Add(new SearchTreeGroupEntry(new GUIContent("ConditionNode")) { level = 1 });
        var ConditionTypes = TypeCache.GetTypesDerivedFrom<ConditionNode>();
        for (int i = 0; i < ConditionTypes.Count; i++) 
        {
            entries.Add(new SearchTreeEntry(new GUIContent(ConditionTypes[i].Name)) 
            { level = 2, userData = ConditionTypes[i] });
        }

        entries.Add(new SearchTreeGroupEntry(new GUIContent("DecoratorNode")) { level = 1 });
        var DecoratorTypes = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
        for (int i = 0; i < DecoratorTypes.Count; i++)
        {
            entries.Add(new SearchTreeEntry(new GUIContent(DecoratorTypes[i].Name))
            { level = 2, userData = DecoratorTypes[i] });
        }

        entries.Add(new SearchTreeGroupEntry(new GUIContent("DescriptionNode")) { level = 1 });
        var DescriptionTypes = TypeCache.GetTypesDerivedFrom<DescriptionNode>();
        for(int i = 0; i < DescriptionTypes.Count; i++) 
        {
            entries.Add(new SearchTreeEntry(new GUIContent(DescriptionTypes[i].Name))
            { level = 2, userData = DescriptionTypes[i] });
        }
            
        return entries;
    }

    public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
    {
        var type = SearchTreeEntry.userData as Type;

        //// マウスの位置にノードを追加
        var worldMousePosition = _editorWindow.rootVisualElement.ChangeCoordinatesTo(_editorWindow.rootVisualElement.parent, context.screenMousePosition - _editorWindow.position.position);
        var localMousePosition = _treeView.contentViewContainer.WorldToLocal(worldMousePosition);

        _treeView.CreateNode(type, new Rect(localMousePosition, new Vector2(100, 100)));
        return true;
    }
}
