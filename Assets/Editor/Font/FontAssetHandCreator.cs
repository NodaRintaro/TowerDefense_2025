// FontAssetHandCreator.cs  
using TMPro;
using UnityEditor;
using UnityEngine.TextCore;

namespace OperatorOverload.Editor.Serialization
{
    public class FontAssetHandCreator
    {
        [MenuItem("CONTEXT/TMP_FontAsset/Create Hand")]
        public static void CreateFontAsset(MenuCommand menuCommand)
        {
            int textureWidth = 256;     // テクスチャの幅(px)  
            int textureHeight = 256;    // テクスチャの高さ(px)  
            int characterAreaWidth = 16;    // ひとつの文字領域の幅(px)  
            int characterAreaHeight = 16;   // ひとつの文字領域の高さ(px)  
            int glyphX = 4; // 文字の x オフセット  
            int glyphY = 4; // 文字の y オフセット  
            int glyphWidth = 3; // 文字の幅  
            int glyphHeight = 5;    // 文字の高さ  
            float horizontalMargin = 1; // 文字間の間隔  

            var fontAsset = menuCommand.context as TMP_FontAsset;
            fontAsset!.glyphTable.Clear();
            fontAsset.characterTable.Clear();

            for (int i = 0x20; i < 0x7f; i++)
            {
                var glyph = new Glyph((uint)i, new GlyphMetrics(glyphWidth, glyphHeight, 0, glyphHeight, glyphWidth + horizontalMargin),
                    new GlyphRect(
                        i % (textureWidth / characterAreaWidth) * characterAreaWidth + glyphX,
                        textureHeight - (i / (textureWidth / characterAreaWidth) * characterAreaHeight + characterAreaHeight - glyphY),
                        glyphWidth, glyphHeight));
                var character = new TMP_Character((uint)i, glyph);

                fontAsset.glyphTable.Add(glyph);
                fontAsset.glyphLookupTable[glyph.index] = glyph;
                fontAsset.characterTable.Add(character);
                fontAsset.characterLookupTable[character.unicode] = character;
            }

            EditorUtility.SetDirty(fontAsset);
            AssetDatabase.SaveAssets();
        }
    }
}