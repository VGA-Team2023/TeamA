using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
using System.Collections.Generic;
#endif

//���t�@�����Xhttps://docs.unity3d.com/ja/2018.4/ScriptReference/PropertyAttribute.html
public class SceneNameAttribute : PropertyAttribute
{

    public int selectedValue = 0;
    [Tooltip("���݃r���h�������̃A�N�e�B�u�ȃV�[�������\�����邩�ǂ���")]
    public bool enableOnly = true;
    public SceneNameAttribute(bool enableOnly = true)
    {
        this.enableOnly = enableOnly;
    }
}

//PropertyDrawer���p�������N���X��Editor�t�H���_�ɕۑ����邩##if UNITY_EDITOR �` #endif�ň͂܂Ȃ��ƃr���h���ɃG���[���ł�
#if UNITY_EDITOR
//PropertyDrawer���p�������N���X�𗘗p���邱�ƂŁAPropertyAttribute�ɃA�N�Z�X���A�C���X�y�N�^�[�̕\����ύX���邱�Ƃ��ł���B
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
            //Scene�̐���0����������Scene is Empty��\������
            //���t�@�����Xhttps://docs.unity3d.com/ja/2018.4/ScriptReference/EditorGUI.LabelField.html
            EditorGUI.LabelField(position, ObjectNames.NicifyVariableName(property.name), "Scene is Empty");
            return;
        }

        int[] sceneNumbers = new int[sceneNames.Length];

        SetSceneNambers(sceneNumbers, sceneNames);

        if (!string.IsNullOrEmpty(property.stringValue))
            sceneNameAttribute.selectedValue = GetIndex(sceneNames, property.stringValue);

        //���t�@�����Xhttps://docs.unity3d.com/ja/2020.3/ScriptReference/EditorGUI.IntPopup.html
        sceneNameAttribute.selectedValue = EditorGUI.IntPopup(position, label.text, sceneNameAttribute.selectedValue, sceneNames, sceneNumbers);

        property.stringValue = sceneNames[sceneNameAttribute.selectedValue];
    }

    /// <summary>
    /// ���݃A�N�e�B�u�ȃV�[����List�Ɋi�[����Path�̂���Ȃ����O���폜����
    /// </summary>
    /// <returns></returns>
    string[] GetEnabledSceneNames()
    {
        //���݃r���h�������̃A�N�e�B�u�ȃV�[�������\�����邩�ǂ���
        List<EditorBuildSettingsScene> scenes = (sceneNameAttribute.enableOnly ? EditorBuildSettings.scenes.Where(scene => scene.enabled) : EditorBuildSettings.scenes).ToList();
        //HashSet�Ƃ͏d�������I�u�W�F�N�g��ǉ��ł��Ȃ����X�g�N���X
        HashSet<string> sceneNames = new HashSet<string>();
        scenes.ForEach(scene =>
        {
            //Path�̕s�v�ȕ������폜
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