// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Bindings.Core.Base;
using Bindings.Core.Binding.Binders;
using Bindings.Core.Binding.BindingContext;
using Bindings.Core.Binding.Bindings.Source.Construction;
using Bindings.Core.Binding.Bindings.SourceSteps;
using Bindings.Core.Binding.Bindings.Target.Construction;
using Bindings.Core.Binding.Combiners;
using Bindings.Core.Binding.ExpressionParse;

namespace Bindings.Core.Binding
{
    public interface IBindingSingletonCache
    {
        IAutoValueConverters AutoValueConverters { get; }
        IBindingDescriptionParser BindingDescriptionParser { get; }
        IPropertyExpressionParser PropertyExpressionParser { get; }
        IValueConverterLookup ValueConverterLookup { get; }
        IBindingNameLookup DefaultBindingNameLookup { get; }
        IBinder Binder { get; }
        ISourceBindingFactory SourceBindingFactory { get; }
        ITargetBindingFactory TargetBindingFactory { get; }
        ISourceStepFactory SourceStepFactory { get; }
        IValueCombinerLookup ValueCombinerLookup { get; }
        IMainThreadAsyncDispatcher MainThreadDispatcher { get; }
    }
}
