using System;
using System.Linq;
using FSharpx;
using Microsoft.FSharp.Core;

namespace SeekYouRS
{
    public static class Reflector
    {
        static FSharpOption<tResult> TryReadProperty<tObject, tResult>(tObject obj, string propertyName)
        {
            var propertyInfos = obj.GetType().GetProperties();

            try
            {
                var identifier = propertyInfos.SingleOrDefault(pi =>
                                                               pi.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));

                if (identifier == null) return FSharpOption<tResult>.None;
                return ((tResult) identifier.GetValue(obj)).Some();
            }
            catch (Exception)
            {
                return FSharpOption<tResult>.None;
            }
        }

        static FSharpOption<tResult> TryReadField<tObject, tResult>(tObject obj, string fieldName)
        {
            var fieldInfos = obj.GetType().GetFields();

            try
            {
                var identifier = fieldInfos.SingleOrDefault(pi =>
                                                               pi.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase));

                if (identifier == null) return FSharpOption<tResult>.None;
                return ((tResult) identifier.GetValue(obj)).Some();
            }
            catch (Exception)
            {
                return FSharpOption<tResult>.None;
            }
        }

        public static tResult ReadValueOrDefault<tObject, tResult>(tObject obj, string name, tResult defaultValue = default(tResult))
        {
            return TryReadField<tObject, tResult>(obj, name)
                .OrElse<tResult>(TryReadProperty<tObject, tResult>(obj, name))
                .GetOrElse(defaultValue);

        }
    }
}