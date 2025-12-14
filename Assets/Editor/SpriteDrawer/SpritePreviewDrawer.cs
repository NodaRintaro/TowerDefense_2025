using UnityEngine;
using UnityEditor;

/// <summary>
/// Sprite変数をInspector上にプレビュー表示するPropertyDrawer
/// </summary>
[CustomPropertyDrawer(typeof(SpritePreviewAttribute))]
public class SpritePreviewDrawer : PropertyDrawer
{
    private const float _maxPreviewSize = 300f;
    private const float _padding = 20f;

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
            Texture2D texture = sprite.texture;
            Rect spriteRect = sprite.rect;

            // アスペクト比を維持しながら表示サイズを計算
            float aspectRatio = spriteRect.width / spriteRect.height;
            float previewWidth, previewHeight;

            if (aspectRatio >= 1f) // 横長または正方形
            {
                previewWidth = Mathf.Min(_maxPreviewSize, spriteRect.width);
                previewHeight = previewWidth / aspectRatio;
            }
            else // 縦長
            {
                previewHeight = Mathf.Min(_maxPreviewSize, spriteRect.height);
                previewWidth = previewHeight * aspectRatio;
            }

            Rect previewRect = new Rect(
                position.x + EditorGUIUtility.labelWidth + _padding,
                position.y + EditorGUIUtility.singleLineHeight + _padding,
                previewWidth,
                previewHeight
            );

            // 背景(チェッカーボード)
            DrawBackboard(previewRect);

            // Spriteのプレビュー描画
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
                previewRect.x + previewWidth + _padding * 2,
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
            Rect spriteRect = sprite.rect;
            float aspectRatio = spriteRect.width / spriteRect.height;
            float previewHeight;

            //スプライト画像の横の大きさが縦の大きさを上回っていればその分画像の縦の大きさを削る
            if (aspectRatio > 1f)
            {
                float previewWidth = Mathf.Min(_maxPreviewSize, spriteRect.width);
                previewHeight = previewWidth / aspectRatio;
            }
            else
            {
                previewHeight = Mathf.Min(_maxPreviewSize, spriteRect.height);
            }

            return EditorGUIUtility.singleLineHeight + previewHeight + _padding * 2;
        }
        return EditorGUIUtility.singleLineHeight;
    }

    /// <summary>
    /// 背景の描画
    /// </summary>
    /// <param name="rect"></param>
    private void DrawBackboard(Rect rect)
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