using U3DMobile;
using UnityEditor;

namespace U3DMobileEditor
{
    internal static class MenuItems
    {
        [MenuItem("U3DMobile/Game Settings")]
        private static void OnGameSettings()
        {
            UIHelper.PingPath<GameSettings>(GameSettings.SavedPath);
        }

        [MenuItem("U3DMobile/Build Settings")]
        private static void OnBuildSettings()
        {
            UIHelper.PingPath<BuildSettings>(BuildSettings.SavedPath);
        }
    }
}
