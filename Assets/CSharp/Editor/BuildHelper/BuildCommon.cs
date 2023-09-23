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
                //如果环境变量没有指定路径, 使用默认路径.
                string path = Environment.GetEnvironmentVariable("_output_dir");
                if (string.IsNullOrWhiteSpace(path))
                {
                    path = "BUILD/Local";
                }

                //确保路径是存在的.
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
