using System;
using System.IO;

namespace U3DMobileEditor
{
    internal static class BuildKey
    {
        internal static string APKJKSFile      (string name) { return $"keystores/android/{name}.jks"        ; }
        internal static string APKJKSPassFile  (string name) { return $"keystores/android/{name}.jkspass.txt"; }
        internal static string APKKeyFile      (string name) { return $"keystores/android/{name}.key.txt"    ; }
        internal static string APKKeyPassFile  (string name) { return $"keystores/android/{name}.keypass.txt"; }

        internal static string IPAPrivKeyFile  (string name) { return $"keystores/ios/{name}.privkey.txt"    ; }
        internal static string IPAProvisionFile(string name) { return $"keystores/ios/{name}.mobileprovision"; }
    }

    internal static class BuildPath
    {
        internal static string outputDirectory
        {
            get
            {
                //user-specified path.
                string path = Environment.GetEnvironmentVariable("_output_dir");
                //default path.
                if (string.IsNullOrWhiteSpace(path))
                {
                    path = "BUILD/Local";
                }

                //make sure the directory exists.
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return path;
            }
        }

        internal static string assetFlavorDirectory
        {
            get
            {
                return "AssetFlavors";
            }
        }
    }
}
