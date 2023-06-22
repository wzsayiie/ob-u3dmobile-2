using UnityEditor;
using UnityEngine;

namespace U3DMobileEditor
{
    [CustomPropertyDrawer(typeof(BundleSerial))]
    internal class BundleSerialDrawer : BaseItemDrawer
    {
        protected override void OnDrawFirstLine(SerializedProperty property)
        {
            Label(110, "Bundle Serial");
            Field(flx, property.FindPropertyRelative("serial"));
        }
    }

    [CustomPropertyDrawer(typeof(CarryOption))]
    internal class CarryOptionDrawer : ListItemDrawer
    {
        protected override void OnDrawFirstLine(SerializedProperty property)
        {
            var    option  = property.FindPropertyRelative("option");
            string theItem = option.stringValue;
            string active  = BuildSettingsInspector.instance.activeCarry;

            bool beingOn =
                !string.IsNullOrWhiteSpace(theItem) &&
                !string.IsNullOrWhiteSpace(active ) &&
                theItem.Trim() == active.Trim()
            ;
            bool afterOn;

            Radio( 20, beingOn, out afterOn);
            Field(flx, option);

            if (!beingOn && afterOn)
            {
                BuildSettingsInspector.instance.activeCarry = theItem;
            }
        }
    }

    [CustomPropertyDrawer(typeof(BundleEntry))]
    internal class BundleEntryDrawer : ListItemDrawer
    {
        protected override int OnGetLines()
        {
            return 2;
        }

        protected override void OnDrawFirstLine(SerializedProperty property)
        {
            Field( 20, property, "selected"  );
            Field(110, property, "fileObj"   );
            Field( 65, property, "packMode"  );
            Field( 90, property, "demandMode");
            Field(flx, property, "carryOpts" );
        }

        protected override void OnDrawSecondLine(SerializedProperty property)
        {
            SerializedProperty fileObj = property.FindPropertyRelative("fileObj");
            UnityEngine.Object objRef  = fileObj.objectReferenceValue;

            if (objRef != null)
            {
                Label(flx, AssetDatabase.GetAssetPath(objRef));
            }
        }
    }

    [CustomPropertyDrawer(typeof(PatchEntry))]
    internal class PatchEntryDrawer : ListItemDrawer
    {
        protected override int OnGetLines()
        {
            return 2;
        }

        protected override void OnDrawFirstLine(SerializedProperty property)
        {
            Field( 20, property, "selected");
            Field(flx, property, "fileObj" );
        }

        protected override void OnDrawSecondLine(SerializedProperty property)
        {
            SerializedProperty fileObj = property.FindPropertyRelative("fileObj");
            UnityEngine.Object objRef  = fileObj.objectReferenceValue;

            if (objRef != null)
            {
                Label(flx, AssetDatabase.GetAssetPath(objRef));
            }
        }
    }

    [CustomEditor(typeof(BuildSettings))]
    internal class BuildSettingsInspector : Editor
    {
        private SerializedProperty _bundleSerial ;
        private SerializedProperty _activeCarry  ;
        private SerializedProperty _carryOptions ;
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

            _bundleSerial  = serializedObject.FindProperty("_bundleSerial" );
            _activeCarry   = serializedObject.FindProperty("_activeCarry"  );
            _carryOptions  = serializedObject.FindProperty("_carryOptions" );
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
