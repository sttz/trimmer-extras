//
// Trimmer Framework for Unity
// https://sttz.ch/trimmer
//

using System.Linq;
using NUnit.Framework;
using sttz.Trimmer;

public class ProfileLoadSaveTest
{
    const string ini = @"
Dummy = no
ParentDummy = no
ParentDummy.Child2[Child2Variant] = Child2VariantValue
ParentDummy.Child2[Child2Variant].Child3 = Child2VariantChild3Value
ParentDummy.Child2[Child2Variant].Child3.Child4 = Child4Value
ParentDummy.Child2[Child2Variant].Child3.Child4.Child5[Default] = Child5DefaultValue
ParentDummy.Child2[Child2Variant].Child3.Child4.Child5[Default1] = Child5Default1Value
ParentDummy.Child2[Child2Variant1] = Child2Variant1Value
ParentDummy.Child2[Child2Variant1].Child3 = Child2Variant1Child3Value
ParentDummy.Child2[Child2Variant2] = Child2Variant2Value
ParentDummy.Child2[Child2Variant2].Child3 = Child2Variant2Child3Vlaue
ParentDummy.Child2[Child2Variant2].Child3.Child4 = Child4Value
ParentDummy.Child2[Child2Variant2].Child3.Child4.Child5[Default] = Child5DefaultValue
ParentDummy.Child2[Child2Variant2].Child3.Child4.Child5[Default1] = Child5Default1Value
ParentDummy.Child1 = Child1Value
Prompt = yes
Prompt.Activation = O-O-O
Prompt.Position = BottomLeft
Prompt.FontSize = 0
BuildOnlyDummy = yes
EditModeDummy = no
BuildSettings = Development, ShowBuiltPlayer
BuildSettings.Scenes[0] = e43934d8cfb594260befa802e5a75cf5
BuildSettings.Scenes[1] = d54f3c0ea30f74f038462ccf1ff35885
BuildSettings.BuildPath = Export/%Target%
IniFile = wb.ini
IniFile.SearchPath = .
VariantDummy[Dummy] = DummyValue
VariantDummy[Dummy1] = Dummy2Value
VariantDummy[Dummy2] = Dummy2Value
";

    [Test]
    public void Test()
    {
        var store = new ValueStore();
        IniAdapter.Load(store, ini);

        var node = store.AddRoot("Superfluous", "Value");
        node.AddChild("Child", "Value");

        var profile = new RuntimeProfile();
        profile.Store = store;
        profile.SaveToStore();
        
        var newIni = IniAdapter.Save(profile.Store);
        System.IO.File.WriteAllText("/Users/adrian/Desktop/orig.ini", SortAndTrim(ini));
        System.IO.File.WriteAllText("/Users/adrian/Desktop/new.ini", SortAndTrim(newIni));
        Assert.True(SortAndTrim(ini) == SortAndTrim(newIni));
    }

    [Test]
    public void ClearTest()
    {
        var store = new ValueStore();
        IniAdapter.Load(store, ini);

        var node = store.AddRoot("Superfluous", "Value");
        node.AddChild("Child", "Value");

        var profile = new RuntimeProfile();
        profile.Store = store;
        profile.CleanStore();
        
        Assert.IsNull(profile.GetOption("Superfluous/Child"));
        Assert.IsNull(profile.GetOption("Superfluous"));

        var option = profile.GetOption("ParentDummy");
        Assert.IsNotNull(option);
        Assert.IsTrue(option.Save() == "no");

        option = profile.GetOption("ParentDummy/Child2:Child2Variant2/Child3/Child4/Child5:Default1");
        Assert.IsNotNull(option);
        Assert.IsNotNull(option.Save() == "Child5Default1Value");
    }

    public string SortAndTrim(string input)
    {
        var lines = input.Split('\n').ToList();

        for (int i = lines.Count - 1; i >= 0; i--) {
            if (lines[i].Trim().Length == 0) {
                lines.RemoveAt(i);
            }
        }

        lines.Sort();

        return string.Join("\n", lines.ToArray());
    }
}
