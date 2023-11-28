using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
using System.Collections.Generic;
#endif

//リファレンスhttps://docs.unity3d.com/ja/2018.4/ScriptReference/PropertyAttribute.html
public class SceneNameAttribute : PropertyAttribute
{

    public int selectedValue = 0;
    [Tooltip("現在ビルドした中のアクティブなシーンだけ表示するかどうか")]
    public bool enableOnly = true;
    public SceneNameAttribute(bool enableOnly = true)
    {
        this.enableOnly = enableOnly;
    }
}

//PropertyDrawerを継承したクラスはEditorフォルダに保存するか##if UNITY_EDITOR ～ #endifで囲まないとビルド時にエラーがでる
#if UNITY_EDITOR
//PropertyDrawerを継承したクラスを利用することで、PropertyAttributeにアクセスし、インスペクターの表示を変更することができる。
[CustomPropertyDrawer(typeof(SceneNameAttribute))]
public class SceneNameDrawer : PropertyDrawer
{
    private SceneNameAttribute sceneNameAttribute
    {
        get
        {
            return (SceneNameAttribute)attribute;
        }
    }


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        string[] sceneNames = GetEnabledSceneNames();

        if (sceneNames.Length == 0)
        {
            //Sceneの数が0だった時にScene is Emptyを表示する
            //リファレンスhttps://docs.unity3d.com/ja/2018.4/ScriptReference/EditorGUI.LabelField.html
            EditorGUI.LabelField(position, ObjectNames.NicifyVariableName(property.name), "Scene is Empty");
            return;
        }

        int[] sceneNumbers = new int[sceneNames.Length];

        SetSceneNambers(sceneNumbers, sceneNames);

        if (!string.IsNullOrEmpty(property.stringValue))
            sceneNameAttribute.selectedValue = GetIndex(sceneNames, property.stringValue);

        //リファレンスhttps://docs.unity3d.com/ja/2020.3/ScriptReference/EditorGUI.IntPopup.html
        sceneNameAttribute.selectedValue = EditorGUI.IntPopup(position, label.text, sceneNameAttribute.selectedValue, sceneNames, sceneNumbers);

        property.stringValue = sceneNames[sceneNameAttribute.selectedValue];
    }

    /// <summary>
    /// 現在アクティブなシーンをListに格納するPathのいらない名前も削除する
    /// </summary>
    /// <returns></returns>
    string[] GetEnabledSceneNames()
    {
        //現在ビルドした中のアクティブなシーンだけ表示するかどうか
        List<EditorBuildSettingsScene> scenes = (sceneNameAttribute.enableOnly ? EditorBuildSettings.scenes.Where(scene => scene.enabled) : EditorBuildSettings.scenes).ToList();
        //HashSetとは重複したオブジェクトを追加できないリストクラス
        HashSet<string> sceneNames = new HashSet<string>();
        scenes.ForEach(scene =>
        {
            //Pathの不要な部分を削除
            sceneNames.Add(scene.path.Substring(scene.path.LastIndexOf("/") + 1).Replace(".unity", string.Empty));
        });
        return sceneNames.ToArray();
    }

    void SetSceneNambers(int[] sceneNumbers, string[] sceneNames)
    {
        for (int i = 0; i < sceneNames.Length; i++)
        {
            sceneNumbers[i] = i;
        }
    }

    int GetIndex(string[] sceneNames, string sceneName)
    {
        int result = 0;
        for (int i = 0; i < sceneNames.Length; i++)
        {
            if (sceneName == sceneNames[i])
            {
                result = i;
                break;
            }
        }
        return result;
    }
}
#endif