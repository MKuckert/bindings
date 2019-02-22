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
using Bindings.Core.Binding.Parse.Binding;
using Bindings.Core.Binding.Parse.Binding.Tibet;
using Bindings.Core.Binding.Parse.PropertyPath;
using Bindings.Core.Binding.ValueConverters;
using Bindings.Core.Converters;
using Bindings.Core.IoC;
using Bindings.Core.Logging;

namespace Bindings.Core.Binding
{
    public class BindingBuilder
    {
        public static void Create<T>() where T : BindingBuilder, new()
        {
            new T().DoRegistration();
        }
        
        public virtual void DoRegistration()
        {
            InitializeIoC();
            RegisterMainThreadDispatcher();
            CreateSingleton();
            RegisterCore();
            RegisterValueConverterProvider();
            RegisterValueCombinerProvider();
            RegisterAutoValueConverters();
            RegisterBindingParser();
            RegisterBindingDescriptionParser();
            RegisterExpressionParser();
            RegisterSourcePropertyPathParser();
            RegisterPlatformSpecificComponents();
            RegisterBindingNameRegistry();
            RegisterBindingFactories();
        }

        private void InitializeIoC()
        {
            IoCProvider.Initialize();
        }

        private void RegisterMainThreadDispatcher()
        {
            IoCProvider.Instance.RegisterSingleton<IMainThreadAsyncDispatcher>(new SynchronizationContextDispatcher());
        }

        protected virtual void RegisterAutoValueConverters()
        {
            var autoValueConverters = CreateAutoValueConverters();
            IoCProvider.Instance.RegisterSingleton(autoValueConverters);
            FillAutoValueConverters(autoValueConverters);
        }

        protected virtual void FillAutoValueConverters(IAutoValueConverters autoValueConverters)
        {
            // nothing to do in base class
        }

        protected virtual IAutoValueConverters CreateAutoValueConverters()
        {
            return new AutoValueConverters();
        }

        protected virtual void CreateSingleton()
        {
            BindingSingletonCache.Initialize();
        }

        protected virtual void RegisterExpressionParser()
        {
            IoCProvider.Instance.RegisterSingleton<IPropertyExpressionParser>(new PropertyExpressionParser());
        }

        protected virtual void RegisterCore()
        {
            IoCProvider.Instance.RegisterSingleton<IBinder>(new FromTextBinder());

            //To get the old behavior back, you can override this registration with
            //Mvx.IoCProvider.RegisterType<IMvxBindingContext, MvxBindingContext>();
            IoCProvider.Instance.RegisterType<IBindingContext, TaskBasedBindingContext>();
        }

        protected virtual void RegisterValueConverterProvider()
        {
            var registry = CreateValueConverterRegistry();
            IoCProvider.Instance.RegisterSingleton<INamedInstanceLookup<IValueConverter>>(registry);
            IoCProvider.Instance.RegisterSingleton<INamedInstanceRegistry<IValueConverter>>(registry);
            IoCProvider.Instance.RegisterSingleton<IValueConverterLookup>(registry);
            IoCProvider.Instance.RegisterSingleton<IValueConverterRegistry>(registry);
            FillValueConverters(registry);
        }

        protected virtual ValueConverterRegistry CreateValueConverterRegistry()
        {
            return new ValueConverterRegistry();
        }

        protected virtual void FillValueConverters(IValueConverterRegistry registry)
        {
            registry.AddOrOverwrite("CommandParameter", new CommandParameterValueConverter());
        }

        protected virtual void RegisterValueCombinerProvider()
        {
            var registry = CreateValueCombinerRegistry();
            IoCProvider.Instance.RegisterSingleton<INamedInstanceLookup<IValueCombiner>>(registry);
            IoCProvider.Instance.RegisterSingleton<INamedInstanceRegistry<IValueCombiner>>(registry);
            IoCProvider.Instance.RegisterSingleton<IValueCombinerLookup>(registry);
            IoCProvider.Instance.RegisterSingleton(registry);
            FillValueCombiners(registry);
        }

        protected virtual IValueCombinerRegistry CreateValueCombinerRegistry()
        {
            return new ValueCombinerRegistry();
        }

        protected virtual void FillValueCombiners(IValueCombinerRegistry registry)
        {
            // note that assembly based registration is not used here for efficiency reasons
            // - see #327 - https://github.com/slodge/MvvmCross/issues/327
            registry.AddOrOverwrite("Add", new AddValueCombiner());
            registry.AddOrOverwrite("Divide", new DivideValueCombiner());
            registry.AddOrOverwrite("Format", new FormatValueCombiner());
            registry.AddOrOverwrite("If", new IfValueCombiner());
            registry.AddOrOverwrite("Modulus", new ModulusValueCombiner());
            registry.AddOrOverwrite("Multiply", new MultiplyValueCombiner());
            registry.AddOrOverwrite("Single", new SingleValueCombiner());
            registry.AddOrOverwrite("Subtract", new SubtractValueCombiner());
            registry.AddOrOverwrite("EqualTo", new EqualToValueCombiner());
            registry.AddOrOverwrite("NotEqualTo", new NotEqualToValueCombiner());
            registry.AddOrOverwrite("GreaterThanOrEqualTo", new GreaterThanOrEqualToValueCombiner());
            registry.AddOrOverwrite("GreaterThan", new GreaterThanValueCombiner());
            registry.AddOrOverwrite("LessThanOrEqualTo", new LessThanOrEqualToValueCombiner());
            registry.AddOrOverwrite("LessThan", new LessThanValueCombiner());
            registry.AddOrOverwrite("Not", new NotValueCombiner());
            registry.AddOrOverwrite("And", new AndValueCombiner());
            registry.AddOrOverwrite("Or", new OrValueCombiner());
            registry.AddOrOverwrite("XOr", new XorValueCombiner());
            registry.AddOrOverwrite("Inverted", new InvertedValueCombiner());

            // Note: MvxValueConverterValueCombiner is not registered - it is unconventional
            //registry.AddOrOverwrite("ValueConverter", new MvxValueConverterValueCombiner());
        }

