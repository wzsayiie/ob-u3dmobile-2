using System;
using System.Collections.Generic;
using U3DMobile;
using UnityEditor.PackageManager;
using UnityEngine;

namespace U3DMobileEditor
{
    //internal static class BuildEnvironment
    //{
    //    //target.
    //    internal static string targetPlatform;
    //    internal static string targetProduct ;

    //    //asset bundle serial.
    //    internal static int bundleSerial;

    //    //package settings:
    //    internal static int    packageSerial ;
    //    internal static string storeChannel  ;
    //    internal static string channelGateway;
    //    internal static string forcedAssetURL;
    //    internal static string forcedPatchURL;
    //    internal static string assetFlavor   ;
    //    internal static string carryOption   ;

    //    internal static Dictionary<string, List<string>> userFlags;

    //    //apk/aab keystore.
    //    internal static string jksFile;
    //    internal static string jksFilePassword;
    //    internal static string jksKey;
    //    internal static string jksKeyPassword;

    //    //ipa mobileprovision.
    //    internal static string mobileProvision;

    //    //app information.
    //    internal static string appPackageID ;
    //    internal static string appVersionStr;
    //    internal static string appVersionNum;

    //    internal static List<string> loadErrors = new List<string>();

    //    internal static void Load()
    //    {
    //        loadErrors.Clear();

    //        //target platform:
    //        targetPlatform = GetEnvironmentString("_target_platform");

    //        if (targetPlatform != "android" &&
    //            targetPlatform != "ios"     )
    //        {
    //            loadErrors.Add($"unexpected platform '{targetPlatform}'");
    //            return;
    //        }

    //        //target product:
    //        targetProduct = GetEnvironmentString("_target_product");

    //        if (targetPlatform == "android" &&
    //            targetProduct  != "apk"     &&
    //            targetProduct  != "aab"     &&
    //            targetProduct  != "bundle"  )
    //        {
    //            loadErrors.Add(
    //                $"unexpected product '{targetProduct}' for platform '{targetPlatform}'"
    //            );
    //            return;
    //        }

    //        if (targetPlatform == "ios"    &&
    //            targetProduct  != "ipa"    &&
    //            targetProduct  != "bundle" )
    //        {
    //            loadErrors.Add(
    //                $"unexpected product '{targetProduct}' for platform '{targetPlatform}'"
    //            );
    //            return;
    //        }

    //        //asset bundle serial:
    //        string bundleSerialStr = GetEnvironmentString("_bundle_serial");
    //        try
    //        {
    //            bundleSerial = int.Parse(bundleSerialStr);
    //        }
    //        catch
    //        {
    //            loadErrors.Add($"unexpected bundle serial '{bundleSerialStr}'");
    //        }

    //        //package settings:
    //        bool needAndroidPackage = targetProduct == "apk" || targetProduct == "aab";
    //        bool needIOSPackage     = targetProduct == "ipa" ;

    //        if (needAndroidPackage || needIOSPackage)
    //        {
    //            //package serial.
    //            string packageSerialStr = GetEnvironmentString("_package_serial");
    //            try
    //            {
    //                packageSerial = int.Parse(packageSerialStr);
    //            }
    //            catch
    //            {
    //                loadErrors.Add($"unexpected package serial '{packageSerialStr}'");
    //            }

    //            //store channel:
    //            storeChannel = GetEnvironmentString("_store_channel");

    //            var gameSettings = AssetHelper.LoadScriptable<GameSettings>(GameSettings.SavedPath);
    //            if (!gameSettings.IsLegalChannel(storeChannel))
    //            {
    //                loadErrors.Add($"illegal store channel '{storeChannel}'");
    //            }

    //            //channel gateway.
    //            channelGateway = GetEnvironmentString("_channel_gateway");
    //            if (!gameSettings.IsLegalGateway(channelGateway))
    //            {
    //                loadErrors.Add($"illegal channel gateway '{channelGateway}'");
    //            }

    //            //forced asset/patch url.
    //            forcedAssetURL = GetEnvironmentString("_forced_asset_url");
    //            forcedPatchURL = GetEnvironmentString("_forced_patch_url");
    //        }
    //    }

    //    internal static void Reset()
    //    {
    //        if (loadErrors.Count > 0)
    //        {
    //            Debug.Log("can not reset settings cause load error");
    //            return;
    //        }
    //    }

    //    private static string GetEnvironmentString(string name)
    //    {
    //        string value = Environment.GetEnvironmentVariable(name);
    //        return value != null ? value.Trim() : null;
    //    }

    //    private static HashSet<string> GetEnvironmentHashSet(string name)
    //    {
    //        return null;
    //    }
    //}
}
