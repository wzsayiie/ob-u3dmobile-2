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
        public string name   ;
        public bool   enabled;
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
        private PackageSerial _serial;

        public int GetPackageSerial()
        {
            return _serial != null ? _serial.serial : 0;
        }

        //games will be distributed to different "channel"s,
        //such as different app stores, and different testing environments.
        //also may connect to the servers by different gateways.
        [SerializeField] private string _activeChannel;
        [SerializeField] private string _activeGateway;
        [SerializeField] private List<StoreChannel  > _channels;
        [SerializeField] private List<ChannelGateway> _gateways;

        public string GetChannel()
        {
            if (string.IsNullOrWhiteSpace(_activeChannel))
            {
                return null;
            }

            string name = _activeChannel.Trim();

            //the target must be an item in the list.
            if (_channels == null || _channels.Count == 0)
            {
                return null;
            }
            foreach (StoreChannel candicate in _channels)
            {
                if (candicate != null &&
                    !string.IsNullOrWhiteSpace(candicate.channel) &&
                    name == candicate.channel.Trim())
                {
                    return name;
                }
            }
            return null;
        }

        public bool IsLegalChannel(string channel)
        {
            if (_channels == null || _channels.Count == 0)
            {
                return false;
            }

            foreach (StoreChannel candicate in _channels)
            {
                if (candicate != null &&
                    !string.IsNullOrWhiteSpace(candicate.channel) &&
                    channel == candicate.channel.Trim())
                {
                    return true;
                }
            }
            return false;
        }

        public string GetGateway()
        {
            if (string.IsNullOrWhiteSpace(_activeGateway))
            {
                return null;
            }

            string name = _activeGateway.Trim();

            //the target must be an item in the list.
            if (_gateways == null || _gateways.Count == 0)
            {
                return null;
            }
            foreach (ChannelGateway candicate in _gateways)
            {
                if (candicate != null &&
                    !string.IsNullOrWhiteSpace(candicate.channel) &&
                    !string.IsNullOrWhiteSpace(candicate.gateway) &&
                    name == candicate.channel.Trim())
                {
                    return candicate.gateway.Trim();
                }
            }
            return null;
        }

        public bool IsLegalGateway(string channelGateway)
        {
            if (_gateways == null || _gateways.Count == 0)
            {
                return false;
            }

            foreach (ChannelGateway candicate in _gateways)
            {
                if (candicate != null &&
                    !string.IsNullOrWhiteSpace(candicate.channel) &&
                    !string.IsNullOrWhiteSpace(candicate.gateway) &&
                    channelGateway == candicate.channel.Trim())
                {
                    return true;
                }
            }
            return false;
        }

        //sometimes the game needs to download assets and patches from a specific server,
        //which is more convenient for debugging.
        [SerializeField]
        private ForcedUrls _forcedUrls;

        public string GetAssetUrl() { return GetForcedUrl(_forcedUrls?.asset); }
        public string GetPatchUrl() { return GetForcedUrl(_forcedUrls?.patch); }

        private string GetForcedUrl(ForcedUrlItem item)
        {
            if (item != null)
            {
                return null;
            }
            if (!item.enabled || string.IsNullOrWhiteSpace(item.url))
            {
                return null;
            }

            return item.url;
        }

        //packages of different channels may contain different assets with same names,
        //such as different game icons.
        //use "flavor" to differentiate them.
        [SerializeField]
        private List<AssetFlavor> _flavors;

        public HashSet<string> GetEnabledFlavors()
        {
            if (_flavors == null || _flavors.Count == 0)
            {
                return null;
            }

            var enabled = new HashSet<string>();
            foreach (AssetFlavor flavor in _flavors)
            {
                string name = flavor.name.Trim();
                if (!string.IsNullOrWhiteSpace(name) && flavor.enabled)
                {
                    enabled.Add(name);
                }
            }
            return enabled.Count > 0 ? enabled : null;
        }

        //some flags may be required in games,
        //such as whether to enable the manager, whether to skip the novice guide.
        [SerializeField]
        private List<UserFlag> _flags;

        public bool GetBoolFlag(string name)
        {
            UserFlag flag = GetUserFlag(name, UserFlagType.Bool);
            return flag != null ? flag.boolValue : false;
        }

        public int GetIntFlag(string name)
        {
            UserFlag flag = GetUserFlag(name, UserFlagType.Int);
            return flag != null ? flag.intValue : 0;
        }

        public string GetStringFlag(string name)
        {
            UserFlag flag = GetUserFlag(name, UserFlagType.String);
            return !string.IsNullOrWhiteSpace(flag?.stringValue) ? flag.stringValue : null;
        }

        private UserFlag GetUserFlag(string name, UserFlagType type)
        {
            if (_flags == null || _flags.Count == 0)
            {
                return null;
            }

            foreach (UserFlag flag in _flags)
            {
                if (flag.name.Trim() == name && flag.type != type)
                {
                    return flag;
                }
            }
            return null;
        }
    }
}
