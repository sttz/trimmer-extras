//
// Trimmer Framework for Unity
// https://sttz.ch/trimmer
//

#if UNITY_EDITOR

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEditor;
using sttz.Trimmer.BaseOptions;

namespace sttz.Trimmer
{

[Capabilities(OptionCapabilities.CanPlayInEditor | OptionCapabilities.ExecuteInEditMode)]
public class OptionEditModeDummy : OptionToggle
{
    protected override void Configure()
    {
        Category = "Debug";
    }

    public override void Apply()
    {
        base.Apply();
        Debug.Log("OptionEditModeDummy.Apply = " + Value);
    }
}

}
#endif
