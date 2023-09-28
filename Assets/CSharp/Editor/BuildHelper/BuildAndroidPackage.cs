using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace U3DMobile.Edit
{
    internal static class BuildAndroidPackage
    {
        internal static void ExportAAB(string keystore, List<string> errors) { Export("aab", keystore, errors); }
        internal static void ExportAPK(string keystore, List<string> errors) { Export("apk", keystore, errors); }

        private static void Export(string product, string keystore, List<string> errors)
        {
            //检查参数:
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
            {
                errors.Add(I18N.TargetIsNotAndroid);
            }

            if (string.IsNullOrWhiteSpace(keystore))
            {
                errors.Add(I18N.NoKeystoreName);
            }

            string jksFile     = null;
            string jksPassFile = null;
            string keyFile     = null;
            string keyPassFile = null;

            if (!string.IsNullOrWhiteSpace(keystore))
            {
                jksFile     = BuildKey.APKJKSFile    (keystore);
                jksPassFile = BuildKey.APKJKSPassFile(keystore);
                keyFile     = BuildKey.APKKeyFile    (keystore);
                keyPassFile = BuildKey.APKKeyPassFile(keystore);

                if (!File.Exists(jksFile    )) { errors.Add($"{I18N.NoJKSFile    }: {jksFile    }"); }
                if (!File.Exists(jksPassFile)) { errors.Add($"{I18N.NoJKSPassFile}: {jksPassFile}"); }
                if (!File.Exists(keyFile    )) { errors.Add($"{I18N.NoKeyFile    }: {keyFile    }"); }
                if (!File.Exists(keyPassFile)) { errors.Add($"{I18N.NoKeyPassFile}: {keyPassFile}"); }
            }

            if (errors.Count > 0)
            {
                return;
            }

            //签名:
            string jksPassText = File.ReadAllText(jksPassFile);
            string keyText     = File.ReadAllText(keyFile    );
            string keyPassText = File.ReadAllText(keyPassFile);

            PlayerSettings.Android.keystoreName = jksFile     ;
            PlayerSettings.Android.keystorePass = jksPassText != null ? jksPassText.Trim() : "";
            PlayerSettings.Android.keyaliasName = keyText     != null ? keyText    .Trim() : "";
            PlayerSettings.Android.keyaliasPass = keyPassText != null ? keyPassText.Trim() : "";

            //构建参数:
            var options = new BuildPlayerOptions
            {
                scenes = new []{ "Assets/Game.unity" },
                target = BuildTarget.Android,
            };

            EditorUserBuildSettings.exportAsGoogleAndroidProject = false;

            if (product == "aab")
            {
                options.locationPathName = $"{BuildPath.outputDirectory}/package.aab";

                EditorUserBuildSettings.buildAppBundle      = true;
                PlayerSettings.Android.useAPKExpansionFiles = true;
            }
            else //apk.
            {
                options.locationPathName = $"{BuildPath.outputDirectory}/package.apk";

                EditorUserBuildSettings.buildAppBundle      = false;
                PlayerSettings.Android.useAPKExpansionFiles = false;
            }

            //构建.
            BuildReport report = BuildPipeline.BuildPlayer(options);
            if (report.summary.result != BuildResult.Succeeded)
            {
                foreach (BuildStep step in report.steps)
                {
                    foreach (BuildStepMessage message in step.messages)
                    {
                        if (message.type == LogType.Exception ||
                            message.type == LogType.Error     )
                        {
                            errors.Add($"{I18N.Error} {step.name}: {message.content}");
                        }
                    }
                }
            }
        }
    }
}
