// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Bindings.Core.Binding.Parse.PropertyPath;
using Bindings.Core.Binding.Parse.PropertyPath.PropertyTokens;
using Bindings.Core.Exeptions;
using Bindings.Core.IoC;

namespace Bindings.Core.Binding.Bindings.Source.Construction
{
    public class SourceBindingFactory
        : ISourceBindingFactory
        , ISourceBindingFactoryExtensionHost
    {
        private ISourcePropertyPathParser _propertyPathParser;

        protected ISourcePropertyPathParser SourcePropertyPathParser => _propertyPathParser ?? (_propertyPathParser = IoCProvider.Instance.Resolve<ISourcePropertyPathParser>());

        private readonly List<ISourceBindingFactoryExtension> _extensions = new List<ISourceBindingFactoryExtension>();

        protected bool TryCreateBindingFromExtensions(object source, PropertyToken propertyToken,
                                            List<PropertyToken> remainingTokens, out ISourceBinding result)
        {
            foreach (var extension in _extensions)
            {
                if (extension.TryCreateBinding(source, propertyToken, remainingTokens, out result))
                {
                    return true;
                }
            }

            result = null;
            return false;
        }

        public ISourceBinding CreateBinding(object source, string combinedPropertyName)
        {
            var tokens = SourcePropertyPathParser.Parse(combinedPropertyName);
            return CreateBinding(source, tokens);
        }

        public ISourceBinding CreateBinding(object source, IList<PropertyToken> tokens)
        {
            if (tokens == null || tokens.Count == 0)
            {
                throw new BindingException("empty token list passed to CreateBinding");
            }

            var currentToken = tokens[0];
            var remainingTokens = tokens.Skip(1).ToList();
            if (TryCreateBindingFromExtensions(source, currentToken, remainingTokens, out var extensionResult))
            {
                return extensionResult;
            }

            if (source != null)
            {
                BindingLog.Warning(
                    "Unable to bind: source property source not found {0} on {1}"
                    , currentToken
                    , source.GetType().Name);
            }

            return new MissingSourceBinding(source);
        }

        public IList<ISourceBindingFactoryExtension> Extensions => _extensions;
    }
}