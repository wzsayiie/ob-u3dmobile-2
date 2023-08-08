using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace U3DMobileEditor
{
    internal static class BuildIOSPackage
    {
        internal static void PackIPA(List<string> errors)
        {
            //check environment.
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.iOS)
            {
                errors.Add("active build target is not ios");
                return;
            }

            //set options.
            var options = new BuildPlayerOptions
            {
                locationPathName = $"{BuildEnvironment.GetOutputDirectory()}/xcproject",
                scenes           = new []{ "Assets/Game.unity" },
                target           = BuildTarget.iOS,
            };

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
                    if (message.type == LogType.Exception ||
                        message.type == LogType.Error     )
                    {
                        errors.Add($"{step.name}: {message.content}");
                    }
                }
            }
        }
    }
}
