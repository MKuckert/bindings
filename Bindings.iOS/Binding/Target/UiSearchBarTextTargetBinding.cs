// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using Bindings.Core.Binding;
using Bindings.Core.Binding.Bindings.Target;
using Bindings.Core.WeakSubscription;
using UIKit;

namespace Bindings.iOS.Binding.Target
{
    public class UiSearchBarTextTargetBinding : PropertyInfoTargetBinding<UISearchBar>
    {
        private IDisposable _subscription;

        public UiSearchBarTextTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
        }

        public override BindingMode DefaultMode => BindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            var searchBar = View;
            if (searchBar == null)
            {
                BindingLog.Error(
                                      "Error - UISearchBar is null in MvxUISearchBarTextTargetBinding");
                return;
            }

            _subscription = searchBar.WeakSubscribe<UISearchBar, UISearchBarTextChangedEventArgs>(nameof(searchBar.TextChanged), HandleSearchBarValueChanged);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (!isDisposing) return;

            _subscription?.Dispose();
            _subscription = null;
        }

        private void HandleSearchBarValueChanged(object sender, UISearchBarTextChangedEventArgs e)
        {
            FireValueChanged(View.Text);
        }
    }
}
