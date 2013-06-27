using System;
using System.Linq;

using Reader = System.Func<object, object>;
using Key = System.Tuple<System.Type, string, System.Type>;
using Cache = System.Collections.Generic.Dictionary<System.Tuple<System.Type, string, System.Type>, System.Func<object, object>>;

namespace SeekYouRS.Utilities
{
    internal static class Reflector
    {
        public static void ClearCache()
        {
            _valueReadersCache.Clear();
        }

        public static TTResult ReadValueOrDefault<TTObject, TTResult>(TTObject obj, string name, TTResult defaultValue = default(TTResult))
        {
            var type = obj.GetType();
            var resultType = typeof(TTResult);
            var key = CreateKey(type, resultType, name);

            var result = TryGetReaderFromCache(key)
                .OrElse(() => TryAddReaderToCache(type, resultType, name))
                .Map(r => (TTResult) r(obj))
                .DefaultsTo(defaultValue);

            return result;
        }

        private static readonly Cache _valueReadersCache = new Cache();

        private static Key CreateKey(Type type, Type resultType, string propertyName)
        {
            return Tuple.Create(type, propertyName, resultType);
        }

        static Option<Reader> TryGetReaderFromCache(Key key)
        {
            return Option.Try<Key,Reader>(_valueReadersCache.TryGetValue, key);
        }

        static Option<Reader> TryAddReaderToCache(Type type, Type resultType, string propertyName)
        {
            var reader =
                TryGetPropertyReader(type, resultType, propertyName)
                    .OrElse(() => TryGetFieldReader(type, resultType, propertyName));
            reader.Do(r => _valueReadersCache.Add(CreateKey(type, resultType, propertyName), r));
            return reader;
        }

        static Option<Reader> TryGetPropertyReader(Type type, Type resultType, string propertyName)
        {
            var propertyInfos = type.GetProperties();
            try
            {
                var identifier =
                    propertyInfos.SingleOrDefault(
                        pi => pi.Name.Equals(propertyName,
                                             StringComparison.OrdinalIgnoreCase));

                if (identifier == null) return Option<Reader>.None;
                if (!resultType.IsAssignableFrom(identifier.PropertyType)) return Option<Reader>.None;

                return Option<Reader>.Some(identifier.GetValue);
            }
            catch (Exception)
            {
                return Option<Reader>.None;
            }
        }

        static Option<Reader> TryGetFieldReader(Type type, Type resultType, string fieldName)
        {
            var fieldInfos = type.GetFields();
            try
            {
                var identifier =
                    fieldInfos.SingleOrDefault(
                        pi => pi.Name.Equals(fieldName,
                                             StringComparison.OrdinalIgnoreCase));

                if (identifier == null) return Option<Reader>.None;
                if (!resultType.IsAssignableFrom(identifier.FieldType)) return Option<Reader>.None;
                return Option<Reader>.Some(identifier.GetValue);
            }
            catch (Exception)
            {
                return Option<Reader>.None;
            }
        }
    }
}