// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Bindings.Core.Exeptions;

namespace Bindings.Core.Binding.Bindings.SourceSteps
{
    public class SourceStepFactory : ISourceStepFactoryRegistry
    {
        private readonly Dictionary<Type, ISourceStepFactory> _subFactories =
            new Dictionary<Type, ISourceStepFactory>();

        public void AddOrOverwrite(Type type, ISourceStepFactory factory)
        {
            _subFactories[type] = factory;
        }

        public ISourceStep Create(SourceStepDescription description)
        {
            if (!_subFactories.TryGetValue(description.GetType(), out var subFactory))
            {
                throw new BindingException("Failed to get factory for step type {0}", description.GetType().Name);
            }

            return subFactory.Create(description);
        }
    }
}