using SAS.TagSystem;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace SAS.TagSystemEditor
{
    [CustomEditor(typeof(Tagger), true)]
    public class TaggerEditor : Editor
    {
        private ReorderableList _componentTagList;

        private void OnEnable()
        {
            _componentTagList = new ReorderableList(serializedObject, serializedObject.FindProperty("m_Tags"), true, true, false, false);
            _componentTagList.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, "Tagged Component List");
            };

            _componentTagList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var componentTag = serializedObject.FindProperty("m_Tags");
                if (componentTag.arraySize > 0)
                {
                    var component = componentTag.GetArrayElementAtIndex(index).FindPropertyRelative("m_Component");
                    var tag = componentTag.GetArrayElementAtIndex(index).FindPropertyRelative("m_Value");
                    var oldValue = tag.stringValue;
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUI.ObjectField(new Rect(rect.x + 5, rect.y, rect.width / 2, rect.height), component.objectReferenceValue, typeof(Component), false);
                  
                    EditorGUI.EndDisabledGroup();
                    if (component.objectReferenceValue is MonoBehaviour)
                    {
                        EditorGUI.BeginDisabledGroup(true);
                        EditorGUI.LabelField(new Rect(rect.width / 2 + 60, rect.y, rect.width / 2, rect.height), tag.stringValue);
                        EditorGUI.EndDisabledGroup();
                    }
                    else
                    {
                        var value = EditorGUI.DelayedTextField(new Rect(rect.width / 2 + 60, rect.y, rect.width / 2, rect.height), tag.stringValue);
                        if (value != tag.stringValue)
                        {
                            tag.stringValue = value;
                            tag.serializedObject.ApplyModifiedProperties();
                        }
                    }
                }
            };
        }

        public override void OnInspectorGUI()
        {
            _componentTagList.DoLayoutList();
        }
    }
}
