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

        private string[]   _languages;
        private string[]   _channels ;
        private string[][] _gateways ;
        private string[][] _assetURLs;
        private string[][] _patchURLs;

        private int _languageIndex;
        private int _channelIndex ;
        private int _gatewayIndex ;
        private int _assetURLIndex;
        private int _patchURLIndex;

        private List<string> _totalFlavors    ;
        private List<bool>   _isFlavorSelected;
        private bool         _isShowFlavors   ;

        private void OnEnable()
        {
            _settings  = (GameSettings)target;
            _userFlags = serializedObject.FindProperty("userFlags");

            var options = AssetHelper.LoadScriptable<GameOptions>(GameOptions.SavedPath);

            options.GetGameLanguages  (out _languages);
            options.GetStoreChannels  (out _channels );
            options.GetChannelGateways(out _gateways );
            options.GetForcedAssetURLs(out _assetURLs);
            options.GetForcedPatchURLs(out _patchURLs);
            _languageIndex = LocateIndex(_languages   , _settings.gameLanguage  );
            _channelIndex  = LocateIndex(_channels    , _settings.storeChannel  );
            _gatewayIndex  = LocateIndex(_gateways [1], _settings.channelGateway);
            _assetURLIndex = LocateIndex(_assetURLs[1], _settings.forcedAssetURL);
            _patchURLIndex = LocateIndex(_patchURLs[1], _settings.forcedPatchURL);

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
            _totalFlavors     = options.GetAssetFlavors();
            _isFlavorSelected = new List<bool>();
            _isShowFlavors    = true;

            HashSet<string> selectedFlavors = _settings.GetAssetFlavors();
            foreach (string item in _totalFlavors)
            {
                bool selected = selectedFlavors.Contains(item);
                _isFlavorSelected.Add(selected);
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
            _settings.packageSerial = EditorGUILayout.IntField("Package Serial", _settings.packageSerial);
        }

        private void DrawGameLanguage() { Popup("Game Language", _languages, ref _languageIndex, ref _settings.gameLanguage); }
        private void DrawStoreChannel() { Popup("Store Channel", _channels , ref _channelIndex , ref _settings.storeChannel); }

        private void Popup(
            string label, string[] values, ref int index, ref string value)
        {
            int newIndex = EditorGUILayout.Popup(label, index, values);
            if (newIndex != index)
            {
                value = values[newIndex];
                index = newIndex;
            }
        }

        private void DrawChannelGateway() { Popup("Channel Gateway" , _gateways , ref _gatewayIndex , ref _settings.channelGateway); }
        private void DrawForcedAssetURL() { Popup("Forced Asset URL", _assetURLs, ref _assetURLIndex, ref _settings.forcedAssetURL); }
        private void DrawForcedPatchURL() { Popup("Forced Patch URL", _patchURLs, ref _patchURLIndex, ref _settings.forcedPatchURL); }

        private void Popup(
            string label, string[][] entries, ref int index, ref string value)
        {
            int newIndex = EditorGUILayout.Popup(label, index, entries[0]);
            EditorGUILayout.LabelField(" ", entries[1][index]);

            if (newIndex != index)
            {
                value = entries[1][newIndex];
                index = newIndex;
            }
        }

        private void DrawAssetFlavors()
        {
            bool changed = false;

            _isShowFlavors = EditorGUILayout.BeginFoldoutHeaderGroup(_isShowFlavors, "Asset Flavors");
            if (_isShowFlavors)
            {
                for (int i = 0; i < _totalFlavors.Count; ++i)
                {
                    bool selected = _isFlavorSelected[i];
                    _isFlavorSelected[i] = EditorGUILayout.Toggle(_totalFlavors[i], selected);

                    changed = changed || (selected != _isFlavorSelected[i]);
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            if (changed)
            {
                var flavors = new HashSet<string>();
                for (int i = 0; i < _totalFlavors.Count; ++i)
                {
                    if (_isFlavorSelected[i])
                    {
                        flavors.Add(_totalFlavors[i]);
                    }
                }
                _settings.SetAssetFlavors(flavors);

                //NOTE:
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
