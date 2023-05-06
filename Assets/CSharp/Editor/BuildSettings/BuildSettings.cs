using System;
using System.Collections.Generic;
using UnityEngine;

namespace U3DMobileEditor
{
    //bundle entry:
    [Serializable]
    internal enum PackStrategy
    {
        Entire  = 0,
        SubDir  = 1,
        SubFile = 2,
    }

    [Serializable]
    internal enum RuntimeDemand
    {
        Necessary = 0,
        OnDemand  = 1,
    }

    [Serializable]
    internal class BundleEntry
    {
        public UnityEngine.Object file    ;
        public PackStrategy       strategy;
        public RuntimeDemand      demand  ;
        public string             carryOpt;
    }

    //build settings.
    internal class BuildSettings : ScriptableObject
    {
        [SerializeField]
        List<BundleEntry> _entries;
    }
}
