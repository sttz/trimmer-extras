//
// Trimmer Framework for Unity
// https://sttz.ch/trimmer
//

#if TR_OptionReference || UNITY_EDITOR

using sttz.Trimmer;
using sttz.Trimmer.BaseOptions;
using UnityEngine;

[Capabilities(OptionCapabilities.PresetDefault)]
public class OptionReference : OptionAsset<GameObject>
{
    protected override void Configure()
    {
        Category = "Debug";
    }

    public override void Apply()
    {
        base.Apply();
        Debug.Log("OptionReference = " + Value);
    }
}

#endif
