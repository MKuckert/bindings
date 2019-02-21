// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bindings.Core.Binding.Bindings.Target.Construction
{
    public class PropertyInfoTargetBindingFactory
        : IPluginTargetBindingFactory
    {
        private readonly Func<object, PropertyInfo, ITargetBinding> _bindingCreator;
        private readonly string _targetName;
        private readonly Type _targetType;

        public PropertyInfoTargetBindingFactory(Type targetType, string targetName,
                                                   Func<object, PropertyInfo, ITargetBinding> bindingCreator)
        {
            _targetType = targetType;
            _targetName = targetName;
            _bindingCreator = bindingCreator;
        }

        protected Type TargetType => _targetType;

        #region IMvxPluginTargetBindingFactory Members

        public IEnumerable<TypeAndNamePair> SupportedTypes => new[]
        { 
            new TypeAndNamePair { Name = _targetName, Type = _targetType } 
        };

        public ITargetBinding CreateBinding(object target, string targetName)
        {
            var targetPropertyInfo = target.GetType().GetProperty(targetName);
            if (targetPropertyInfo != null)
            {
                try
                {
                    return _bindingCreator(target, targetPropertyInfo);
                }
                catch (Exception exception)
                {
                    BindingLog.Error(
                        "Problem creating target binding for {0} - exception {1}", _targetType.Name,
                        exception.ToString());
                }
            }

            return null;
        }

        #endregion IMvxPluginTargetBindingFactory Members
    }
}