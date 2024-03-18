// Copyright (c) 2024 Samuel Zwijsen (swzwij)
// This work is licensed under a varient of the MIT License Agreement.
// To view a copy of this license, visit the License URL (https://swzwij.notion.site/Tool-License-4b6f56a8be234a9dbf6ee3da31e71a92).
// 
// NOTICE: You must provide appropriate credit to the author 
// (see license for details).

#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;

namespace Swzwij.Crayon
{
    /// <summary>
    /// Stores data associating a custom icon with specific folder names. 
    /// </summary>
    public class CrayonFolderIcon : ScriptableObject
    {
        /// <summary>
        /// The texture to be used as the custom folder icon.
        /// </summary>
        [SerializeField] private Texture2D _icon;

        /// <summary>
        /// A list of folder names that should use the specified icon.
        /// </summary>
        [SerializeField] private List<string> folderNames;

        /// <summary>
        /// Provides access to the icon texture.
        /// </summary>
        public Texture2D Icon => _icon;

        /// <summary>
        /// Provides access to the list of associated folder names.
        /// </summary>
        public List<string> FolderNames => folderNames;

        /// <summary>
        /// Triggers a rebuild of the folder icon library.
        /// </summary>
        public void OnValidate() => CrayonBox.BuildIconList();
    }
}

#endif