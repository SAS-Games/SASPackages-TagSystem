using SAS.TagSystem;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SAS.TagSystem.Editor
{
    public class ContextMenuCommand
    {
        [MenuItem("CONTEXT/Component/Add Tag")]
        private static void AddTag(MenuCommand command)
        {
            var self = (Component)command.context;
            var tagger = self.gameObject.GetComponent<Tagger>();

            if (tagger == null)
                tagger = self.gameObject.AddComponent<Tagger>();
            tagger.SetTag(self);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }

        [MenuItem("CONTEXT/Component/Remove Tag")]
        private static void RemoveTag(MenuCommand command)
        {
            var self = (Component)command.context;
            var tagger = self.gameObject.GetComponent<Tagger>();
            tagger.RemoveTag(self);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
    }
}
