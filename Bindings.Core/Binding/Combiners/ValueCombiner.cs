// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Bindings.Core.Binding.Bindings.SourceSteps;

namespace Bindings.Core.Binding.Combiners
{
    public abstract class ValueCombiner
        : IValueCombiner
    {
        public virtual Type SourceType(IEnumerable<ISourceStep> steps)
        {
            return typeof(object);
        }

        public virtual void SetValue(IEnumerable<ISourceStep> steps, object value)
        {
            // do nothing
        }

        public virtual bool TryGetValue(IEnumerable<ISourceStep> steps, out object value)
        {
            value = null;
            return false;
        }

        public virtual IEnumerable<Type> SubStepTargetTypes(IEnumerable<ISourceStep> subSteps,
                                                            Type overallTargetType)
        {
            // by default a combiner just demand objects from its sources
            return subSteps.Select(x => typeof(object));
        }
    }
}