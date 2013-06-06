using System;
using System.Linq;

namespace SeekYouRS
{
    static class Reflector
    {
        static bool TryReadProperty<TTObject, TTResult>(TTObject obj, string propertyName, out TTResult result)
        {
            result = default(TTResult);
            var propertyInfos = obj.GetType().GetProperties();

            try
            {
                var identifier =
                    propertyInfos.SingleOrDefault(
                        pi => pi.Name.Equals(propertyName,
                                             StringComparison.OrdinalIgnoreCase));

                if (identifier == null) return false;

                result = (TTResult) identifier.GetValue(obj);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        static bool TryReadField<TTObject, TTResult>(TTObject obj, string fieldName, out TTResult result)
        {
            result = default(TTResult);
            var filedInfos = obj.GetType().GetFields();

            try
            {
                var identifier =
                    filedInfos.SingleOrDefault(
                        pi => pi.Name.Equals(fieldName,
                                             StringComparison.OrdinalIgnoreCase));

                if (identifier == null) return false;

                result = (TTResult) identifier.GetValue(obj);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static TTResult ReadValueOrDefault<TTObject, TTResult>(TTObject obj, string propertyName,
                                                                      TTResult defaultValue = default(TTResult))
        {
            TTResult result;

            if (TryReadField(obj, propertyName, out result)
                || TryReadProperty(obj, propertyName, out result))
                return result;

            return defaultValue;
        }
    }
}