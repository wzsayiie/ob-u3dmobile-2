using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace U3DMobileEditor
{
    internal static class BuildAndroidPackage
    {
        internal static void PackAAB(List<string> errors) { Pack("aab", errors); }
        internal static void PackAPK(List<string> errors) { Pack("apk", errors); }

        private static void Pack(string product, List<string> errors)
        {
            //check environment.
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
            {
                errors.Add("active build target is not android");
                return;
            }

            //set options:
            var options = new BuildPlayerOptions();

            if (product == "aab")
            {
                options.locationPathName = $"{BuildEnvironment.GetOutputDirectory()}/package.aab";

                EditorUserBuildSettings.buildAppBundle = true;
                PlayerSettings.Android.useAPKExpansionFiles = true;
            }
            else //apk.
            {
                options.locationPathName = $"{BuildEnvironment.GetOutputDirectory()}/package.apk";

                EditorUserBuildSettings.buildAppBundle = false;
                PlayerSettings.Android.useAPKExpansionFiles = false;
            }

            options.scenes = new[] { "Assets/Game.unity" }; 
            options.target = BuildTarget.Android;

            //build:
            BuildReport report = BuildPipeline.BuildPlayer(options);

            //succeeded.
            if (report.summary.result == BuildResult.Succeeded)
            {
                return;
            }
            //failed.
            foreach (BuildStep step in report.steps)
            {
                foreach (BuildStepMessage message in step.messages)
                {
                    if (message.type != LogType.Exception &&
                        message.type != LogType.Error     )
                    {
                        errors.Add($"{step.name}: {message.content}");
                    }
                }
            }
        }
    }
}
