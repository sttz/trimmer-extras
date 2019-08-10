//
// Trimmer Framework for Unity
// https://sttz.ch/trimmer
//

#if TR_OptionParentDummy || UNITY_EDITOR

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using sttz.Trimmer.BaseOptions;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace sttz.Trimmer
{

public class OptionParentDummy : OptionToggle
{
    protected override void Configure()
    {
        Category = "Debug";
    }

    public override void Apply()
    {
        base.Apply();
    }

    public class OptionParentDummyChild1 : OptionString { }

    public class OptionParentDummyChild2 : OptionString
    {
        protected override void Configure()
        {
            DefaultValue = "";
            Variance = OptionVariance.Dictionary;
            VariantDefaultParameter = "Child2Variant";
        }

        public class OptionParentDummyChild3 : OptionString
        {
            public class OptionParentDummyChild4 : OptionString
            {
                public class OptionParentDummyChild5 : OptionString
                {
                    protected override void Configure()
                    {
                        Variance = OptionVariance.Dictionary;
                    }
                }
            }
        }
    }

}

}
#endif
