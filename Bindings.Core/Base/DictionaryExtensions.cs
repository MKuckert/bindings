using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bindings.Core.Base
{
	public static class DictionaryExtensions
    {
		public static IDictionary<string, object> ToPropertyDictionary(this object input)
		{
			if (input == null)
				return new Dictionary<string, object>();

			if (input is IDictionary<string, object>)
				return (IDictionary<string, object>)input;

			var propertyInfos = from property in input.GetType()
													  .GetProperties(BindingFlags.Instance | BindingFlags.Public |
																	 BindingFlags.FlattenHierarchy)
								where property.CanRead
								select property;

			var dictionary = new Dictionary<string, object>();
			foreach (var propertyInfo in propertyInfos)
			{
				dictionary[propertyInfo.Name] = propertyInfo.GetValue(input);
			}
			return dictionary;
		}
	}
}
