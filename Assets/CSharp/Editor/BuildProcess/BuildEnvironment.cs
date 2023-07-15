using System;
using System.Collections.Generic;
using U3DMobile;

namespace U3DMobileEditor
{
    internal class EnvironmentArguments
    {
        //platform.
        public string targetPlatform;
        public string targetProduct ;

        //keys.
        public string apkKeystore   ;
        public string ipaProvision  ;

        //application information.
        public string appPackageId  ;
        public string appVersionStr ;
        public int    appVersionNum ;

        //game setting.
        public int    packageSerial ;
        public string firstLanguage ;
        public string storeChannel  ;
        public string channelGateway;
        public string forcedAssetURL;
        public string forcedPatchURL;

        public HashSet<string>            assetFlavors;
        public Dictionary<string, object> userFlags;

        //build settings.
        public int    bundleSerial  ;
        public string carryOption   ;
    }

    internal static class BuildEnvironment
    {
        internal static EnvironmentArguments ParseEnvironment()
        {
            var args = new EnvironmentArguments();

            args.targetPlatform = GetEnvString("_target_platform" , "");
            args.targetProduct  = GetEnvString("_target_product"  , "");

            args.apkKeystore    = GetEnvString("_apk_keystore"    , "");
            args.ipaProvision   = GetEnvString("_ipa_provision"   , "");

            args.appPackageId   = GetEnvString("_app_package_id"  , "com.enterprise.game");
            args.appVersionStr  = GetEnvString("_app_verison_str" , "1.0.0");
            args.appVersionNum  = GetEnvInt   ("_app_version_num" , 1 );

            args.packageSerial  = GetEnvInt   ("_package_serial"  , 0 );
            args.firstLanguage  = GetEnvString("_first_language"  , "");
            args.storeChannel   = GetEnvString("_store_channel"   , "");
            args.channelGateway = GetEnvString("_channel_gateway" , "");
            args.forcedAssetURL = GetEnvString("_forced_asset_url", "none");
            args.forcedPatchURL = GetEnvString("_forced_patch_url", "none");
            args.assetFlavors   = GetStringSet("_asset_flavors"   );
            args.userFlags      = GetObjDict  ("_user_flags"      );

            args.bundleSerial   = GetEnvInt   ("_bundle_serial"   , 0 );
            args.carryOption    = GetEnvString("_carry_option"    , "");

            Log.Info("_target_platform : {0}", args.targetPlatform);
            Log.Info("_target_product  : {0}", args.targetProduct );
            Log.Info("_apk_keystore    : {0}", args.apkKeystore   );
            Log.Info("_ipa_provision   : {0}", args.ipaProvision  );
            Log.Info("_app_package_id  : {0}", args.appPackageId  );
            Log.Info("_app_verison_str : {0}", args.appVersionStr );
            Log.Info("_app_version_num : {0}", args.appVersionNum );
            Log.Info("_package_serial  : {0}", args.packageSerial );
            Log.Info("_first_language  : {0}", args.firstLanguage );
            Log.Info("_store_channel   : {0}", args.storeChannel  );
            Log.Info("_channel_gateway : {0}", args.channelGateway);
            Log.Info("_forced_asset_url: {0}", args.forcedAssetURL);
            Log.Info("_forced_patch_url: {0}", args.forcedPatchURL);

            int flavorCount = args.assetFlavors.Count;
            int flavorIndex = 0;
            Log.Info("_aaset_flavors count: {0}", flavorCount);
            foreach (string item in args.assetFlavors)
            {
                Log.Info("_asset_flavors {0}/{1}: {2}", ++flavorIndex, flavorCount, item);
            }

            int flagCount = args.userFlags.Count;
            int flagIndex = 0;
            Log.Info("_user_flags count: {0}", flagCount);
            foreach (KeyValuePair<string, object> entry in args.userFlags)
            {
                Log.Info("_user_flags {0}/{1}: {2}: {3}", ++flagIndex, flagCount, entry.Key, entry.Value);
            }

            Log.Info("_bundle_serial: {0}", args.bundleSerial);
            Log.Info("_carry_option : {0}", args.carryOption );

            return args;
        }

        private static string GetEnvString(string name, string defaultValue)
        {
            string value = Environment.GetEnvironmentVariable(name);
            return !string.IsNullOrWhiteSpace(value) ? value.Trim() : defaultValue;
        }

        private static int GetEnvInt(string name, int defaultValue)
        {
            string value = Environment.GetEnvironmentVariable(name);
            try
            {
                return int.Parse(value);
            }
            catch
            {
                return defaultValue;
            }
        }

        private static HashSet<string> GetStringSet(string name)
        {
            var set = new HashSet<string>();

            string raw = Environment.GetEnvironmentVariable(name);
            if (string.IsNullOrWhiteSpace(raw))
            {
                return set;
            }

            string[] items = raw.Split(';');
            foreach (string item in items)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    set.Add(item);
                }
            }

            return set;
        }

        private static Dictionary<string, object> GetObjDict(string name)
        {
            var dict = new Dictionary<string, object>();

            string raw = Environment.GetEnvironmentVariable(name);
            if (string.IsNullOrWhiteSpace(raw))
            {
                return dict;
            }

            string[] entries = raw.Split(";");
            foreach (string entry in entries)
            {
                if (string.IsNullOrWhiteSpace(entry))
                {
                    continue;
                }

                string[] pair = entry.Split(":");
                if (pair.Length != 2                   ||
                    string.IsNullOrWhiteSpace(pair[0]) ||
                    string.IsNullOrWhiteSpace(pair[1]) )
                {
                    continue;
                }

                string key   = pair[0].Trim();
                string value = pair[1].Trim();

                if (bool.TryParse(value, out bool boolValue))
                {
                    dict[key] = boolValue;
                }
                else if (int.TryParse(value, out int intValue))
                {
                    dict[key] = intValue;
                }
                else
                {
                    dict[key] = value;
                }
            }

            return dict;
        }

        internal static List<string> CheckEnvironment(EnvironmentArguments args)
        {
            var errors = new List<string>();

            return errors;
        }
    }
}
