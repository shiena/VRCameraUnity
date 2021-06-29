using UnityEditor;

static class ForceForcusGameView
{
    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        void OnPlayModeStateChanged(PlayModeStateChange mode)
        {
            if (mode == PlayModeStateChange.EnteredPlayMode)
            {
                EditorApplication.ExecuteMenuItem("Window/General/Game");
            }
        }

        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }
}