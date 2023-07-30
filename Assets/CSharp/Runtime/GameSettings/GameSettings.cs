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
        [SerializeField] private string _firstLanguage ;
        [SerializeField] private string _storeChannel  ;
        [SerializeField] private string _channelGateway;
        [SerializeField] private string _assetURL      ;
        [SerializeField] private string _patchURL      ;

        [SerializeField] private List<string>   _assetFlavors;
        [SerializeField] private List<UserFlag> _userFlags   ;

    #if UNITY_EDITOR
        public int    packageSerial  { get { return _packageSerial ; } set { _packageSerial  = value; } }
        public string firstLanguage  { get { return _firstLanguage ; } set { _firstLanguage  = value; } }
        public string storeChannel   { get { return _storeChannel  ; } set { _storeChannel   = value; } }
        public string channelGateway { get { return _channelGateway; } set { _channelGateway = value; } }
        public string assetURL       { get { return _assetURL      ; } set { _assetURL       = value; } }
        public string patchURL       { get { return _patchURL      ; } set { _patchURL       = value; } }
    #else
        public int    packageSerial  { get { return _packageSerial ; } }
        public string firstLanguage  { get { return _firstLanguage ; } }
        public string storeChannel   { get { return _storeChannel  ; } }
        public string channelGateway { get { return _channelGateway; } }
        public string assetURL       { get { return _assetURL      ; } }
        public string patchURL       { get { return _patchURL      ; } }
    #endif

    #if UNITY_EDITOR
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
    #endif

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

    #if UNITY_EDITOR

        public bool IsValidUserFlags(Dictionary<string, object> targets, out HashSet<string> illegals)
        {
            if (targets == null || targets.Count == 0)
            {
                illegals = null;
                return true;
            }

            var legals = new Dictionary<string, UserFlag>();
            if (_userFlags != null && _userFlags.Count > 0)
            {
                foreach (UserFlag item in _userFlags)
                {
                    if (item == null)
                    {
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(item.name))
                    {
                        continue;
                    }

                    legals[item.name.Trim()] = item;
                }
            }

            illegals = new HashSet<string>();
            foreach (KeyValuePair<string, object> pair in targets)
            {
                if (!legals.ContainsKey(pair.Key))
                {
                    illegals.Add(pair.Key);
                    continue;
                }
                
                UserFlag flag = legals[pair.Key];
                if (flag.type == UserFlagType.Bool && pair.Value is not bool)
                {
                    illegals.Add(pair.Key);
                    continue;
                }
                if (flag.type == UserFlagType.Int && pair.Value is not int)
                {
                    illegals.Add(pair.Key);
                    continue;
                }
                if (flag.type == UserFlagType.String && pair.Value is not string)
                {
                    illegals.Add(pair.Key);
                    continue;
                }
            }
            return illegals.Count == 0;
        }

    #endif

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
