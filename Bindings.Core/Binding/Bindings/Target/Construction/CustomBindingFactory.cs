// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Bindings.Core.Logging;

namespace Bindings.Core.Binding.Bindings.Target.Construction
{
    public class CustomBindingFactory<TTarget>
        : IPluginTargetBindingFactory
        where TTarget : class
    {
        private readonly Func<TTarget, ITargetBinding> _targetBindingCreator;
        private readonly string _targetFakePropertyName;

        public CustomBindingFactory(string targetFakePropertyName,
                                       Func<TTarget, ITargetBinding> targetBindingCreator)
        {
            _targetFakePropertyName = targetFakePropertyName;
            _targetBindingCreator = targetBindingCreator;
        }

        #region IMvxPluginTargetBindingFactory Members

        public IEnumerable<TypeAndNamePair> SupportedTypes => new[]
        { 
            new TypeAndNamePair(typeof(TTarget), _targetFakePropertyName) 
        };

        public ITargetBinding CreateBinding(object target, string targetName)
        {
            var castTarget = target as TTarget;
            if (castTarget == null)
            {
                Log.Error("Passed an invalid target for MvxCustomBindingFactory");
                return null;
            }

            return _targetBindingCreator(castTarget);
        }

        #endregion IMvxPluginTargetBindingFactory Members
    }
}