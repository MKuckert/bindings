// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Bindings.Core.Exeptions;

namespace Bindings.Core.Binding.Bindings
{
    public static class BindingModeExtensions
    {
        public static BindingMode IfDefault(this BindingMode bindingMode, BindingMode modeIfDefault)
        {
            if (bindingMode == BindingMode.Default)
                return modeIfDefault;
            return bindingMode;
        }

        public static bool RequireSourceObservation(this BindingMode bindingMode)
        {
            switch (bindingMode)
            {
                case BindingMode.Default:
                    BindingLog.Trace("Mode of default seen for binding - assuming TwoWay");
                    return true;

                case BindingMode.OneWay:
                case BindingMode.TwoWay:
                    return true;

                case BindingMode.OneTime:
                case BindingMode.OneWayToSource:
                    return false;

                default:
                    throw new BindingException("Unexpected ActualBindingMode");
            }
        }

        public static bool RequiresTargetObservation(this BindingMode bindingMode)
        {
            switch (bindingMode)
            {
                case BindingMode.Default:
                    BindingLog.Trace("Mode of default seen for binding - assuming TwoWay");
                    return true;

                case BindingMode.OneWay:
                case BindingMode.OneTime:
                    return false;

                case BindingMode.TwoWay:
                case BindingMode.OneWayToSource:
                    return true;

                default:
                    throw new BindingException("Unexpected ActualBindingMode");
            }
        }

        public static bool RequireTargetUpdateOnFirstBind(this BindingMode bindingMode)
        {
            switch (bindingMode)
            {
                case BindingMode.Default:
                    BindingLog.Trace("Mode of default seen for binding - assuming TwoWay");
                    return true;

                case BindingMode.OneWay:
                case BindingMode.OneTime:
                case BindingMode.TwoWay:
                    return true;

                case BindingMode.OneWayToSource:
                    return false;

                default:
                    throw new BindingException("Unexpected ActualBindingMode");
            }
        }
    }
}
