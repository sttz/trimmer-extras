//
// Trimmer Framework for Unity - https://sttz.ch/trimmer
// Copyright © 2017 Adrian Stutz
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Unity editor helper utility that packs together multiple
/// assets into one (as supported by Unity).
/// </summary>
[CreateAssetMenu(fileName = "PackageManifest.asset", menuName = "Package Manifest")]
public class EditorPackageManifest : ScriptableObject
{
    public enum EntryType
    {
        Asset,
        TextureMips
    }

    [Serializable]
    public struct AssetEntry
    {
        public EntryType type;
        public UnityEngine.Object[] references;
    }

    public AssetEntry[] entries;

    [ContextMenu("Build")]
    public void Build()
    {
        var path = EditorUtility.SaveFilePanelInProject(
            "Editor Package", "EditorPackage.asset", "asset",
            "Choose where to save the editor package asset"
        );
        if (string.IsNullOrEmpty(path)) return;

        var assets = new List<UnityEngine.Object>();
        foreach (var entry in entries) {
            if (entry.type == EntryType.Asset) {
                assets.AddRange(entry.references);
            } else if (entry.type == EntryType.TextureMips) {
                var tex = CombineMips.Combine(entry.references.Cast<Texture2D>());
                if (tex == null) return;
                assets.Add(tex);
            }
        }

        assets.RemoveAll(o => o == null);

        if (assets.Count == 0) {
            Debug.LogError("No assets in packages.");
            return;
        }

        CombineAssets.CombineObjects(assets, path);
    }
}