using UnityEditor;
using UnityEngine;

namespace U3DMobile.Edit
{
    internal static class UIHelper
    {
        internal static void PingPath<T>(string path) where T : ScriptableObject
        {
            var obj = AssetHelper.LoadScriptable<T>(path);

            Selection.activeObject = obj;
            EditorGUIUtility.PingObject(obj);
        }
    }
}
