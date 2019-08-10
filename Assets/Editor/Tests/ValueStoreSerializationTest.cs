//
// Trimmer Framework for Unity
// https://sttz.ch/trimmer
//

using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using sttz.Trimmer;
using sttz.Trimmer.Extensions;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class ValueStoreSerializationTest
{
    int maxDepth = 4;
    int maxVariants = 3;
    int maxChildren = 3;

    [Test]
    public void EditorTest()
    {
        var rnd = new System.Random(42);
        var store = new ValueStore();

        for (int i = 0; i < 10; i++) {
            var name = "Root_" + i;
            var root = store.AddRoot(name, name + "_Value");
            PopulateNode(root, rnd, name, 1);
        }

        var clone = new List<ValueStore.Node>(store.RootCount);
        foreach (var root in store.Roots) {
            clone.Add(root.Clone());
        }

        var receiver = (ISerializationCallbackReceiver)store;
        receiver.OnBeforeSerialize();
        receiver.OnAfterDeserialize();

        /*var builder = new StringBuilder();
        foreach (var node in clone) {
            PrintRecursive(node, 0, builder);
        }
        File.WriteAllText("/Users/adrian/Desktop/test1.out", builder.ToString());

        builder = new StringBuilder();
        foreach (var node in store.nodes) {
            PrintRecursive(node, 0, builder);
        }
        File.WriteAllText("/Users/adrian/Desktop/test2.out", builder.ToString());*/

        Assert.AreEqual(store.RootCount, clone.Count);

        int j = 0;
        foreach (var root in store.Roots) {
            AssertRecursive(root, clone[j++]);
        }
    }

    public void PopulateNode(ValueStore.Node node, System.Random rnd, string basename, int depth)
    {
        var numVariants = rnd.Next(0, maxVariants);
        var numChildren = rnd.Next(0, maxChildren);

        for (int i = 0; i < numVariants; i++) {
            var name = basename + "/Variant_" + i;

            var variant = node.AddVariant(name, name + "_Value");

            if (depth < maxDepth)
                PopulateNode(variant, rnd, name, depth + 1);
        }

        for (int i = 0; i < numChildren; i++) {
            var name = basename + "/Child_" + i;

            var child = node.AddChild(name, name + "_Value");

            if (depth < maxDepth)
                PopulateNode(child, rnd, name, depth + 1);
        }
    }

    public void AssertRecursive(ValueStore.Node node1, ValueStore.Node node2)
    {
        Assert.AreEqual(node1.Name, node2.Name);
        Assert.AreEqual(node1.Value, node2.Value);

        Assert.AreEqual(node1.VariantCount, node2.VariantCount);
        foreach (var pair in node1.Variants.IterateWith(node2.Variants)) {
            AssertRecursive(pair.First, pair.Second);
        }

        Assert.AreEqual(node1.ChildCount, node2.ChildCount);
        foreach (var pair in node1.Children.IterateWith(node2.Children)) {
            AssertRecursive(pair.First, pair.Second);
        }
    }

    public void PrintRecursive(ValueStore.Node node, int depth, StringBuilder builder)
    {
        var indent = new string('-', depth * 2);

        builder.Append(indent);
        builder.Append(" ");
        builder.Append(node.Name);
        builder.Append(" = ");
        builder.Append(node.Value);
        builder.Append("\n");

        builder.Append(indent);
        builder.Append(" ");
        builder.Append(node.VariantCount);
        builder.Append(" variants:\n");

        foreach (var variant in node.Variants) {
            PrintRecursive(variant, depth + 1, builder);
        }

        builder.Append(indent);
        builder.Append(" ");
        builder.Append(node.ChildCount);
        builder.Append(" children:\n");

        foreach (var children in node.Children) {
            PrintRecursive(children, depth + 1, builder);
        }
    }
}
