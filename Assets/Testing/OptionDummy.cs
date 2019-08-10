//
// Trimmer Framework for Unity
// https://sttz.ch/trimmer
//

#if TR_OptionDummy || UNITY_EDITOR

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using sttz.Trimmer.BaseOptions;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace sttz.Trimmer
{

public class OptionDummy : OptionToggle
{
    protected override void Configure()
    {
        Category = "Debug";
    }

    public override void Apply()
    {
        base.Apply();
    }

    public override bool IsAvailable(IEnumerable<BuildTarget> targets)
    {
        return targets.Contains(BuildTarget.iOS);
    }
}

}
#endif
