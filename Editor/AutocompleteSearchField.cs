using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using System.Linq;

namespace SAS.TagSystemEditor
{
	[Serializable]
	public class AutocompleteSearchField
	{
		public Action<List<string>> onInputChanged;
		public Action<string> onConfirm;

		private string _searchString = string.Empty;
		private List<string> _originalStrings { get; }
		private List<string> _results = new List<string>();


		SearchField searchField;

		public AutocompleteSearchField(List<string> strArray)
		{
			_originalStrings = new List<string>(strArray);

		}

		public void DoSearchField(bool asToolbar)
		{
			var rect = GUILayoutUtility.GetRect(1, 1, 18, 18, GUILayout.ExpandWidth(true));
			GUILayout.BeginHorizontal();
			DoSearchField(rect, asToolbar);
			GUILayout.EndHorizontal();
			rect.y += 18;
		}

		private void DoSearchField(Rect rect, bool asToolbar)
		{
			if (searchField == null)
				searchField = new SearchField();

			var result = asToolbar ? searchField.OnToolbarGUI(rect, _searchString) : searchField.OnGUI(rect, _searchString);
			if (result != _searchString)
			{
				FilterSearchResult(result);
				onInputChanged?.Invoke(_results);
				_searchString = result;
			}

			if (GUIUtility.keyboardControl == searchField.searchFieldControlID)
				EditorWindow.focusedWindow?.Repaint();
		}

		private void FilterSearchResult(string searchString)
		{
			_results.Clear();
			if (!string.IsNullOrEmpty(searchString))
				_results.AddRange(_originalStrings.Where(p => p.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1));
			else
				_results.AddRange(_originalStrings);
		}
	}
}