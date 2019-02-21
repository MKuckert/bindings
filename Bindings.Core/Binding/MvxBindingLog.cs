// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;

namespace MvvmCross.Binding
{
    public static class MvxBindingLog
    {
        public static void Trace(string message, params object[] args)
        {
            Debug.WriteLine("Trace " + string.Format(message, args));
        }

        public static void Warning(string message, params object[] args)
        {
            Debug.WriteLine("Warn " + string.Format(message, args));
        }

        public static void Error(string message, params object[] args)
        {
            Debug.WriteLine("Error " + string.Format(message, args));
        }
    }
}
