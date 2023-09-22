using System;
using System.Collections.Generic;
using UnityEngine;

namespace U3DMobileEditor
{
    //generally speaking, there is no need to dynamically switch languages within a game.
    //however, games released in southeast asia are an exception,
    //as there is a large population of chinese and english speakers in the region.
    [Serializable]
    internal class GameLanguage
    {
        public string language;
    }

    //games will be distributed to different "channel"s,
    //such as different app stores, and different testing environments.
    [Serializable]
    internal class StoreChannel
    {
        public string channel;
    }

    //packages from different channels may connect to the same gateway,
    //for example, packages from various app stores in the same region.
    [Serializable]
    internal class ChannelGateway
    {
        public string channel;
        public string gateway;
    }

    //sometimes the game needs to download assets and patches from a specific server,
    //which is more convenient for debugging.
    [Serializable]
    internal class ForcedURL
    {
        public string name;
        public string url ;
    }

    //packages of different channels may contain different assets with same names,
    //such as different game icons.
    //use "flavor" to differentiate them.
    [Serializable]
    internal class AssetFlavor
    {
        public string flavor;
    }

    internal class GameOptions : ScriptableObject
    {
        internal const string SavedPath = "Assets/GameOptions.asset";

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
            //the first item "none" is default.
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
            //the first item "none" is default.
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
