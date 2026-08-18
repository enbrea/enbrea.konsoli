#region Enbrea.Konsoli - Copyright (C) STÜBER SYSTEMS GmbH
/*    
 *    Enbrea.Konsoli 
 *    
 *    Copyright (C) STÜBER SYSTEMS GmbH
 *
 *    Licensed under the MIT License.
 * 
 */
#endregion

namespace Enbrea.Konsoli
{
    /// <summary>
    /// Extensions for type <see cref="string"/>
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Cuts or pads a string to a given width.
        /// </summary>
        /// <param name="value">The original string value</param>
        /// <param name="totalWidth">The width of the resulting string</param>
        /// <returns>Gives back a string with a length equal to totalWidth.</returns>
        public static string CutOrPadRight(this string value, int totalWidth)
        {
            if (totalWidth > 0)
            {
                if (value.Length > totalWidth)
                {
                    if (totalWidth > 3)
                    {
                        return $"{value.Substring(0, totalWidth - 3)}...";
                    }
                    else
                    {
                        return new string('.', totalWidth);
                    }
                }
                else
                {
                    return value.PadRight(totalWidth);
                }
            }
            else
            {
                return value;
            }
        }
    }
}
