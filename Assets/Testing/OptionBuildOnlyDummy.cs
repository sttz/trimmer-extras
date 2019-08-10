//
// Trimmer Framework for Unity
// https://sttz.ch/trimmer
//

#if TR_OptionBuildOnlyDummy || UNITY_EDITOR

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using sttz.Trimmer.BaseOptions;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace sttz.Trimmer
{

[Capabilities(OptionCapabilities.ConfiguresBuild)]
public class OptionBuildOnlyDummy : OptionToggle
{
    protected override void Configure()
    {
        Category = "Debug";
    }

    public override void Apply()
    {
        base.Apply();
    }
}

}
#endif
