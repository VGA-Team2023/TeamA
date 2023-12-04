using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

[CustomPropertyDrawer(typeof(AnimationParameterAttribute))]
public class AnimationParameterPropertyDrawer : PropertyDrawer
{
    private int _index = -1;
    private GUIContent[] _parameterNames = default;
    private bool isSuccessSetUp = default;
    private string[] _strings;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var oldstrings = _strings;
        _strings = GetElements(property);
        if (_index == -1 || !_strings.Equals(oldstrings) || !isSuccessSetUp)
        {
            isSuccessSetUp = SetUp(position, property, label);
        }
        if (isSuccessSetUp)
        {
            int oldIndex = _index;
            _index = EditorGUI.Popup(position, label, _index, _parameterNames);

            if (oldIndex != _index)
            {
                property.stringValue = _parameterNames[_index].text;
            }
        }
    }

    private bool SetUp(Rect position, SerializedProperty property, GUIContent label)
    {
        // �A�j���[�^�[���擾
        Animator animator = (property.serializedObject.targetObject as Component).GetComponent<Animator>();
        if (animator == null)
        {
            EditorGUI.LabelField(position, "AnimationParameter�������g�p����Ă��܂����AAnimatorComponent���A�^�b�`����Ă��܂���");
            return false; // �擾�Ɏ��s�����珈�����I������B
        }

        // �A�j���[�V�����R���g���[�����擾
        AnimatorController animatorController = animator.runtimeAnimatorController as AnimatorController;
        if (animatorController == null)
        {
            EditorGUI.LabelField(position, "AnimationParameter�������g�p����Ă��܂����AAnimatorController���A�T�C������Ă��܂���B");
            return false;
        }

        // �p�����[�^���̔z����쐬
        string[] parameterNames = animatorController.parameters.Select(p => p.name).ToArray();
        if (parameterNames.Length == 0)
        {
            EditorGUI.LabelField(position, "AnimationParameter�������g�p����Ă��܂����Aparameter ������ݒ肳��Ă��܂���B");
            return false;
        }

        // GUI�ɕ`�悷�邽�߂̃R���e���g���쐬
        _parameterNames = new GUIContent[parameterNames.Length];

        // �R���e���g�ɖ��O��ݒ肷��B
        for (int i = 0; i < parameterNames.Length; i++)
        {
            _parameterNames[i] = new GUIContent($"{parameterNames[i]}");
        }

        // �Đݒ�p�v���O����
        if (!string.IsNullOrEmpty(property.stringValue))
        {
            bool sceneNameFound = false;
            for (int i = 0; i < _parameterNames.Length; i++)
            {
                if (_parameterNames[i].text == property.stringValue)
                {
                    _index = i;
                    sceneNameFound = true;
                    break;
                }
            }
            if (!sceneNameFound)
                _index = 0;
        }
        else _index = 0;

        // �v���p�e�B�ɒl��ݒ肷��B
        property.stringValue = _parameterNames[_index].text;

        return true;
    }

    private string[] GetElements(SerializedProperty property)
    {
        // �A�j���[�^�[���擾
        Animator animator = (property.serializedObject.targetObject as Component).GetComponent<Animator>();
        if (animator == null)
        {
            return new string[0];
        }

        // �A�j���[�V�����R���g���[�����擾
        AnimatorController animatorController = animator.runtimeAnimatorController as AnimatorController;
        if (animatorController == null)
        {
            return new string[0];
        }

        // �p�����[�^���̔z����쐬
        return animatorController.parameters.Select(p => p.name).ToArray();
    }
}
