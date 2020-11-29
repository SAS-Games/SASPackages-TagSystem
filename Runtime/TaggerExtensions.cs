using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace SAS.TagSystem
{
    public static class TaggerExtensions
    {
        public static T GetComponent<T>(this Component gameObject, string tag) where T : Component
        {
            if (string.IsNullOrEmpty(tag))
                return gameObject.GetComponent<T>();
            else
                return (T)gameObject.GetComponent<Tagger>()?.Find<T>(tag).FirstOrDefault();
        }
    }
}
