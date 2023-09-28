using System;
using System.Collections.Generic;
using UnityEngine;

namespace U3DMobile.Edit
{
    //构建参数:
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

    //搭载选项.
    [Serializable]
    internal class CarryOption
    {
        public string option;
    }

    //资产条目:
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
        public UnityEngine.Object fileObj   ;
        public PackMode           packMode  ;
        public DemandMode         demandMode;
        public string             carryOpts ;
    }

    //补丁条目.
    [Serializable]
    internal class PatchEntry
    {
        public UnityEngine.Object fileObj;
    }

    //构建设置.
    internal class BuildProfile : ScriptableObject
    {
        internal const string SavedPath = "Assets/BuildProfile.asset";

        //资产序列号.
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

        //强制构建.
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

        //复用资产.
        [SerializeField]
        private UsePastBundleCheck _usePastBundle;

        internal void SetUsePastBundle(bool usePastBundle)
        {
            _usePastBundle ??= new UsePastBundleCheck();
            _usePastBundle.usePastBundle = usePastBundle;
        }

        internal bool IsUsePastBundle()
        {
            return _usePastBundle != null && _usePastBundle.usePastBundle;
        }

        //一般而言开发者倾向于安装包搭载所有资产, 这样用户从商店安装游戏后不需要再下载额外的资产包.
        //遗憾的是某些商店, 如Google Play, 对安装包的大小有所限制.
        //用"搭载选项"来描述哪些资产会被放在安装包内.
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

        //资产条目.
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

        //补丁条目.
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
