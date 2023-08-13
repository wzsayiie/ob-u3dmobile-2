using System;
using System.Text;
using UnityEngine;

namespace U3DMobile
{
    public static class Log
    {
        private static StringBuilder s_infoBuidler ;
        private static StringBuilder s_errorBuilder;

        public static void Group(Action action)
        {
            if (action == null)
            {
                return;
            }

            s_infoBuidler  = new StringBuilder();
            s_errorBuilder = new StringBuilder();

            action();

            if (s_infoBuidler .Length > 0) { InfoFormat ("{0}", s_infoBuidler .ToString()); }
            if (s_errorBuilder.Length > 0) { ErrorFormat("{0}", s_errorBuilder.ToString()); }
            s_infoBuidler  = null;
            s_errorBuilder = null;
        }

        public static void Info(string format, params object[] objects)
        {
            if (s_infoBuidler != null)
            {
                s_infoBuidler.AppendFormat(format, objects);
                s_infoBuidler.AppendLine();
            }
            else
            {
                InfoFormat(format, objects);
            }
        }

        public static void Error(string format, params object[] objects)
        {
            if (s_errorBuilder != null)
            {
                s_errorBuilder.AppendFormat(format, objects);
                s_errorBuilder.AppendLine();
            }
            else
            {
                ErrorFormat(format, objects);
            }
        }

        public static void InfoString (string text) { InfoFormat ("{0}", text); }
        public static void ErrorString(string text) { ErrorFormat("{0}", text); }

        public static void InfoFormat(string format, params object[] objects)
        {
            Debug.LogFormat(format, objects);
        }

        public static void ErrorFormat(string format, params object[] objects)
        {
            Debug.LogErrorFormat(format, objects);
        }
    }
}
