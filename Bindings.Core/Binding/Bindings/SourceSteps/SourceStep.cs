// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using Bindings.Core.Converters;
using Bindings.Core.Logging;

namespace Bindings.Core.Binding.Bindings.SourceSteps
{
    public abstract class SourceStep
        : ISourceStep
    {
        private object _dataContext;

        protected SourceStepDescription Description { get; }

        protected SourceStep(SourceStepDescription description)
        {
            Description = description;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            // nothing to do in the base class
        }

        public virtual Type TargetType { get; set; }

        public virtual Type SourceType => typeof(object);

        public object DataContext
        {
            get
            {
                return _dataContext;
            }
            set
            {
                if (_dataContext == value)
                    return;

                _dataContext = value;
                OnDataContextChanged();
            }
        }

        protected virtual void OnDataContextChanged()
        {
            // nothing to do in the base class
        }

        public void SetValue(object value)
        {
            var sourceValue = ApplyValueConverterTargetToSource(value);

            if (sourceValue == BindingConstant.DoNothing)
                return;

            if (sourceValue == BindingConstant.UnsetValue)
                return;

            SetSourceValue(sourceValue);
        }

        private object ApplyValueConverterTargetToSource(object value)
        {
            if (Description.Converter == null)
                return value;

            return Description.Converter.ConvertBack(value,
                                                      SourceType,
                                                      Description.ConverterParameter,
                                                      CultureInfo.CurrentUICulture);
        }

        private object ApplyValueConverterSourceToTarget(object value)
        {
            if (Description.Converter == null)
            {
                return value;
            }

            try
            {
                return
                    Description.Converter.Convert(value,
                                                   TargetType,
                                                   Description.ConverterParameter,
                                                   CultureInfo.CurrentUICulture);
            }
            catch (Exception exception)
            {
                // pokemon exception - force the use of Fallback in this case
                // we expect this exception to occur sometimes - so only "Diagnostic" level logging here
                Log.Trace(
                    "Problem seen during binding execution for {0} - problem {1}",
                    Description.ToString(),
                    exception);
            }

            return BindingConstant.UnsetValue;
        }

        protected abstract void SetSourceValue(object sourceValue);

        protected virtual void SendSourcePropertyChanged()
        {
            _changed?.Invoke(this, EventArgs.Empty);
        }

        private object ConvertSourceToTarget(object value)
        {
            if (value == BindingConstant.DoNothing)
                return value;

            if (value != BindingConstant.UnsetValue)
            {
                value = ApplyValueConverterSourceToTarget(value);
            }

            if (value != BindingConstant.UnsetValue)
            {
                return value;
            }

            if (Description.FallbackValue != null)
                return Description.FallbackValue;

            return BindingConstant.UnsetValue;
        }

        // ReSharper disable once InconsistentNaming
        private event EventHandler _changed;

        public event EventHandler Changed
        {
            add
            {
                var alreadyHasListeners = _changed != null;
                _changed += value;
                if (!alreadyHasListeners)
                    OnFirstChangeListenerAdded();
            }
            remove
            {
                _changed -= value;
                if (_changed == null)
                    OnLastChangeListenerRemoved();
            }
        }

        protected virtual void OnLastChangeListenerRemoved()
        {
            // base class does nothing by default
        }

        protected virtual void OnFirstChangeListenerAdded()
        {
            // base class does nothing by default
        }

        public object GetValue()
        {
            var sourceValue = GetSourceValue();
            var value = ConvertSourceToTarget(sourceValue);
            return value;
        }

        protected abstract object GetSourceValue();
    }

    public abstract class SourceStep<T> : SourceStep
        where T : SourceStepDescription
    {
        protected new T Description => (T)base.Description;

        protected SourceStep(T description)
            : base(description)
        {
        }
    }
}