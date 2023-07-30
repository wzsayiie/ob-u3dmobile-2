using System;
using System.Collections.Generic;
using UnityEngine;

namespace U3DMobileEditor
{
    //build parameters:
    [Serializable]
    internal class BundleSerial
    {
        public int serial;
    }

    [Serializable]
    internal class ForceRebuildCheck
    {
        public bool forceRebuild;
    }

    [Serializable]
    internal class UsePastBundleCheck
    {
        public bool usePastBundle;
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

        internal void SetBundleSerial(int serial)
        {
            _bundleSerial ??= new BundleSerial();
            _bundleSerial.serial = serial;
        }

        internal int GetBundleSerial()
        {
            return _bundleSerial != null ? _bundleSerial.serial : 0;
        }

        //force unity rebuild asset bundles.
        [SerializeField]
        private ForceRebuildCheck _forceRebuild;

        internal void SetForceRebuild(bool forceRebuild)
        {
            _forceRebuild ??= new ForceRebuildCheck();
            _forceRebuild.forceRebuild = forceRebuild;
        }

        internal bool IsForceRebuild()
        {
            return _forceRebuild != null && _forceRebuild.forceRebuild;
        }

        //use past asset bundle cache.
        [SerializeField]
        private UsePastBundleCheck _usePastBundle;

        internal void SetUsePastCache(bool usePastBundle)
        {
            _usePastBundle ??= new UsePastBundleCheck();
            _usePastBundle.usePastBundle = usePastBundle;
        }

        internal bool IsUsePastCache()
        {
            return _usePastBundle != null && _usePastBundle.usePastBundle;
        }

        //for different distribution channels,
        //the asset bundles put into the installation package may be different.
        //use the "CarryOption" to control this point.
        [SerializeField] private string _currentCarry;
        [SerializeField] private List<CarryOption> _carryOptions;

        internal bool IsValidCarry(string option)
        {
            if (string.IsNullOrWhiteSpace(option))
            {
                return false;
            }
            if (_carryOptions == null || _carryOptions.Count == 0)
            {
                return false;
            }

            option = option.Trim();
            foreach (CarryOption item in _carryOptions)
            {
                if (item               != null  &&
                    item.option        != null  &&
                    item.option.Trim() == option)
                {
                    return true;
                }
            }
            return false;
        }

        internal void SetCurrentCarry(string option)
        {
            _currentCarry = !string.IsNullOrWhiteSpace(option) ? option.Trim(): "";
        }

        internal string CurrentCarry()
        {
            return !string.IsNullOrWhiteSpace(_currentCarry) ? _currentCarry.Trim() : null;
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
