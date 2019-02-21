// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using Bindings.Core.Binding.Bindings.Source.Chained;
using Bindings.Core.Binding.Bindings.Source.Leaf;
using Bindings.Core.Binding.Parse.PropertyPath.PropertyTokens;
using Bindings.Core.Exeptions;

namespace Bindings.Core.Binding.Bindings.Source.Construction
{
    /// <summary>
    /// Uses a global cache of calls in Reflection namespace
    /// </summary>
    public class PropertySourceBindingFactoryExtension
        : ISourceBindingFactoryExtension
    {
        private static readonly ConcurrentDictionary<int, PropertyInfo> PropertyInfoCache = new ConcurrentDictionary<int, PropertyInfo>();

        public bool TryCreateBinding(object source, PropertyToken currentToken, List<PropertyToken> remainingTokens, out ISourceBinding result)
        {
            if (source == null)
            {
                result = null;
                return false;
            }

            result = remainingTokens.Count == 0 ? CreateLeafBinding(source, currentToken) : CreateChainedBinding(source, currentToken, remainingTokens);
            return result != null;
        }

        protected virtual ChainedSourceBinding CreateChainedBinding(object source, PropertyToken propertyToken,
                                                                       List<PropertyToken> remainingTokens)
        {
            if (propertyToken is IndexerPropertyToken indexPropertyToken)
            {
                var itemPropertyInfo = FindPropertyInfo(source);
                if (itemPropertyInfo == null)
                    return null;

                return new IndexerChainedSourceBinding(source, itemPropertyInfo, indexPropertyToken,
                                                          remainingTokens);
            }

            if (propertyToken is PropertyNamePropertyToken propertyNameToken)
            {
                var propertyInfo = FindPropertyInfo(source, propertyNameToken.PropertyName);

                if (propertyInfo == null)
                    return null;

                return new SimpleChainedSourceBinding(source, propertyInfo,
                                                         remainingTokens);
            }

            throw new BindingException("Unexpected property chaining - seen token type {0}",
                                   propertyToken.GetType().FullName);
        }

        protected virtual ISourceBinding CreateLeafBinding(object source, PropertyToken propertyToken)
        {
            if (propertyToken is IndexerPropertyToken indexPropertyToken)
            {
                var itemPropertyInfo = FindPropertyInfo(source);
                if (itemPropertyInfo == null)
                    return null;
                return new IndexerLeafPropertyInfoSourceBinding(source, itemPropertyInfo, indexPropertyToken);
            }

            if (propertyToken is PropertyNamePropertyToken propertyNameToken)
            {
                var propertyInfo = FindPropertyInfo(source, propertyNameToken.PropertyName);
                if (propertyInfo == null)
                    return null;
                return new SimpleLeafPropertyInfoSourceBinding(source, propertyInfo);
            }

            if (propertyToken is EmptyPropertyToken)
            {
                return new DirectToSourceBinding(source);
            }

            throw new BindingException("Unexpected property source - seen token type {0}", propertyToken.GetType().FullName);
        }

        private PropertyInfo FindPropertyInfo(object source, string propertyName = "Item")
        {
            var sourceType = source.GetType();
            var key = (sourceType.FullName + "." + propertyName).GetHashCode();

            PropertyInfo pi;
            if (PropertyInfoCache.TryGetValue(key, out pi))
                return pi;

            //Get lowest property
            while (sourceType != null)
            {
                //Use BindingFlags.DeclaredOnly to avoid AmbiguousMatchException
                pi = sourceType.GetProperty(propertyName, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
                if (pi != null)
                {
                    break;
                }
                sourceType = sourceType.BaseType;
            }

            PropertyInfoCache.TryAdd(key, pi);
            return pi;
        }
    }
}
