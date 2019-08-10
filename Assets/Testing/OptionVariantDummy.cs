//
// Trimmer Framework for Unity
// https://sttz.ch/trimmer
//

#if TR_OptionVariantDummy || UNITY_EDITOR

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using sttz.Trimmer.BaseOptions;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace sttz.Trimmer
{

public class OptionVariantDummy : OptionString
{
    protected override void Configure()
    {
        Variance = OptionVariance.Dictionary;
        VariantDefaultParameter = "Dummy";
    }

    public override void Apply()
    {
        base.Apply();
    }
}

}
#endif
