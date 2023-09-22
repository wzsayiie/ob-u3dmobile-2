using System;
using System.Collections.Generic;
using U3DMobile;

namespace U3DMobileEditor
{
    internal static class BuildHelper
    {
        internal static void LaunchWithEnvironment()
        {
            //parse arguments:
            BuildArguments args = BuildEnvironment.ParseArguments();

            var errors = new List<string>();
            BuildEnvironment.CheckArguments(args, errors);
            if (errors.Count > 0)
            {
                throw WriteErrors("Argument Error", errors);
            }

            //update settings:
            BuildEnvironment.UpdateSettings(args);

            errors.Clear();
            BuildAssetBundles.SwitchAssetFlavors(args.assetFlavors, errors);
            if (errors.Count > 0)
            {
                throw WriteErrors("Switch Asset Flavors Error", errors);
            }

            //pack asset bundles.
            errors.Clear();
            if (args.targetPlatform == "android")
            {
                BuildAssetBundles.PackForAndroid(errors);
                if (errors.Count > 0)
                {
                    throw WriteErrors("Pack Android Bundles Error", errors);
                }
            }
            else if (args.targetPlatform == "ios")
            {
                BuildAssetBundles.PackForIOS(errors);
                if (errors.Count > 0)
                {
                    throw WriteErrors("Pack iOS Bundles Error", errors);
                }
            }

            //pack packages.
            errors.Clear();
            if (args.targetProduct == "aab")
            {
                BuildAndroidPackage.ExportAAB(args.apkKeystore, errors);
                if (errors.Count > 0)
                {
                    throw WriteErrors("Export Android AAB Error", errors);
                }
            }
            else if (args.targetProduct == "apk")
            {
                BuildAndroidPackage.ExportAPK(args.apkKeystore, errors);
                if (errors.Count > 0)
                {
                    throw WriteErrors("Export Android APK Error", errors);
                }
            }
            else if (args.targetProduct == "ipa")
            {
                BuildIOSPackage.ExportXCProject(args.ipaProvision, errors);
                if (errors.Count > 0)
                {
                    throw WriteErrors("Export iOS Xcode Project Error", errors);
                }
            }
        }

        internal static void SwitchAssetFlavors()
        {
            var settings = AssetHelper.LoadScriptable<GameSettings>(GameSettings.SavedPath);
            HashSet<string> flavors = settings.GetAssetFlavors();

            var errors = new List<string>();
            BuildAssetBundles.SwitchAssetFlavors(flavors, errors);
            if (errors.Count > 0)
            {
                WriteErrors("Switch Asset Flavors Error", errors);
            }
        }

        internal static void PackBundlesForAndroid()
        {
            var errors = new List<string>();
            BuildAssetBundles.PackForAndroid(errors);
            if (errors.Count > 0)
            {
                WriteErrors("Pack Bundles for Android Error", errors);
            }
        }

        internal static void PackBundlesForIOS()
        {
            var errors = new List<string>();
            BuildAssetBundles.PackForIOS(errors);
            if (errors.Count > 0)
            {
                WriteErrors("Pack Bundles for iOS Error", errors);
            }
        }

        internal static void CopyPatches()
        {
            var errors = new List<string>();
            BuildPatches.Copy(errors);
            if (errors.Count > 0)
            {
                WriteErrors("Copy Patches Error", errors);
            }
        }

        internal static void ExportAndroidAAB(string keystore)
        {
            var errors = new List<string>();
            BuildAndroidPackage.ExportAAB(keystore, errors);

            if (errors.Count > 0)
            {
                WriteErrors("Export Android AAB Error", errors);
            }
        }

        internal static void ExportAndroidAPK(string keystore)
        {
            var errors = new List<string>();
            BuildAndroidPackage.ExportAPK(keystore, errors);

            if (errors.Count > 0)
            {
                WriteErrors("Export Android APK Error", errors);
            }
        }

        internal static void ExportIOSProject(string provision)
        {
            var errors = new List<string>();
            BuildIOSPackage.ExportXCProject(provision, errors);

            if (errors.Count > 0)
            {
                WriteErrors("Export iOS Xcode Project Error", errors);
            }
        }

        private static Exception WriteErrors(string brief, List<string> errors)
        {
            Log.Group(() =>
            {
                Log.Error(brief);
                for (int i = 0; i < errors.Count; ++i)
                {
                    Log.Error("Error ({0}/{1}): {2}", i + 1, errors.Count, errors[i]);
                }
            });

            return new Exception(brief);
        }
    }
}
