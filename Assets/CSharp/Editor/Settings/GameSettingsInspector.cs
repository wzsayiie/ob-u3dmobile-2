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
        protected override void OnDrawLine(int _, SerializedProperty property)
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

            _languageList = options.GameLanguages  ();
            _channelList  = options.StoreChannels  ();
            _gatewayList  = options.ChannelGateways();
            _assetURLList = options.AssetURLs      ();
            _patchURLList = options.PatchURLs      ();

            _languageIndex = LocateIndex(_languageList   , _settings.firstLanguage );
            _channelIndex  = LocateIndex(_channelList    , _settings.storeChannel  );
            _gatewayIndex  = LocateIndex(_gatewayList [1], _settings.channelGateway);
            _assetURLIndex = LocateIndex(_assetURLList[1], _settings.assetURL      );
            _patchURLIndex = LocateIndex(_patchURLList[1], _settings.patchURL      );

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
            _flavorList    = options.AssetFlavors();
            _flavorIsOn    = new bool[_flavorList.Length];
            _isShowFlavors = true;

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
            DrawFirstLanguage ();
            DrawStoreChannel  ();
            DrawChannelGateway();
            DrawAssetURL      ();
            DrawPatchURL      ();
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

        private void DrawFirstLanguage()
        {
            Popup("First Language", _languageList, null, ref _languageIndex, (string value) =>
            {
                _settings.firstLanguage = value;
            });
        }

        private void DrawStoreChannel()
        {
            Popup("Store Channel", _channelList, null, ref _channelIndex , (string value) =>
            {
                _settings.storeChannel = value;
            });
        }

        private void DrawChannelGateway()
        {
            string[][] list = _gatewayList;
            Popup("Channel Gateway", list[0], list[1], ref _gatewayIndex, (string value) =>
            {
                _settings.channelGateway = value;
            });
        }
        
        private void DrawAssetURL()
        {
            string[][] list = _assetURLList;
            Popup("Forced Asset URL", list[0], list[1], ref _assetURLIndex, (string value) =>
            {
                _settings.assetURL = value;
            });
        }
        
        private void DrawPatchURL()
        {
            string[][] list = _patchURLList;
            Popup("Forced Patch URL", list[0], list[1], ref _patchURLIndex, (string value) =>
            {
                _settings.patchURL = value;
            });
        }

        private void Popup(
            string label, string[] keys, string[] values, ref int index, Action<string> change)
        {
            int newIndex = EditorGUILayout.Popup(label, index, keys);
            if (newIndex != index)
            {
                index = newIndex;
                change(values != null ? values[index] : keys[index]);
                EditorUtility.SetDirty(_settings);
            }

            if (values != null)
            {
                EditorGUILayout.LabelField(" ", index != 0 ? values[index] : "-");
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

            if (GUILayout.Button("Switch Asset Flavors"))
            {
                MenuItems.SwitchAssetFlavors();
            }
        }

        private void DrawUserFlags()
        {
            EditorGUILayout.PropertyField(_userFlags, new GUIContent("User Flags"));
        }
    }
}
