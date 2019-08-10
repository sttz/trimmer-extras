//
// Trimmer Framework for Unity
// https://sttz.ch/trimmer
//

#if TR_OptionFloatDummy || UNITY_EDITOR

using sttz.Trimmer;
using sttz.Trimmer.BaseOptions;

public class OptionFloatDummy : OptionFloat
{
    protected override void Configure()
    {
        DefaultValue = 1;
        Category = "Debug";
        MinValue = 0f;
        MaxValue = 1f;
    }

    public override void Apply()
    {
        base.Apply();
    }
}
#endif
