using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Romontinka.Server.Core.Context
{
    /// <summary>
    /// Строковые расширения.
    /// Способ иенной замены в строках был взят с сайта http://mo.notono.us/2008/07/c-stringinject-format-strings-by-key.html
    /// </summary>
    public static class StringExtention
    {
        /// <summary>
        /// Extension method that replaces keys in a string with the values of matching object properties.
        /// <remarks>Uses <see cref="string.Format()"/> internally; custom formats should match those used for that method.</remarks>
        /// </summary>
        /// <param name="formatString">The format string, containing keys like {foo} and {foo:SomeFormat}.</param>
        /// <param name="injectionObject">The object whose properties should be injected in the string</param>
        /// <returns>A version of the formatString string with keys replaced by (formatted) key values.</returns>
        public static string Inject(this string formatString, object injectionObject)
        {
            return formatString.Inject(GetPropertyHash(injectionObject));
        }

        /// <summary>
        /// Extension method that replaces keys in a string with the values of matching dictionary entries.
        /// <remarks>Uses <see cref="string.Format()"/> internally; custom formats should match those used for that method.</remarks>
        /// </summary>
        /// <param name="formatString">The format string, containing keys like {foo} and {foo:SomeFormat}.</param>
        /// <param name="dictionary">An <see cref="IDictionary"/> with keys and values to inject into the string</param>
        /// <returns>A version of the formatString string with dictionary keys replaced by (formatted) key values.</returns>
        public static string Inject(this string formatString, IDictionary dictionary)
        {
            return formatString.Inject(new Hashtable(dictionary));
        }

        /// <summary>
        /// Extension method that replaces keys in a string with the values of matching dictionary entries.
        /// <remarks>Uses <see cref="string.Format()"/> internally; custom formats should match those used for that method.</remarks>
        /// </summary>
        /// <param name="formatString">The format string, containing keys like {foo} and {foo:SomeFormat}.</param>
        /// <param name="dictionary">An <see cref="IDictionary"/> with keys and values to inject into the string</param>
        /// <returns>A version of the formatString string with dictionary keys replaced by (formatted) key values.</returns>
        public static object InjectValue(this string formatString, IDictionary dictionary)
        {
            if (string.IsNullOrWhiteSpace(formatString))
            {
                return null;
            } //if

            formatString = formatString.Trim();

            if (formatString.StartsWith("{") && formatString.EndsWith("}"))
            {
                var key = formatString.Trim(new[] {'{', '}'});
                if (!key.Contains("{"))
                {
                    if (dictionary.Contains(key))
                    {
                        return dictionary[key];
                    } //if
                } //if
            } //if

            return Inject(formatString, new Hashtable(dictionary));
        }

        /// <summary>
        /// Contaits the keyword "else"
        /// </summary>
        private const string ElseCondition = "else";

        /// <summary>
        /// Contains the keyword "EmptyOrNull"
        /// </summary>
        private const string EmptyOrNullCondition = "EmptyOrNull";

        /// <summary>
        /// Extension method that replaces keys in a string with the values of matching hashtable entries.
        /// <remarks>Uses <see cref="string.Format()"/> internally; custom formats should match those used for that method.</remarks>
        /// </summary>
        /// <param name="formatString">The format string, containing keys like {foo} and {foo:SomeFormat}.</param>
        /// <param name="attributes">A <see cref="Hashtable"/> with keys and values to inject into the string</param>
        /// <returns>A version of the formatString string with hastable keys replaced by (formatted) key values.</returns>
        public static string Inject(this string formatString, Hashtable attributes)
        {
            string result = formatString;
            if (attributes == null || formatString == null)
            {
                return result;
            }
            //TODO: inject culture definition
            //regex replacement of key with value, where the generic key format is:
            
            Regex attributeRegex = new Regex("{(\\w+)(?:}|(?::(.[^}]*)}))");
            
            //for \\w+ = foo, matches {foo} and {foo:SomeFormat}

            //loop through matches, since each key may be used more than once (and with a different format string)
            foreach (Match m in attributeRegex.Matches(formatString))
            {
                if (m.Groups.Count < 2)
                {
                    continue;
                } //if

                string replacement = m.ToString();
                object replacementValue = string.Empty;
                if (attributes.ContainsKey(m.Groups[1].Value))
                {
                    replacementValue = attributes[m.Groups[1].Value];
                } //if
                else
                {
                    attributes[m.Groups[1].Value]=null;
                } //else

                if (m.Groups[2].Length > 0) //matched {foo:SomeFormat} or {foo:1:value1|2:value}
                {
                    var groupSource = m.Groups[2].Value;
                    if (groupSource.Contains("="))
                    {
                        var replacementRegex = new Regex("(\\w+)\\=([\\w,\\s,\'\",/,\\,\\[,\\],]*)");
                        IDictionary<string,string> values = new Dictionary<string, string>();
                        
                        foreach (Match replacementGroup in replacementRegex.Matches(groupSource))
                        {
                            if (replacementGroup.Groups.Count<2)
                            {
                                continue;
                            } //if
                            values[replacementGroup.Groups[1].Value]= replacementGroup.Groups[2].Value;

                        } //foreach
                        var value = replacementValue == null ? string.Empty : replacementValue.ToString();
                        if (values.ContainsKey(value))
                        {
                            replacement = values[value];
                        } //if
                        else if (string.IsNullOrWhiteSpace(value) && values.ContainsKey(EmptyOrNullCondition))
                        {
                            replacement = values[EmptyOrNullCondition];
                        }
                        else if (values.ContainsKey(ElseCondition))
                        {
                            replacement = values[ElseCondition];
                        } //if
                    } //if
                    else
                    {
                        //do a double string.Format - first to build the proper format string, and then to format the replacement value
                        string attributeFormatString = string.Format(CultureInfo.InvariantCulture, "{{0:{0}}}",
                                                                     m.Groups[2]);
                        replacement = string.Format(CultureInfo.CurrentCulture, attributeFormatString, replacementValue);
                    } //else
                }
                else //matched {foo}
                {
                    replacement = replacementValue == null ? string.Empty : replacementValue.ToString();
                }
                
                //perform replacements, one match at a time
                if (replacement.Contains("[") && replacement.Contains("]"))
                {
                    replacement = replacement.Replace("[","{").Replace("]","}").Inject(attributes);
                } //if
                result = result.Replace(m.ToString(), replacement); //attributeRegex.Replace(result, replacement, 1);
            }
            return result;
        }

        /// <summary>
        /// Creates a HashTable based on current object state.
        /// <remarks>Copied from the MVCToolkit HtmlExtensionUtility class</remarks>
        /// </summary>
        /// <param name="properties">The object from which to get the properties</param>
        /// <returns>A <see cref="Hashtable"/> containing the object instance's property names and their values</returns>
        private static Hashtable GetPropertyHash(object properties)
        {
            Hashtable values = null;
            if (properties != null)
            {
                values = new Hashtable();
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(properties);
                foreach (PropertyDescriptor prop in props)
                {
                    values.Add(prop.Name, prop.GetValue(properties));
                }
            }
            return values;
        }

        /// <summary>
        /// Gets a substring whole word ended.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <param name="size">The wanted size.</param>
        /// <returns>The string.</returns>
        public static string LeftWord(this string value, int size)
        {
            if (string.IsNullOrWhiteSpace(value) || size<=0 || size>=value.Length)
            {
                return value;
            } //if

            int index = size;

            while (!char.IsWhiteSpace(value[index]) || index<=0)
            {
                index--;
            } //while

            if (index<=0)
            {
                index = size;
            } //if

            return value.Substring(0, index);
        }
    }
}