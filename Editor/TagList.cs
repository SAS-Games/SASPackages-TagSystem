using System.IO;
using UnityEditor;
using UnityEngine;

namespace SAS.TagSystem.Editor
{
    [CreateAssetMenu(fileName = "Tag List", menuName = "SAS/Tag List")]
    [System.Serializable]
    public class TagList : ScriptableObject
    {
        public string[] tags = new string[] { };

        public static TagList Instance(string assetName = "Tag List")
        {
            var basePath = "Assets/Editor Default Resources/TagList/";
            var filePath = $"Assets/Editor Default Resources/TagList/{assetName}.asset";
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            var asset = AssetDatabase.LoadAssetAtPath<TagList>(filePath);
            if (asset == null)
            {
                asset = CreateInstance<TagList>();
                AssetDatabase.CreateAsset(asset, $"{filePath}");
                AssetDatabase.SaveAssets();
            }

            var tagListAssets = EditorGUIUtility.Load(filePath) as TagList;
            return tagListAssets;
        }

        public static string[] GetList(string assetName = "Tag List")
        {
            return Instance(assetName).tags;
        }
    }
}
