#region ENBREA Konsoli - Copyright (C) STÜBER SYSTEMS GmbH
/*    
 *    ENBREA Konsoli 
 *    
 *    Copyright (C) STÜBER SYSTEMS GmbH
 *
 *    Licensed under the MIT License, Version 2.0. 
 * 
 */
#endregion

using System;

namespace Enbrea.Konsoli
{
    /// <summary>
    /// Represents a file size
    /// </summary>
    public struct FileSize
    {
        /// <summary>
        /// The raw file size value in Bytes
        /// </summary>
        public long Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSize"/> class.
        /// </summary>
        /// <param name="value">Initial value in Bytes</param>
        public FileSize(long value)
        {
            Value = value;
        }

        /// <summary>
        /// Long to FileSize operator
        /// </summary>
        /// <param name="value">Value in Bytes</param>
        public static implicit operator FileSize(long value)
        {
            return new FileSize(value);
        }

        /// <summary>
        /// FileSize to long operator
        /// </summary>
        /// <param name="fs">File size</param>
        public static implicit operator long(FileSize fs)
        {
            return fs.Value;
        }

        /// <summary>
        /// Equality operator for FileSize values 
        /// </summary>
        /// <param name="fs1">First file size</param>
        /// <param name="fs2">Second file size</param>
        /// <returns>True if not equal</returns>
        public static bool operator !=(FileSize fs1, FileSize fs2)
        {
            return !(fs1 == fs2);
        }

        /// <summary>
        /// Equality operator for FileSize values 
        /// </summary>
        /// <param name="fs1">First file size</param>
        /// <param name="fs2">Second file size</param>
        /// <returns>True if equal</returns>
        public static bool operator ==(FileSize fs1, FileSize fs2)
        {
            return (fs1.Value == fs2.Value);
        }

        /// <summary>
        /// Comparing this instance with another one
        /// </summary>
        /// <param name="obj">Another instance</param>
        /// <returns>True if equal</returns>
        public override bool Equals(object obj)
        {
            if (obj is FileSize)
            {
                return ((FileSize)obj) == this;
            }
            return base.Equals(obj);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>
        /// Displays file size as string
        /// </summary>
        /// <returns>File size as string</returns>
        public override string ToString()
        {
            return ToString(FileSizeUnit.Auto);
        }

        /// <summary>
        /// Displays the file size as string 
        /// </summary>
        /// <param name="unit">File size unit</param>
        /// <returns>File size as string</returns>
        /// <remarks>
        /// Code adapted from: https://lonewolfonline.net/format-number-kb-mb-gb/
        /// </remarks>
        public string ToString(FileSizeUnit unit)
        {
            if (unit == FileSizeUnit.Auto)
            {
                const int scale = 1024;
                string[] orders = new string[] { Strings.TB, Strings.GB, Strings.MB, Strings.KB, Strings.Bytes };
                long max = (long)Math.Pow(scale, orders.Length - 1);

                foreach (var order in orders)
                {
                    if (Value > max)
                    {
                        return string.Format("{0:##.##} {1}", decimal.Divide(Value, max), order);
                    }
                    max /= scale;
                }
                return string.Format("0 {0}", Strings.Bytes);
            }
            else
            {
                return string.Format("{0:##.##} {1}", Value, Strings.Bytes);
            }
        }
    }
}