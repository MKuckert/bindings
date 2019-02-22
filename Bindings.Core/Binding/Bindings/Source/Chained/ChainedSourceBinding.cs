// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using Bindings.Core.Binding.Bindings.Source.Construction;
using Bindings.Core.Binding.Parse.PropertyPath.PropertyTokens;
using Bindings.Core.Converters;
using Bindings.Core.Logging;

namespace Bindings.Core.Binding.Bindings.Source.Chained
{
    public abstract class ChainedSourceBinding
        : PropertyInfoSourceBinding
    {
        private readonly IList<PropertyToken> _childTokens;
        private ISourceBinding _currentChildBinding;

        protected ChainedSourceBinding(
            object source,
            PropertyInfo propertyInfo,
            IList<PropertyToken> childTokens)
            : base(source, propertyInfo)
        {
            _childTokens = childTokens;
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (_currentChildBinding != null)
                {
                    _currentChildBinding.Changed -= ChildSourceBindingChanged;
                    _currentChildBinding.Dispose();
                    _currentChildBinding = null;
                }
            }

            base.Dispose(isDisposing);
        }

        private ISourceBindingFactory SourceBindingFactory => BindingSingletonCache.Instance.SourceBindingFactory;

        public override Type SourceType
        {
            get
            {
                if (_currentChildBinding == null)
                    return typeof(object);

                return _currentChildBinding.SourceType;
            }
        }

        protected void UpdateChildBinding()
        {
            if (_currentChildBinding != null)
            {
                _currentChildBinding.Changed -= ChildSourceBindingChanged;
                _currentChildBinding.Dispose();
                _currentChildBinding = null;
            }

            if (PropertyInfo == null)
            {
                return;
            }

            var currentValue = PropertyInfo.GetValue(Source, PropertyIndexParameters());
            if (currentValue == null)
            {
                // value will be missing... so end consumer will need to use fallback values
            }
            else
            {
                _currentChildBinding = SourceBindingFactory.CreateBinding(currentValue, _childTokens);
                _currentChildBinding.Changed += ChildSourceBindingChanged;
            }
        }

        protected abstract object[] PropertyIndexParameters();

        private void ChildSourceBindingChanged(object sender, EventArgs e)
        {
            FireChanged();
        }

        protected override void OnBoundPropertyChanged()
        {
            UpdateChildBinding();
            FireChanged();
        }

        public override object GetValue()
        {
            if (_currentChildBinding == null)
            {
                return BindingConstant.UnsetValue;
            }

            return _currentChildBinding.GetValue();
        }

        public override void SetValue(object value)
        {
            if (_currentChildBinding == null)
            {
                Log.Warning("SetValue ignored in binding - target property path missing");
                return;
            }

            _currentChildBinding.SetValue(value);
        }
    }
}
