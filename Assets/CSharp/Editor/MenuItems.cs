using U3DMobile;
using UnityEditor;

namespace U3DMobileEditor
{
    internal static class MenuItems
    {
        [MenuItem("U3DMobile/Game Settings")]
        private static void OnGameSettings()
        {
            Utility.PingPath<GameSettings>("Assets/Resources/GameSettings.asset");
        }

        [MenuItem("U3DMobile/Build Settings")]
        private static void OnBuildSettings()
        {
            Utility.PingPath<BuildSettings>("Assets/BuildSettings.asset");
        }
    }
}
