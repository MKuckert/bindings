// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Bindings.Core.Binding.Bindings.SourceSteps;
using Bindings.Core.Converters;

namespace Bindings.Core.Binding.Combiners
{
    public class ValueConverterValueCombiner : ValueCombiner
    {
        private readonly IValueConverter _valueConverter;

        public ValueConverterValueCombiner(IValueConverter valueConverter)
        {
            _valueConverter = valueConverter;
        }

        public override void SetValue(IEnumerable<ISourceStep> steps, object value)
        {
            var sourceSteps = steps as ISourceStep[] ?? steps.ToArray();
            var sourceStep = sourceSteps.First();
            var parameter = GetParameterValue(sourceSteps);

            if (_valueConverter == null)
            {
                // null value converter always fails
                return;
            }
            var converted = _valueConverter.ConvertBack(value, sourceStep.SourceType, parameter,
                                                        CultureInfo.CurrentUICulture);
            sourceStep.SetValue(converted);
        }

        private Type _targetType = typeof(object);

        public override IEnumerable<Type> SubStepTargetTypes(IEnumerable<ISourceStep> subSteps, Type overallTargetType)
        {
            _targetType = overallTargetType;
            return base.SubStepTargetTypes(subSteps, overallTargetType);
        }

        private static object GetParameterValue(IEnumerable<ISourceStep> steps)
        {
            var parameterStep = steps.Skip(1).FirstOrDefault();
            object parameter = null;
            if (parameterStep != null)
            {
                parameter = parameterStep.GetValue();
            }
            return parameter;
        }

        public override bool TryGetValue(IEnumerable<ISourceStep> steps, out object value)
        {
            var sourceSteps = steps as ISourceStep[] ?? steps.ToArray();
            var sourceStep = sourceSteps.First();
            var parameter = GetParameterValue(sourceSteps);

            var sourceValue = sourceStep.GetValue();
            if (sourceValue == BindingConstant.DoNothing)
            {
                value = BindingConstant.DoNothing;
                return true;
            }

            if (sourceValue == BindingConstant.UnsetValue)
            {
                value = BindingConstant.UnsetValue;
                return true;
            }

            if (_valueConverter == null)
            {
                value = BindingConstant.UnsetValue;
                return true;
            }

            value = _valueConverter.Convert(sourceValue, _targetType, parameter, CultureInfo.CurrentUICulture);
            return true;
        }
    }
}