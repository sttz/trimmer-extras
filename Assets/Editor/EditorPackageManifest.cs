//
// Trimmer Framework for Unity
// https://sttz.ch/trimmer
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
[CreateAssetMenu(fileName = "PackageManifest.asset", menuName = "Trimmer/Package Manifest")]
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
