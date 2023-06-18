using UnityEditor;
using UnityEngine;

namespace U3DMobileEditor
{
    internal class BaseItemDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * OnGetLines();
        }

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            _w = rect.width;

            float lineH  = EditorGUIUtility.singleLineHeight;
            int   lines  = OnGetLines ();
            float indent = OnGetIndent();

            if (lines >= 1) { _x = indent; _y = 0        ; OnDrawFirstLine (property); }
            if (lines >= 2) { _x = indent; _y = lineH * 1; OnDrawSecondLine(property); }
            if (lines >= 3) { _x = indent; _y = lineH * 2; OnDrawThirdLine (property); }
        }

        protected const float flx = 0;
        private   const float gap = 4;

        private float _x = 0;
        private float _y = 0;
        private float _w = 0;

        protected virtual float OnGetIndent()
        {
            return 14;
        }

        protected virtual int OnGetLines()
        {
            return 1;
        }

        protected virtual void OnDrawFirstLine (SerializedProperty property) {}
        protected virtual void OnDrawSecondLine(SerializedProperty property) {}
        protected virtual void OnDrawThirdLine (SerializedProperty property) {}
    }

    internal class ListItemDrawer : BaseItemDrawer
    {
        protected override float OnGetIndent()
        {
            return 0;
        }
    }
}
