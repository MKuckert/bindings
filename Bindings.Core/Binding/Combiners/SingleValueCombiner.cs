// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Bindings.Core.Binding.Bindings.SourceSteps;
using Bindings.Core.Converters;

namespace Bindings.Core.Binding.Combiners
{
    public class SingleValueCombiner : ValueCombiner
    {
        public override Type SourceType(IEnumerable<ISourceStep> steps)
        {
            var firstStep = steps.FirstOrDefault();
            if (firstStep == null)
                return typeof(object);

            return firstStep.SourceType;
        }

        public override void SetValue(IEnumerable<ISourceStep> steps, object value)
        {
            var firstStep = steps.FirstOrDefault();

            firstStep?.SetValue(value);
        }

        public override bool TryGetValue(IEnumerable<ISourceStep> steps, out object value)
        {
            var firstStep = steps.FirstOrDefault();
            if (firstStep == null)
            {
                value = BindingConstant.UnsetValue;
                return true;
            }

            value = firstStep.GetValue();
            return true;
        }
    }
}