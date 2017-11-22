//
// Trimmer Framework for Unity - https://sttz.ch/trimmer
// Copyright © 2017 Adrian Stutz
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
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