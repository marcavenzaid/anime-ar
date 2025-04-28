using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class HeadTexturesManager : MonoBehaviour
{
    [MenuItem("Tools/Copy Head Textures to Texture Folder")]
    public static void ProcessTextures() {
        string sourceFolder = "Assets/UniVRM/Textures";
        string destinationFolder = "Assets/Textures/Heads";

        // Ensure destination folder exists
        if (!AssetDatabase.IsValidFolder(destinationFolder)) {
            AssetDatabase.CreateFolder("Assets", "Sprites");
        }

        // Get all textures in the source folder with the pattern *.Textures/Thumbnail
        string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { sourceFolder });

        foreach (string guid in guids) {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);

            // Check if the texture matches the pattern *.Textures/Thumbnail
            if (assetPath.Contains(".Textures/Thumbnail")) {
                Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);

                if (texture != null) {
                    // Determine the renamed file path by correcting the pattern
                    string directory = Path.GetDirectoryName(assetPath);
                    string parentFolderName = new DirectoryInfo(directory).Name;
                    string fileExtension = Path.GetExtension(assetPath);
                    string newFilename = $"{parentFolderName}.Thumbnail{fileExtension}";
                    string destPath = Path.Combine(destinationFolder, newFilename);

                    // Check if it already exists in the destination folder
                    if (!File.Exists(destPath)) {
                        // Copy the texture to the destination folder
                        AssetDatabase.CopyAsset(assetPath, destPath);

                        // Rename the copied texture by removing 'Thumbnail'
                        AssetDatabase.RenameAsset(destPath, newFilename);

                        Debug.Log($"Copied and renamed {assetPath} to {destPath}");
                    } else {
                        Debug.Log($"Texture already exists at {destPath}");
                    }
                }
            }
        }

        // Refresh the asset database to reflect the changes
        AssetDatabase.Refresh();
    }

}
