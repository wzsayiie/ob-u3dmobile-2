using UnityEditor;
using UnityEngine;

namespace U3DMobile.Edit
{
    [CustomPropertyDrawer(typeof(BundleSerial))]
    internal class BundleSerialDrawer : BaseItemDrawer
    {
        protected override void OnDrawLine(int _, SerializedProperty property)
        {
            Label(110, I18N.BundleSerial);
            Field(flx, property.FindPropertyRelative("serial"));
        }
    }

    [CustomPropertyDrawer(typeof(ForceRebuildCheck))]
    internal class ForceRebuildCheckDrawer : BaseItemDrawer
    {
        protected override void OnDrawLine(int _, SerializedProperty property)
        {
            Label(110, I18N.ForceRebuild);
            Field(flx, property.FindPropertyRelative("forceRebuild"));
        }
    }

    [CustomPropertyDrawer(typeof(UsePastBundleCheck))]
    internal class UsePastCacheCheckDrawer : BaseItemDrawer
    {
        protected override void OnDrawLine(int _, SerializedProperty property)
        {
            Label(110, I18N.UsePastBundle);
            Field(flx, property.FindPropertyRelative("usePastBundle"));
        }
    }

    [CustomPropertyDrawer(typeof(CarryOption))]
    internal class CarryOptionDrawer : ListItemDrawer
    {
        protected override void OnDrawLine(int _, SerializedProperty property)
        {
            var    option  = property.FindPropertyRelative("option");
            string curItem = option.stringValue;
            string actItem = BuildProfileInspector.instance.currentCarry;

            string curTrim = curItem?.Trim();
            string actTrim = actItem?.Trim();
            bool   beingOn = curTrim != null && curTrim == actTrim;
            bool   afterOn ;

            Radio( 20, beingOn, out afterOn);
            Field(flx, option);

            if (!beingOn && afterOn)
            {
                BuildProfileInspector.instance.currentCarry = curTrim;
            }
        }
    }

    [CustomPropertyDrawer(typeof(BundleEntry))]
    internal class BundleEntryDrawer : ListItemDrawer
    {
        protected override int OnGetLines()
        {
            return 3;
        }

        protected override void OnDrawLine(int line, SerializedProperty property)
        {
            switch (line)
            {
                case 0: DrawFileEntry(property); break;
                case 1: DrawCarryOpts(property); break;
                case 2: DrawFilePath (property); break;
            }
        }

        private void DrawFileEntry(SerializedProperty property)
        {
            Field(140, property, "fileObj"   );
            Field( 70, property, "packMode"  );
            Field(flx, property, "demandMode");
        }

        private void DrawCarryOpts(SerializedProperty property)
        {
            Field(flx, property, "carryOpts");
        }

        private void DrawFilePath(SerializedProperty property)
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

        protected override void OnDrawLine(int line, SerializedProperty property)
        {
            switch (line)
            {
                case 0: DrawFileObj (property); break;
                case 1: DrawFilePath(property); break;
            }
        }

        private void DrawFileObj(SerializedProperty property)
        {
            Field(flx, property, "fileObj" );
        }

        private void DrawFilePath(SerializedProperty property)
        {
            SerializedProperty fileObj = property.FindPropertyRelative("fileObj");
            UnityEngine.Object objRef  = fileObj.objectReferenceValue;

            if (objRef != null)
            {
                Label(flx, AssetDatabase.GetAssetPath(objRef));
            }
        }
    }

    [CustomEditor(typeof(BuildProfile))]
    internal class BuildProfileInspector : Editor
    {
        private SerializedProperty _bundleSerial ;
        private SerializedProperty _forceRebuild ;
        private SerializedProperty _usePastBundle;
        private SerializedProperty _currentCarry ;
        private SerializedProperty _carryOptions ;
        private SerializedProperty _bundleEntries;
        private SerializedProperty _bundlePatches;

        internal static BuildProfileInspector instance;

        internal string currentCarry
        {
            get { return _currentCarry.stringValue ; }
            set { _currentCarry.stringValue = value; }
        }

        private void OnEnable()
        {
            instance = this;

            _bundleSerial  = serializedObject.FindProperty("_bundleSerial" );
            _forceRebuild  = serializedObject.FindProperty("_forceRebuild" );
            _usePastBundle = serializedObject.FindProperty("_usePastBundle");
            _currentCarry  = serializedObject.FindProperty("_currentCarry" );
            _carryOptions  = serializedObject.FindProperty("_carryOptions" );
            _bundleEntries = serializedObject.FindProperty("_bundleEntries");
            _bundlePatches = serializedObject.FindProperty("_bundlePatches");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_bundleSerial , new GUIContent(I18N.BundleSerial ));
            EditorGUILayout.PropertyField(_forceRebuild , new GUIContent(I18N.ForceRebuild ));
            EditorGUILayout.PropertyField(_usePastBundle, new GUIContent(I18N.UsePastBundle));
            EditorGUILayout.PropertyField(_carryOptions , new GUIContent(I18N.CarryOptions ));

            var alignmentButton = new GUIStyle(GUI.skin.button)
            {
                alignment = TextAnchor.MiddleLeft
            };

            EditorGUILayout.PropertyField(_bundleEntries, new GUIContent(I18N.BundleEntries));
            if (GUILayout.Button(new string(' ', 8) + I18N.PackBundleForAndroid, alignmentButton))
            {
                FunctionMenu.PackBundleForAndroid();
            }
            if (GUILayout.Button(new string(' ', 8) + I18N.PackBundleForIOS, alignmentButton))
            {
                FunctionMenu.PackBundleForIOS();
            }

            EditorGUILayout.PropertyField(_bundlePatches, new GUIContent(I18N.PatchEntries));
            if (GUILayout.Button(new string(' ', 8) + I18N.CopyPatch, alignmentButton))
            {
                FunctionMenu.CopyPatches();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
