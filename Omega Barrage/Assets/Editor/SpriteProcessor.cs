using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpriteProcessor : AssetPostprocessor {

    void OnPostprocessTexture(Texture2D texture)
    {
        string lowercaseAssetPath = assetPath.ToLower();
        bool isInSpritesDirectory = lowercaseAssetPath.IndexOf("/art/sprites/spritesheets/") != -1;

        if (isInSpritesDirectory)
        {
            TextureImporter textureImporter = (TextureImporter)assetImporter;
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.spriteImportMode = SpriteImportMode.Multiple;
            textureImporter.spritePixelsPerUnit = 16;
        }
    }

}
