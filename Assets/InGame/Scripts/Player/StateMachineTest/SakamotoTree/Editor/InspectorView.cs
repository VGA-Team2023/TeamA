using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor;

public class InspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<InspectorView, UxmlTraits> { }
    Editor editor;
    public InspectorView() 
    {

    }

    internal void UpdateSelection(NodeView nodeView)
    {
        Clear();
        UnityEngine.Object.DestroyImmediate(editor);
        editor = Editor.CreateEditor(nodeView.Node);
        IMGUIContainer container = new IMGUIContainer(() => 
        {
            if (editor.target) 
            {
                editor.OnInspectorGUI();
            }
        });
        Add(container);
    }
}
