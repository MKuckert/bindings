// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Bindings.Core.Binding.Bindings.SourceSteps;
using Bindings.Core.Binding.Extensions;
using Bindings.Core.Converters;

namespace Bindings.Core.Binding.Combiners
{
    public class InvertedValueCombiner
			: BooleanValueCombiner
	{
		protected override bool TryCombine(IEnumerable<bool> stepValues, out object value)
		{
			value = stepValues.Any(x => !x);
			return true;
		}
	}

    public class AndValueCombiner
        : BooleanValueCombiner
    {
        protected override bool TryCombine(IEnumerable<bool> stepValues, out object value)
        {
            value = stepValues.All(x => x);
            return true;
        }
    }

    public class OrValueCombiner
        : BooleanValueCombiner
    {
        protected override bool TryCombine(IEnumerable<bool> stepValues, out object value)
        {
            value = stepValues.Any(x => x);
            return true;
        }
    }

    public class NotValueCombiner
        : BooleanValueCombiner
    {
        protected override bool TryCombine(IEnumerable<bool> stepValues, out object value)
        {
            value = stepValues.All(x => !x);
            return true;
        }
    }

    public class XorValueCombiner
        : BooleanValueCombiner
    {
        protected override bool TryCombine(IEnumerable<bool> stepValues, out object value)
        {
            var values = stepValues as bool[] ?? stepValues.ToArray();
            value = values.Any(x => !x)
                && values.Any(x => x);
            return true;
        }
    }

    public abstract class BooleanValueCombiner
        : ValueCombiner
    {
        public override bool TryGetValue(
            IEnumerable<ISourceStep> steps, out object value)
        {
            var stepValues = new List<bool>();
            foreach (var step in steps)
            {
                var objectValue = step.GetValue();

                if (objectValue == BindingConstant.DoNothing)
                {
                    value = BindingConstant.DoNothing;
                    return true;
                }
                if (objectValue == BindingConstant.UnsetValue)
                {
                    value = BindingConstant.UnsetValue;
                    return true;
                }

                if (!TryConvertToBool(objectValue, out var booleanValue))
                {
                    value = BindingConstant.UnsetValue;
                    return true;
                }
                stepValues.Add(booleanValue);
            }

            return TryCombine(stepValues, out value);
        }

        protected abstract bool TryCombine(IEnumerable<bool> stepValues, out object value);

        private bool TryConvertToBool(object objectValue, out bool booleanValue)
        {
            booleanValue = objectValue.ConvertToBoolean();
            return true;
        }
    }
}