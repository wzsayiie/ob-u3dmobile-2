using U3DMobile;
using UnityEditor;

namespace U3DMobileEditor
{
    internal static class MenuItems
    {
        private const int GameSettingsMenu  = 100;
        private const int BuildSettingsMenu = 200;
        private const int ExportPackageMenu = 300;

        [MenuItem("U3DMobile/Game Settings", false, GameSettingsMenu)]
        private static void OnGameSettings()
        {
            UIHelper.PingPath<GameSettings>(GameSettings.SavedPath);
        }

        [MenuItem("U3DMobile/Game Options", false, GameSettingsMenu)]
        private static void OnGameOptions()
        {
            UIHelper.PingPath<GameOptions>(GameOptions.SavedPath);
        }

        [MenuItem("U3DMobile/Build Settings", false, BuildSettingsMenu)]
        private static void OnBuildSettings()
        {
            UIHelper.PingPath<BuildSettings>(BuildSettings.SavedPath);
        }

        [MenuItem("U3DMobile/Export Android AAB", false, ExportPackageMenu)]
        private static void OnExportAndroidAAB()
        {
            //NOTE: show a dialog for secondary confirmation,
            //to prevent time-consuming tasks caused by accidental touches
            bool yes = EditorUtility.DisplayDialog(
                $"Export Android AAB",
                $"Export Android AAB to '{BuildPath.GetOutputDirectory()}'",
                $"Yes",
                $"No"
            );

            if (yes)
            {
                BuildHelper.ExportAndroidAAB("master");
            }
        }

        [MenuItem("U3DMobile/Export Android APK", false, ExportPackageMenu)]
        private static void OnExportAndroidAPK()
        {
            bool yes = EditorUtility.DisplayDialog(
                $"Export Android APK",
                $"Export Android APK to '{BuildPath.GetOutputDirectory()}'",
                $"Yes",
                $"No"
            );

            if (yes)
            {
                BuildHelper.ExportAndroidAPK("master");
            }
        }

        [MenuItem("U3DMobile/Export iOS Xcode Project", false, ExportPackageMenu)]
        private static void OnExportIOSProject()
        {
            bool yes = EditorUtility.DisplayDialog(
                $"Export iOS Xcode Project",
                $"Export iOS Xcode Project to '{BuildPath.GetOutputDirectory()}'",
                $"Yes",
                $"No"
            );

            if (yes)
            {
                BuildHelper.ExportIOSProject("master");
            }
        }
    }
}
