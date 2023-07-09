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

        [SerializeField] private int    _packageSerial ;
        [SerializeField] private string _gameLanguage  ;
        [SerializeField] private string _storeChannel  ;
        [SerializeField] private string _channelGateway;
        [SerializeField] private string _forcedAssetURL;
        [SerializeField] private string _forcedPatchURL;

        [SerializeField] private List<string>   _assetFlavors;
        [SerializeField] private List<UserFlag> _userFlags   ;

        public int    packageSerial  { get { return _packageSerial ; } set { _packageSerial  = value; } }
        public string gameLanguage   { get { return _gameLanguage  ; } set { _gameLanguage   = value; } }
        public string storeChannel   { get { return _storeChannel  ; } set { _storeChannel   = value; } }
        public string channelGateway { get { return _channelGateway; } set { _channelGateway = value; } }
        public string forcedAssetURL { get { return _forcedAssetURL; } set { _forcedAssetURL = value; } }
        public string forcedPatchURL { get { return _forcedPatchURL; } set { _forcedPatchURL = value; } }

        public HashSet<string> GetAssetFlavors()
        {
            var flavors = new HashSet<string>();

            if (_assetFlavors != null)
            {
                foreach (string item in _assetFlavors)
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
            _assetFlavors = new List<string>();

            if (flavors != null)
            {
                foreach (string item in flavors)
                {
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        _assetFlavors.Add(item.Trim());
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
