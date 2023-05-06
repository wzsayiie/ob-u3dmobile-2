using U3DMobile;
using UnityEditor;
using UnityEngine;

namespace U3DMobileEditor
{
    [CustomPropertyDrawer(typeof(PackageIdentifier))]
    internal class PackageIdentifierDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect r, SerializedProperty property, GUIContent label)
        {
            Rect _1 = new Rect(r.x      , r.y, 110          , EditorGUIUtility.singleLineHeight);
            Rect _2 = new Rect(r.x + 110, r.y, r.width - 110, EditorGUIUtility.singleLineHeight);

            EditorGUI.LabelField(_1, "Package Identifier");
            EditorGUI.PropertyField(_2, property.FindPropertyRelative("iden"), GUIContent.none);
        }
    }

    [CustomPropertyDrawer(typeof(StoreChannel))]
    internal class StoreChannelDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect r, SerializedProperty property, GUIContent label)
        {
            Rect _1 = new Rect(r.x      , r.y, 20           , EditorGUIUtility.singleLineHeight);
            Rect _2 = new Rect(r.x +  20, r.y, 90           , EditorGUIUtility.singleLineHeight);
            Rect _3 = new Rect(r.x + 120, r.y, 60           , EditorGUIUtility.singleLineHeight);
            Rect _4 = new Rect(r.x + 180, r.y, r.width - 180, EditorGUIUtility.singleLineHeight);

            SerializedProperty channel = property.FindPropertyRelative("channel");

            string current = channel.stringValue;
            string active  = GameSettingsInspector.instance.activeChan;
            bool   oldIsOn = (
                !string.IsNullOrWhiteSpace(current) &&
                !string.IsNullOrWhiteSpace(active ) &&
                current.Trim() == active.Trim()
            );

            //use bold style to remind users that this is a "radio" toggle.
            bool newIsOn = EditorGUI.Toggle(_1, oldIsOn, GUI.skin.GetStyle("BoldToggle"));
            if (!oldIsOn && newIsOn)
            {
                GameSettingsInspector.instance.activeChan = current;
            }
            EditorGUI.PropertyField(_2, channel, GUIContent.none);

            EditorGUI.LabelField(_3, "Gateway");
            EditorGUI.PropertyField(_4, property.FindPropertyRelative("gateway"), GUIContent.none);
        }
    }

    [CustomPropertyDrawer(typeof(ForcedUrlItem))]
    internal class ForcedUrlsDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect r, SerializedProperty property, GUIContent label)
        {
            Rect _1 = new Rect(r.x      , r.y, 20           , EditorGUIUtility.singleLineHeight);
            Rect _2 = new Rect(r.x +  20, r.y, 80           , EditorGUIUtility.singleLineHeight);
            Rect _3 = new Rect(r.x + 100, r.y, r.width - 100, EditorGUIUtility.singleLineHeight);

            EditorGUI.PropertyField(_1, property.FindPropertyRelative("enabled"), GUIContent.none);

            switch (property.name)
            {
                case "asset": EditorGUI.LabelField(_2, "Asset URL"); break;
                case "patch": EditorGUI.LabelField(_2, "Patch URL"); break;
            }
            EditorGUI.PropertyField(_3, property.FindPropertyRelative("url"), GUIContent.none);
        }
    }

    [CustomPropertyDrawer(typeof(AssetFlavor))]
    internal class AssetFlavorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect r, SerializedProperty property, GUIContent label)
	    {
            Rect _1 = new Rect(r.x        , r.y, 40           , EditorGUIUtility.singleLineHeight);
            Rect _2 = new Rect(r.x    + 40, r.y, r.width - 120, EditorGUIUtility.singleLineHeight);
            Rect _3 = new Rect(r.xMax - 70, r.y, 50           , EditorGUIUtility.singleLineHeight);
            Rect _4 = new Rect(r.xMax - 20, r.y, 20           , EditorGUIUtility.singleLineHeight);

            EditorGUI.LabelField(_1, "Flavor");
            EditorGUI.PropertyField(_2, property.FindPropertyRelative("name"), GUIContent.none);

            EditorGUI.LabelField(_3, "Enabled");
            EditorGUI.PropertyField(_4, property.FindPropertyRelative("enabled"), GUIContent.none);
        }
    }

    [CustomPropertyDrawer(typeof(UserFlag))]
    internal class UserFlagDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect r, SerializedProperty property, GUIContent label)
        {
            Rect _1 = new Rect(r.x      , r.y, 90           , EditorGUIUtility.singleLineHeight);
            Rect _2 = new Rect(r.x + 100, r.y, 60           , EditorGUIUtility.singleLineHeight);
            Rect _3 = new Rect(r.x + 170, r.y, 20           , EditorGUIUtility.singleLineHeight);
            Rect _4 = new Rect(r.x + 170, r.y, r.width - 170, EditorGUIUtility.singleLineHeight);
            Rect _5 = new Rect(r.x + 170, r.y, r.width - 170, EditorGUIUtility.singleLineHeight);

            EditorGUI.PropertyField(_1, property.FindPropertyRelative("name"), GUIContent.none);

            SerializedProperty type = property.FindPropertyRelative("type");
            EditorGUI.PropertyField(_2, type, GUIContent.none);

            switch ((UserFlagType)type.enumValueFlag)
            {
                case UserFlagType.Bool: {
                    EditorGUI.PropertyField(_3, property.FindPropertyRelative("boolValue"), GUIContent.none);
                    break;
                }
                case UserFlagType.Int: {
                    EditorGUI.PropertyField(_4, property.FindPropertyRelative("intValue"), GUIContent.none);
                    break;
                }
                case UserFlagType.String: {
                    EditorGUI.PropertyField(_5, property.FindPropertyRelative("stringValue"), GUIContent.none);
                    break;
                }
            }
        }
    }

    [CustomEditor(typeof(GameSettings))]
    internal class GameSettingsInspector : Editor
    {
        private SerializedProperty _identifier;
        private SerializedProperty _activeChan;
        private SerializedProperty _channels  ;
        private SerializedProperty _forcedUrls;
        private SerializedProperty _flavors   ;
        private SerializedProperty _flags     ;

        internal static GameSettingsInspector instance;

        internal string activeChan
        {
            get { return _activeChan.stringValue ; }
            set { _activeChan.stringValue = value; }
        }

        private void OnEnable()
        {
            instance = this;

            _identifier = serializedObject.FindProperty("_identifier");
            _activeChan = serializedObject.FindProperty("_activeChan");
            _channels   = serializedObject.FindProperty("_channels");
            _forcedUrls = serializedObject.FindProperty("_forcedUrls");
            _flavors    = serializedObject.FindProperty("_flavors");
            _flags      = serializedObject.FindProperty("_flags");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);

            //package identifier.
            EditorGUILayout.PropertyField(_identifier, new GUIContent("Package Identifier"));

            //store channels.
            EditorGUILayout.PropertyField(_channels, new GUIContent("Store Channels"));

            //forced urls.
            EditorGUILayout.PropertyField(_forcedUrls, new GUIContent("Forced URLs"));

            //asset flavors.
            EditorGUILayout.PropertyField(_flavors, new GUIContent("Asset Flavors"));
            if (GUILayout.Button("Switch Flavors"))
            {
            }

            //user flags.
            EditorGUILayout.PropertyField(_flags, new GUIContent("User Flags"));

            EditorGUI.EndDisabledGroup();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
