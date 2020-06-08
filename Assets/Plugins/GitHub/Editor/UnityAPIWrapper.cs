using GitHub.Unity;
using UnityEditor;

namespace Assets.Plugins.GitHub.Editor
{
    [InitializeOnLoad]
    public class UnityAPIWrapper : ScriptableSingleton<UnityAPIWrapper>
    {
        static UnityAPIWrapper()
        {
#if UNITY_2018_2_OR_NEWER
            UnityEditor.Editor.finishedDefaultHeaderGUI += editor => {
                UnityShim.Raise_Editor_finishedDefaultHeaderGUI(editor);
            };
#endif
        }
    }
}
