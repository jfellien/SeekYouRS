using System;
using System.Linq;

namespace SeekYouRS
{
    public static class Reflector
    {
        static bool TryReadProperty<tObject, tResult>(tObject obj, string propertyName, out tResult result)
        {
            result = default(tResult);
            var propertyInfos = obj.GetType().GetProperties();

            try
            {
                var identifier =
                    propertyInfos.SingleOrDefault(
                        pi => pi.Name.Equals(propertyName,
                                             StringComparison.OrdinalIgnoreCase));

                if (identifier == null) return false;

                result = (tResult) identifier.GetValue(obj);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        static bool TryReadField<tObject, tResult>(tObject obj, string fieldName, out tResult result)
        {
            result = default(tResult);
            var filedInfos = obj.GetType().GetFields();

            try
            {
                var identifier =
                    filedInfos.SingleOrDefault(
                        pi => pi.Name.Equals(fieldName,
                                             StringComparison.OrdinalIgnoreCase));

                if (identifier == null) return false;

                result = (tResult)identifier.GetValue(obj);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static tResult ReadValueOrDefault<tObject, tResult>(tObject obj, string name, tResult defaultValue = default(tResult))
        {
            tResult result;

            if (TryReadField(obj, name, out result)
                || TryReadProperty(obj, name, out result))
                return result;

            return defaultValue;
        }
    }
}