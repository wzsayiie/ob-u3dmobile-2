using System.Collections.Generic;
using U3DMobile;

namespace U3DMobileEditor
{
    internal static class BuildProcess
    {
        internal static void Launch()
        {
            //parse arguments.
            BuildArguments args = BuildEnvironment.ParseEnvironment();

            //check arguments.
            List<string> errors = BuildEnvironment.CheckEnvironment(args);
            if (errors != null && errors.Count > 0)
            {
                for (int i = 0; i < errors.Count; ++i)
                {
                    Log.Error("Argument Error ({0}/{1}): {2}", i + 1, errors.Count, errors[i]);
                }
                return;
            }
        }
    }
}
