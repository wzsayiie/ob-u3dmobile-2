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
        public string option;
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
        public UnityEngine.Object fileObj   ;
        public PackMode           packMode  ;
        public DemandMode         demandMode;
        public string             carryOpts ;
    }

    //patch entry.
    [Serializable]
    internal class PatchEntry
    {
        public bool               selected;
        public UnityEngine.Object fileObj ;
    }

    //build settings.
    internal class BuildSettings : ScriptableObject
    {
        internal const string SavedPath = "Assets/BuildSettings.asset";

        //asset bundle set serial.
        [SerializeField]
        private BundleSerial _bundleSerial;

        internal int GetBundleSerial()
        {
            return _bundleSerial != null ? _bundleSerial.serial : 0;
        }

        //for different distribution channels,
        //the asset bundles put into the installation package may be different.
        //use the "CarryOption" to control this point.
        [SerializeField] private string _activeCarry;
        [SerializeField] private List<CarryOption> _carryOptions;

        internal string GetCarryOption()
        {
            if (string.IsNullOrWhiteSpace(_activeCarry))
            {
                return null;
            }
            if (_carryOptions == null || _carryOptions.Count == 0)
            {
                return null;
            }

            string target = _activeCarry.Trim();
            foreach (CarryOption item in _carryOptions)
            {
                if (item != null && item.option == target)
                {
                    return target;
                }
            }
            return null;
        }

        //asset bundle entries.
        [SerializeField]
        private List<BundleEntry> _bundleEntries;

        internal List<BundleEntry> GetBundleEntries()
        {
            if (_bundleEntries != null && _bundleEntries.Count > 0)
            {
                return _bundleEntries;
            }
            else
            {
                return null;
            }
        }

        //script patch entries.
        [SerializeField]
        private List<PatchEntry> _bundlePatches;

        internal List<PatchEntry> GetBundlePatches()
        {
            if (_bundlePatches != null && _bundlePatches.Count > 0)
            {
                return _bundlePatches;
            }
            else
            {
                return null;
            }
        }
    }
}
