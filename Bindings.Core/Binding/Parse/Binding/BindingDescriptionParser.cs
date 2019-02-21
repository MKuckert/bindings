// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Bindings.Core.Binding.Binders;
using Bindings.Core.Binding.Bindings;
using Bindings.Core.Binding.Bindings.SourceSteps;
using Bindings.Core.Binding.Combiners;
using Bindings.Core.Binding.Parse.Binding.Lang;
using Bindings.Core.Binding.Parse.Binding.Tibet;
using Bindings.Core.Converters;
using Bindings.Core.IoC;

namespace Bindings.Core.Binding.Parse.Binding
{
    public class BindingDescriptionParser
        : IBindingDescriptionParser
    {
        private IBindingParser _bindingParser;
        private IValueConverterLookup _valueConverterLookup;

        protected IBindingParser BindingParser
        {
            get
            {
                _bindingParser = _bindingParser ?? IoCProvider.Instance.Resolve<IBindingParser>();
                return _bindingParser;
            }
        }

        private ILanguageBindingParser _languageBindingParser;

        protected ILanguageBindingParser LanguageBindingParser
        {
            get
            {
                _languageBindingParser = _languageBindingParser ?? IoCProvider.Instance.Resolve<ILanguageBindingParser>();
                return _languageBindingParser;
            }
        }

        protected IValueConverterLookup ValueConverterLookup
        {
            get
            {
                _valueConverterLookup = _valueConverterLookup ?? IoCProvider.Instance.Resolve<IValueConverterLookup>();
                return _valueConverterLookup;
            }
        }

        protected IValueConverter FindConverter(string converterName)
        {
            if (converterName == null)
                return null;

            var toReturn = ValueConverterLookup.Find(converterName);
            if (toReturn == null)
                BindingLog.Trace("Could not find named converter for {0}", converterName);

            return toReturn;
        }

        protected IValueCombiner FindCombiner(string combiner)
        {
            return BindingSingletonCache.Instance.ValueCombinerLookup.Find(combiner);
        }

        public IEnumerable<BindingDescription> Parse(string text)
        {
            var parser = BindingParser;
            return Parse(text, parser);
        }

        public IEnumerable<BindingDescription> LanguageParse(string text)
        {
            var parser = LanguageBindingParser;
            return Parse(text, parser);
        }

        public IEnumerable<BindingDescription> Parse(string text, IBindingParser parser)
        {
            if (!parser.TryParseBindingSpecification(text, out var specification))
            {
                BindingLog.Error(
                                      "Failed to parse binding specification starting with {0}",
                                      text == null ? "" : (text.Length > 20 ? text.Substring(0, 20) : text));
                return null;
            }

            if (specification == null)
                return null;

            return from item in specification
                   select SerializableBindingToBinding(item.Key, item.Value);
        }

        public BindingDescription ParseSingle(string text)
        {
            var parser = BindingParser;
            if (!parser.TryParseBindingDescription(text, out var description))
            {
                BindingLog.Error(
                                      "Failed to parse binding description starting with {0}",
                                      text == null ? "" : (text.Length > 20 ? text.Substring(0, 20) : text));
                return null;
            }

            if (description == null)
                return null;

            return SerializableBindingToBinding(null, description);
        }

        public BindingDescription SerializableBindingToBinding(string targetName,
                                                                  SerializableBindingDescription description)
        {
            return new BindingDescription
            {
                TargetName = targetName,
                Source = SourceStepDescriptionFrom(description),
                Mode = description.Mode,
            };
        }

        private SourceStepDescription SourceStepDescriptionFrom(SerializableBindingDescription description)
        {
            if (description.Path != null)
            {
                return new PathSourceStepDescription()
                {
                    SourcePropertyPath = description.Path,
                    Converter = FindConverter(description.Converter),
                    ConverterParameter = description.ConverterParameter,
                    FallbackValue = description.FallbackValue
                };
            }

            if (description.Literal != null)
            {
                var literal = description.Literal;
                if (literal == TibetBindingParser.LiteralNull)
                    literal = null;

                return new LiteralSourceStepDescription()
                {
                    Literal = literal,
                    Converter = FindConverter(description.Converter),
                    ConverterParameter = description.ConverterParameter,
                    FallbackValue = description.FallbackValue
                };
            }

            if (description.Function != null)
            {
                // first look for a combiner with the name
                var combiner = FindCombiner(description.Function);
                if (combiner != null)
                {
                    return new CombinerSourceStepDescription()
                    {
                        Combiner = combiner,
                        InnerSteps = description.Sources == null
                            ? new List<SourceStepDescription>() :
                            description.Sources.Select(s => SourceStepDescriptionFrom(s)).ToList(),
                        Converter = FindConverter(description.Converter),
                        ConverterParameter = description.ConverterParameter,
                        FallbackValue = description.FallbackValue
                    };
                }
                else
                {
                    // no combiner, then drop back to looking for a converter
                    var converter = FindConverter(description.Function);
                    if (converter == null)
                    {
                        BindingLog.Error("Failed to find combiner or converter for {0}", description.Function);
                    }

                    if (description.Sources == null || description.Sources.Count == 0)
                    {
                        BindingLog.Error("Value Converter {0} supplied with no source", description.Function);
                        return new LiteralSourceStepDescription()
                        {
                            Literal = null,
                        };
                    }
                    else if (description.Sources.Count > 2)
                    {
                        BindingLog.Error("Value Converter {0} supplied with too many parameters - {1}", description.Function, description.Sources.Count);
                        return new LiteralSourceStepDescription()
                        {
                            Literal = null,
                        };
                    }
                    else
                    {
                        return new CombinerSourceStepDescription()
                        {
                            Combiner = new ValueConverterValueCombiner(converter),
                            InnerSteps = description.Sources.Select(SourceStepDescriptionFrom).ToList(),
                            Converter = FindConverter(description.Converter),
                            ConverterParameter = description.ConverterParameter,
                            FallbackValue = description.FallbackValue
                        };
                    }
                }
            }

            // this probably suggests that the path is the entire source object
            return new PathSourceStepDescription()
            {
                SourcePropertyPath = null,
                Converter = FindConverter(description.Converter),
                ConverterParameter = description.ConverterParameter,
                FallbackValue = description.FallbackValue
            };
        }
    }
}
