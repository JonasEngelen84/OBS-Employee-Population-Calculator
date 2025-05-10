using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Employee_Population_Calculator.App.Extensions
{
    /// <summary>
    /// Static extension class that deals with custom String manipulation.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Interpolates a given string value using a Lamda Expression without a name validation.
        /// Meaning that every instance {...} is interpolated with the given object regardless of construct.
        /// </summary>
        /// <param name="value">The string value to interpolate in.</param>
        /// <param name="object">The object to interpolate with.</param>
        /// <param name="uri">Indicates if the result should be processed as a uri element.</param>
        /// <returns>The interpolated string value.</returns>
        public static string Interpolate(this string value, object @object, bool uri = true)
        {
            return value.Interpolate("", @object, uri);
        }

        /// <summary>
        /// Interpolates a given string value using a Lamda Expression with name validation.
        /// Only replaces instances of {...} with a interpolated value from the given object
        /// if and only if ... starts with the given name. As such this replaces only instances
        /// who are a liking to {name....}.
        ///
        /// If only the name is found with in the brackets, then ToString() is assumed to be invoked.
        /// As such {name} is equivalent to {name.ToString()}.
        /// </summary>
        /// <param name="value">The string value to interpolate in.</param>
        /// <param name="name">The name of the interpolation to perform (the interpolation prefix)</param>
        /// <param name="object">The object to interpolate.</param>
        /// <param name="uri">Indicates if the result should be processed as a uri element.</param>
        /// <returns>The interpolated string value.</returns>
        public static string Interpolate(this string value, string name, object @object, bool uri = true)
        {
            return Regex.Replace(value, $"{{({name}.*?)}}",
                match => {
                    var p = Expression.Parameter(@object.GetType(), name.Any() ? name.Replace(".", "_") : @object.GetType().Name);

                    var lamdaContents = match.Groups[1].Value;
                    if (!lamdaContents.Any())
                        lamdaContents = p.Name;

                    lamdaContents = lamdaContents.Replace(name, p.Name);

                    var e = System.Linq.Dynamic.Core.DynamicExpressionParser.ParseLambda(new[] { p }, null, lamdaContents);
                    var result = (e.Compile().DynamicInvoke(@object) ?? "").ToString();

                    if (uri)
                        result = result.ToUriArg();

                    return result;
                });
        }

        /// <summary>
        /// Escapes a string in a way so that it can be used inside a parameter of a URL or URI.
        /// </summary>
        /// <param name="value">The string to escape.</param>
        /// <returns>The escaped string.</returns>
        public static string ToUriArg(this string value)
        {
            return Uri.EscapeDataString(value);
        }

        /// <summary>
        /// Interpolates a given string value using a Lamda Expression with the names as the keys in the given dictionary.
        /// Only replaces instances of {...} with a interpolated value from the given key value pair in the dictionary
        /// if and only if ... starts with the given key. As such this replaces only instances
        /// who are a liking to {key....}.
        /// </summary>
        /// <param name="value">The string value to interpolate in.</param>
        /// <param name="args">The objects to interpolate with.</param>
        /// <param name="uri">Indicates if the result should be processed as a uri element.</param>
        /// <returns>The interpolated string value.</returns>
        public static string InterpolateAll(this string value, IDictionary<string, object> args, bool uri = true)
        {
            return args.Aggregate(value, (current, keyValuePair) => current.Interpolate(keyValuePair.Key, keyValuePair.Value, uri));
        }
    }
}
