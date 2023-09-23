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

    [CustomEditor(typeof(GameProfile))]
    internal class GameProfileInspector : Editor
    {
        private GameProfile        _profile  ;
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
            _profile   = (GameProfile)target;
            _userFlags = serializedObject.FindProperty("_userFlags");

            var opt = AssetHelper.LoadScriptable<GameProfileOpt>(GameProfileOpt.SavedPath);

            _languageList = opt.GameLanguages  ();
            _channelList  = opt.StoreChannels  ();
            _gatewayList  = opt.ChannelGateways();
            _assetURLList = opt.AssetURLs      ();
            _patchURLList = opt.PatchURLs      ();

            _languageIndex = LocateIndex(_languageList   , _profile.firstLanguage );
            _channelIndex  = LocateIndex(_channelList    , _profile.storeChannel  );
            _gatewayIndex  = LocateIndex(_gatewayList [1], _profile.channelGateway);
            _assetURLIndex = LocateIndex(_assetURLList[1], _profile.assetURL      );
            _patchURLIndex = LocateIndex(_patchURLList[1], _profile.patchURL      );

            InitializeAssetFlavors(opt);
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

        private void InitializeAssetFlavors(GameProfileOpt opt)
        {
            _flavorList    = opt.AssetFlavors();
            _flavorIsOn    = new bool[_flavorList.Length];
            _isShowFlavors = true;

            HashSet<string> selectedFlavors = _profile.GetAssetFlavors();
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
            int serial = _profile.packageSerial;
            _profile.packageSerial = EditorGUILayout.IntField(I18N.PackageSerial, serial);

            if (serial != _profile.packageSerial)
            {
                //NOTE: if change the field values of a serialized object through custom properties,
                //need to set dirty flags.
                EditorUtility.SetDirty(_profile);
            }
        }

        private void DrawFirstLanguage()
        {
            Popup(I18N.FirstLanguage, _languageList, null, ref _languageIndex, (string value) =>
            {
                _profile.firstLanguage = value;
            });
        }

        private void DrawStoreChannel()
        {
            Popup(I18N.StoreChannel, _channelList, null, ref _channelIndex , (string value) =>
            {
                _profile.storeChannel = value;
            });
        }

        private void DrawChannelGateway()
        {
            string[][] list = _gatewayList;
            Popup(I18N.ChannelGateway, list[0], list[1], ref _gatewayIndex, (string value) =>
            {
                _profile.channelGateway = value;
            });
        }
        
        private void DrawAssetURL()
        {
            string[][] list = _assetURLList;
            Popup(I18N.AssetURL, list[0], list[1], ref _assetURLIndex, (string value) =>
            {
                _profile.assetURL = value;
            });
        }
        
        private void DrawPatchURL()
        {
            string[][] list = _patchURLList;
            Popup(I18N.PatchURL, list[0], list[1], ref _patchURLIndex, (string value) =>
            {
                _profile.patchURL = value;
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
                EditorUtility.SetDirty(_profile);
            }

            if (values != null)
            {
                EditorGUILayout.LabelField(" ", index != 0 ? values[index] : "-");
            }
        }

        private void DrawAssetFlavors()
        {
            bool changed = false;

            _isShowFlavors = EditorGUILayout.BeginFoldoutHeaderGroup(_isShowFlavors, I18N.AssetFlavor);
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
                _profile.SetAssetFlavors(flavors);

                EditorUtility.SetDirty(_profile);
            }

            if (GUILayout.Button(I18N.SwitchAssetFlavor))
            {
                FunctionMenu.SwitchAssetFlavor();
            }
        }

        private void DrawUserFlags()
        {
            EditorGUILayout.PropertyField(_userFlags, new GUIContent(I18N.UserFlag));
        }
    }
}
