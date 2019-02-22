// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Bindings.Core.Binding.Extensions;
using Bindings.Core.Logging;

namespace Bindings.Core.Binding.Bindings.Target
{
    public abstract class ConvertingTargetBinding : TargetBinding
    {
        private bool _isUpdatingSource;
        private bool _isUpdatingTarget;
        private object _updatingSourceWith;

        protected ConvertingTargetBinding(object target)
            : base(target)
        {
        }

        public override BindingMode DefaultMode => BindingMode.OneWay;

        protected abstract void SetValueImpl(object target, object value);

        public override void SetValue(object value)
        {
            Log.Trace("Receiving SetValue to " + (value ?? ""));
            var target = Target;
            if (target == null)
            {
                Log.Warning("Weak Target is null in {0} - skipping set", GetType().Name);
                return;
            }

            if (ShouldSkipSetValueForPlatformSpecificReasons(target, value))
                return;

            if (ShouldSkipSetValueForViewSpecificReasons(target, value))
                return;

            var safeValue = MakeSafeValue(value);

            // to prevent feedback loops, we don't pass on 'same value' updates from the source while we are updating it
            if (_isUpdatingSource)
            {
                if (safeValue == null)
                {
                    if (_updatingSourceWith == null)
                        return;
                }
                else
                {
                    if (safeValue.Equals(_updatingSourceWith))
                        return;
                }
            }

            try
            {
                _isUpdatingTarget = true;
                SetValueImpl(target, safeValue);
            }
            finally
            {
                _isUpdatingTarget = false;
            }
        }

        protected virtual bool ShouldSkipSetValueForViewSpecificReasons(object target, object value)
        {
            return false;
        }

        protected virtual bool ShouldSkipSetValueForPlatformSpecificReasons(object target, object value)
        {
            return false;
        }

        protected virtual object MakeSafeValue(object value)
        {
            var safeValue = TargetType.MakeSafeValue(value);
            return safeValue;
        }

        protected sealed override void FireValueChanged(object newValue)
        {
            // we don't allow 'reentrant' updates of any kind from target to source
            if (_isUpdatingTarget || _isUpdatingSource)
                return;

            Log.Trace("Firing changed to " + (newValue ?? ""));
            try
            {
                _isUpdatingSource = true;
                _updatingSourceWith = newValue;

                base.FireValueChanged(newValue);
            }
            finally
            {
                _isUpdatingSource = false;
                _updatingSourceWith = null;
            }
        }
    }

    public abstract class ConvertingTargetBinding<TTarget, TValue> : TargetBinding<TTarget, TValue>
        where TTarget : class
    {
        private bool _isUpdatingSource;
        private bool _isUpdatingTarget;
        private TValue _updatingSourceWith;

        protected ConvertingTargetBinding(TTarget target)
            : base(target)
        {
        }

        public override BindingMode DefaultMode => BindingMode.OneWay;

        protected abstract void SetValueImpl(TTarget target, TValue value);

        protected override void SetValue(TValue value)
        {
            var target = Target;
            if (target == null)
            {
                Log.Warning("Weak Target is null in {0} - skipping set", GetType().Name);
                return;
            }

            if (ShouldSkipSetValueForPlatformSpecificReasons(target, value))
                return;

            if (ShouldSkipSetValueForViewSpecificReasons(target, value))
                return;

            var safeValue = MakeSafeValue(value);

            // to prevent feedback loops, we don't pass on 'same value' updates from the source while we are updating it
            if (_isUpdatingSource)
            {
                if (EqualityComparer<TValue>.Default.Equals(value, _updatingSourceWith))
                {
                    return;
                }
            }

            try
            {
                _isUpdatingTarget = true;
                SetValueImpl(target, safeValue);
            }
            finally
            {
                _isUpdatingTarget = false;
            }
        }

        protected virtual bool ShouldSkipSetValueForViewSpecificReasons(TTarget target, TValue value)
        {
            return false;
        }

        protected virtual bool ShouldSkipSetValueForPlatformSpecificReasons(TTarget target, TValue value)
        {
            return false;
        }

        protected virtual TValue MakeSafeValue(TValue value)
        {
            var safeValue = (TValue)TargetType.MakeSafeValue(value);
            return safeValue;
        }

        protected sealed override void FireValueChanged(TValue newValue)
        {
            // we don't allow 'reentrant' updates of any kind from target to source
            if (_isUpdatingTarget || _isUpdatingSource)
                return;

            Log.Trace("Firing changed to " + newValue);
            try
            {
                _isUpdatingSource = true;
                _updatingSourceWith = newValue;

                base.FireValueChanged(newValue);
            }
            finally
            {
                _isUpdatingSource = false;
                _updatingSourceWith = default(TValue);
            }
        }
    }
}