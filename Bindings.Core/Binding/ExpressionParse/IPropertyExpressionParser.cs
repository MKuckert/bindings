// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

namespace Bindings.Core.Binding.ExpressionParse
{
    public interface IPropertyExpressionParser
    {
        IParsedExpression Parse<TObj, TRet>(Expression<Func<TObj, TRet>> propertyPath);

        IParsedExpression Parse(LambdaExpression propertyPath);
    }
}