// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Bindings.Core.Converters;
using Bindings.Core.Exeptions;

namespace Bindings.Core.Binding.Bindings.SourceSteps
{
    public class CombinerSourceStep : SourceStep<CombinerSourceStepDescription>
    {
        private readonly List<ISourceStep> _subSteps;

        public CombinerSourceStep(CombinerSourceStepDescription description)
            : base(description)
        {
            var sourceStepFactory = BindingSingletonCache.Instance.SourceStepFactory;
            _subSteps = description.InnerSteps
                                   .Select(d => sourceStepFactory.Create(d))
                                   .ToList();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                UnsubscribeFromChangedEvents();
                foreach (var step in _subSteps)
                {
                    step.Dispose();
                }
            }

            base.Dispose(isDisposing);
        }

        protected override void OnFirstChangeListenerAdded()
        {
            SubscribeToChangedEvents();
            base.OnFirstChangeListenerAdded();
        }

        public override Type TargetType
        {
            get
            {
                return base.TargetType;
            }
            set
            {
                base.TargetType = value;
                SetSubTypeTargetTypes();
            }
        }

        private void SetSubTypeTargetTypes()
        {
            var targetTypes = Description.Combiner.SubStepTargetTypes(_subSteps, TargetType);
            var targetTypeList = targetTypes.ToList();
            if (targetTypeList.Count != _subSteps.Count)
                throw new BindingException("Description.Combiner provided incorrect length TargetType list");

            for (var i = 0; i < targetTypeList.Count; i++)
            {
                _subSteps[i].TargetType = targetTypeList[i];
            }
        }

        private bool _isSubscribeToChangedEvents;

        private void SubscribeToChangedEvents()
        {
            if (_isSubscribeToChangedEvents)
                return;

            foreach (var subStep in _subSteps)
            {
                subStep.Changed += SubStepOnChanged;
            }
            _isSubscribeToChangedEvents = true;
        }

        protected override void OnLastChangeListenerRemoved()
        {
            UnsubscribeFromChangedEvents();
            base.OnLastChangeListenerRemoved();
        }

        private void UnsubscribeFromChangedEvents()
        {
            if (!_isSubscribeToChangedEvents)
                return;

            foreach (var subStep in _subSteps)
            {
                subStep.Changed -= SubStepOnChanged;
            }
            _isSubscribeToChangedEvents = false;
        }

        private void SubStepOnChanged(object sender, EventArgs args)
        {
            SendSourcePropertyChanged();
        }

        protected override void OnDataContextChanged()
        {
            foreach (var step in _subSteps)
            {
                step.DataContext = DataContext;
            }
            base.OnDataContextChanged();
        }

        public override Type SourceType => Description.Combiner.SourceType(_subSteps);

        protected override void SetSourceValue(object sourceValue)
        {
            if (sourceValue == BindingConstant.UnsetValue)
                return;

            if (sourceValue == BindingConstant.DoNothing)
                return;

            Description.Combiner.SetValue(_subSteps, sourceValue);
        }

        protected override object GetSourceValue()
        {
            object value;
            if (!Description.Combiner.TryGetValue(_subSteps, out value))
                value = BindingConstant.UnsetValue;

            return value;
        }
    }
}