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
		static class Styles
		{
			public const float resultHeight = 20f;
			public const float resultsBorderWidth = 2f;
			public const float resultsMargin = 15f;
			public const float resultsLabelOffset = 2f;

			public static readonly GUIStyle entryEven;
			public static readonly GUIStyle entryOdd;
			public static readonly GUIStyle labelStyle;
			public static readonly GUIStyle resultsBorderStyle;

			static Styles()
			{
				entryOdd = new GUIStyle("CN EntryBackOdd");
				entryEven = new GUIStyle("CN EntryBackEven");
				resultsBorderStyle = new GUIStyle("hostview");

				labelStyle = new GUIStyle(EditorStyles.label)
				{
					alignment = TextAnchor.MiddleLeft,
					richText = true
				};
			}
		}

		public Action<List<string>> onInputChanged;
		public Action<string> onConfirm;
		
		private string _searchString;
		List<string> _originalStrings = new List<string>();
		List<string> _results = new List<string>(); 


		SearchField searchField;

		public AutocompleteSearchField(List<string> strArray)
        {
			_originalStrings = strArray;

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
			if(searchField == null)
				searchField = new SearchField();

			var result = asToolbar
				? searchField.OnToolbarGUI(rect, _searchString)
				: searchField.OnGUI(rect, _searchString);

			if (result != _searchString)
			{
				FilterSearchResult(result);
				onInputChanged?.Invoke(_results);
			}

			_searchString = result;

			if(HasSearchbarFocused())
				RepaintFocusedWindow();
		}

		private void FilterSearchResult(string searchString)
		{
			_results.Clear();
			if (!string.IsNullOrEmpty(searchString))
				_results.AddRange(_originalStrings.Where(p => p.Contains(searchString)));
			else
				_results = _originalStrings;
		}

		bool HasSearchbarFocused()
		{
			return GUIUtility.keyboardControl == searchField.searchFieldControlID;
		}

		static void RepaintFocusedWindow()
		{
			if(EditorWindow.focusedWindow != null)
			{
				EditorWindow.focusedWindow.Repaint();
			}
		}
	}
}