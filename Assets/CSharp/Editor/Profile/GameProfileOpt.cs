using System;
using System.Collections.Generic;
using UnityEngine;

namespace U3DMobileEditor
{
    //一般而言游戏内不需要动态切换语言.
    //但如果上架东南亚则是个例外, 因为那里有大量汉语和英语人口.
    [Serializable]
    internal class GameLanguage
    {
        public string language;
    }

    //游戏可能被发布到不同的商店渠道上, 如AppStore, Google Play等.
    //不同渠道的安装包表现和行为可能会有所差异, 如隐私条款等.
    [Serializable]
    internal class StoreChannel
    {
        public string channel;
    }

    //即使是同一商店渠道的安装包, 连接的服务器网关也会有所差异. 例如通过同一渠道发布到不同地区.
    //开发环境的内部测试也会导致连接不同网关的情况.
    [Serializable]
    internal class ChannelGateway
    {
        public string channel;
        public string gateway;
    }

    //从CDN服务器下载资产包和代码补丁是游戏的常见做法,
    //可以避免游戏更新后用户从商店重新下载巨大的安装包.
    [Serializable]
    internal class ForcedURL
    {
        public string name;
        public string url ;
    }

    //上架到不同地区的安装包, 使用的资产包可能是不一样的, 例如图标, 启动背景图, 关卡背景等.
    //用"资产变种"区分它们.
    [Serializable]
    internal class AssetFlavor
    {
        public string flavor;
    }

    internal class GameProfileOpt : ScriptableObject
    {
        internal const string SavedPath = "Assets/GameProfileOpt.asset";

        [SerializeField] private List<GameLanguage  > _gameLanguages  ;
        [SerializeField] private List<StoreChannel  > _storeChannels  ;
        [SerializeField] private List<ChannelGateway> _channelGateways;
        [SerializeField] private List<ForcedURL     > _assetURLs      ;
        [SerializeField] private List<ForcedURL     > _patchURLs      ;
        [SerializeField] private List<AssetFlavor   > _assetFlavors   ;

        internal string[] GameLanguages() { return GetItems(_gameLanguages, i => i?.language); }
        internal string[] StoreChannels() { return GetItems(_storeChannels, i => i?.channel ); }

        private string[] GetItems<T>(List<T> list, Func<T, string> pick)
        {
            //第一项"none"是默认值.
            var valueList = new List<string> { "none" };

            if (list != null)
            {
                foreach (T item in list)
                {
                    string value = pick(item);
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        continue;
                    }

                    valueList.Add(value.Trim());
                }
            }

            return valueList.ToArray();
        }

        internal string[][] ChannelGateways() { return GetEntries(_channelGateways, i => i?.channel, i => i?.gateway); }
        internal string[][] AssetURLs      () { return GetEntries(_assetURLs      , i => i?.name   , i => i?.url    ); }
        internal string[][] PatchURLs      () { return GetEntries(_patchURLs      , i => i?.name   , i => i?.url    ); }

        private string[][] GetEntries<T>(List<T> list, Func<T, string> pickK, Func<T, string> pickV)
        {
            //第一项"none"是默认值.
            var keyList   = new List<string>{ "none" };
            var valueList = new List<string>{ "none" };

            if (list != null)
            {
                foreach (T item in list)
                {
                    string key = pickK(item);
                    if (string.IsNullOrWhiteSpace(key))
                    {
                        continue;
                    }

                    string value = pickV(item);
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        continue;
                    }

                    keyList  .Add(key  .Trim());
                    valueList.Add(value.Trim());
                }
            }

            var entries = new string[2][]; {
                entries[0] = keyList  .ToArray();
                entries[1] = valueList.ToArray();
            }
            return entries;
        }

        internal string[] AssetFlavors()
        {
            var list = new List<string>();

            if (_assetFlavors != null)
            {
                foreach (AssetFlavor item in _assetFlavors)
                {
                    if (item != null && !string.IsNullOrWhiteSpace(item.flavor))
                    {
                        list.Add(item.flavor.Trim());
                    }
                }
            }

            return list.ToArray();
        }

        internal bool IsValidGameLanguage  (string target) { return IsOneOf(GameLanguages  ()   , target); }
        internal bool IsValidStoreChannel  (string target) { return IsOneOf(StoreChannels  ()   , target); }
        internal bool IsValidChannelGateway(string target) { return IsOneOf(ChannelGateways()[0], target); }
        internal bool IsValidAssetURL      (string target) { return IsOneOf(AssetURLs      ()[0], target); }
        internal bool IsValidPatchURL      (string target) { return IsOneOf(PatchURLs      ()[0], target); }

        private static bool IsOneOf(string[] options, string target)
        {
            if (options == null || options.Length == 0)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(target))
            {
                return false;
            }

            foreach (string item in options)
            {
                if (item == target)
                {
                    return true;
                }
            }
            return false;
        }

        internal bool IsValidAssetFlavors(HashSet<string> targets, out HashSet<string> illegals)
        {
            if (targets == null || targets.Count == 0)
            {
                illegals = null;
                return true;
            }

            var legals = new HashSet<string>();
            foreach (string item in AssetFlavors())
            {
                legals.Add(item);
            }

            illegals = new HashSet<string>();
            foreach (string item in targets)
            {
                if (!legals.Contains(item))
                {
                    illegals.Add(item);
                }
            }
            return illegals.Count == 0;
        }
    }
}
