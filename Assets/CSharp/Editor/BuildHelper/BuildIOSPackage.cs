using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;

namespace U3DMobile.Edit
{
    internal static class BuildIOSPackage
    {
        internal static void ExportXCProject(string provision, List<string> errors)
        {
            //检查参数:
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.iOS)
            {
                errors.Add(I18N.TargetIsNotIOS);
            }

            if (errors.Count > 0)
            {
                return;
            }

            //构建参数.
            var options = new BuildPlayerOptions
            {
                locationPathName = $"{BuildPath.outputDirectory}/xcproject",
                scenes           = new []{ "Assets/Game.unity" },
                target           = BuildTarget.iOS,
            };

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
