// Copyright (c) 2024 Samuel Zwijsen (swzwij)
// This work is licensed under a varient of the MIT License Agreement.
// To view a copy of this license, visit the License URL (https://swzwij.notion.site/Tool-License-4b6f56a8be234a9dbf6ee3da31e71a92).
// 
// NOTICE: You must provide appropriate credit to the author 
// (see license for details).

#if UNITY_EDITOR

using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Swzwij.FolderArchitecture
{
    /// <summary>
    /// Defines an editor tool to create a standard folder structure for Unity projects.
    /// </summary>
    public class FolderArchitect : EditorWindow
    {
        private const string ROOT_FOLDER = "Assets/";
        private const string SCRIPT_NAME = "FolderArchitect.cs";
        private const string DESTINATION_FOLDER = "Scripts/Utils";

        /// <summary>
        /// Provides a list of essential folders for a typical Unity project.
        /// </summary>
        private static readonly List<string> _folders = new()
        {
            "Art",
            "Art/Animations",
            "Art/Materials",
            "Art/Models",
            "Art/Rigs",
            "Art/Shaders",
            "Art/Textures",
            "Audio",
            "Audio/Sound Effects",
            "Audio/Music",
            "Editor",
            "Plugins",
            "Prefabs",
            "Resources",
            "Scenes",
            "Scripts",
            "Scripts/Utils",
            "Settings"
        };

        /// <summary>
        /// Generates the defined folder structure within the Unity project.
        /// </summary>
        [MenuItem("Tools/Folder Architect/Generate Default Architecture")]
        private static void Generate() => GenerateCustomArchitecture(null);

        /// <summary>
        /// Generates a custom folder structure based on a provided architecture list.
        /// </summary>
        /// <param name="architecture">A list of folders to create.</param>
        public static void GenerateCustomArchitecture(List<string> architecture)
        {
            List<string> folderArchitecture = architecture ?? _folders;

            foreach (var path in folderArchitecture
                         .Select(folder => ROOT_FOLDER + folder)
                         .Where(path => !Directory.Exists(path)))
            {
                Directory.CreateDirectory(path);
            }

            AssetDatabase.Refresh();

            MoveSelf();
        }

        /// <summary>
        /// Moves this script to its designated location within the project.
        /// </summary>
        private static void MoveSelf()
        {
            AssetDatabase.MoveAsset
            (
                AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(CreateInstance<FolderArchitect>())), 
                ROOT_FOLDER + DESTINATION_FOLDER + "/" + SCRIPT_NAME
            );
        }

        /// <summary>
        /// Converts a newline-separated string representation of a folder structure into a list of folders.
        /// </summary>
        /// <param name="architecture">A string where each line represents a folder.</param>
        /// <returns>A list of folder paths.</returns>
        public static List<string> ProcessArchitecture(string architecture)
        {
            string[] lines = architecture.Split('\n');

            return lines.Select(line => line.Trim()).ToList();
        }
    }
}

#endif