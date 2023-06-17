using UnityEditor;
using UnityEngine;

namespace U3DMobileEditor
{
    [CustomPropertyDrawer(typeof(BundleSerial))]
    internal class BundleSerialDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect r, SerializedProperty property, GUIContent label)
        {
            var _1 = new Rect(r.x      , r.y, 110          , EditorGUIUtility.singleLineHeight);
            var _2 = new Rect(r.x + 110, r.y, r.width - 110, EditorGUIUtility.singleLineHeight);

            EditorGUI.LabelField(_1, "Bundle Serial");
            EditorGUI.PropertyField(_2, property.FindPropertyRelative("serial"), GUIContent.none);
        }
    }

    [CustomPropertyDrawer(typeof(CarryOption))]
    internal class CarryOptionDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect r, SerializedProperty property, GUIContent label)
        {
            var _1 = new Rect(r.x     , r.y, 20          , EditorGUIUtility.singleLineHeight);
            var _2 = new Rect(r.x + 20, r.y, r.width - 20, EditorGUIUtility.singleLineHeight);

            SerializedProperty name = property.FindPropertyRelative("option");

            string cursor = name.stringValue;
            string active = BuildSettingsInspector.instance.activeCarry;
            bool oldIsOn = (
                !string.IsNullOrWhiteSpace(cursor) &&
                !string.IsNullOrWhiteSpace(active) &&
                cursor.Trim() == active.Trim()
            );

            //use bold style to remind users that this is a "radio" toggle.
            bool newIsOn = EditorGUI.Toggle(_1, oldIsOn, GUI.skin.GetStyle("BoldToggle"));
            if (!oldIsOn && newIsOn)
            {
                BuildSettingsInspector.instance.activeCarry = cursor;
            }
            EditorGUI.PropertyField(_2, name, GUIContent.none);
        }
    }

    [CustomPropertyDrawer(typeof(BundleEntry))]
    internal class BundleEntryDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2;
        }

        public override void OnGUI(Rect r, SerializedProperty property, GUIContent label)
        {
            //first line:
            var _1 = new Rect(r.x      , r.y,  20          , EditorGUIUtility.singleLineHeight);
            var _2 = new Rect(r.x +  20, r.y, 110          , EditorGUIUtility.singleLineHeight);
            var _3 = new Rect(r.x + 135, r.y,  65          , EditorGUIUtility.singleLineHeight);
            var _4 = new Rect(r.x + 205, r.y,  90          , EditorGUIUtility.singleLineHeight);
            var _5 = new Rect(r.x + 300, r.y, r.width - 300, EditorGUIUtility.singleLineHeight);

            SerializedProperty file = property.FindPropertyRelative("fileObj");
            UnityEngine.Object fObj = file.objectReferenceValue;

            EditorGUI.PropertyField(_1, property.FindPropertyRelative("selected"  ), GUIContent.none);
            EditorGUI.PropertyField(_2, file, GUIContent.none);
            EditorGUI.PropertyField(_3, property.FindPropertyRelative("packMode"  ), GUIContent.none);
            EditorGUI.PropertyField(_4, property.FindPropertyRelative("demandMode"), GUIContent.none);
            EditorGUI.PropertyField(_5, property.FindPropertyRelative("carryOpts" ), GUIContent.none);

            //second line:
            var _6 = new Rect(
                r.x    , EditorGUIUtility.singleLineHeight + r.y,
                r.width, EditorGUIUtility.singleLineHeight
            );
            if (fObj != null)
            {
                EditorGUI.LabelField(_6, AssetDatabase.GetAssetPath(fObj));
            }
        }
    }

    [CustomPropertyDrawer(typeof(PatchEntry))]
    internal class PatchEntryDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2;
        }

        public override void OnGUI(Rect r, SerializedProperty property, GUIContent label)
        {
            //first line:
            var _1 = new Rect(r.x     , r.y, 20          , EditorGUIUtility.singleLineHeight);
            var _2 = new Rect(r.x + 20, r.y, r.width - 20, EditorGUIUtility.singleLineHeight);

            SerializedProperty file = property.FindPropertyRelative("fileObj");
            UnityEngine.Object fObj = file.objectReferenceValue;

            EditorGUI.PropertyField(_1, property.FindPropertyRelative("selected"), GUIContent.none);
            EditorGUI.PropertyField(_2, file, GUIContent.none);

            //second line:
            var _3 = new Rect(
                r.x    , EditorGUIUtility.singleLineHeight + r.y,
                r.width, EditorGUIUtility.singleLineHeight
            );
            if (fObj != null)
            {
                EditorGUI.LabelField(_3, AssetDatabase.GetAssetPath(fObj));
            }
        }
    }

    [CustomEditor(typeof(BuildSettings))]
    internal class BuildSettingsInspector : Editor
    {
        private SerializedProperty _bundleSerial;
        private SerializedProperty _activeCarry;
        private SerializedProperty _carryOptions;
        private SerializedProperty _bundleEntries;
        private SerializedProperty _bundlePatches;

        internal static BuildSettingsInspector instance;

        internal string activeCarry
        {
            get { return _activeCarry.stringValue ; }
            set { _activeCarry.stringValue = value; }
        }

        private void OnEnable()
        {
            instance = this;

            _bundleSerial  = serializedObject.FindProperty("_bundleSerial");
            _activeCarry   = serializedObject.FindProperty("_activeCarry");
            _carryOptions  = serializedObject.FindProperty("_carryOptions");
            _bundleEntries = serializedObject.FindProperty("_bundleEntries");
            _bundlePatches = serializedObject.FindProperty("_bundlePatches");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_bundleSerial, new GUIContent("Bundle Serial"));
            EditorGUILayout.PropertyField(_carryOptions, new GUIContent("Package Carry Options"));

            EditorGUILayout.PropertyField(_bundleEntries, new GUIContent("Bundle Entries"));
            if (GUILayout.Button("Pack Selected (for Android)"))
            {
            }
            if (GUILayout.Button("Pack Selected (for iOS)"))
            {
            }

            EditorGUILayout.PropertyField(_bundlePatches, new GUIContent("Patch Entries"));
            if (GUILayout.Button("Copy Selected Patches"))
            {
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
