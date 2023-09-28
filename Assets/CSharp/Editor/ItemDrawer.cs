using System;
using UnityEditor;
using UnityEngine;

namespace U3DMobile.Edit
{
    internal class BaseItemDrawer : PropertyDrawer
    {
        protected const float flx = 0;

        private const float columnGap = 4;
        private const float lineGap   = 2;

        private float _x = 0;
        private float _y = 0;
        private float _w = 0;

        public override sealed float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return (EditorGUIUtility.singleLineHeight + lineGap) * OnGetLines();
        }

        public override sealed void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            _w = rect.width + OnGetWidthExtension();

            float height = EditorGUIUtility.singleLineHeight + lineGap;
            int   line   = OnGetLines();
            float indent = OnGetIndent();

            for (int i = 0; i < line; ++i)
            {
                _x = rect.x + indent;
                _y = rect.y + height * i;
                OnDrawLine(i, property);
            }
        }

        protected virtual int OnGetLines()
        {
            return 1;
        }

        protected virtual float OnGetIndent()
        {
            //全局条目添加一个缩进, 和列表条目对齐.
            return 14;
        }

        protected virtual float OnGetWidthExtension()
        {
            return 18;
        }

        protected virtual void OnDrawLine(int line, SerializedProperty property)
        {
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
                //单选框使用一个样式, 和勾选框有所区别.
                on = EditorGUI.Toggle(rect, beingOn, GUI.skin.GetStyle("BoldToggle"));
            });

            afterOn = on;
        }

        private void DrawElement(float width, Action<Rect> delegation)
        {
            if (width == flx && _x < _w)
            {
                width = _w - _x;
            }

            var rect = new Rect(_x, _y, width, EditorGUIUtility.singleLineHeight);
            delegation(rect);

            _x += width + columnGap;
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
