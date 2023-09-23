using System;
using System.Collections.Generic;
using U3DMobile;

namespace U3DMobileEditor
{
    internal static class BuildHelper
    {
        internal static void LaunchWithEnvironment()
        {
            //检查参数:
            BuildArguments args = BuildEnvironment.ParseArguments();

            var errors = new List<string>();
            BuildEnvironment.CheckArguments(args, errors);
            if (errors.Count > 0)
            {
                throw WriteErrors("Argument Error", errors);
            }

            //更新设置:
            BuildEnvironment.UpdateProfile(args);

            errors.Clear();
            BuildAssetBundle.SwitchAssetFlavors(args.assetFlavors, errors);
            if (errors.Count > 0)
            {
                throw WriteErrors("Switch Asset Flavors Error", errors);
            }

            //打包资产.
            errors.Clear();
            if (args.targetPlatform == "android")
            {
                BuildAssetBundle.PackForAndroid(errors);
                if (errors.Count > 0)
                {
                    throw WriteErrors("Pack Android Bundles Error", errors);
                }
            }
            else if (args.targetPlatform == "ios")
            {
                BuildAssetBundle.PackForIOS(errors);
                if (errors.Count > 0)
                {
                    throw WriteErrors("Pack iOS Bundles Error", errors);
                }
            }

            //生成安装包.
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
            var profile = AssetHelper.LoadScriptable<GameProfile>(GameProfile.SavedPath);
            HashSet<string> flavors = profile.GetAssetFlavors();

            var errors = new List<string>();
            BuildAssetBundle.SwitchAssetFlavors(flavors, errors);
            if (errors.Count > 0)
            {
                WriteErrors("Switch Asset Flavors Error", errors);
            }
        }

        internal static void PackBundlesForAndroid()
        {
            var errors = new List<string>();
            BuildAssetBundle.PackForAndroid(errors);
            if (errors.Count > 0)
            {
                WriteErrors("Pack Bundles for Android Error", errors);
            }
        }

        internal static void PackBundlesForIOS()
        {
            var errors = new List<string>();
            BuildAssetBundle.PackForIOS(errors);
            if (errors.Count > 0)
            {
                WriteErrors("Pack Bundles for iOS Error", errors);
            }
        }

        internal static void CopyPatches()
        {
            var errors = new List<string>();
            BuildPatch.Copy(errors);
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
