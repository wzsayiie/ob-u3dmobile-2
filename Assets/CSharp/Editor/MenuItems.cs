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
        internal static void ShowGameSettings()
        {
            UIHelper.PingPath<GameSettings>(GameSettings.SavedPath);
        }

        [MenuItem("U3DMobile/Game Options", false, GameSettingsMenu)]
        internal static void ShowGameOptions()
        {
            UIHelper.PingPath<GameOptions>(GameOptions.SavedPath);
        }
        
        [MenuItem("U3DMobile/Switch Asset Flavors", false, GameSettingsMenu)]
        internal static void SwitchAssetFlavors()
        {
            BuildHelper.SwitchAssetFlavors();
        }

        [MenuItem("U3DMobile/Build Settings", false, BuildSettingsMenu)]
        internal static void ShowBuildSettings()
        {
            UIHelper.PingPath<BuildSettings>(BuildSettings.SavedPath);
        }

        [MenuItem("U3DMobile/Pack Bundles for Android", false, BuildSettingsMenu)]
        internal static void PackBundlesForAndroid()
        {
            //NOTE: show a dialog for secondary confirmation,
            //to prevent time-consuming tasks caused by accidental touches
            bool yes = EditorUtility.DisplayDialog(
                $"Pack Bundles for Android",
                $"Pack Bundles for Android to '{BuildPath.outputDirectory}'",
                $"Yes",
                $"No"
            );

            if (yes)
            {
                BuildHelper.PackBundlesForAndroid();
            }
        }

        [MenuItem("U3DMobile/Pack Bundles for iOS", false, BuildSettingsMenu)]
        internal static void PackBundlesForIOS()
        {
            bool yes = EditorUtility.DisplayDialog(
                $"Pack Bundles for iOS",
                $"Pack Bundles for iOS to '{BuildPath.outputDirectory}'",
                $"Yes",
                $"No"
            );

            if (yes)
            {
                BuildHelper.PackBundlesForIOS();
            }
        }

        [MenuItem("U3DMobile/Copy Patches", false, BuildSettingsMenu)]
        internal static void CopyPatches()
        {
            BuildHelper.CopyPatches();
        }

        [MenuItem("U3DMobile/Export Android AAB", false, ExportPackageMenu)]
        internal static void ExportAndroidAAB()
        {
            //NOTE: show a dialog for secondary confirmation,
            //to prevent time-consuming tasks caused by accidental touches
            bool yes = EditorUtility.DisplayDialog(
                $"Export Android AAB",
                $"Export Android AAB to '{BuildPath.outputDirectory}'",
                $"Yes",
                $"No"
            );

            if (yes)
            {
                BuildHelper.ExportAndroidAAB("master");
            }
        }

        [MenuItem("U3DMobile/Export Android APK", false, ExportPackageMenu)]
        internal static void ExportAndroidAPK()
        {
            bool yes = EditorUtility.DisplayDialog(
                $"Export Android APK",
                $"Export Android APK to '{BuildPath.outputDirectory}'",
                $"Yes",
                $"No"
            );

            if (yes)
            {
                BuildHelper.ExportAndroidAPK("master");
            }
        }

        [MenuItem("U3DMobile/Export iOS Xcode Project", false, ExportPackageMenu)]
        internal static void ExportIOSProject()
        {
            bool yes = EditorUtility.DisplayDialog(
                $"Export iOS Xcode Project",
                $"Export iOS Xcode Project to '{BuildPath.outputDirectory}'",
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
