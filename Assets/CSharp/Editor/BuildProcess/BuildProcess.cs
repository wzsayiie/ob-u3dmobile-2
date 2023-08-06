using System;
using System.Collections.Generic;
using U3DMobile;

namespace U3DMobileEditor
{
    internal static class BuildProcess
    {
        internal static void Launch()
        {
            //parse arguments:
            BuildArguments args = BuildEnvironment.ParseEnvironment();

            var errors = new List<string>();
            BuildEnvironment.CheckEnvironment(args, errors);
            if (errors.Count > 0)
            {
                throw WriteErrors("Argument Error", errors);
            }

            //update settings:
            BuildEnvironment.UpdateSettings(args);

            errors.Clear();
            BuildAssetBundle.SwitchAssetFlavors(args.assetFlavors);
            if (errors.Count > 0)
            {
                throw WriteErrors("Switch Asset Flavors Error", errors);
            }

            //pack asset bundles.
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

            //pack packages.
            errors.Clear();
            if (args.targetProduct == "aab")
            {
                BuildAndroidPackage.PackAAB(errors);
                if (errors.Count > 0)
                {
                    throw WriteErrors("Pack Android AAB Error", errors);
                }
            }
            else if (args.targetProduct == "apk")
            {
                BuildAndroidPackage.PackAPK(errors);
                if (errors.Count > 0)
                {
                    throw WriteErrors("Pack Android APK Error", errors);
                }
            }
            else if (args.targetProduct == "ipa")
            {
                BuildIOSPackage.PackIPA(errors);
                if (errors.Count > 0)
                {
                    throw WriteErrors("Pack iOS IPA Error", errors);
                }
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
