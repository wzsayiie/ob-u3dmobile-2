using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace U3DMobileEditor
{
    internal class BuildArgs
    {
        //target.
        public string TargetPlatform;
        public string TargetProduct ;

        //app information.
        public string AppPackageID ;
        public string AppVersionStr;
        public string AppVersionNum;

        //game settings:
        public int    PackageSerial ;
        public string StoreChannel  ;
        public string ChannelGateway;
        public string ForcedAssetURL;
        public string ForcedPatchURL;
        public string AssetFlavor   ;

        public Dictionary<string, List<string>> UserFlags;

        //build settings.
        public int    BundleSerial;
        public string CarryOption ;

        //apk keystore.
        public string JKSFile;
        public string JKSFilePassword;
        public string JKSKey;
        public string JKSKeyPassword;

        //ipa mobileprovision.
        public string MobileProvision;
    }

    internal static class BuildProcess
    {
        internal static void Launch()
        {
            var buildArgs = new BuildArgs();
            var argErrors = new List<string>();
            AnalyseArgs(buildArgs, argErrors);

            if (argErrors.Count > 0)
            {
                foreach (var error in argErrors)
                {
                    Debug.LogFormat("Argument Error: {0}", error);
                }
                throw new Exception("Argument Error");
            }
        }

        private static void AnalyseArgs(BuildArgs args, List<string> errors)
        {
            //target platform:
            args.TargetPlatform = GetEnvironmentVariable("_target_platform");

            if (args.TargetPlatform != "android" &&
                args.TargetPlatform != "ios"     )
            {
                errors.Add($"unexpected platform {args.TargetPlatform}");
                return;
            }

            //target product:
            args.TargetProduct = GetEnvironmentVariable("_target_product");

            if (args.TargetPlatform == "android" &&
                args.TargetProduct  != "apk"     &&
                args.TargetProduct  != "aab"     &&
                args.TargetProduct  != "res"     )
            {
                errors.Add(
                    $"unexpected product {args.TargetProduct} for platform {args.TargetPlatform}"
                );
                return;
            }

            if (args.TargetPlatform == "ios" &&
                args.TargetProduct  != "ipa" &&
                args.TargetProduct  != "res" )
            {
                errors.Add(
                    $"unexpected product {args.TargetProduct} for platform {args.TargetPlatform}"
                );
                return;
            }

            //app information:
            bool needAndrPkg = args.TargetProduct == "apk" || args.TargetProduct == "aab";
            bool needIOSPkg  = args.TargetProduct == "ipa" ;

            if (needAndrPkg || needIOSPkg)
            {
            }

            //apk keystore:
            if (needAndrPkg)
            do
            {
                string name = GetEnvironmentVariable("_apk_keystore");
                if (string.IsNullOrEmpty(name))
                {
                    errors.Add("no input jks name");
                    break;
                }

                args.JKSFile = $"keystore/android/{name}.jks";
                if (!File.Exists(args.JKSFile))
                {
                    errors.Add($"did not find {args.JKSFile}");
                }

                string filePasswordFrom = $"keystore/android/{name}.jkspass.txt";
                string keyFrom          = $"keystore/android/{name}.key.txt";
                string keyPasswordFrom  = $"keystore/android/{name}.keypass.txt";

                args.JKSFilePassword = ReadAllText(filePasswordFrom);
                if (string.IsNullOrWhiteSpace(args.JKSFilePassword))
                {
                    errors.Add($"did not get jks password from {filePasswordFrom}");
                }

                args.JKSKey = ReadAllText(keyFrom);
                if (string.IsNullOrWhiteSpace(args.JKSKey))
                {
                    errors.Add($"did not get key from {keyFrom}");
                }

                args.JKSKeyPassword = ReadAllText(keyPasswordFrom);
                if (string.IsNullOrWhiteSpace(args.JKSKeyPassword))
                {
                    errors.Add($"did not get key from {keyPasswordFrom}");
                }
            }
            while (false);

            //ipa mobileprovision:
            if (needIOSPkg)
            do
            {
                string name = GetEnvironmentVariable("_ipa_provision");
                if (string.IsNullOrWhiteSpace(name))
                {
                    errors.Add("no input mobileprovision name");
                    break;
                }

                args.MobileProvision = $"keystores/ios/{name}.mobileprivision";
                if (!File.Exists(args.MobileProvision))
                {
                    errors.Add($"did not find {args.MobileProvision}");
                }
            }
            while (false);
        }

        private static Dictionary<string, List<string>> GetEnvironmentDictionary(string name)
        {
            return null;
        }

        private static string GetEnvironmentVariable(string name)
        {
            string value = Environment.GetEnvironmentVariable(name);
            return value != null ? value.Trim() : null;
        }

        private static string ReadAllText(string file)
        {
            try
            {
                return File.ReadAllText(file);
            }
            catch
            {
                return null;
            }
        }

        private static void AssignArgs(BuildArgs args, List<string> errors)
        {
        }
    }
}
