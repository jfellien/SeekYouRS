using System;
using System.Collections.Generic;
using System.Linq;

using ValueGetterKey = System.Tuple<System.Type, string, System.Type>;
using ValueGetter = System.Func<object, object>;

namespace SeekYouRS
{
    public static class Reflector
    {
        public static void ClearCache()
        {
            _valueGetters.Clear();
        }

        public static tResult ReadValueOrDefault<tObject, tResult>(tObject obj, string name, tResult defaultValue = default(tResult))
        {
            var type = obj.GetType();
            var resultType = typeof(tResult);
            var key = Tuple.Create(type, name, resultType);
            ValueGetter getter;

            if (_valueGetters.TryGetValue(key, out getter))
                return (tResult)getter(obj);

            if (!TryGetPropertyReader(type, resultType, name, out getter)
                && !TryGetFieldReader(type, resultType, name, out getter))
                getter = _ => defaultValue;

            _valueGetters.Add(key, getter);
            return (tResult)getter(obj);
        }

        private static readonly Dictionary<ValueGetterKey, ValueGetter> 
            _valueGetters = new Dictionary<ValueGetterKey, ValueGetter>();

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