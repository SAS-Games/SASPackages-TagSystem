using UnityEngine;

namespace SAS.TagSystem.Editor
{
    [CreateAssetMenu(fileName = "Tag List", menuName = "SAS/Tag List")]
    [System.Serializable]
    public class TagList : ScriptableObject
    {
        public string[] tags;

        public static TagList Instance(string assetName = "Tag List")
        {
            var tagListAssets = Resources.Load(assetName, typeof(TagList)) as TagList;
            return tagListAssets;
        }

        public static string[] GetList(string assetName = "Tag List")
        {
            return Instance(assetName).tags;
        }
    }
}