        protected virtual void RegisterBindingParser()
        {
            if (IoCProvider.Instance.CanResolve<IBindingParser>())
            {
                Log.Trace("Binding Parser already registered - so skipping Default parser");
                return;
            }
            Log.Trace("Registering Default Binding Parser");
            IoCProvider.Instance.RegisterSingleton(CreateBindingParser());
        }

        protected virtual IBindingParser CreateBindingParser()
        {
            return new TibetBindingParser();
        }

        protected virtual void RegisterBindingDescriptionParser()
        {
            var parser = CreateBindingDescriptionParser();
            IoCProvider.Instance.RegisterSingleton(parser);
        }

        private static IBindingDescriptionParser CreateBindingDescriptionParser()
        {
            var parser = new BindingDescriptionParser();
            return parser;
        }

        protected virtual void RegisterSourcePropertyPathParser()
        {
            var tokeniser = CreateSourcePropertyPathParser();
            IoCProvider.Instance.RegisterSingleton(tokeniser);
        }

        protected virtual ISourcePropertyPathParser CreateSourcePropertyPathParser()
        {
            return new SourcePropertyPathParser();
        }

        protected virtual void RegisterBindingNameRegistry()
        {
            var registry = new BindingNameRegistry();
            IoCProvider.Instance.RegisterSingleton<IBindingNameLookup>(registry);
            IoCProvider.Instance.RegisterSingleton<IBindingNameRegistry>(registry);
            FillDefaultBindingNames(registry);
        }

        protected virtual void FillDefaultBindingNames(IBindingNameRegistry registry)
        {
            // base class has nothing to register
        }

        protected virtual void RegisterPlatformSpecificComponents()
        {
            // nothing to do here
        }

        protected virtual void RegisterBindingFactories()
        {
            RegisterMvxBindingFactories();
        }

        protected virtual void RegisterMvxBindingFactories()
        {
            RegisterSourceStepFactory();
            RegisterSourceFactory();
            RegisterTargetFactory();
        }

        protected virtual void RegisterSourceStepFactory()
        {
            var sourceStepFactory = CreateSourceStepFactoryRegistry();
            FillSourceStepFactory(sourceStepFactory);
            IoCProvider.Instance.RegisterSingleton(sourceStepFactory);
            IoCProvider.Instance.RegisterSingleton<ISourceStepFactory>(sourceStepFactory);
        }

        protected virtual void FillSourceStepFactory(ISourceStepFactoryRegistry registry)
        {
            registry.AddOrOverwrite(typeof(CombinerSourceStepDescription), new CombinerSourceStepFactory());
            registry.AddOrOverwrite(typeof(PathSourceStepDescription), new PathSourceStepFactory());
            registry.AddOrOverwrite(typeof(LiteralSourceStepDescription), new LiteralSourceStepFactory());
        }

        protected virtual ISourceStepFactoryRegistry CreateSourceStepFactoryRegistry()
        {
            return new SourceStepFactory();
        }

        protected virtual void RegisterSourceFactory()
        {
            var sourceFactory = CreateSourceBindingFactory();
            IoCProvider.Instance.RegisterSingleton(sourceFactory);
            if (sourceFactory is ISourceBindingFactoryExtensionHost extensionHost)
            {
                RegisterSourceBindingFactoryExtensions(extensionHost);
                IoCProvider.Instance.RegisterSingleton(extensionHost);
            }
            else
                Log.Trace("source binding factory extension host not provided - so no source extensions will be used");
        }

        protected virtual void RegisterSourceBindingFactoryExtensions(ISourceBindingFactoryExtensionHost extensionHost)
        {
            extensionHost.Extensions.Add(new PropertySourceBindingFactoryExtension());
        }

        protected virtual ISourceBindingFactory CreateSourceBindingFactory()
        {
            return new SourceBindingFactory();
        }

        protected virtual void RegisterTargetFactory()
        {
            var targetRegistry = CreateTargetBindingRegistry();
            IoCProvider.Instance.RegisterSingleton(targetRegistry);
            IoCProvider.Instance.RegisterSingleton<ITargetBindingFactory>(targetRegistry);
            FillTargetFactories(targetRegistry);
        }

        protected virtual ITargetBindingFactoryRegistry CreateTargetBindingRegistry()
        {
            return new TargetBindingFactoryRegistry();
        }

        protected virtual void FillTargetFactories(ITargetBindingFactoryRegistry registry)
        {
            // base class has nothing to register
        }
    }
}