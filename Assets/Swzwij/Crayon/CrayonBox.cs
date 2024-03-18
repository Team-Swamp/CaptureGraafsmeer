// Copyright (c) 2024 Samuel Zwijsen (swzwij)
// This work is licensed under a varient of the MIT License Agreement.
// To view a copy of this license, visit the License URL (https://swzwij.notion.site/Tool-License-4b6f56a8be234a9dbf6ee3da31e71a92).
// 
// NOTICE: You must provide appropriate credit to the author 
// (see license for details).

#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Swzwij.Crayon
{
    /// <summary>
    /// Manages a library of custom folder icons.
    /// </summary>
    public class CrayonBox : AssetPostprocessor
    {
        /// <summary>
        /// The path where folder icon assets are located within the project.
        /// </summary>
        private const string ASSET_PATH = "Swzwij/Crayon/CrayonBox";

        /// <summary>
        /// A dictionary of folder names and their associated icon textures.
        /// </summary>
        internal static readonly Dictionary<string, Texture> i_icons = new();

        /// <summary>
        /// Handles changes to assets within the project, rebuilding the icon library if necessary.
        /// </summary>
        /// <param name="imported">Newly imported assets.</param>
        /// <param name="deleted">Deleted assets.</param>
        /// <param name="moved">Assets that have been moved.</param>
        /// <param name="movedFrom">Original paths of moved assets.</param>
        private static void OnPostprocessAllAssets(string[] imported, string[] deleted, string[] moved, string[] movedFrom)
        {
            if (!ContainsIconAsset(imported, deleted, moved, movedFrom))
                return;

            BuildIconList();
        }

        /// <summary>
        /// Checks if changes to assets involve the folder icon assets.
        /// </summary>
        /// <param name="assetArrays">Arrays of asset paths related to the changes.</param>
        /// <returns>True if a relevant icon asset has changed, false otherwise.</returns>
        private static bool ContainsIconAsset(params string[][] assetArrays)
        {
            return assetArrays.SelectMany(assets => assets)
                .Any(asset => ReplaceSeparatorCharacters(Path.GetDirectoryName(asset)) == "Assets/" + ASSET_PATH);
        }

        /// <summary>
        /// Standardizes path separators for consistency.
        /// </summary>
        /// <param name="path">The path to normalize.</param>
        /// <returns>The path with forward slashes as separators.</returns>
        private static string ReplaceSeparatorCharacters(string path) => path.Replace("\\", "/");

        /// <summary>
        /// Initializes or rebuilds the library of folder icons.
        /// </summary>
        internal static void BuildIconList()
        {
            i_icons.Clear();

            DirectoryInfo directory = new(Application.dataPath + "/" + ASSET_PATH);
            LoadTextures(directory);
            LoadFolderIcons(directory);
        }

        /// <summary>
        /// Loads standard textures as potential folder icons.
        /// </summary>
        /// <param name="directory">The directory containing the icon assets.</param>
        private static void LoadTextures(DirectoryInfo directory)
        {
            FileInfo[] textures = directory.GetFiles("*.png");

            foreach (FileInfo texture in textures)
            {
                string texturePath = $"Assets/{ASSET_PATH}/{texture.Name}";
                Texture iconTexture = (Texture)AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture2D));
                i_icons.Add(Path.GetFileNameWithoutExtension(texture.Name), iconTexture);
            }
        }

        /// <summary>
        /// Loads associations between textures and folder names from the assets.
        /// </summary>
        /// <param name="directory">The directory containing the icon assets</param>
        private static void LoadFolderIcons(DirectoryInfo directory)
        {
            FileInfo[] assets = directory.GetFiles("*.asset");

            foreach (FileInfo asset in assets)
            {
                string assetPath = $"Assets/{ASSET_PATH}/{asset.Name}";
                CrayonFolderIcon folderIcon = (CrayonFolderIcon)AssetDatabase.LoadAssetAtPath(assetPath, typeof(CrayonFolderIcon));

                if (folderIcon == null)
                    continue;

                Texture texture = folderIcon.Icon;
                List<string> folderNames = folderIcon.FolderNames;

                foreach (var folderName in folderNames
                             .Where(folderName => folderName != null))
                {
                    i_icons.TryAdd(folderName, texture);
                }
            }
        }
    }
}

#endif