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