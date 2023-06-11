using System;
using System.Collections.Generic;
using UnityEngine;

namespace U3DMobileEditor
{
    //bundle serial.
    [Serializable]
    internal class BundleSerial
    {
        public int serial;
    }

    //bundle carry option.
    [Serializable]
    internal class CarryOption
    {
        public string name;
    }

    //bundle entry:
    [Serializable]
    internal enum PackMode
    {
        Entire  = 0,
        SubDir  = 1,
        SubFile = 2,
    }

    [Serializable]
    internal enum DemandMode
    {
        Necessary = 0,
        OnDemand  = 1,
    }

    [Serializable]
    internal class BundleEntry
    {
        public bool               selected  ;
        public UnityEngine.Object file      ;
        public PackMode           packMode  ;
        public DemandMode         demandMode;
        public string             carryOpts ;
    }

    //patch entry.
    [Serializable]
    internal class PatchEntry
    {
        public bool               selected;
        public UnityEngine.Object file    ;
    }

    //build settings.
    internal class BuildSettings : ScriptableObject
    {
        internal const string SavedPath = "Assets/BuildSettings.asset";

        //asset bundle set serial.
        [SerializeField]
        private BundleSerial _serial;

        internal int GetBundleSerial()
        {
            return _serial != null ? _serial.serial : 0;
        }

        //for different distribution channels,
        //the asset bundles put into the installation package may be different.
        //use the "CarryOption" to control this point.
        [SerializeField]
        private string _activeCarry;
        [SerializeField]
        private List<CarryOption> _carryOptions;

        internal string GetCarryOption()
        {
            if (_carryOptions == null)
            {
                return null;
            }

            string name = _activeCarry.Trim();

            //the target must be an item in the list.
            if (_carryOptions == null || _carryOptions.Count == 0)
            {
                return null;
            }
            foreach (CarryOption candicate in _carryOptions)
            {
                if (candicate != null &&
                    !string.IsNullOrWhiteSpace(candicate.name) &&
                    name == candicate.name.Trim())
                {
                    return name;
                }
            }
            return null;
        }

        //asset bundle entries.
        [SerializeField]
        private List<BundleEntry> _entries;

        internal List<BundleEntry> GetEntries()
        {
            return _entries != null && _entries.Count > 0 ? _entries : null;
        }

        //script patch entries.
        [SerializeField]
        private List<PatchEntry> _patches;

        internal List<PatchEntry> GetPatches()
        {
            return _patches != null && _patches.Count > 0 ? _patches : null;
        }
    }
}
