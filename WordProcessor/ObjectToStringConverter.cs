using System;
using System.Collections.Generic;

namespace WordProcessor
{
	public sealed class ObjectToStringConverter
	{
		private Dictionary<Type, Func<object, string>> converters = new Dictionary<Type, Func<object, string>>();

		public ObjectToStringConverter()
		{

		}

		public void Register(Type type, Func<object, string> converter)
		{
			converters[type] = converter;
		}

		public void Register<T>(Func<T, string> converter)
		{
			converters[typeof(T)] = (obj) => converter((T)obj);
		}

		public string ToString(object obj)
		{
			if (obj == null)
				return "null";
			var type = obj.GetType();
			if (converters.ContainsKey(type))
				return converters[type](obj);
			else
				return obj.ToString();
		}
	}
}