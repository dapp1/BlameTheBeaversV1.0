using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class TextureCompressionOverride : Editor
{
    [MenuItem("Assets/TextureImport/DownsizeTexture")]
    public static async void DownsizeTexture()
    {
        var obj = Selection.activeObject;
        var path = AssetDatabase.GetAssetPath(obj);
        if (obj.GetType() != typeof(Texture2D))
        {
            Debug.LogError($"NonTexture, <color=#bb9900>Type: {obj.GetType()}</color>\n Path: {path}");
            await Task.Delay(5);
            return;
        }
        TextureImporter pImporter = AssetImporter.GetAtPath(path) as TextureImporter;
        if (pImporter == null)
        {
            Debug.LogError($"<color=#991321>pImporter not found, skip </color><color=#aaaaff>{path.Split('/').Last()}</color>");
            return;
        }
        if (!pImporter.isReadable)
        {
            pImporter.isReadable = true;
            pImporter.SaveAndReimport();
        }
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer == null)
        {
            Debug.LogError($"<color=#991321>importer not found, skip </color><color=#aaaaff>{path.Split('/').Last()}</color>");
            return;
        }
        importer.maxTextureSize = importer.maxTextureSize / 2 > 64 ? importer.maxTextureSize / 2 : 64;
        TextureImporterFormat usedFormat = importer.maxTextureSize > 1024 ? TextureImporterFormat.ASTC_8x8 : importer.maxTextureSize <= 512 ? TextureImporterFormat.ASTC_5x5 : TextureImporterFormat.ASTC_6x6;
        var android = importer.GetPlatformTextureSettings("Android");
        if (android.overridden)
        {
            android.maxTextureSize = importer.maxTextureSize;
            android.format = usedFormat;
        }
        importer.SetPlatformTextureSettings(android);
        var ios = importer.GetPlatformTextureSettings("iPhone");
        if (ios.overridden)
        {
            ios.maxTextureSize = importer.maxTextureSize;
            ios.format = usedFormat;
        }
        importer.SetPlatformTextureSettings(ios);
        importer.SaveAndReimport();
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/TextureImport/UpgradeQuality")]
    public static async void UpgradeQuality()
    {
        var objects = Selection.objects;
        foreach (var obj in objects)
        {
            var path = AssetDatabase.GetAssetPath(obj);
            if (obj.GetType() != typeof(Texture2D))
            {
                Debug.LogError($"NonTexture, <color=#bb9900>Type: {obj.GetType()}</color>\n Path: {path}");
                await Task.Delay(5);
                return;
            }
            TextureImporter pImporter = AssetImporter.GetAtPath(path) as TextureImporter;
            if (pImporter == null)
            {
                Debug.LogError($"<color=#991321>pImporter not found, skip </color><color=#aaaaff>{path.Split('/').Last()}</color>");
                return;
            }
            if (!pImporter.isReadable)
            {
                pImporter.isReadable = true;
                pImporter.SaveAndReimport();
            }
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer == null)
            {
                Debug.LogError($"<color=#991321>importer not found, skip </color><color=#aaaaff>{path.Split('/').Last()}</color>");
                return;
            }
            importer.maxTextureSize = importer.maxTextureSize * 2 <= 2048 ? importer.maxTextureSize * 2 : 2048;
            TextureImporterFormat usedFormat = importer.maxTextureSize > 1024 ? TextureImporterFormat.ASTC_6x6 : importer.maxTextureSize <= 512 ? TextureImporterFormat.ASTC_4x4 : TextureImporterFormat.ASTC_5x5;
            var android = importer.GetPlatformTextureSettings("Android");
            if (android.overridden)
            {
                android.maxTextureSize = importer.maxTextureSize;
                android.format = usedFormat;
                android.compressionQuality = 100;
            }
            importer.SetPlatformTextureSettings(android);
            var ios = importer.GetPlatformTextureSettings("iPhone");
            if (ios.overridden)
            {
                ios.maxTextureSize = importer.maxTextureSize;
                ios.format = usedFormat;
                ios.compressionQuality = 100;
            }
            importer.SetPlatformTextureSettings(ios);
            importer.SaveAndReimport();
            AssetDatabase.Refresh();
            await Task.Delay(5);
        }
    }

    [MenuItem("Assets/TextureImport/RemoveMipMaps")]
    public static async void RemoveMipMaps()
    {
        var objects = Selection.objects;
        foreach (var obj in objects)
        {
            var path = AssetDatabase.GetAssetPath(obj);
            if (obj.GetType() != typeof(Texture2D))
            {
                Debug.LogError($"NonTexture, <color=#bb9900>Type: {obj.GetType()}</color>\n Path: {path}");
                await Task.Delay(5);
                continue;
            }
            await Task.Delay(5);
            TextureImporter pImporter = AssetImporter.GetAtPath(path) as TextureImporter;
            if (pImporter == null || !pImporter.mipmapEnabled)
            {
                Debug.LogError($"<color=#991321>Skip </color><color=#aaaaff>{path.Split('/').Last()}</color>");
                continue;
            }
            pImporter.isReadable = true;
            pImporter.mipmapEnabled = false;
            pImporter.SaveAndReimport();
            AssetDatabase.Refresh();
        }
    }

    [MenuItem("Assets/TextureImport/OverrideCompression")]
    public static async void OverrideCompression()
    {
        var objects = Selection.objects;
        int c = 0;
        foreach (var obj in objects)
        {
            c++;
            float prc = ((float)c / (float)objects.Length);

            var path = AssetDatabase.GetAssetPath(obj);
            if (obj.GetType() != typeof(Texture2D))
            {
                Debug.LogError($"NonTexture, <color=#bb9900>Type: {obj.GetType()}</color>\n Path: {path}");
                await Task.Delay(5);
                continue;
            }
            Debug.Log($"<color=#168743>Start proceed file: </color><color=#aaaaff>{path.Split('/').Last()}</color>\n Path: {path}");
            await Task.Delay(5);
            TextureImporter pImporter = AssetImporter.GetAtPath(path) as TextureImporter;
            if (pImporter == null)
            {
                Debug.LogError($"<color=#991321>pImporter not found, skip </color><color=#aaaaff>{path.Split('/').Last()}</color>");
                continue;
            }
            pImporter.isReadable = true;
            pImporter.mipmapEnabled = false;
            pImporter.SaveAndReimport();
            if (pImporter.GetPlatformTextureSettings("Android").overridden)
            {
                Debug.Log($"<color=#84661a>File already overriden: </color><color=#aaaaff>{path.Split('/').Last()}</color>");
                await Task.Delay(5);
                continue;
            }
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (texture == null)
            {
                Debug.LogError($"<color=#991321>texture not found, skip </color><color=#aaaaff>{path.Split('/').Last()}</color>");
                continue;
            }
            int maxSize = Mathf.Max(texture.width, texture.height);
            var pwr = Mathf.FloorToInt(maxSize / 32f);
            for (int i = 0; i <= 6; i++)
            {
                var nearest = Mathf.Pow(2, i);
                if (nearest > pwr)
                {
                    pwr = (int)Mathf.Pow(2, i - 1);
                    break;
                }
            }
            maxSize = Mathf.Max(pwr, 1) * 32;
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer == null)
            {
                Debug.LogError($"<color=#991321>importer not found, skip </color><color=#aaaaff>{path.Split('/').Last()}</color>");
                continue;
            }
            if (!importer.crunchedCompression)
            {
                maxSize = Mathf.Min(maxSize, 1024, importer.maxTextureSize);
            }
            else
            {
                maxSize = Mathf.Min(maxSize, 1024);
                maxSize /= importer.compressionQuality < 100 ? 2 : 1;
                importer.compressionQuality = 100;
                importer.crunchedCompression = false;
            }
            Debug.Log($"<color=#256294>Max size for asset </color><color=#aaaaff>{path.Split('/').Last()}</color> : {maxSize}");
            await Task.Delay(5);
            importer.maxTextureSize = maxSize;
            importer.mipmapEnabled = false;
            TextureImporterFormat usedFormat = maxSize > 1024 ? TextureImporterFormat.ASTC_8x8 : maxSize <= 512 ? TextureImporterFormat.ASTC_5x5 : TextureImporterFormat.ASTC_6x6;
            var current = importer.GetDefaultPlatformTextureSettings();
            TextureImporterPlatformSettings android = new TextureImporterPlatformSettings();
            android.overridden = true;
            android.textureCompression = TextureImporterCompression.Compressed;
            android.maxTextureSize = current.maxTextureSize;
            android.format = usedFormat;
            android.compressionQuality = 50;
            android.name = "Android";
            TextureImporterPlatformSettings iOS = new TextureImporterPlatformSettings();
            iOS.overridden = true;
            iOS.textureCompression = TextureImporterCompression.Compressed;
            iOS.maxTextureSize = current.maxTextureSize;
            iOS.format = usedFormat;
            iOS.compressionQuality = 50;
            iOS.name = "iPhone";
            importer.SetPlatformTextureSettings(android);
            importer.SetPlatformTextureSettings(iOS);
            importer.SaveAndReimport();
            importer.isReadable = true;
            AssetDatabase.Refresh();
            Debug.Log($"<color=#00aa23>File </color><color=#aaaaff>{path.Split('/').Last()}</color><color=#00aa23> was successfuly updated\n</color>{path}");
            await Task.Delay(10);
        }
    }
}
