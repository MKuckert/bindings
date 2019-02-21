// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Bindings.Core.Binding.Bindings.SourceSteps;
using Bindings.Core.Converters;

namespace Bindings.Core.Binding.Bindings
{
    public class BindingDescription
    {
        public BindingDescription()
        {
        }

        public BindingDescription(string targetName, string sourcePropertyPath, IValueConverter converter,
                                     object converterParameter, object fallbackValue, BindingMode mode)
        {
            TargetName = targetName;
            Mode = mode;
            Source = new PathSourceStepDescription
            {
                SourcePropertyPath = sourcePropertyPath,
                Converter = converter,
                ConverterParameter = converterParameter,
                FallbackValue = fallbackValue,
            };
        }

        public string TargetName { get; set; }
        public BindingMode Mode { get; set; }
        public SourceStepDescription Source { get; set; }

        public override string ToString()
        {
            return $"binding {TargetName} for {(Source == null ? "-null" : Source.ToString())}";
        }
    }
}