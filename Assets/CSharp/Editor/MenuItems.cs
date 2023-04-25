using U3DMobile;
using UnityEditor;

static class MenuItems
{
    [MenuItem("U3DMobile/Game Settings")]
    static void OnGameSettings()
    {
        Utility.PingPath<GameSettings>("Assets/Resources/GameSettings.asset");
    }

    [MenuItem("U3DMobile/Build Settings")]
    static void OnBuildSettings()
    {
        Utility.PingPath<BuildSettings>("Assets/BuildSettings.asset");
    }
}
