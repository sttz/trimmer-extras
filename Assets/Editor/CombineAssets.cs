//
// Trimmer Framework for Unity
// https://sttz.ch/trimmer
//

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CombineAssets : EditorWindow
{
    [MenuItem("Window/Combine Assets")]
    public static void Open()
    {
        EditorWindow.GetWindow<CombineAssets>(true, "Combine Assets");
    }

    static System.Type UnityObject = typeof(Object);

    List<Object> assets;
    UnityEditorInternal.ReorderableList list;
    GUIStyle paddingStyle;

    void OnEnable()
    {
        if (list == null) {
            assets = new List<Object>();
            list = new UnityEditorInternal.ReorderableList(assets, typeof(Object));
            list.drawElementCallback = OnDrawElement;
            list.elementHeight = 16;
        }

        paddingStyle = new GUIStyle();
        paddingStyle.padding = new RectOffset(5, 5, 5, 5);
    }

    void OnGUI()
    {
        EditorGUILayout.BeginVertical(paddingStyle);

        list.DoLayoutList();

        if (GUILayout.Button("Combine")) {
            var path = EditorUtility.SaveFilePanelInProject(
                "Combine Asset", "Combined.asset", "asset",
                "Choose where to save combined asset"
            );
            if (!string.IsNullOrEmpty(path)) {
                CombineObjects(assets, path);
            }
        }

        if (Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragPerform) {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
        }
        if (Event.current.type == EventType.DragPerform) {
            DragAndDrop.AcceptDrag();
            assets.AddRange(DragAndDrop.objectReferences);
        }

        EditorGUILayout.EndVertical();
    }

    void OnDrawElement(Rect rect, int index, bool selected, bool focused)
    {
        EditorGUI.BeginChangeCheck();

        var item = assets[index];
        item = EditorGUI.ObjectField(rect, item, UnityObject, false);

        if (EditorGUI.EndChangeCheck()) {
            assets[index] = item;
        }
    }

    /// <summary>
    /// Combine multiple Unity objects into a single asset.
    /// </summary>
    public static void CombineObjects(IEnumerable<UnityEngine.Object> objects, string outputPath)
    {
        var created = false;
        foreach (var obj in objects) {
            var source = obj;
            if (AssetDatabase.Contains(obj)) {
                source = Object.Instantiate(obj);
                source.name = obj.name;
            }
            if (!created) {
                created = true;
                AssetDatabase.CreateAsset(source, outputPath);
            } else {
                AssetDatabase.AddObjectToAsset(source, outputPath);
            }
        }

        AssetDatabase.ImportAsset(outputPath);

        Debug.Log("Combined objects into asset: " + outputPath);
    }
}
