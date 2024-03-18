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

namespace Swzwij.Crayon
{
    /// <summary>
    /// Provides custom folder icon functionality for the Unity Editor.
    /// </summary>  
    [InitializeOnLoad]
    public class Crayon
    {
        /// <summary>
        /// Initializes the Crayon class, preparing folder icon customization.
        /// </summary>
        static Crayon()
        {
            CrayonBox.BuildIconList();
            EditorApplication.projectWindowItemOnGUI += DrawFolderIcons;
        }

        /// <summary>
        /// Handles drawing custom folder icons in the Project Window.
        /// </summary>
        /// <param name="guid">The unique identifier of the project asset.</param>
        /// <param name="rect">The drawing rectangle for the icon.</param>
        private static void DrawFolderIcons(string guid, Rect rect)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Dictionary<string, Texture> icons = CrayonBox.i_icons;

            if (SkipProcess(path, icons))
                return;

            Rect imageRect = GenerateRect(rect);
            Texture texture = CrayonBox.i_icons[Path.GetFileName(path)];

            if (texture == null)
                return;

            GUI.DrawTexture(imageRect, texture);
        }

        /// <summary>
        /// Determines whether to skip custom icon processing for a given path.
        /// </summary>
        /// <param name="path">The asset path to evaluate.</param>
        /// <param name="icons">A dictionary of available custom icons.</param>
        /// <returns>True to skip icon drawing, false to proceed.</returns>
        private static bool SkipProcess(string path, Dictionary<string, Texture> icons)
        {
            return string.IsNullOrEmpty(path) || 
                   Event.current.type != EventType.Repaint || 
                   !Directory.Exists(path) || 
                   !icons.ContainsKey(Path.GetFileName(path));
        }

        /// <summary>
        /// Generates a suitable rectangle for drawing the custom folder icon.
        /// </summary>
        /// <param name="rect">The original rectangle provided.</param>
        /// <returns>A modified rectangle to accommodate the icon.</returns>
        private static Rect GenerateRect(Rect rect)
        {
            Rect imageRect;

            if (rect.height > 20)
                imageRect = new Rect(rect.x - 1, rect.y - 1, rect.width + 2, rect.width + 2);
            else if (rect.x > 20)
                imageRect = new Rect(rect.x - 1, rect.y - 1, rect.height + 2, rect.height + 2);
            else
                imageRect = new Rect(rect.x + 2, rect.y - 1, rect.height + 2, rect.height + 2);

            return imageRect;
        }
    }
}

#endif