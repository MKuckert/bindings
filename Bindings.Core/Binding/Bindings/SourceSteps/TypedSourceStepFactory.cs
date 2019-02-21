// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Bindings.Core.Binding.Bindings.SourceSteps
{
    public abstract class TypedSourceStepFactory<T>
        : ISourceStepFactory
        where T : SourceStepDescription
    {
        public ISourceStep Create(SourceStepDescription description)
        {
            return TypedCreate((T)description);
        }

        protected abstract ISourceStep TypedCreate(T description);
    }
}