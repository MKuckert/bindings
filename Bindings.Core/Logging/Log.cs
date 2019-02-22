// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace Bindings.Core.Logging
{
    public static class Log
    {
        private static ILogger _logger = new DebugWriteLineLogger();

        public static ILogger Logger
        {
            get => _logger;
            set => _logger = value ?? throw new ArgumentNullException(nameof(value));
        }

        public static void Trace(string message, params object[] args)
        {
            Logger.Trace(message, args);
        }

        public static void Warning(string message, params object[] args)
        {
            Logger.Warning(message, args);
        }

        public static void Error(string message, params object[] args)
        {
            Logger.Error(message, args);
        }
    }
}
