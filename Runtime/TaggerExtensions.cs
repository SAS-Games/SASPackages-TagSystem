using System.Linq;
using UnityEngine;
using System;

namespace SAS.TagSystem
{
    public static class TaggerExtensions
    {
        public static T GetComponent<T>(this Component component, string tag)
        {
            return (T)(object)component.GetComponent(typeof(T), tag);
        }

        public static Component GetComponent(this Component component, Type type, string tag)
        {
            if (string.IsNullOrEmpty(tag))
                return component.GetComponent(type);
            else
                return component.GetComponent<Tagger>()?.Find(type, tag).FirstOrDefault();
        }

        public static T GetComponentInChildren<T>(this Component component, string tag, bool includeInactive = false) where T : Component
        {
            return GetComponentByTag(component.GetComponentsInChildren<T>(includeInactive), tag);
        }

        public static Component GetComponentInChildren(this Component component, Type type, string tag, bool includeInactive = false)
        {
            return GetComponentByTag(component.GetComponentsInChildren(type, includeInactive), tag);
        }

        public static T GetComponentInParent<T>(this Component component, string tag, bool includeInactive = false) where T : Component
        {
            return GetComponentByTag(component.GetComponentsInParent<T>(includeInactive), tag);
        }

        public static T[] GetComponentsInChildren<T>(this Component component, string tag, bool includeInactive = false) where T : Component
        {
            return GetComponentsByTag(component.GetComponentsInChildren<T>(includeInactive), tag);
        }

        public static Component[] GetComponentsInChildren(this Component component, Type type, string tag, bool includeInactive = false)
        {
            return GetComponentsByTag(component.GetComponentsInChildren(type, includeInactive), tag);
        }

        private static T GetComponentByTag<T>(T[] components, string tag) where T : Component
        {
            if (string.IsNullOrEmpty(tag))
                return components.FirstOrDefault();
            else
                return components.FirstOrDefault(component => component.GetComponent<Tagger>()?.Find(component)?.Value == tag);
        }

        private static T[] GetComponentsByTag<T>(T[] components, string tag) where T : Component
        {
            if (string.IsNullOrEmpty(tag))
                return components;
            else
                return components.Where(component => component.GetComponent<Tagger>()?.Find(component)?.Value == tag).ToArray();
        }
    }
}
