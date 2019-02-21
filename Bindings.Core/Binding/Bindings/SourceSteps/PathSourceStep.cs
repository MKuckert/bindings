// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Bindings.Core.Binding.Bindings.Source;
using Bindings.Core.Binding.Bindings.Source.Construction;
using Bindings.Core.Converters;

namespace Bindings.Core.Binding.Bindings.SourceSteps
{
    public class PathSourceStep : SourceStep<PathSourceStepDescription>
    {
        private ISourceBinding _sourceBinding;

        public PathSourceStep(PathSourceStepDescription description)
            : base(description)
        {
        }

        private ISourceBindingFactory SourceBindingFactory => BindingSingletonCache.Instance.SourceBindingFactory;

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                ClearPathSourceBinding();
            }

            base.Dispose(isDisposing);
        }

        public override Type SourceType
        {
            get
            {
                if (_sourceBinding == null)
                    return typeof(object);

                return _sourceBinding.SourceType;
            }
        }

        //TODO: optim: dont recreate the source binding on each datacontext change, as SourcePropertyPath does not change.
        //TODO: optim: don't subscribe to the Changed event if the binding mode does not need it.
        protected override void OnDataContextChanged()
        {
            ClearPathSourceBinding();
            _sourceBinding = SourceBindingFactory.CreateBinding(DataContext, Description.SourcePropertyPath);
            if (_sourceBinding != null)
            {
                _sourceBinding.Changed += SourceBindingOnChanged;
            }
            base.OnDataContextChanged();
        }

        private void ClearPathSourceBinding()
        {
            if (_sourceBinding != null)
            {
                _sourceBinding.Changed -= SourceBindingOnChanged;
                _sourceBinding.Dispose();
                _sourceBinding = null;
            }
        }

        private void SourceBindingOnChanged(object sender, EventArgs args)
        {
            SendSourcePropertyChanged();
        }

        protected override void SetSourceValue(object sourceValue)
        {
            if (_sourceBinding == null)
                return;

            if (sourceValue == BindingConstant.UnsetValue)
                return;

            if (sourceValue == BindingConstant.DoNothing)
                return;

            _sourceBinding.SetValue(sourceValue);
        }

        protected override object GetSourceValue()
        {
            if (_sourceBinding == null)
            {
                return BindingConstant.UnsetValue;
            }

            return _sourceBinding.GetValue();
        }
    }
}