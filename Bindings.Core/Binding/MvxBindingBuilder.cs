// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Bindings;
using MvvmCross.Base;
using MvvmCross.Binding.Binders;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Source.Construction;
using MvvmCross.Binding.Bindings.SourceSteps;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Binding.Combiners;
using MvvmCross.Binding.ExpressionParse;
using MvvmCross.Binding.Parse.Binding;
using MvvmCross.Binding.Parse.Binding.Tibet;
using MvvmCross.Binding.Parse.PropertyPath;
using MvvmCross.Binding.ValueConverters;
using MvvmCross.Converters;
using MvvmCross.IoC;

namespace MvvmCross.Binding
{
    public class MvxBindingBuilder
    {
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
            MvxIoCProvider.Initialize();
        }

        private void RegisterMainThreadDispatcher()
        {
            Mvx.IoCProvider.RegisterSingleton<IMvxMainThreadAsyncDispatcher>(new MainThreadDispatcher());
        }

        protected virtual void RegisterAutoValueConverters()
        {
            var autoValueConverters = CreateAutoValueConverters();
            Mvx.IoCProvider.RegisterSingleton<IMvxAutoValueConverters>(autoValueConverters);
            FillAutoValueConverters(autoValueConverters);
        }

        protected virtual void FillAutoValueConverters(IMvxAutoValueConverters autoValueConverters)
        {
            // nothing to do in base class
        }

        protected virtual IMvxAutoValueConverters CreateAutoValueConverters()
        {
            return new MvxAutoValueConverters();
        }

        protected virtual void CreateSingleton()
        {
            MvxBindingSingletonCache.Initialize();
        }

        protected virtual void RegisterExpressionParser()
        {
            Mvx.IoCProvider.RegisterSingleton<IMvxPropertyExpressionParser>(new MvxPropertyExpressionParser());
        }

        protected virtual void RegisterCore()
        {
            Mvx.IoCProvider.RegisterSingleton<IMvxBinder>(new MvxFromTextBinder());

            //To get the old behavior back, you can override this registration with
            //Mvx.IoCProvider.RegisterType<IMvxBindingContext, MvxBindingContext>();
            Mvx.IoCProvider.RegisterType<IMvxBindingContext, MvxTaskBasedBindingContext>();
        }

        protected virtual void RegisterValueConverterProvider()
        {
            var registry = CreateValueConverterRegistry();
            Mvx.IoCProvider.RegisterSingleton<IMvxNamedInstanceLookup<IMvxValueConverter>>(registry);
            Mvx.IoCProvider.RegisterSingleton<IMvxNamedInstanceRegistry<IMvxValueConverter>>(registry);
            Mvx.IoCProvider.RegisterSingleton<IMvxValueConverterLookup>(registry);
            Mvx.IoCProvider.RegisterSingleton<IMvxValueConverterRegistry>(registry);
            FillValueConverters(registry);
        }

        protected virtual MvxValueConverterRegistry CreateValueConverterRegistry()
        {
            return new MvxValueConverterRegistry();
        }

