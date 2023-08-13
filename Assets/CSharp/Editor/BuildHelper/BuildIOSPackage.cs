using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;

namespace U3DMobileEditor
{
    internal static class BuildIOSPackage
    {
        internal static void ExportXCProject(string provision, List<string> errors)
        {
            //check arguments:
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.iOS)
            {
                errors.Add("active build target is not ios");
            }

            if (errors.Count > 0)
            {
                return;
            }

            //build options.
            var options = new BuildPlayerOptions
            {
                locationPathName = $"{BuildPath.GetOutputDirectory()}/xcproject",
                scenes           = new []{ "Assets/Game.unity" },
                target           = BuildTarget.iOS,
            };

            //build.
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
                            errors.Add($"{step.name}: {message.content}");
                        }
                    }
                }
            }
        }

        [PostProcessBuild]
        internal static void OnPostProcessBuild(BuildTarget target, string path)
        {
            if (target != BuildTarget.iOS)
            {
                return;
            }
        }
    }
}
