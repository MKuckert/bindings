// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Bindings.Core.Converters;

namespace Bindings.Core.Binding.Binders
{
    public class AutoValueConverters
        : IAutoValueConverters
    {
        private class Key
        {
            public Key(Type viewModel, Type view)
            {
                ViewType = view;
                ViewModelType = viewModel;
            }

            private Type ViewModelType { get; }
            private Type ViewType { get; }

            public override bool Equals(object obj)
            {
                if (!(obj is Key rhs))
                    return false;

                return ViewModelType == rhs.ViewModelType
                       && ViewType == rhs.ViewType;
            }

            public override int GetHashCode()
            {
                return ViewModelType.GetHashCode() + ViewType.GetHashCode();
            }
        }

        private readonly Dictionary<Key, IValueConverter> _lookup = new Dictionary<Key, IValueConverter>();

        public IValueConverter Find(Type viewModelType, Type viewType)
        {
            _lookup.TryGetValue(new Key(viewModelType, viewType), out var result);
            return result;
        }

        public void Register(Type viewModelType, Type viewType, IValueConverter converter)
        {
            _lookup[new Key(viewModelType, viewType)] = converter;
        }
    }
}