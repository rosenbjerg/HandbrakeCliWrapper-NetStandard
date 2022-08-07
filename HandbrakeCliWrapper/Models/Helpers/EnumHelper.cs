using HandbrakeCliWrapper.Enums;
using HandbrakeCliWrapper.Models.Extensions;
using System;
using System.Linq;

namespace HandbrakeCliWrapper.Models.Helpers {
    internal static class EnumHelper {
        private static readonly Type[] NoFormatTypes = new Type[] {
            typeof(AudioDither),
            typeof(NlmeansTune),
            typeof(ChromaSmoothTune),
            typeof(UnsharpTune),
            typeof(LapsharpTune),
            typeof(DeblockTune),
        };

        internal static string Format(Enum value) {
            Type valueType = value.GetType();
            string format = value.ToString();
            if (NoFormatTypes.Contains(valueType))
                return format;

            format = format.TrimStart('_');

            bool terminateEarly = false;
            if (valueType == typeof(Mixdown)) {
                format = format.Replace("__", ".");
                terminateEarly = true;
            }
            else if (valueType == typeof(AudioEncoder)) {
                format = format.Replace("__", ":");
                terminateEarly = true;
            }
            else if (valueType == typeof(ColorMatrix)) {
                terminateEarly = true;
            }

            if (terminateEarly)
                return format;

            if (Enum.GetValues(valueType).Cast<object>().AllOrButOne(x => x.ToString().All(y => y == '_' || Char.IsNumber(y))))
                format = format.Replace("_", ".");
            else
                format = format.Replace("_", "-");

            return format;
        }
    }
}