// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Bindings.Core.Base;
using Bindings.Core.Binding.Binders;
using Bindings.Core.Binding.Bindings;
using Bindings.Core.Binding.Bindings.SourceSteps;
using Bindings.Core.Binding.Combiners;
using Bindings.Core.Converters;
using Bindings.Core.Exeptions;
using Bindings.Core.IoC;

namespace Bindings.Core.Binding.BindingContext
{
    public class BaseFluentBindingDescription<TTarget>
        : ApplicableTo<TTarget>, IBaseFluentBindingDescription
        where TTarget : class
    {
        private readonly TTarget _target;
        private readonly IBindingContextOwner _bindingContextOwner;

        private readonly BindingDescription _bindingDescription = new BindingDescription();
        private readonly SourceStepDescription _sourceStepDescription = new SourceStepDescription();
        private ISourceSpec _sourceSpec;

        public interface ISourceSpec
        {
            SourceStepDescription CreateSourceStep(SourceStepDescription inputs);
        }

        public class KnownPathSourceSpec
            : ISourceSpec
        {
            private readonly string _knownSourcePath;

            public KnownPathSourceSpec(string knownSourcePath)
            {
                _knownSourcePath = knownSourcePath;
            }

            public SourceStepDescription CreateSourceStep(SourceStepDescription inputs)
            {
                return new PathSourceStepDescription()
                {
                    Converter = inputs.Converter,
                    ConverterParameter = inputs.ConverterParameter,
                    FallbackValue = inputs.FallbackValue,
                    SourcePropertyPath = _knownSourcePath
                };
            }
        }

        public class FreeTextSourceSpec
            : ISourceSpec
        {
            private readonly string _freeText;

            public FreeTextSourceSpec(string freeText)
            {
                _freeText = freeText;
            }

            public SourceStepDescription CreateSourceStep(SourceStepDescription inputs)
            {
                var parser = IoCProvider.Instance.Resolve<IBindingDescriptionParser>();
                var parsedDescription = parser.ParseSingle(_freeText);

                if (inputs.Converter == null
                    && inputs.FallbackValue == null)
                {
                    return parsedDescription.Source;
                }

                if (parsedDescription.Source.Converter == null
                    && parsedDescription.Source.FallbackValue == null)
                {
                    var parsedStep = parsedDescription.Source;
                    parsedStep.Converter = inputs.Converter;
                    parsedStep.ConverterParameter = inputs.ConverterParameter;
                    parsedStep.FallbackValue = inputs.FallbackValue;
                    return parsedStep;
                }

                return SourceSpecHelpers.WrapInsideSingleCombiner(inputs, parsedDescription.Source);
            }
        }

        public class FullySourceSpec
            : ISourceSpec
        {
            private readonly SourceStepDescription _sourceStepDescription;

            public FullySourceSpec(SourceStepDescription sourceStepDescription)
            {
                _sourceStepDescription = sourceStepDescription;
            }

            public SourceStepDescription CreateSourceStep(SourceStepDescription inputs)
            {
                if (inputs.Converter == null || inputs.FallbackValue == null)
                {
                    return _sourceStepDescription;
                }

                return SourceSpecHelpers.WrapInsideSingleCombiner(inputs, _sourceStepDescription);
            }
        }

        public class CombinerSourceSpec
            : ISourceSpec
        {
            private readonly bool _useParser;
            private readonly string[] _properties;
            private readonly IValueCombiner _combiner;

            public CombinerSourceSpec(IValueCombiner combiner, string[] properties, bool useParser)
            {
                _combiner = combiner;
                _useParser = useParser;
                _properties = properties;
            }

            public SourceStepDescription CreateSourceStep(SourceStepDescription inputs)
            {
                var parser = IoCProvider.Instance.Resolve<IBindingDescriptionParser>();
                var innerSteps = _useParser ?
                    _properties.Select(p => parser.ParseSingle(p).Source) :
                    _properties.Select(p => new PathSourceStepDescription { SourcePropertyPath = p });

                return new CombinerSourceStepDescription
                {
                    Combiner = _combiner,
                    Converter = inputs.Converter,
                    ConverterParameter = inputs.ConverterParameter,
                    FallbackValue = inputs.FallbackValue,
                    InnerSteps = innerSteps.ToList()
                };
            }
        }

        public static class SourceSpecHelpers
        {
            public static SourceStepDescription WrapInsideSingleCombiner(SourceStepDescription inputs,
                                                                        SourceStepDescription sourceStepDescription)
            {
                return new CombinerSourceStepDescription()
                {
                    Combiner = new SingleValueCombiner(),
                    Converter = inputs.Converter,
                    ConverterParameter = inputs.ConverterParameter,
                    FallbackValue = inputs.FallbackValue,
                    InnerSteps = new List<SourceStepDescription>()
                            {
                                sourceStepDescription
                            }
                };
            }
        }

        protected object ClearBindingKey { get; set; }

        object IBaseFluentBindingDescription.ClearBindingKey
        {
            get => ClearBindingKey;
            set => ClearBindingKey = value;
        }

        protected BindingDescription BindingDescription => _bindingDescription;

        protected SourceStepDescription SourceStepDescription => _sourceStepDescription;

        protected void SetFreeTextPropertyPath(string sourcePropertyPath)
        {
            if (_sourceSpec != null)
                throw new BindingException("You cannot set the source path of a Fluent binding more than once");

            _sourceSpec = new FreeTextSourceSpec(sourcePropertyPath);
        }

        protected void SetKnownTextPropertyPath(string sourcePropertyPath)
        {
            if (_sourceSpec != null)
                throw new BindingException("You cannot set the source path of a Fluent binding more than once");

            _sourceSpec = new KnownPathSourceSpec(sourcePropertyPath);
        }

        protected void SetCombiner(IValueCombiner combiner, string[] properties, bool useParser)
        {
            if (_sourceSpec != null)
                throw new BindingException("You cannot set the source path of a Fluent binding more than once");

            _sourceSpec = new CombinerSourceSpec(combiner, properties, useParser);
        }

        protected void SourceOverwrite(BindingDescription bindingDescription)
        {
            if (_sourceSpec != null)
                throw new BindingException("You cannot set the source path of a Fluent binding more than once");

            _bindingDescription.Mode = bindingDescription.Mode;
            _bindingDescription.TargetName = bindingDescription.TargetName;

            _sourceSpec = new FullySourceSpec(bindingDescription.Source);
        }

        protected void FullOverwrite(BindingDescription bindingDescription)
        {
            if (_sourceSpec != null)
                throw new BindingException("You cannot set the source path of a Fluent binding more than once");

            _sourceSpec = new FullySourceSpec(bindingDescription.Source);
        }

        public BaseFluentBindingDescription(IBindingContextOwner bindingContextOwner, TTarget target)
        {
            _bindingContextOwner = bindingContextOwner;
            _target = target;
        }

        protected static string TargetPropertyName(Expression<Func<TTarget, object>> targetPropertyPath)
        {
            var parser = BindingSingletonCache.Instance.PropertyExpressionParser;
            var targetPropertyName = parser.Parse(targetPropertyPath).Print();
            return targetPropertyName;
        }

        protected static string SourcePropertyPath<TSource>(Expression<Func<TSource, object>> sourceProperty)
        {
            var parser = BindingSingletonCache.Instance.PropertyExpressionParser;
            var sourcePropertyPath = parser.Parse(sourceProperty).Print();
            return sourcePropertyPath;
        }

        protected static IValueConverter ValueConverterFromName(string converterName)
        {
            var converter = BindingSingletonCache.Instance.ValueConverterLookup.Find(converterName);
            return converter;
        }

        protected BindingDescription CreateBindingDescription()
        {
            EnsureTargetNameSet();

            SourceStepDescription source;
            if (_sourceSpec == null)
            {
                source = new PathSourceStepDescription()
                {
                    Converter = _sourceStepDescription.Converter,
                    ConverterParameter = _sourceStepDescription.ConverterParameter,
                    FallbackValue = _sourceStepDescription.FallbackValue
                };
            }
            else
            {
                source = _sourceSpec.CreateSourceStep(_sourceStepDescription);
            }

            var toReturn = new BindingDescription()
            {
                Mode = BindingDescription.Mode,
                TargetName = BindingDescription.TargetName,
                Source = source
            };

            return toReturn;
        }

        public override void Apply()
        {
            var bindingDescription = CreateBindingDescription();
            _bindingContextOwner.AddBinding(_target, bindingDescription, ClearBindingKey);
            base.Apply();
        }

        public override void ApplyTo(TTarget what)
        {
            var bindingDescription = CreateBindingDescription();
            _bindingContextOwner.AddBinding(what, bindingDescription, ClearBindingKey);
            base.ApplyTo(what);
        }

        protected void EnsureTargetNameSet()
        {
            if (!string.IsNullOrEmpty(BindingDescription.TargetName))
                return;

            BindingDescription.TargetName =
                BindingSingletonCache.Instance.DefaultBindingNameLookup.DefaultFor(typeof(TTarget));
        }
    }
}
