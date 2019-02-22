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
using Bindings.Core.Exeptions;
using Bindings.Core.IoC;

namespace Bindings.Core.Binding
{
    // this class is not perfect OO and it gets in the way of testing
    // however, it is here for speed - to help avoid obscene numbers of Mvx.IoCProvider.Resolve<T> calls during binding
    public class BindingSingletonCache
        : Singleton<IBindingSingletonCache>, IBindingSingletonCache
    {
        public static IBindingSingletonCache Initialize()
        {
            if (Instance != null)
                throw new BindingException("You should only initialize MvxBindingSingletonCache once");

            var instance = new BindingSingletonCache();
            return instance;
        }

        private IAutoValueConverters _autoValueConverters;
        private IBindingDescriptionParser _bindingDescriptionParser;
        private ISourceBindingFactory _sourceBindingFactory;
        private ITargetBindingFactory _targetBindingFactory;
        private IPropertyExpressionParser _propertyExpressionParser;
        private IValueConverterLookup _valueConverterLookup;
        private IBindingNameLookup _defaultBindingName;
        private IBinder _binder;
        private ISourceStepFactory _sourceStepFactory;
        private IValueCombinerLookup _valueCombinerLookup;
        private IMainThreadAsyncDispatcher _mainThreadDispatcher;

        public IAutoValueConverters AutoValueConverters
        {
            get
            {
                _autoValueConverters = _autoValueConverters ?? IoCProvider.Instance.Resolve<IAutoValueConverters>();
                return _autoValueConverters;
            }
        }

        public IBindingDescriptionParser BindingDescriptionParser
        {
            get
            {
                _bindingDescriptionParser = _bindingDescriptionParser ?? IoCProvider.Instance.Resolve<IBindingDescriptionParser>();
                return _bindingDescriptionParser;
            }
        }

        public IPropertyExpressionParser PropertyExpressionParser
        {
            get
            {
                _propertyExpressionParser = _propertyExpressionParser ?? IoCProvider.Instance.Resolve<IPropertyExpressionParser>();
                return _propertyExpressionParser;
            }
        }

        public IValueConverterLookup ValueConverterLookup
        {
            get
            {
                _valueConverterLookup = _valueConverterLookup ?? IoCProvider.Instance.Resolve<IValueConverterLookup>();
                return _valueConverterLookup;
            }
        }

        public IValueCombinerLookup ValueCombinerLookup
        {
            get
            {
                _valueCombinerLookup = _valueCombinerLookup ?? IoCProvider.Instance.Resolve<IValueCombinerLookup>();
                return _valueCombinerLookup;
            }
        }

        public IBindingNameLookup DefaultBindingNameLookup
        {
            get
            {
                _defaultBindingName = _defaultBindingName ?? IoCProvider.Instance.Resolve<IBindingNameLookup>();
                return _defaultBindingName;
            }
        }

        public IBinder Binder
        {
            get
            {
                _binder = _binder ?? IoCProvider.Instance.Resolve<IBinder>();
                return _binder;
            }
        }

        public ISourceBindingFactory SourceBindingFactory
        {
            get
            {
                _sourceBindingFactory = _sourceBindingFactory ?? IoCProvider.Instance.Resolve<ISourceBindingFactory>();
                return _sourceBindingFactory;
            }
        }

        public ITargetBindingFactory TargetBindingFactory
        {
            get
            {
                _targetBindingFactory = _targetBindingFactory ?? IoCProvider.Instance.Resolve<ITargetBindingFactory>();
                return _targetBindingFactory;
            }
        }

        public ISourceStepFactory SourceStepFactory
        {
            get
            {
                _sourceStepFactory = _sourceStepFactory ?? IoCProvider.Instance.Resolve<ISourceStepFactory>();
                return _sourceStepFactory;
            }
        }

        public IMainThreadAsyncDispatcher MainThreadDispatcher
        {
            get
            {
                _mainThreadDispatcher = _mainThreadDispatcher ?? IoCProvider.Instance.Resolve<IMainThreadAsyncDispatcher>();
                return _mainThreadDispatcher;
            }
        }
    }
}
