using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace SAS.TagSystem.Editor
{
    [CustomEditor(typeof(TagList), true)]
    public class TagListInspectorEditor : UnityEditor.Editor
    {
		private ReorderableList _tagsList;
		private SerializedProperty _tags; 

		private void OnEnable()
		{
			_tags = serializedObject.FindProperty("tags");
			CreateReorderableList();
		}

		private void CreateReorderableList()
		{
			_tagsList = new ReorderableList(serializedObject, _tags, false, true, true, true);
			_tagsList.drawHeaderCallback = (Rect rect) =>
			{
				EditorGUI.LabelField(rect, "Tags");
			};

			_tagsList.onAddCallback = (list) =>
			{
				_tags.InsertArrayElementAtIndex(_tags.arraySize);
				var currValue = _tags.GetArrayElementAtIndex(_tags.arraySize - 1);
				var newValue = GetUniqueName("New Tag", (target as TagList).tags);
				currValue.stringValue = newValue;
				serializedObject.ApplyModifiedProperties();
			};

			_tagsList.onRemoveCallback = (list) =>
			{
				if (_tags.GetArrayElementAtIndex(list.index) != null)
					_tags.DeleteArrayElementAtIndex(list.index);
				serializedObject.ApplyModifiedProperties();
			};

			_tagsList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
			{
				if (_tags.arraySize > 0)
				{
					EditorGUI.BeginDisabledGroup(true);
					EditorGUI.TextField(new Rect(rect.x + 5, rect.y, rect.width / 2, rect.height), "Tag " + index);
					EditorGUI.EndDisabledGroup();
					var currValue = _tags.GetArrayElementAtIndex(index);
					var newValue = EditorGUI.DelayedTextField(new Rect(rect.width / 2 + 40, rect.y, rect.width / 2 - 20, rect.height), currValue.stringValue);
					if (newValue != currValue.stringValue)
					{
						newValue = GetUniqueName(newValue, (target as TagList).tags);
						currValue.stringValue = newValue;
						serializedObject.ApplyModifiedProperties();
					}
				}
			};
		}

		public override void OnInspectorGUI()
        {
			_tagsList.DoLayoutList();
		}

		private string GetUniqueName(string nameBase, string[] usedNames)
		{
			string name = nameBase;
			int counter = 1;
			while (usedNames.Contains(name.Trim()))
			{
				name = nameBase + " " + counter;
				counter++;
			}
			return name;
		}
	}
}
