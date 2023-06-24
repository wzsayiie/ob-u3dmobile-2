using System;
using System.Collections.Generic;
using UnityEngine;

namespace U3DMobile
{
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

    public class GameSettings : ScriptableObject
    {
        public const string SavedPath = "Assets/Resources/GameSettings.asset";

        [SerializeField] public int    packageSerial ;
        [SerializeField] public string gameLanguage  ;
        [SerializeField] public string storeChannel  ;
        [SerializeField] public string channelGateway;
        [SerializeField] public string forcedAssetURL;
        [SerializeField] public string forcedPatchURL;

        [SerializeField] public List<string>   assetFlavors;
        [SerializeField] public List<UserFlag> userFlags   ;

        public HashSet<string> GetAssetFlavors()
        {
            var flavors = new HashSet<string>();

            if (assetFlavors != null)
            {
                foreach (string item in assetFlavors)
                {
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        flavors.Add(item.Trim());
                    }
                }
            }

            return flavors;
        }

        public void SetAssetFlavors(HashSet<string> flavors)
        {
            assetFlavors = new List<string>();

            if (flavors != null)
            {
                foreach (string item in flavors)
                {
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        assetFlavors.Add(item.Trim());
                    }
                }
            }
        }

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
            if (userFlags == null || userFlags.Count == 0)
            {
                return null;
            }

            foreach (UserFlag item in userFlags)
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
