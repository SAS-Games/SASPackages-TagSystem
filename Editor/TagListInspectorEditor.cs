using SAS.TagSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace SAS.TagSystemEditor
{
    [CustomEditor(typeof(TagList), true)]
    public class TagListInspectorEditor : Editor
    {
		AutocompleteSearchField autocompleteSearchField;
		private List<string> _filteredTags = new List<string>() { "sadad", "sdfsf" };
		private ReorderableList tagsList;

		public static void Show()
        {
			var tagListSO = Resources.Load<TagList>("Tag List");
			Selection.activeObject = tagListSO;
		}

		private void OnEnable()
		{

			_filteredTags = (target as TagList).tags.ToList();
			_filteredTags = _filteredTags.Distinct().ToList();

			autocompleteSearchField = new AutocompleteSearchField((target as TagList).tags.ToList());
			autocompleteSearchField.onInputChanged = OnInputChanged;


			tagsList = new ReorderableList(_filteredTags, typeof(string), false, true, true, true);
			tagsList.drawHeaderCallback = (Rect rect) =>
			{
				EditorGUI.LabelField(rect, "Tags");
			};

			tagsList.onSelectCallback = list =>
			{
				Debug.Log(list.index);
			};

			tagsList.onAddCallback = list =>
			{
				var tags = (target as TagList).tags;
				var tag = GetUniqueName("New Tag", (target as TagList).tags);
				Array.Resize(ref tags, tags.Length + 1);
				tags[tags.Length - 1] = tag;
				(target as TagList).tags = tags;
				_filteredTags.Add(tag);

			};

			tagsList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
			{
				var oldValue = _filteredTags[index];
				var newValue = EditorGUI.DelayedTextField(new Rect(rect.x + 5, rect.y, rect.width - 10, rect.height), _filteredTags[index]);
				if (newValue != oldValue)
				{
					_filteredTags[index] = newValue;
				}
			};
		}

        private void OnInputChanged(List<string> filteredTags)
        {
			_filteredTags = filteredTags;
		}

		public override void OnInspectorGUI()
        {
			GUILayout.Label("Search Tag", EditorStyles.boldLabel);
			autocompleteSearchField.DoSearchField(true);
			tagsList.DoLayoutList();
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
