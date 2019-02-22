// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Bindings.Core.Exeptions;
using Bindings.Core.Logging;

namespace Bindings.Core.Binding.Parse.Binding.Swiss
{
    public class SwissBindingParser
        : BindingParser
    {
        protected virtual IEnumerable<char> TerminatingCharacters()
        {
            return new[] { '=', ',', ';', '(', ')' };
        }

        private void ParsePath(string block, SerializableBindingDescription description)
        {
            ParseEquals(block);
            ThrowExceptionIfPathAlreadyDefined(description);
            description.Path = ReadTextUntilNonQuotedOccurrenceOfAnyOf(',', ';');
        }

        private void ParseConverter(string block, SerializableBindingDescription description)
        {
            ParseEquals(block);
            var converter = ReadTargetPropertyName();
            if (!string.IsNullOrEmpty(description.Converter))
                Log.Warning("Overwriting existing Converter with {0}", converter);
            description.Converter = converter;
        }

        private void ParseConverterParameter(string block, SerializableBindingDescription description)
        {
            ParseEquals(block);
            if (description.ConverterParameter != null)
                Log.Warning("Overwriting existing ConverterParameter");
            description.ConverterParameter = ReadValue();
        }

        private void ParseCommandParameter(string block, SerializableBindingDescription description)
        {
            if (!IsComplete &&
               CurrentChar == '(')
            {
                // following https://github.com/MvvmCross/MvvmCross/issues/704, if the next character is "(" then
                // we can treat CommandParameter as a normal non-keyword block
                ParseNonKeywordBlockInto(description, block);
            }
            else
            {
                ParseEquals(block);
                if (!string.IsNullOrEmpty(description.Converter))
                    Log.Warning("Overwriting existing Converter with CommandParameter");
                description.Converter = "CommandParameter";
                description.ConverterParameter = ReadValue();
            }
        }

        private void ParseFallbackValue(string block, SerializableBindingDescription description)
        {
            ParseEquals(block);
            if (description.FallbackValue != null)
                Log.Warning("Overwriting existing FallbackValue");
            description.FallbackValue = ReadValue();
        }

        private void ParseMode(string block, SerializableBindingDescription description)
        {
            ParseEquals(block);
            description.Mode = ReadBindingMode();
        }

        protected virtual void ParseNextBindingDescriptionOptionInto(SerializableBindingDescription description)
        {
            if (IsComplete)
                return;

            var block = ReadTextUntilNonQuotedOccurrenceOfAnyOf(TerminatingCharacters().ToArray());
            block = block.Trim();
            if (string.IsNullOrEmpty(block))
            {
                HandleEmptyBlock(description);
                return;
            }

            switch (block)
            {
                case "Path":
                    ParsePath(block, description);
                    break;
                case "Converter":
                    ParseConverter(block, description);
                    break;
                case "ConverterParameter":
                    ParseConverterParameter(block, description);
                    break;
                case "CommandParameter":
                    ParseCommandParameter(block, description);
                    break;
                case "FallbackValue":
                    ParseFallbackValue(block, description);
                    break;
                case "Mode":
                    ParseMode(block, description);
                    break;
                default:
                    ParseNonKeywordBlockInto(description, block);
                    break;
            }
        }

        protected virtual void HandleEmptyBlock(SerializableBindingDescription description)
        {
            // default implementation doesn't do any special handling on an empty block
        }

        protected virtual void ParseNonKeywordBlockInto(SerializableBindingDescription description, string block)
        {
            if (!IsComplete && CurrentChar == '(')
            {
                ParseFunctionStyleBlockInto(description, block);
            }
            else
            {
                ThrowExceptionIfPathAlreadyDefined(description);
                description.Path = block;
            }
        }

        protected virtual void ParseFunctionStyleBlockInto(SerializableBindingDescription description, string block)
        {
            description.Converter = block;
            MoveNext();
            if (IsComplete)
                throw new BindingException("Unterminated () pair for converter {0}", block);

            ParseChildBindingDescriptionInto(description);
            SkipWhitespace();
            switch (CurrentChar)
            {
                case ')':
                    MoveNext();
                    break;

                case ',':
                    MoveNext();
                    ReadConverterParameterAndClosingBracket(description);
                    break;

                default:
                    throw new BindingException("Unexpected character {0} while parsing () contents", CurrentChar);
            }
        }

        protected void ReadConverterParameterAndClosingBracket(SerializableBindingDescription description)
        {
            SkipWhitespace();
            description.ConverterParameter = ReadValue();
            SkipWhitespace();
            if (CurrentChar != ')')
                throw new BindingException("Unterminated () pair for converter {0}");
            MoveNext();
        }

        protected void ParseChildBindingDescriptionInto(SerializableBindingDescription description,
            ParentIsLookingForComma parentIsLookingForComma = ParentIsLookingForComma.ParentIsLookingForComma)
        {
            SkipWhitespace();
            description.Function = "Single";
            description.Sources = new[]
                {
                    ParseBindingDescription(parentIsLookingForComma)
                };
        }

        protected void ThrowExceptionIfPathAlreadyDefined(SerializableBindingDescription description)
        {
            if (description.Path != null && 
                description.Literal != null && 
                description.Function != null)
            {
                throw new BindingException(
                    "Make sure you are using ';' to separate multiple bindings. You cannot specify Path/Literal/Combiner more than once - position {0} in {1}",
                    CurrentIndex, FullText);
            }
        }

        protected override SerializableBindingDescription ParseBindingDescription() => 
            ParseBindingDescription(ParentIsLookingForComma.ParentIsNotLookingForComma);

        protected enum ParentIsLookingForComma
        {
            ParentIsLookingForComma,
            ParentIsNotLookingForComma
        }

        protected virtual SerializableBindingDescription ParseBindingDescription(
            ParentIsLookingForComma parentIsLookingForComma)
        {
            var description = new SerializableBindingDescription();
            SkipWhitespace();

            while (true)
            {
                ParseNextBindingDescriptionOptionInto(description);

                SkipWhitespace();
                if (IsComplete)
                    return description;

                switch (CurrentChar)
                {
                    case ',':
                        if (parentIsLookingForComma == ParentIsLookingForComma.ParentIsLookingForComma)
                            return description;

                        MoveNext();
                        break;

                    case ';':
                    case ')':
                        return description;

                    default:
                        if (DetectOperator())
                            ParseOperatorWithLeftHand(description);
                        else
                            throw new BindingException(
                                "Unexpected character {0} at position {1} in {2} - expected string-end, ',' or ';'",
                                CurrentChar,
                                CurrentIndex,
                                FullText);
                        break;
                }
            }
        }

        protected virtual SerializableBindingDescription ParseOperatorWithLeftHand(
            SerializableBindingDescription description)
        {
            throw new BindingException("Operators not expected in base SwissBinding");
        }

        protected virtual bool DetectOperator() => false;
    }
}