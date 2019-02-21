// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Bindings.Core.Binding.Bindings.SourceSteps;
using Bindings.Core.Converters;

namespace Bindings.Core.Binding.Combiners
{
    public class FormatValueCombiner : ValueCombiner
    {
        public override bool TryGetValue(IEnumerable<ISourceStep> steps, out object value)
        {
            var list = steps.ToList();

            if (list.Count < 1)
            {
                BindingLog.Warning("Format called with no parameters - will fail");
                value = BindingConstant.DoNothing;
                return true;
            }

            var formatObject = list.First().GetValue();
            if (formatObject == BindingConstant.DoNothing)
            {
                value = BindingConstant.DoNothing;
                return true;
            }

            if (formatObject == BindingConstant.UnsetValue)
            {
                value = BindingConstant.UnsetValue;
                return true;
            }

            var formatString = formatObject == null ? "" : formatObject.ToString();

            var values = list.Skip(1).Select(s => s.GetValue()).ToArray();

            if (values.Any(v => v == BindingConstant.DoNothing))
            {
                value = BindingConstant.DoNothing;
                return true;
            }

            if (values.Any(v => v == BindingConstant.UnsetValue))
            {
                value = BindingConstant.UnsetValue;
                return true;
            }

            value = string.Format(formatString, values);
            return true;
        }
    }
}