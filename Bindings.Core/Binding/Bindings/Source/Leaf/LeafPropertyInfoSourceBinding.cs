// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using Bindings.Core.Binding.Extensions;
using Bindings.Core.Converters;
using Bindings.Core.Logging;

namespace Bindings.Core.Binding.Bindings.Source.Leaf
{
    public abstract class LeafPropertyInfoSourceBinding : PropertyInfoSourceBinding
    {
        protected LeafPropertyInfoSourceBinding(object source, PropertyInfo propertyInfo)
            : base(source, propertyInfo)
        {
        }

        public override Type SourceType => PropertyInfo?.PropertyType;

        protected override void OnBoundPropertyChanged()
        {
            FireChanged();
        }

        public override object GetValue()
        {
            if (PropertyInfo == null)
            {
                return BindingConstant.UnsetValue;
            }

            if (!PropertyInfo.CanRead)
            {
                Log.Error("GetValue ignored in binding - target property is writeonly");
                return BindingConstant.UnsetValue;
            }

            try
            {
                return PropertyInfo.GetValue(Source, PropertyIndexParameters());
            }
            catch (TargetInvocationException)
            {
                // for dictionary lookups we quite often expect this during binding
                // for list-based lookups we quite often expect this during binding
                return BindingConstant.UnsetValue;
            }
        }

        protected abstract object[] PropertyIndexParameters();

        public override void SetValue(object value)
        {
            if (PropertyInfo == null)
            {
                Log.Warning("SetValue ignored in binding - source property {0} is missing", PropertyName);
                return;
            }

            if (!PropertyInfo.CanWrite)
            {
                Log.Warning("SetValue ignored in binding - target property is readonly");
                return;
            }

            try
            {
                var propertyType = PropertyInfo.PropertyType;
                var safeValue = propertyType.MakeSafeValue(value);

                // if safeValue matches the existing value, then don't call set
                if (EqualsCurrentValue(safeValue))
                    return;

                PropertyInfo.SetValue(Source, safeValue, PropertyIndexParameters());
            }
            catch (Exception exception)
            {
                Log.Error("SetValue failed with exception - " + exception);
            }
        }
    }
}