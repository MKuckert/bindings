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
    public class IfValueCombiner
        : ValueCombiner
    {
        public override bool TryGetValue(IEnumerable<ISourceStep> steps, out object value)
        {
            var list = steps.ToList();
            switch (list.Count)
            {
                case 2:
                    return TryEvaluateif (list[0], list[1], null, out value);

                case 3:
                    return TryEvaluateif (list[0], list[1], list[2], out value);

                default:
                    BindingLog.Warning("Unexpected substep count of {0} in 'If' ValueCombiner", list.Count);
                    return base.TryGetValue(list, out value);
            }
        }

        private bool TryEvaluateif (ISourceStep testStep, ISourceStep ifStep, ISourceStep elseStep, out object value)
        {
            var result = testStep.GetValue();
            if (result == BindingConstant.DoNothing)
            {
                value = BindingConstant.DoNothing;
                return true;
            }

            if (result == BindingConstant.UnsetValue)
            {
                value = BindingConstant.UnsetValue;
                return true;
            }

            if (IsTrue(result))
            {
                value = ReturnSubStepResult(ifStep);
                return true;
            }

            value = ReturnSubStepResult(elseStep);
            return true;
        }

        protected virtual bool IsTrue(object result)
        {
            return result.ConvertToBoolean();
        }

        protected virtual object ReturnSubStepResult(ISourceStep subStep)
        {
            if (subStep == null)
            {
                return BindingConstant.UnsetValue;
            }
            return subStep.GetValue();
        }
    }
}