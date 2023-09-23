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
                throw WriteErrors(I18N.ErrorWhenParseEnvironment, errors);
            }

            //更新设置:
            BuildEnvironment.UpdateProfile(args);

            errors.Clear();
            BuildAssetBundle.SwitchAssetFlavors(args.assetFlavors, errors);
            if (errors.Count > 0)
            {
                throw WriteErrors(I18N.ErrorWhenSwitchAssetFlavor, errors);
            }

            //打包资产.
            errors.Clear();
            if (args.targetPlatform == "android")
            {
                BuildAssetBundle.PackForAndroid(errors);
                if (errors.Count > 0)
                {
                    throw WriteErrors(I18N.ErrorWhenPackBundleForAndroid, errors);
                }
            }
            else if (args.targetPlatform == "ios")
            {
                BuildAssetBundle.PackForIOS(errors);
                if (errors.Count > 0)
                {
                    throw WriteErrors(I18N.ErrorWhenPackBundleForIOS, errors);
                }
            }

            //生成安装包.
            errors.Clear();
            if (args.targetProduct == "aab")
            {
                BuildAndroidPackage.ExportAAB(args.apkKeystore, errors);
                if (errors.Count > 0)
                {
                    throw WriteErrors(I18N.ErrorWhenExportAndroidAAB, errors);
                }
            }
            else if (args.targetProduct == "apk")
            {
                BuildAndroidPackage.ExportAPK(args.apkKeystore, errors);
                if (errors.Count > 0)
                {
                    throw WriteErrors(I18N.ErrorWhenExportAndroidAPK, errors);
                }
            }
            else if (args.targetProduct == "ipa")
            {
                BuildIOSPackage.ExportXCProject(args.ipaProvision, errors);
                if (errors.Count > 0)
                {
                    throw WriteErrors(I18N.ErrorWhenExportIOSProject, errors);
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
                WriteErrors(I18N.ErrorWhenSwitchAssetFlavor, errors);
            }
        }

        internal static void PackBundlesForAndroid()
        {
            var errors = new List<string>();
            BuildAssetBundle.PackForAndroid(errors);
            if (errors.Count > 0)
            {
                WriteErrors(I18N.ErrorWhenPackBundleForAndroid, errors);
            }
        }

        internal static void PackBundlesForIOS()
        {
            var errors = new List<string>();
            BuildAssetBundle.PackForIOS(errors);
            if (errors.Count > 0)
            {
                WriteErrors(I18N.ErrorWhenPackBundleForIOS, errors);
            }
        }

        internal static void CopyPatches()
        {
            var errors = new List<string>();
            BuildPatch.Copy(errors);
            if (errors.Count > 0)
            {
                WriteErrors(I18N.ErrorWhenCopyPatch, errors);
            }
        }

        internal static void ExportAndroidAAB(string keystore)
        {
            var errors = new List<string>();
            BuildAndroidPackage.ExportAAB(keystore, errors);

            if (errors.Count > 0)
            {
                WriteErrors(I18N.ErrorWhenExportAndroidAAB, errors);
            }
        }

        internal static void ExportAndroidAPK(string keystore)
        {
            var errors = new List<string>();
            BuildAndroidPackage.ExportAPK(keystore, errors);

            if (errors.Count > 0)
            {
                WriteErrors(I18N.ErrorWhenExportAndroidAPK, errors);
            }
        }

        internal static void ExportIOSProject(string provision)
        {
            var errors = new List<string>();
            BuildIOSPackage.ExportXCProject(provision, errors);

            if (errors.Count > 0)
            {
                WriteErrors(I18N.ErrorWhenExportIOSProject, errors);
            }
        }

        private static Exception WriteErrors(string brief, List<string> errors)
        {
            Log.Group(() =>
            {
                Log.Error(brief);
                for (int i = 0; i < errors.Count; ++i)
                {
                    Log.Error($"{I18N.Error} ({i + 1}/{errors.Count}): {errors[i]}");
                }
            });

            return new Exception(brief);
        }
    }
}
