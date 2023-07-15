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
        [SerializeField] private List<ForcedURL     > _forcedAssetURLs;
        [SerializeField] private List<ForcedURL     > _forcedPatchURLs;
        [SerializeField] private List<AssetFlavor   > _assetFlavors   ;

        internal string[] GetGameLanguages() { return GetItems(_gameLanguages, i => i?.language); }
        internal string[] GetStoreChannels() { return GetItems(_storeChannels, i => i?.channel ); }

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

        internal string[][] GetChannelGateways() { return GetEntries(_channelGateways, i => i?.channel, i => i?.gateway); }
        internal string[][] GetForcedAssetURLs() { return GetEntries(_forcedAssetURLs, i => i?.name   , i => i?.url    ); }
        internal string[][] GetForcedPatchURLs() { return GetEntries(_forcedPatchURLs, i => i?.name   , i => i?.url    ); }

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

        internal string[] GetAssetFlavors()
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
    }
}