        protected virtual void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            registry.AddOrOverwrite("CommandParameter", new MvxCommandParameterValueConverter());
        }

        protected virtual void RegisterValueCombinerProvider()
        {
            var registry = CreateValueCombinerRegistry();
            Mvx.IoCProvider.RegisterSingleton<IMvxNamedInstanceLookup<IMvxValueCombiner>>(registry);
            Mvx.IoCProvider.RegisterSingleton<IMvxNamedInstanceRegistry<IMvxValueCombiner>>(registry);
            Mvx.IoCProvider.RegisterSingleton<IMvxValueCombinerLookup>(registry);
            Mvx.IoCProvider.RegisterSingleton<IMvxValueCombinerRegistry>(registry);
            FillValueCombiners(registry);
        }

        protected virtual IMvxValueCombinerRegistry CreateValueCombinerRegistry()
        {
            return new MvxValueCombinerRegistry();
        }

        protected virtual void FillValueCombiners(IMvxValueCombinerRegistry registry)
        {
            // note that assembly based registration is not used here for efficiency reasons
            // - see #327 - https://github.com/slodge/MvvmCross/issues/327
            registry.AddOrOverwrite("Add", new MvxAddValueCombiner());
            registry.AddOrOverwrite("Divide", new MvxDivideValueCombiner());
            registry.AddOrOverwrite("Format", new MvxFormatValueCombiner());
            registry.AddOrOverwrite("If", new MvxIfValueCombiner());
            registry.AddOrOverwrite("Modulus", new MvxModulusValueCombiner());
            registry.AddOrOverwrite("Multiply", new MvxMultiplyValueCombiner());
            registry.AddOrOverwrite("Single", new MvxSingleValueCombiner());
            registry.AddOrOverwrite("Subtract", new MvxSubtractValueCombiner());
            registry.AddOrOverwrite("EqualTo", new MvxEqualToValueCombiner());
            registry.AddOrOverwrite("NotEqualTo", new MvxNotEqualToValueCombiner());
            registry.AddOrOverwrite("GreaterThanOrEqualTo", new MvxGreaterThanOrEqualToValueCombiner());
            registry.AddOrOverwrite("GreaterThan", new MvxGreaterThanValueCombiner());
            registry.AddOrOverwrite("LessThanOrEqualTo", new MvxLessThanOrEqualToValueCombiner());
            registry.AddOrOverwrite("LessThan", new MvxLessThanValueCombiner());
            registry.AddOrOverwrite("Not", new MvxNotValueCombiner());
            registry.AddOrOverwrite("And", new MvxAndValueCombiner());
            registry.AddOrOverwrite("Or", new MvxOrValueCombiner());
            registry.AddOrOverwrite("XOr", new MvxXorValueCombiner());
            registry.AddOrOverwrite("Inverted", new MvxInvertedValueCombiner());

            // Note: MvxValueConverterValueCombiner is not registered - it is unconventional
            //registry.AddOrOverwrite("ValueConverter", new MvxValueConverterValueCombiner());
        }

        protected virtual void RegisterBindingParser()
        {
            if (Mvx.IoCProvider.CanResolve<IMvxBindingParser>())
            {
                MvxBindingLog.Trace("Binding Parser already registered - so skipping Default parser");
                return;
            }
            MvxBindingLog.Trace("Registering Default Binding Parser");
            Mvx.IoCProvider.RegisterSingleton(CreateBindingParser());
        }

        protected virtual IMvxBindingParser CreateBindingParser()
        {
            return new MvxTibetBindingParser();
        }

        protected virtual void RegisterBindingDescriptionParser()
        {
            var parser = CreateBindingDescriptionParser();
            Mvx.IoCProvider.RegisterSingleton(parser);
        }

        private static IMvxBindingDescriptionParser CreateBindingDescriptionParser()
        {
            var parser = new MvxBindingDescriptionParser();
            return parser;
        }

        protected virtual void RegisterSourcePropertyPathParser()
        {
            var tokeniser = CreateSourcePropertyPathParser();
            Mvx.IoCProvider.RegisterSingleton<IMvxSourcePropertyPathParser>(tokeniser);
        }

        protected virtual IMvxSourcePropertyPathParser CreateSourcePropertyPathParser()
        {
            return new MvxSourcePropertyPathParser();
        }

        protected virtual void RegisterBindingNameRegistry()
        {
            var registry = new MvxBindingNameRegistry();
            Mvx.IoCProvider.RegisterSingleton<IMvxBindingNameLookup>(registry);
            Mvx.IoCProvider.RegisterSingleton<IMvxBindingNameRegistry>(registry);
            FillDefaultBindingNames(registry);
        }

        protected virtual void FillDefaultBindingNames(IMvxBindingNameRegistry registry)
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
            Mvx.IoCProvider.RegisterSingleton<IMvxSourceStepFactoryRegistry>(sourceStepFactory);
            Mvx.IoCProvider.RegisterSingleton<IMvxSourceStepFactory>(sourceStepFactory);
        }

        protected virtual void FillSourceStepFactory(IMvxSourceStepFactoryRegistry registry)
        {
            registry.AddOrOverwrite(typeof(MvxCombinerSourceStepDescription), new MvxCombinerSourceStepFactory());
            registry.AddOrOverwrite(typeof(MvxPathSourceStepDescription), new MvxPathSourceStepFactory());
            registry.AddOrOverwrite(typeof(MvxLiteralSourceStepDescription), new MvxLiteralSourceStepFactory());
        }

        protected virtual IMvxSourceStepFactoryRegistry CreateSourceStepFactoryRegistry()
        {
            return new MvxSourceStepFactory();
        }

        protected virtual void RegisterSourceFactory()
        {
            var sourceFactory = CreateSourceBindingFactory();
            Mvx.IoCProvider.RegisterSingleton<IMvxSourceBindingFactory>(sourceFactory);
            var extensionHost = sourceFactory as IMvxSourceBindingFactoryExtensionHost;
            if (extensionHost != null)
            {
                RegisterSourceBindingFactoryExtensions(extensionHost);
                Mvx.IoCProvider.RegisterSingleton<IMvxSourceBindingFactoryExtensionHost>(extensionHost);
            }
            else
                MvxBindingLog.Trace("source binding factory extension host not provided - so no source extensions will be used");
        }

        protected virtual void RegisterSourceBindingFactoryExtensions(IMvxSourceBindingFactoryExtensionHost extensionHost)
        {
            extensionHost.Extensions.Add(new MvxPropertySourceBindingFactoryExtension());
        }

        protected virtual IMvxSourceBindingFactory CreateSourceBindingFactory()
        {
            return new MvxSourceBindingFactory();
        }

        protected virtual void RegisterTargetFactory()
        {
            var targetRegistry = CreateTargetBindingRegistry();
            Mvx.IoCProvider.RegisterSingleton<IMvxTargetBindingFactoryRegistry>(targetRegistry);
            Mvx.IoCProvider.RegisterSingleton<IMvxTargetBindingFactory>(targetRegistry);
            FillTargetFactories(targetRegistry);
        }

        protected virtual IMvxTargetBindingFactoryRegistry CreateTargetBindingRegistry()
        {
            return new MvxTargetBindingFactoryRegistry();
        }

        protected virtual void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            // base class has nothing to register
        }
    }
}