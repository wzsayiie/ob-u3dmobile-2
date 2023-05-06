using System;
using System.Collections.Generic;
using UnityEngine;

namespace U3DMobileEditor
{
    //bundle identifier:
    [Serializable]
    internal class BundleIdentifier
    {
        public int iden;
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

    //build settings.
    internal class BuildSettings : ScriptableObject
    {
        //
        [SerializeField]
        private BundleIdentifier _identifier;

        internal int GetBundleIdentifier()
        {
            return _identifier != null ? _identifier.iden : 0;
        }

        //
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

        //
        [SerializeField]
        private List<BundleEntry> _entries;

        internal List<BundleEntry> GetEntries()
        {
            return _entries != null && _entries.Count > 0 ? _entries : null;
        }
    }
}
