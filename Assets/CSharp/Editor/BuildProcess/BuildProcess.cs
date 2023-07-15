using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace U3DMobileEditor
{
    internal static class BuildProcess
    {
        internal static void Launch()
        {
            BuildEnvironment.ParseEnvironment();
        }
    }
}
