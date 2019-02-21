// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace Bindings.Core.IoC
{
    public interface IIoCProvider
    {
        bool CanResolve<T>()
            where T : class;

        bool CanResolve(Type type);

        T Resolve<T>()
            where T : class;

        bool TryResolve(Type type, out object resolved);

        void RegisterType<TFrom, TTo>()
            where TFrom : class
            where TTo : class, TFrom;

        void RegisterSingleton<TInterface>(TInterface theObject)
            where TInterface : class;

        object IoCConstruct(Type type);
    }
}
