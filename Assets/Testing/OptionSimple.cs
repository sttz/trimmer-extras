//
// Trimmer Framework for Unity
// https://sttz.ch/trimmer
//

#if TR_OptionSimple || UNITY_EDITOR

using sttz.Trimmer;
using sttz.Trimmer.BaseOptions;

[Capabilities(OptionCapabilities.PresetWithFeature)]
public class OptionSimple : OptionToggle
{
    protected override void Configure()
    {
        Category = "Debug";
    }

    public override void Apply()
    {
        base.Apply();
    }

    #if UNITY_EDITOR

    override public bool ShouldIncludeOnlyFeature()
    {
        return Value;
    }
    
    #endif
}
#endif
