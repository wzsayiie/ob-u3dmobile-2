using System;
using System.Collections.Generic;
using U3DMobile;
using UnityEditor;
using UnityEngine;

namespace U3DMobileEditor
{
    [CustomPropertyDrawer(typeof(UserFlag))]
    internal class UserFlagDrawer : ListItemDrawer
    {
        protected override void OnDrawFirstLine(SerializedProperty property)
        {
            SerializedProperty type = property.FindPropertyRelative("type");

            Field(90, property, "name");
            Field(60, type);

            switch ((UserFlagType)type.enumValueFlag)
            {
                case UserFlagType.Bool  : Field( 20, property, "boolValue"  ); break;
                case UserFlagType.Int   : Field(flx, property, "intValue"   ); break;
                case UserFlagType.String: Field(flx, property, "stringValue"); break;
            }
        }
    }

    [CustomEditor(typeof(GameSettings))]
    internal class GameSettingsInspector : Editor
    {
        private GameSettings       _settings ;
        private SerializedProperty _userFlags;

        private string[]   _languageList ;
        private string[]   _channelList  ;
        private string[][] _gatewayList  ;
        private string[][] _assetURLList ;
        private string[][] _patchURLList ;

        private int        _languageIndex;
        private int        _channelIndex ;
        private int        _gatewayIndex ;
        private int        _assetURLIndex;
        private int        _patchURLIndex;

        private string[]   _flavorList   ;
        private bool[]     _flavorIsOn   ;
        private bool       _isShowFlavors;

        private void OnEnable()
        {
            _settings  = (GameSettings)target;
            _userFlags = serializedObject.FindProperty("_userFlags");

            var options = AssetHelper.LoadScriptable<GameOptions>(GameOptions.SavedPath);

            _languageList = options.GetGameLanguages  ();
            _channelList  = options.GetStoreChannels  ();
            _gatewayList  = options.GetChannelGateways();
            _assetURLList = options.GetForcedAssetURLs();
            _patchURLList = options.GetForcedPatchURLs();

            _languageIndex = LocateIndex(_languageList   , _settings.gameLanguage  );
            _channelIndex  = LocateIndex(_channelList    , _settings.storeChannel  );
            _gatewayIndex  = LocateIndex(_gatewayList [1], _settings.channelGateway);
            _assetURLIndex = LocateIndex(_assetURLList[1], _settings.forcedAssetURL);
            _patchURLIndex = LocateIndex(_patchURLList[1], _settings.forcedPatchURL);

            InitializeAssetFlavors(options);
        }

        private int LocateIndex(string[] list, string target)
        {
            if (!string.IsNullOrWhiteSpace(target) && list != null)
            {
                for (int i = 0; i < list.Length; ++i)
                {
                    if (list[i] == target)
                    {
                        return i;
                    }
                }
            }

            //the first item is default "none".
            return 0;
        }

        private void InitializeAssetFlavors(GameOptions options)
        {
            _flavorList = options.GetAssetFlavors();
            _flavorIsOn = new bool[_flavorList.Length];

            HashSet<string> selectedFlavors = _settings.GetAssetFlavors();
            for (int i = 0; i < _flavorList.Length; ++i)
            {
                _flavorIsOn[i] = selectedFlavors.Contains(_flavorList[i]);
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            
            DrawPackageSerial ();
            DrawGameLanguage  ();
            DrawStoreChannel  ();
            DrawChannelGateway();
            DrawForcedAssetURL();
            DrawForcedPatchURL();
            DrawAssetFlavors  ();
            DrawUserFlags     ();

            EditorGUI.EndDisabledGroup();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawPackageSerial()
        {
            int serial = _settings.packageSerial;
            _settings.packageSerial = EditorGUILayout.IntField("Package Serial", serial);

            if (serial != _settings.packageSerial)
            {
                //NOTE: if change the field values of a serialized object through custom properties,
                //need to set dirty flags.
                EditorUtility.SetDirty(_settings);
            }
        }

        private void DrawGameLanguage()
        {
            Popup("Game Language", _languageList, _languageIndex, (int newIndex) =>
            {
                _settings.gameLanguage = _languageList[newIndex];
                _languageIndex = newIndex;
            });
        }

        private void DrawStoreChannel()
        {
            Popup("Store Channel", _channelList, _channelIndex , (int newIndex) =>
            {
                _settings.storeChannel = _channelList[newIndex];
                _channelIndex = newIndex;
            });
        }

        private void DrawChannelGateway()
        {
            Popup("Channel Gateway", _gatewayList[0], _gatewayIndex, (int newIndex) =>
            {
                _settings.channelGateway = _gatewayList[1][newIndex];
                _gatewayIndex = newIndex;
            });
        }
        
        private void DrawForcedAssetURL()
        {
            Popup("Forced Asset URL", _assetURLList[0], _assetURLIndex, (int newIndex) =>
            {
                _settings.forcedAssetURL = _assetURLList[1][newIndex];
                _assetURLIndex = newIndex;
            });
        }
        
        private void DrawForcedPatchURL()
        {
            Popup("Forced Patch URL", _patchURLList[0], _patchURLIndex, (int newIndex) =>
            {
                _settings.forcedPatchURL = _patchURLList[1][newIndex];
                _patchURLIndex = newIndex;
            });
        }

        private void Popup(string label, string[] list, int index, Action<int> change)
        {
            int newIndex = EditorGUILayout.Popup(label, index, list);
            if (newIndex != index)
            {
                change(newIndex);
                EditorUtility.SetDirty(_settings);
            }
        }

        private void DrawAssetFlavors()
        {
            bool changed = false;

            _isShowFlavors = EditorGUILayout.BeginFoldoutHeaderGroup(_isShowFlavors, "Asset Flavors");
            if (_isShowFlavors)
            {
                for (int i = 0; i < _flavorList.Length; ++i)
                {
                    bool on = _flavorIsOn[i];
                    _flavorIsOn[i] = EditorGUILayout.Toggle(_flavorList[i], on);

                    changed = changed || (on != _flavorIsOn[i]);
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            if (changed)
            {
                var flavors = new HashSet<string>();
                for (int i = 0; i < _flavorList.Length; ++i)
                {
                    if (_flavorIsOn[i])
                    {
                        flavors.Add(_flavorList[i]);
                    }
                }
                _settings.SetAssetFlavors(flavors);

                EditorUtility.SetDirty(_settings);
            }

            if (GUILayout.Button("Copy Selected Flavors"))
            {
            }
        }

        private void DrawUserFlags()
        {
            EditorGUILayout.PropertyField(_userFlags, new GUIContent("User Flags"));
        }
    }
}
