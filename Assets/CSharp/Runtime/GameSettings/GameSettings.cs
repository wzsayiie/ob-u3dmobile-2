using System;
using System.Collections.Generic;
using UnityEngine;

namespace U3DMobile
{
    //installation package serial.
    [Serializable]
    public class PackageSerial
    {
        public int serial;
    }

    //game language.
    [Serializable]
    public class GameLanguage
    {
        public string language;
    }

    //store channel:
    [Serializable]
    public class StoreChannel
    {
        public string channel;
    }

    [Serializable]
    public class ChannelGateway
    {
        public string channel;
        public string gateway;
    }

    //forced urls:
    [Serializable]
    public class ForcedUrlItem
    {
        public bool   enabled;
        public string url    ;
    }

    [Serializable]
    public class ForcedUrls
    {
        public ForcedUrlItem asset;
        public ForcedUrlItem patch;
    }

    //asset flavor.
    [Serializable]
    public class AssetFlavor
    {
        public bool   enabled;
        public string flavor ;
    }

    //user flag:
    [Serializable]
    public enum UserFlagType
    {
        Bool   = 0,
        Int    = 1,
        String = 2,
    }

    [Serializable]
    public class UserFlag
    {
        public string       name       ;
        public UserFlagType type       ;
        public bool         boolValue  ;
        public int          intValue   ;
        public string       stringValue;
    }

    //game settings.
    public class GameSettings : ScriptableObject
    {
        public const string SavedPath = "Assets/Resources/GameSettings.asset";

        //use a custom package serial.
        //the "version" and "version code" fields provided by the system may be used
        //by publishers for other purposes.
        [SerializeField]
        private PackageSerial _packageSerial;

        public int GetPackageSerial()
        {
            return _packageSerial != null ? _packageSerial.serial : 0;
        }

        //generally speaking, there is no need to dynamically switch languages within a game.
        //however, games released in southeast asia are an exception,
        //as there is a large population of chinese and english speakers in the region.
        [SerializeField] private string _preferredLanguage;
        [SerializeField] private List<GameLanguage> _languages;

        public string GetPreferredLanguage()
        {
            if (string.IsNullOrWhiteSpace(_preferredLanguage))
            {
                return null;
            }
            if (_languages == null || _languages.Count == 0)
            {
                return null;
            }

            string target = _preferredLanguage.Trim();
            foreach (GameLanguage item in _languages)
            {
                if (item != null && item.language == target)
                {
                    return target;
                }
            }

            return null;
        }

        public bool IsLegalLanguage(string language)
        {
            if (string.IsNullOrWhiteSpace(language))
            {
                return false;
            }
            if (_languages == null || _languages.Count == 0)
            {
                return false;
            }

            string target = language.Trim();
            foreach (GameLanguage item in _languages)
            {
                if (item != null && item.language == target)
                {
                    return true;
                }
            }

            return false;
        }

        //games will be distributed to different "channel"s,
        //such as different app stores, and different testing environments.
        [SerializeField] private string _activeChannel;
        [SerializeField] private List<StoreChannel> _channels;

        public string GetChannel()
        {
            if (string.IsNullOrWhiteSpace(_activeChannel))
            {
                return null;
            }
            if (_channels == null || _channels.Count == 0)
            {
                return null;
            }

            string target = _activeChannel.Trim();
            foreach (StoreChannel item in _channels)
            {
                if (item != null && item.channel == target)
                {
                    return target;
                }
            }

            return null;
        }

        public bool IsLegalChannel(string channel)
        {
            if (string.IsNullOrWhiteSpace(channel))
            {
                return false;
            }
            if (_channels == null || _channels.Count == 0)
            {
                return false;
            }

            string target = channel.Trim();
            foreach (StoreChannel item in _channels)
            {
                if (item != null && item.channel == target)
                {
                    return true;
                }
            }
            return false;
        }

