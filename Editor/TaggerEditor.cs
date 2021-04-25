using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using SAS.Utilities.Editor;
using EditorUtility = SAS.Utilities.Editor.EditorUtility;

namespace SAS.TagSystem.Editor
{
    [CustomEditor(typeof(Tagger), true)]
    public class TaggerEditor : UnityEditor.Editor
    {
        private ReorderableList _componentTagList;
        private static TagList Instance;
        public static string[] TagList
        {
            get
            {
                if (Instance == null)
                    Instance = Resources.Load("Tag List", typeof(TagList)) as TagList;
                return Instance.tags;
            }
        }

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

                    Rect pos = new Rect(rect.width / 2 + 60, rect.y, rect.width / 2 - 20, rect.height);
                    int id = GUIUtility.GetControlID("SearchableStringDrawer".GetHashCode(), FocusType.Keyboard, pos);
                    EditorUtility.DropDown(id, pos, TagList, Array.IndexOf(TagList, tag.stringValue), selectedIndex => OnTagSelected(component.objectReferenceValue, selectedIndex), ShowTagList);
                }
            };
        }

        public override void OnInspectorGUI()
        {
            _componentTagList.DoLayoutList();
        }

        private void OnTagSelected(UnityEngine.Object targetObject, int index)
        {
            var tagger = ((Component)target).gameObject.GetComponent<Tagger>();
            tagger.SetTag((Component)targetObject, index != -1 ? TagList[index] : string.Empty);
        }

        private void ShowTagList()
        {
            Selection.activeObject = Instance;
        }
    }
}
