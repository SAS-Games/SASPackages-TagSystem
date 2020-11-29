using SAS.TagSystem;
using UnityEditor;
using UnityEngine;

namespace SAS.TagSystemEditor
{
    [CustomEditor(typeof(Component), true)]
    public class ComponentTagEditor : Editor
    {
        private  TagList _tagList;
        public override void OnInspectorGUI()
        {
            var self = (Component)target;
            var tagger = ((Component)target).gameObject.GetComponent<Tagger>();
            
            var tag = tagger?.GetTag((Component)serializedObject.targetObject);
            EditorGUILayout.BeginHorizontal();
            if (tag != null)
            {
                if (_tagList == null)
                {
                    tag = EditorGUILayout.DelayedTextField(new GUIContent("Tag"), tag);
                    tagger.SetTag((Component)serializedObject.targetObject, tag);
                }
                else
                {
                    var index = System.Array.IndexOf(_tagList.tags, tag);
                    index = EditorGUILayout.Popup(new GUIContent("Tag"), index, _tagList.tags);
                    if (index != -1)
                        tagger.SetTag((Component)serializedObject.targetObject, tag);
                }

                _tagList = (TagList)EditorGUILayout.ObjectField(_tagList, typeof(TagList), false);
            }
            EditorGUILayout.EndHorizontal();

            var rect = EditorGUILayout.GetControlRect();
            if (self is Component behaviour)
            {
                if (tag == null)
                {
                    if (GUI.Button(new Rect(rect.width - 52, rect.y, 70, rect.height), "Add Tag"))
                    {
                        if (tagger == null)
                            tagger = behaviour.gameObject.AddComponent<Tagger>();
                        tagger.SetTag(behaviour);
                    }
                }
                else
                {
                    if (GUI.Button(new Rect(rect.width - 67, rect.y, 85, rect.height), "Remove Tag"))
                    {
                        tagger.RemoveTag(behaviour);
                    }
                }

            }

            base.OnInspectorGUI();
        }
    }
}
