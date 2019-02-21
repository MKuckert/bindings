// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Input;
using Bindings.Core.Base;

namespace Bindings.Core.WeakSubscription
{
    public static class WeakSubscriptionExtensions
    {
        public static NotifyPropertyChangedEventSubscription WeakSubscribe(this INotifyPropertyChanged source,
                                                                              EventHandler<PropertyChangedEventArgs> eventHandler)
        {
            return new NotifyPropertyChangedEventSubscription(source, eventHandler);
        }

        public static NamedNotifyPropertyChangedEventSubscription<T> WeakSubscribe<T>(this INotifyPropertyChanged source,
                                                                               Expression<Func<T>> property,
                                                                               EventHandler<PropertyChangedEventArgs> eventHandler)
        {
            return new NamedNotifyPropertyChangedEventSubscription<T>(source, property, eventHandler);
        }

        public static NamedNotifyPropertyChangedEventSubscription<T> WeakSubscribe<T>(this INotifyPropertyChanged source,
                                                                               string property,
                                                                               EventHandler<PropertyChangedEventArgs> eventHandler)
        {
            return new NamedNotifyPropertyChangedEventSubscription<T>(source, property, eventHandler);
        }

        public static NotifyCollectionChangedEventSubscription WeakSubscribe(this INotifyCollectionChanged source,
                                                                                EventHandler<NotifyCollectionChangedEventArgs> eventHandler)
        {
            return new NotifyCollectionChangedEventSubscription(source, eventHandler);
        }

        public static GeneralEventSubscription WeakSubscribe(this EventInfo eventInfo,
                                                                object source,
                                                                EventHandler<EventArgs> eventHandler)
        {
            return new GeneralEventSubscription(source, eventInfo, eventHandler);
        }

        public static ValueEventSubscription<T> WeakSubscribe<T>(this EventInfo eventInfo,
                                                                    object source,
                                                                    EventHandler<ValueEventArgs<T>> eventHandler)
        {
            return new ValueEventSubscription<T>(source, eventInfo, eventHandler);
        }

        public static CanExecuteChangedEventSubscription WeakSubscribe(this ICommand source,
                                                                          EventHandler<EventArgs> eventHandler)
        {
            return new CanExecuteChangedEventSubscription(source, eventHandler);
        }

        public static WeakEventSubscription<TSource> WeakSubscribe<TSource>(this TSource source, string eventName, EventHandler eventHandler)
            where TSource : class
        {
            return new WeakEventSubscription<TSource>(source, eventName, eventHandler);
        }

        public static WeakEventSubscription<TSource, TEventArgs> WeakSubscribe<TSource, TEventArgs>(this TSource source, string eventName, EventHandler<TEventArgs> eventHandler)
            where TSource : class
        {
            return new WeakEventSubscription<TSource, TEventArgs>(source, eventName, eventHandler);
        }
    }
}
