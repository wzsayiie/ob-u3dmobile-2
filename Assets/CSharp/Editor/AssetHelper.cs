using System.IO;
using UnityEditor;
using UnityEngine;

namespace U3DMobileEditor
{
    internal static class AssetHelper
    {
        internal static T LoadScriptable<T>(string path) where T : ScriptableObject
        {
            //try load the asset.
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset != null)
            {
                return asset;
            }

            //create the asset if needed:
            string dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
            return asset;
        }
    }
}
