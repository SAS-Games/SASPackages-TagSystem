using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAS.TagSystem
{
	[DisallowMultipleComponent()]
	public class Tagger : MonoBehaviour
	{
		[Serializable]
		public class Tag
        {
			[SerializeField] private Component m_Component;
			[SerializeField] private string m_Value;

			public Component Component => m_Component;

            public string Value { get => m_Value; set => m_Value = value; }

			public Tag(Component component, string val)
            {
				m_Component = component;
				m_Value = val;
            }
        }

		public IEnumerable<Component> Find<T>(string tag) where T: Component
		{
			return m_Tags.Where(item => item.Value == tag && item.Component.GetType() == typeof(T)).Select(item => item.Component);
		}

		public IEnumerable<Component> Find(Type type, string tag)
		{
			return m_Tags.Where(item => item.Value == tag && item.Component.GetType() == type).Select(item => item.Component);
		}

		public Tag Find(Component component)
		{
			return m_Tags.FirstOrDefault(tag => tag.Component == component);
		}

		[SerializeField] private List<Tag> m_Tags = new List<Tag>();

		public List<string> tags = new List<string>();


        void Awake ()
		{
			/*for (var i = 0; i < tags.Count; i++) {
				var tag = tags [i];
				tags [i] = tag.Trim ().ToLower ();
			}
			UpdateTagSystem ();*/
		}

		public void AddTag (string toAdd)
		{
			var tag = toAdd.ToLower ();
			if (!tags.Contains (tag)) {
				RemoveFromTagSystem ();
				tags.Add (tag);
				UpdateTagSystem ();
			}
		}

        public void RemoveTag (string toRemove)
		{
			var tag = toRemove.ToLower ();
			if (tags.Contains (tag)) {
				RemoveFromTagSystem ();
				tags.Remove (tag);
				UpdateTagSystem ();
			}
		}

		public string GetTag(Component component)
        {
			var tag = m_Tags.Find(ele => ele.Component == component);
			return tag?.Value;
        }

		public void SetTag(Component component, string tagValue = "")
		{
			var tag = m_Tags.Find(ele => ele.Component == component);
			if (tag != null)
				tag.Value = tagValue;
			else
				m_Tags.Add(new Tag(component, tagValue));
		}

		public void RemoveTag(Component component)
		{
			var tag = m_Tags.Find(ele => ele.Component == component);
			m_Tags.Remove(tag);
		}

		public List<string> GetTags ()
		{
			return tags;
		}

		void OnDestroy ()
		{
			RemoveFromTagSystem ();
		}

		void OnDisable ()
		{
			RemoveFromTagSystem ();
		}

		void OnEnable() {
			UpdateTagSystem ();
		}

		void UpdateTagSystem ()
		{
			TagSystem.AddObject (this);
		}

		void RemoveFromTagSystem ()
		{
			TagSystem.RemoveObject (this);
		}

#if UNITY_EDITOR
		private void Reset()
        {
			if (!Application.isPlaying)
				SetTag(transform, "");
		}
#endif
	}
}