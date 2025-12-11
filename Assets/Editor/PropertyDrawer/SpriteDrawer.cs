using UnityEngine;
using UnityEditor;

namespace SpriteDrawer
{
    /// <summary>
    /// Sprite変数をInspector上にプレビュー表示するPropertyDrawer
    /// </summary>
    [CustomPropertyDrawer(typeof(SpritePreviewAttribute))]
    public class SpritePreviewDrawer : PropertyDrawer
    {
        private const float PreviewSize = 64f;
        private const float Padding = 2f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.ObjectReference)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            EditorGUI.BeginProperty(position, label, property);

            // Sprite参照フィールドの描画
            Rect fieldRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(fieldRect, property, label);

            // Spriteが設定されている場合、プレビューを表示
            Sprite sprite = property.objectReferenceValue as Sprite;
            if (sprite != null)
            {
                Rect previewRect = new Rect(
                    position.x + EditorGUIUtility.labelWidth + Padding,
                    position.y + EditorGUIUtility.singleLineHeight + Padding,
                    PreviewSize,
                    PreviewSize
                );

                // 背景（チェッカーボード）
                DrawCheckerboard(previewRect);

                // Spriteのプレビュー描画
                Texture2D texture = sprite.texture;
                Rect spriteRect = sprite.rect;
                Rect texCoords = new Rect(
                    spriteRect.x / texture.width,
                    spriteRect.y / texture.height,
                    spriteRect.width / texture.width,
                    spriteRect.height / texture.height
                );

                GUI.DrawTextureWithTexCoords(previewRect, texture, texCoords);

                // 枠線
                EditorGUI.DrawRect(new Rect(previewRect.x - 1, previewRect.y - 1, previewRect.width + 2, 1), Color.black);
                EditorGUI.DrawRect(new Rect(previewRect.x - 1, previewRect.y + previewRect.height, previewRect.width + 2, 1), Color.black);
                EditorGUI.DrawRect(new Rect(previewRect.x - 1, previewRect.y, 1, previewRect.height), Color.black);
                EditorGUI.DrawRect(new Rect(previewRect.x + previewRect.width, previewRect.y, 1, previewRect.height), Color.black);

                // サイズ情報表示
                Rect infoRect = new Rect(
                    previewRect.x + PreviewSize + Padding * 2,
                    previewRect.y,
                    200,
                    EditorGUIUtility.singleLineHeight
                );
                EditorGUI.LabelField(infoRect, $"Size: {(int)spriteRect.width} x {(int)spriteRect.height}");
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            Sprite sprite = property.objectReferenceValue as Sprite;
            if (sprite != null)
            {
                return EditorGUIUtility.singleLineHeight + PreviewSize + Padding * 2;
            }
            return EditorGUIUtility.singleLineHeight;
        }

        private void DrawCheckerboard(Rect rect)
        {
            int checkSize = 8;
            Color color1 = new Color(0.8f, 0.8f, 0.8f);
            Color color2 = new Color(0.6f, 0.6f, 0.6f);

            for (int y = 0; y < rect.height; y += checkSize)
            {
                for (int x = 0; x < rect.width; x += checkSize)
                {
                    Rect checkRect = new Rect(rect.x + x, rect.y + y, checkSize, checkSize);
                    Color color = ((x / checkSize) + (y / checkSize)) % 2 == 0 ? color1 : color2;
                    EditorGUI.DrawRect(checkRect, color);
                }
            }
        }
    }

    /// <summary>
    /// Sprite変数にプレビューを表示するための属性
    /// 使用例: [SpritePreview] public Sprite mySprite;
    /// </summary>
    public class SpritePreviewAttribute : PropertyAttribute
    {
    }
}
