// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Bindings.Core.Binding.Bindings.SourceSteps;

namespace Bindings.Core.Binding.Combiners
{
    public interface IValueCombiner
    {
        Type SourceType(IEnumerable<ISourceStep> steps);

        void SetValue(IEnumerable<ISourceStep> steps, object value);

        bool TryGetValue(IEnumerable<ISourceStep> steps, out object value);

        IEnumerable<Type> SubStepTargetTypes(IEnumerable<ISourceStep> subSteps, Type overallTargetType);
    }
}