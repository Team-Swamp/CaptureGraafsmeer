// Copyright (c) 2024 Samuel Zwijsen (swzwij)
// This work is licensed under a varient of the MIT License Agreement.
// To view a copy of this license, visit the License URL (https://swzwij.notion.site/Tool-License-4b6f56a8be234a9dbf6ee3da31e71a92).
// 
// NOTICE: You must provide appropriate credit to the author 
// (see license for details).

#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Swzwij.FolderArchitecture
{
    /// <summary>
    /// Provides a custom editor window for creating folder structures within a Unity project.
    /// </summary>
    public class FolderArchitectWindow : EditorWindow
    {
        /// <summary>
        /// Holds a list of desired folder paths (one per line) to be created within the project.
        /// </summary>
        [Multiline] private string inputText = "Art\r\nArt/Animations\r\nArt/Materials\r\nArt/Models\r\nArt/Rigs\r\nArt/Shaders\r\nArt/Textures\r\nAudio\r\nAudio/Sound Effects\r\nAudio/Music\r\nEditor\r\nPlugins\r\nPrefabs\r\nResources\r\nScenes\r\nScripts\r\nScripts/Utils\r\nSettings";

        /// <summary>
        /// Opens the Folder Architect window in the Unity Editor.
        /// </summary>
        [MenuItem("Tools/Folder Architect/Create Custom Architecture")]
        private static void Init()
        {
            FolderArchitectWindow window = (FolderArchitectWindow)GetWindow(typeof(FolderArchitectWindow));
            window.Show();
            window.titleContent = new GUIContent("Architecture Creator");
        }

        /// <summary>
        /// Renders the GUI for the Folder Architect window and handles folder creation logic. 
        /// </summary>
        private void OnGUI()
        {
            List<string> formatedStructure;
            inputText = EditorGUILayout.TextArea(inputText, GUILayout.ExpandHeight(true));

            if (GUILayout.Button("Generate Architecture"))
            {
                formatedStructure = FolderArchitect.ProcessArchitecture(inputText);
                FolderArchitect.GenerateCustomArchitecture(formatedStructure);
            }
        }
    }
}

#endif