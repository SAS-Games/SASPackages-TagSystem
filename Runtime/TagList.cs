using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SAS.TagSystem
{
    [CreateAssetMenu(fileName = "Tag List", menuName = "SAS/Tag List")]
    [System.Serializable]
    public class TagList : ScriptableObject
    {
        public string[] tags;
    }
}
