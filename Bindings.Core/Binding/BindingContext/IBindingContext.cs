// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Bindings.Core.Base;
using Bindings.Core.Binding.Bindings;

namespace Bindings.Core.Binding.BindingContext
{
    public interface IBindingContext
        : IDataConsumer
    {
        event EventHandler DataContextChanged;

        IBindingContext Init(object dataContext, object firstBindingKey, IEnumerable<BindingDescription> firstBindingValue);

        IBindingContext Init(object dataContext, object firstBindingKey, string firstBindingValue);

        void RegisterBinding(object target, IUpdateableBinding binding);

        void RegisterBindingsWithClearKey(object clearKey, IEnumerable<KeyValuePair<object, IUpdateableBinding>> bindings);

        void RegisterBindingWithClearKey(object clearKey, object target, IUpdateableBinding binding);

        void ClearBindings(object clearKey);

        void ClearAllBindings();

        void DelayBind(Action action);
    }
}