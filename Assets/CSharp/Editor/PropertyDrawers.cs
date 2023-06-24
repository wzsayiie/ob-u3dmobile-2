using UnityEditor;
using UnityEngine;

namespace U3DMobileEditor
{
    internal class BaseItemDrawer : PropertyDrawer
    {
        protected const float flx = 0;

        private const float columnGap = 4;
        private const float lineGap   = 2;

        private float _x = 0;
        private float _y = 0;
        private float _w = 0;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return (EditorGUIUtility.singleLineHeight + lineGap) * OnGetLines();
        }

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            _w = rect.width + OnGetWidthExtension();

            float lineH  = EditorGUIUtility.singleLineHeight + lineGap;
            int   line   = OnGetLines();
            float indent = OnGetIndent();

            if (line >= 1) { _x = rect.x + indent; _y = rect.y            ; OnDrawFirstLine (property); }
            if (line >= 2) { _x = rect.x + indent; _y = rect.y + lineH * 1; OnDrawSecondLine(property); }
            if (line >= 3) { _x = rect.x + indent; _y = rect.y + lineH * 2; OnDrawThirdLine (property); }
        }

        protected virtual int OnGetLines()
        {
            return 1;
        }

        protected virtual float OnGetIndent()
        {
            //list items have indents.
            //adding an indent for non-list items can make the ui looks tidier.
            return 14;
        }

        protected virtual float OnGetWidthExtension()
        {
            return 18;
        }

        protected virtual void OnDrawFirstLine (SerializedProperty property) {}
        protected virtual void OnDrawSecondLine(SerializedProperty property) {}
        protected virtual void OnDrawThirdLine (SerializedProperty property) {}

        private delegate void DrawDelegation(Rect rect);

        private void DrawElement(float width, DrawDelegation delegation)
        {
            if (width == flx && _x < _w)
            {
                width = _w - _x;
            }

            var rect = new Rect(_x, _y, width, EditorGUIUtility.singleLineHeight);
            delegation(rect);

            _x += width + columnGap;
        }

        protected void Field(float width, SerializedProperty property, string relativeName)
        {
            DrawElement(width, (Rect rect) => {
                SerializedProperty relative = property.FindPropertyRelative(relativeName);
                EditorGUI.PropertyField(rect, relative, GUIContent.none);
            });
        }

        protected void Field(float width, SerializedProperty property)
        {
            DrawElement(width, (Rect rect) => {
                EditorGUI.PropertyField(rect, property, GUIContent.none);
            });
        }
        
        protected void Label(float width, string text)
        {
            DrawElement(width, (Rect rect) => {
                EditorGUI.LabelField(rect, text);
            });
        }

        protected void Radio(float width, bool beingOn, out bool afterOn)
        {
            bool on = false;
            DrawElement(width, (Rect rect) => {
                //use bold style to remind users that this is a "radio" toggle.
                on = EditorGUI.Toggle(rect, beingOn, GUI.skin.GetStyle("BoldToggle"));
            });

            afterOn = on;
        }
    }

    internal class ListItemDrawer : BaseItemDrawer
    {
        protected override float OnGetIndent()
        {
            return 0;
        }

        protected override float OnGetWidthExtension()
        {
            return 48;
        }
    }
}
