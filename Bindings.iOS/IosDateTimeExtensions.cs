// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Foundation;

namespace Bindings.iOS
{
    public static class IosDateTimeExtensions
    {
        private static readonly DateTime ReferenceNsDateTime = new DateTime(2001, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime ToDateTimeUtc(this NSDate date)
        {
            return ReferenceNsDateTime.AddSeconds(date.SecondsSinceReferenceDate);
        }

        public static NSDate ToNsDate(this DateTime date)
        {
            return NSDate.FromTimeIntervalSinceReferenceDate((date - ReferenceNsDateTime).TotalSeconds);
        }

        public static DateTime WithKind(this DateTime date, DateTimeKind kind)
        {
            return new DateTime(date.Ticks, kind);
        }
    }
}