        //packages from different channels may connect to the same gateway,
        //for example, packages from various app stores in the same region.
        [SerializeField] private string _activeGateway;
        [SerializeField] private List<ChannelGateway> _gateways;

        public string GetGateway()
        {
            if (string.IsNullOrWhiteSpace(_activeGateway))
            {
                return null;
            }
            if (_gateways == null || _gateways.Count == 0)
            {
                return null;
            }

            string target = _activeGateway.Trim();
            foreach (ChannelGateway item in _gateways)
            {
                if (item == null)
                {
                    continue;
                }
                if (item.channel != target)
                {
                    continue;
                }
                if (string.IsNullOrWhiteSpace(item.gateway))
                {
                    continue;
                }

                return item.gateway.Trim();
            }
            return null;
        }

        public bool IsLegalGateway(string gateway)
        {
            if (string.IsNullOrWhiteSpace(gateway))
            {
                return false;
            }
            if (_gateways == null || _gateways.Count == 0)
            {
                return false;
            }

            string target = gateway.Trim();
            foreach (ChannelGateway item in _gateways)
            {
                if (item == null)
                {
                    continue;
                }
                if (string.IsNullOrWhiteSpace(item.channel))
                {
                    continue;
                }
                if (item.gateway != target)
                {
                    continue;
                }

                return true;
            }
            return false;
        }

        //sometimes the game needs to download assets and patches from a specific server,
        //which is more convenient for debugging.
        [SerializeField]
        private ForcedUrls _forcedUrls;

        public string GetForcedAssetUrl() { return GetForcedUrl(_forcedUrls?.asset); }
        public string GetForcedPatchUrl() { return GetForcedUrl(_forcedUrls?.patch); }

        private string GetForcedUrl(ForcedUrlItem item)
        {
            if (item == null)
            {
                return null;
            }
            if (!item.enabled || string.IsNullOrWhiteSpace(item.url))
            {
                return null;
            }

            return item.url.Trim();
        }

        //packages of different channels may contain different assets with same names,
        //such as different game icons.
        //use "flavor" to differentiate them.
        [SerializeField]
        private List<AssetFlavor> _assetFlavors;

        public HashSet<string> GetEnabledFlavors()
        {
            if (_assetFlavors == null || _assetFlavors.Count == 0)
            {
                return null;
            }

            var enabled = new HashSet<string>();
            foreach (AssetFlavor item in _assetFlavors)
            {
                if (item == null)
                {
                    continue;
                }
                if (!item.enabled || string.IsNullOrWhiteSpace(item.flavor))
                {
                    continue;
                }

                string flavor = item.flavor.Trim();
                enabled.Add(flavor);
            }
            return enabled.Count > 0 ? enabled : null;
        }

        //some flags may be required in games,
        //such as whether to enable the manager, whether to skip the novice guide.
        [SerializeField]
        private List<UserFlag> _userFlags;

        public bool GetBoolFlag(string name)
        {
            UserFlag flag = GetUserFlag(name, UserFlagType.Bool);
            return flag != null && flag.boolValue;
        }

        public int GetIntFlag(string name)
        {
            UserFlag flag = GetUserFlag(name, UserFlagType.Int);
            return flag != null ? flag.intValue : 0;
        }

        public string GetStringFlag(string name)
        {
            UserFlag flag = GetUserFlag(name, UserFlagType.String);
            if (flag != null && !string.IsNullOrWhiteSpace(flag.stringValue))
            {
                return flag.stringValue;
            }
            else
            {
                return null;
            }
        }

        private UserFlag GetUserFlag(string name, UserFlagType type)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }
            if (_userFlags == null || _userFlags.Count == 0)
            {
                return null;
            }

            foreach (UserFlag item in _userFlags)
            {
                if (item == null)
                {
                    continue;
                }
                if (item.name == null || item.name.Trim() != name)
                {
                    continue;
                }
                if (item.type != type)
                {
                    continue;
                }

                return item;
            }
            return null;
        }
    }
}
