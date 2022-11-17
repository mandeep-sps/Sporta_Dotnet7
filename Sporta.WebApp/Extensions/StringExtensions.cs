using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sporta.WebApp.Extensions
{
    /// <summary>
    /// String Extensions
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Trim And Lower
        /// </summary>
        public static string TrimAndLower(this string str)
        {
            return str.Trim().ToLower();
        }

        /// <summary>
        /// Truncates a string to a specified max length
        /// </summary>
        public static string Truncate(this string str, int maxLength = 240)
        {
            return !string.IsNullOrEmpty(str) ? str.Substring(0, Math.Min(str.Length, maxLength)) : str;
        }

        /// <summary>
        /// Converts json to a specified type T
        /// </summary>
        /// <returns>An object of type T deserialized from a json string.</returns>
        public static T FromJson<T>(this string json) where T : class, new()
        {
            if (json != null)
                try
                {
                    return JsonConvert.DeserializeObject<T>(json);
                }
                catch (Exception)
                {
                    throw new JsonSerializationException($"An error occured attempting to deserialize to {typeof(T)}.");
                }

            return new T();
        }

        /// <summary>
        /// Converts json to a specified type T
        /// </summary>
        /// <returns>An object of type T deserialized from a json string.</returns>
        public static IEnumerable<T> FromJsonArray<T>(this string json) where T : class, new()
        {
            if (json != null)
                try
                {
                    return JsonConvert.DeserializeObject<IEnumerable<T>>(json);
                }
                catch (Exception)
                {
                    throw new JsonSerializationException($"An error occured attempting to deserialize to {typeof(T)}.");
                }

            return Enumerable.Empty<T>();
        }

        /// <summary>
        /// Extends a Stored Procedure call to contain the number of parameters
        /// </summary>
        /// <returns>A formatted Stored Procedure string with specified number parameters.</returns>
        public static string ExtendParameters(this string storedProc, int numberOfParameters = 1)
        {
            return
                $"EXECUTE dbo.{storedProc} {string.Join(", ", Enumerable.Range(0, numberOfParameters).Select(param => $"{{{param}}}"))}";
        }
    }
}
