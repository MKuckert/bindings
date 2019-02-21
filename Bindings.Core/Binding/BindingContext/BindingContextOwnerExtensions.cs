// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Bindings.Core.Binding.Binders;
using Bindings.Core.Binding.Bindings;
using Bindings.Core.IoC;

namespace Bindings.Core.Binding.BindingContext
{
    public static partial class BindingContextOwnerExtensions
    {
        public static IBinder Binder => BindingSingletonCache.Instance.Binder;

        public static void CreateBindingContext(this IBindingContextOwner view)
        {
            view.BindingContext = IoCProvider.Instance.Resolve<IBindingContext>();
        }

        public static void CreateBindingContext(this IBindingContextOwner view, string bindingText)
        {
            view.BindingContext = IoCProvider.Instance.Resolve<IBindingContext>().Init(null, view, bindingText);
        }

        public static void CreateBindingContext(this IBindingContextOwner view,
                                                IEnumerable<BindingDescription> bindings)
        {
            view.BindingContext = IoCProvider.Instance.Resolve<IBindingContext>().Init(null, view, bindings);
        }

        /*
		 * This overload removed at present - it caused confusion on iOS
		 * because it could lead to the bindings being described before
		 * the table cells were fully available
        public static void DelayBind(this IMvxBindingContextOwner view, params IMvxApplicable[] applicables)
        {
            view.BindingContext.DelayBind(() => applicables.Apply());
        }
        */

        public static void DelayBind(this IBindingContextOwner view, Action bindingAction)
        {
            view.BindingContext.DelayBind(bindingAction);
        }

        public static void AddBinding(this IBindingContextOwner view, object target, IUpdateableBinding binding, object clearKey = null)
        {
            if (clearKey == null)
            {
                view.BindingContext.RegisterBinding(target, binding);
            }
            else
            {
                view.BindingContext.RegisterBindingWithClearKey(clearKey, target, binding);
            }
        }

        public static void AddBindings(this IBindingContextOwner view, object target, IEnumerable<IUpdateableBinding> bindings, object clearKey = null)
        {
            if (bindings == null)
                return;

            foreach (var binding in bindings)
                view.AddBinding(target, binding, clearKey);
        }

        public static void AddBindings(this IBindingContextOwner view, object target, string bindingText, object clearKey = null)
        {
            var bindings = Binder.Bind(view.BindingContext.DataContext, target, bindingText);
            view.AddBindings(target, bindings, clearKey);
        }

        public static void AddBinding(this IBindingContextOwner view, object target,
                                      BindingDescription bindingDescription, object clearKey = null)
        {
            var descriptions = new[] { bindingDescription };
            view.AddBindings(target, descriptions, clearKey);
        }

        public static void AddBindings(this IBindingContextOwner view, object target,
                                       IEnumerable<BindingDescription> bindingDescriptions, object clearKey = null)
        {
            var bindings = Binder.Bind(view.BindingContext.DataContext, target, bindingDescriptions);
            view.AddBindings(target, bindings, clearKey);
        }

        public static void AddBindings(this IBindingContextOwner view,
                                       IDictionary<object, string> bindingMap,
                                       object clearKey = null)
        {
            if (bindingMap == null)
                return;

            foreach (var kvp in bindingMap)
            {
                view.AddBindings(kvp.Key, kvp.Value, clearKey);
            }
        }

        public static void AddBindings(this IBindingContextOwner view,
                                       IDictionary<object, IEnumerable<BindingDescription>> bindingMap,
                                       object clearKey = null)
        {
            if (bindingMap == null)
                return;

            foreach (var kvp in bindingMap)
            {
                view.AddBindings(kvp.Key, kvp.Value, clearKey);
            }
        }
    }
}