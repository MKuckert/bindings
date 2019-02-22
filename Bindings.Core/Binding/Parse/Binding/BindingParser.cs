// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bindings.Core.Base;
using Bindings.Core.Exeptions;
using Bindings.Core.Logging;

namespace Bindings.Core.Binding.Parse.Binding
{
    public abstract class BindingParser
        : Parser, IBindingParser
    {
        protected abstract SerializableBindingDescription ParseBindingDescription();

        public bool TryParseBindingDescription(string text, out SerializableBindingDescription requestedDescription)
        {
            try
            {
                Reset(text);
                requestedDescription = ParseBindingDescription();
                return true;
            }
            catch (Exception exception)
            {
                Log.Error("Problem parsing binding {0}", exception);
                requestedDescription = null;
                return false;
            }
        }

        public bool TryParseBindingSpecification(string text, out SerializableBindingSpecification requestedBindings)
        {
            try
            {
                Reset(text);

                var toReturn = new SerializableBindingSpecification();
                while (!IsComplete)
                {
                    SkipWhitespaceAndDescriptionSeparators();
                    var result = ParseTargetPropertyNameAndDescription();
                    toReturn[result.Key] = result.Value;
                    SkipWhitespaceAndDescriptionSeparators();
                }

                requestedBindings = toReturn;
                return true;
            }
            catch (Exception exception)
            {
                Log.Error("Problem parsing binding {0}", exception);
                requestedBindings = null;
                return false;
            }
        }

        protected KeyValuePair<string, SerializableBindingDescription> ParseTargetPropertyNameAndDescription()
        {
            var targetPropertyName = ReadTargetPropertyName();
            SkipWhitespace();
            var description = ParseBindingDescription();
            return new KeyValuePair<string, SerializableBindingDescription>(targetPropertyName, description);
        }

        protected void ParseEquals(string block)
        {
            if (IsComplete)
                throw new BindingException("Cannot terminate binding expression during option {0} in {1}",
                                       block,
                                       FullText);
            if (CurrentChar != '=')
                throw new BindingException("Must follow binding option {0} with an '=' in {1}",
                                       block,
                                       FullText);

            MoveNext();
            if (IsComplete)
                throw new BindingException("Cannot terminate binding expression during option {0} in {1}",
                                       block,
                                       FullText);
        }

        protected BindingMode ReadBindingMode()
        {
            return (BindingMode)ReadEnumerationValue(typeof(BindingMode));
        }

        protected string ReadTextUntilNonQuotedOccurrenceOfAnyOf(params char[] terminationCharacters)
        {
            var terminationLookup = terminationCharacters.ToDictionary(c => c, c => true);
            SkipWhitespace();
            var toReturn = new StringBuilder();

            while (!IsComplete)
            {
                var currentChar = CurrentChar;
                if (currentChar == '\'' || currentChar == '\"')
                {
                    var subText = ReadQuotedString();
                    toReturn.Append(currentChar);
                    toReturn.Append(subText);
                    toReturn.Append(currentChar);
                    continue;
                }

                if (terminationLookup.ContainsKey(currentChar))
                {
                    break;
                }

                toReturn.Append(currentChar);
                MoveNext();
            }

            return toReturn.ToString();
        }

        protected string ReadTargetPropertyName()
        {
            return ReadValidCSharpName();
        }

        protected void SkipWhitespaceAndOptionSeparators()
        {
            SkipWhitespaceAndCharacters(',');
        }

        protected void SkipWhitespaceAndDescriptionSeparators()
        {
            SkipWhitespaceAndCharacters(';');
        }
    }
}