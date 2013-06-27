using System;
using System.Collections.Generic;
using System.Linq;

namespace SeekYouRS.Utilities
{
    internal static class Reflector
    {
        public static void ClearCache()
        {
            ValueGetters.Clear();
        }

        public static TTResult ReadValueOrDefault<TTObject, TTResult>(TTObject obj, string name, TTResult defaultValue = default(TTResult))
        {
            var type = obj.GetType();
            var resultType = typeof(TTResult);
            var key = Tuple.Create(type, name, resultType);
            Func<object, object> getter;

            if (ValueGetters.TryGetValue(key, out getter))
                return (TTResult)getter(obj);

            if (!TryGetPropertyReader(type, resultType, name, out getter)
                && !TryGetFieldReader(type, resultType, name, out getter))
                getter = _ => defaultValue;

            ValueGetters.Add(key, getter);
            return (TTResult)getter(obj);
        }

        private static readonly Dictionary<Tuple<Type, string, Type>, Func<object, object>>
            ValueGetters = new Dictionary<Tuple<Type, string, Type>, Func<object, object>>();

        static bool TryGetPropertyReader(Type type, Type resultType, string propertyName, out Func<object, object> propertyReader)
        {
            propertyReader = _ => { throw new NotSupportedException(); };

            var propertyInfos = type.GetProperties();
            try
            {
                var identifier =
                    propertyInfos.SingleOrDefault(
                        pi => pi.Name.Equals(propertyName,
                                             StringComparison.OrdinalIgnoreCase));

                if (identifier == null) return false;
                if (!resultType.IsAssignableFrom(identifier.PropertyType)) return false;

                propertyReader = identifier.GetValue;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        static bool TryGetFieldReader(Type type, Type resultType, string fieldName, out Func<object, object> fieldReader)
        {
            fieldReader = _ => { throw new NotSupportedException(); };

            var fieldInfos = type.GetFields();
            try
            {
                var identifier =
                    fieldInfos.SingleOrDefault(
                        pi => pi.Name.Equals(fieldName,
                                             StringComparison.OrdinalIgnoreCase));

                if (identifier == null) return false;
                if (!resultType.IsAssignableFrom(identifier.FieldType)) return false;
                fieldReader = identifier.GetValue;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}