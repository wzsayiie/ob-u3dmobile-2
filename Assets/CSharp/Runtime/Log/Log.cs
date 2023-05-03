using UnityEngine;

namespace U3DMobile
{
    public static class Log
    {
        public static void Info(string format, params object[] objects)
        {
            Debug.LogFormat(format, objects);
        }

        public static void Error(string format, params object[] objects)
        {
            Debug.LogErrorFormat(format, objects);
        }

        public static void InfoString(string text)
        {
            Debug.Log(text);
        }

        public static void ErrorString(string text)
        {
            Debug.LogError(text);
        }
    }
}
