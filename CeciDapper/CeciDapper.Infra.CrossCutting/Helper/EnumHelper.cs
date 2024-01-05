using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace CeciDapper.Infra.CrossCutting.Helper
{
    /// <summary>
    /// Utility class for working with enumerations.
    /// </summary>
    /// <typeparam name="T">The enumeration type.</typeparam>
    [ExcludeFromCodeCoverage]
    public static class EnumHelper<T>
        where T : struct, Enum // This constraint requires C# 7.3 or later.
    {
        /// <summary>
        /// Gets a list of the enumeration values.
        /// </summary>
        /// <param name="value">The enumeration.</param>
        /// <returns>A list of the enumeration values.</returns>
        public static IList<T> GetValues(Enum value)
        {
            var enumValues = new List<T>();

            foreach (FieldInfo fi in value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                enumValues.Add((T)Enum.Parse(value.GetType(), fi.Name, false));
            }
            return enumValues;
        }

        /// <summary>
        /// Parses a string to the corresponding enumeration value.
        /// </summary>
        /// <param name="value">The string to parse.</param>
        /// <returns>The corresponding enumeration value.</returns>
        public static T Parse(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        /// <summary>
        /// Gets a list of the names of the enumeration values.
        /// </summary>
        /// <param name="value">The enumeration.</param>
        /// <returns>A list of the names of the enumeration values.</returns>
        public static IList<string> GetNames(Enum value)
        {
            return value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name).ToList();
        }

        /// <summary>
        /// Gets a list of the display values of the enumeration values.
        /// </summary>
        /// <param name="value">The enumeration.</param>
        /// <returns>A list of the display values of the enumeration values.</returns>
        public static IList<string> GetDisplayValues(Enum value)
        {
            return GetNames(value).Select(obj => GetDisplayValue(Parse(obj))).ToList();
        }

        /// <summary>
        /// Gets the display value of an enumeration value.
        /// </summary>
        /// <param name="value">The enumeration value.</param>
        /// <returns>The display value of the enumeration value.</returns>
        public static string GetDisplayValue(T value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes[0].ResourceType != null)
                return lookupResource(descriptionAttributes[0].ResourceType, descriptionAttributes[0].Name);

            if (descriptionAttributes == null) return string.Empty;
            return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
        }

        /// <summary>
        /// Looks up a resource value using the provided resource manager and resource key.
        /// </summary>
        /// <param name="resourceManagerProvider">The type that provides the resource manager.</param>
        /// <param name="resourceKey">The key of the resource.</param>
        /// <returns>The resource value if found; otherwise, the resource key itself.</returns>
        private static string lookupResource(Type resourceManagerProvider, string resourceKey)
        {
            var resourceKeyProperty = resourceManagerProvider.GetProperty(resourceKey,
                BindingFlags.Static | BindingFlags.Public, null, typeof(string),
                new Type[0], null);
            if (resourceKeyProperty != null)
            {
                return (string)resourceKeyProperty.GetMethod.Invoke(null, null);
            }

            return resourceKey; // Fallback with the key name
        }
    }
}
