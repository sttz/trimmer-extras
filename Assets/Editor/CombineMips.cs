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

using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Helper class to combine multiple textures into one, assigning the
/// different textures to mip levels.
/// </summary>
/// <remarks>
/// The input textures need to have a size that is power of two. If two 
/// input textures has the same size, one will overwrite the other. If 
/// there are less input textures than mip levels, some levels will be
/// left blank, in which case Unity will make them grey.
/// </remarks>
public class CombineMips
{
    [MenuItem("Assets/Combine Mips")]
    public static void Combine()
    {
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (string.IsNullOrEmpty(path)) {
            Debug.LogError("Could not determine output path.");
            return;
        }

        var dir = Path.GetDirectoryName(path);
        var file = Path.GetFileNameWithoutExtension(path);
        var target = Path.Combine(dir, file + ".asset");

        var combined = Combine(Selection.objects.OfType<Texture2D>());
        if (combined == null) return;

        AssetDatabase.CreateAsset(combined, target);
        Debug.Log("Saved combined texture to: " + target, combined);
    }

    [MenuItem("Assets/Combine Mips", true)]
    static bool ValidateCombine()
    {
        return Selection.objects.OfType<Texture2D>().Count() > 1;
    }

    public static Texture2D Combine(string directory)
    {
        if (!Directory.Exists(directory)) {
            Debug.LogError("Directory '" + directory + "' does not exist or is not a directory.");
            return null;
        }

        var textures = new List<Texture2D>();
        var files = Directory.GetFiles(directory);
        foreach (var file in files) {
            var ext = Path.GetExtension(file);
            if (ext == ".meta") continue;

            var tex = (Texture2D)AssetDatabase.LoadAssetAtPath(file, typeof(Texture2D));
            if (tex != null) {
                textures.Add(tex);
            }
        }

        if (textures.Count == 0) {
            Debug.LogError("No textures found in '" + directory + "'");
            return null;
        }

        return Combine(textures);
    }

    public static Texture2D Combine(IEnumerable<string> paths)
    {
        var textures = new List<Texture2D>();
        foreach (var path in paths) {
            var tex = (Texture2D)AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));
            if (tex == null) {
                Debug.LogError("Could not load texture at path: " + path);
                return null;
            }
            textures.Add(tex);
        }

        return Combine(textures);
    }

    public static Texture2D Combine(IEnumerable<Texture2D> textures)
    {
        var max = textures.OrderByDescending(t => t.width).First();
        var maxPower = (int)Mathf.Log(max.width, 2);
        var combined = new Texture2D(max.width, max.height, max.format, true);
        foreach (var tex in textures.OrderBy(t => t.width)) {
            var power = -1;
            for (int i = maxPower; i >= 0; i--) {
                if (Mathf.Pow(2, i) == tex.width) {
                    power = i;
                    break;
                }
            }

            if (power < 0) {
                Debug.LogError("Could not determine mip level of texture " + tex.name, tex);
                return null;
            }

            var mipLevel = maxPower - power;
            Debug.Log("Using " + tex.name + " as mip level " + mipLevel, tex);

            var pixels = tex.GetPixels32(0);
            combined.SetPixels32(pixels, mipLevel);
        }

        combined.Apply(false);

        return combined;
    }
}