using U3DMobile;
using UnityEditor;
using UnityEngine;

namespace U3DMobileEditor
{
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
            Rect _1 = new Rect(r.x      , r.y, 110          , EditorGUIUtility.singleLineHeight);
            Rect _2 = new Rect(r.x + 115, r.y,  65          , EditorGUIUtility.singleLineHeight);
            Rect _3 = new Rect(r.x + 185, r.y,  80          , EditorGUIUtility.singleLineHeight);
            Rect _4 = new Rect(r.x + 270, r.y, r.width - 270, EditorGUIUtility.singleLineHeight);

            SerializedProperty file = property.FindPropertyRelative("file");
            EditorGUI.PropertyField(_1, file, GUIContent.none);

            UnityEngine.Object fObj = file.objectReferenceValue;
            EditorGUI.BeginDisabledGroup(fObj == null);

            EditorGUI.PropertyField(_2, property.FindPropertyRelative("strategy"), GUIContent.none);
            EditorGUI.PropertyField(_3, property.FindPropertyRelative("demand"  ), GUIContent.none);
            EditorGUI.PropertyField(_4, property.FindPropertyRelative("carryOpt"), GUIContent.none);

            EditorGUI.EndDisabledGroup();

            //second line:
            Rect _6 = new Rect(
                r.x    , EditorGUIUtility.singleLineHeight + r.y,
                r.width, EditorGUIUtility.singleLineHeight
            );
            if (fObj != null)
            {
                EditorGUI.LabelField(_6, AssetDatabase.GetAssetPath(fObj));
            }
        }
    }

    [CustomEditor(typeof(BuildSettings))]
    internal class BuildSettingsInspector : Editor
    {
        private SerializedProperty _entries;

        internal static BuildSettingsInspector instance;

        private void OnEnable()
        {
            instance = this;

            _entries = serializedObject.FindProperty("_entries");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            //bundle entries:
            EditorGUILayout.PropertyField(_entries, new GUIContent("Bundle Entries"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}
