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

namespace Enbrea.Konsoli.Internals;

/// <summary>
/// Represents a file size.
/// </summary>
internal readonly struct FileSize : IEquatable<FileSize>
{
    private const long BytesPerKilobyte = 1024L;
    private const long BytesPerMegabyte = BytesPerKilobyte * 1024L;
    private const long BytesPerGigabyte = BytesPerMegabyte * 1024L;
    private const long BytesPerTerabyte = BytesPerGigabyte * 1024L;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileSize"/> struct.
    /// </summary>
    /// <param name="value">The file size in bytes.</param>
    public FileSize(long value)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(value);

        Value = value;
    }

    /// <summary>
    /// Gets the raw file size in bytes.
    /// </summary>
    public long Value { get; }

    public static implicit operator FileSize(long value)
    {
        return new FileSize(value);
    }

    public static implicit operator long(FileSize fileSize)
    {
        return fileSize.Value;
    }

    public static bool operator !=(FileSize left, FileSize right)
    {
        return !left.Equals(right);
    }

    public static bool operator ==(FileSize left, FileSize right)
    {
        return left.Equals(right);
    }

    public bool Equals(FileSize other)
    {
        return Value == other.Value;
    }

    public override bool Equals(object obj)
    {
        return obj is FileSize other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    /// <summary>
    /// Returns the formatted file size using the supplied localized strings.
    /// </summary>
    /// <param name="strings">The strings used for formatting file-size units.</param>
    /// <returns>The formatted file size.</returns>
    public string ToString(ConsoleWriterStrings strings)
    {
        return ToString(FileSizeUnit.Auto, strings);
    }

    /// <summary>
    /// Returns the formatted file size using the supplied localized strings.
    /// </summary>
    /// <param name="unit">The file size formatting mode.</param>
    /// <param name="strings">The strings used for formatting file-size units.</param>
    /// <returns>The formatted file size.</returns>
    public string ToString(FileSizeUnit unit, ConsoleWriterStrings strings)
    {
        ArgumentNullException.ThrowIfNull(strings);

        return unit switch
        {
            FileSizeUnit.Auto => GenerateAutoString(strings),
            FileSizeUnit.BytesOnly => $"{Value} {strings.Bytes}",
            _ => throw new ArgumentOutOfRangeException(nameof(unit))
        };
    }

    /// <summary>
    /// Formats the file size value with the specified divisor and unit.
    /// </summary>
    private static string Format(long value, long divisor, string unit)
    {
        return $"{decimal.Divide(value, divisor):0.##} {unit}";
    }

    /// <summary>
    /// Generates a string representation using the most appropriate unit.
    /// </summary>
    private string GenerateAutoString(ConsoleWriterStrings strings)
    {
        if (Value >= BytesPerTerabyte)
        {
            return Format(Value, BytesPerTerabyte, strings.TB);
        }

        if (Value >= BytesPerGigabyte)
        {
            return Format(Value, BytesPerGigabyte, strings.GB);
        }

        if (Value >= BytesPerMegabyte)
        {
            return Format(Value, BytesPerMegabyte, strings.MB);
        }

        if (Value >= BytesPerKilobyte)
        {
            return Format(Value, BytesPerKilobyte, strings.KB);
        }

        return $"{Value} {strings.Bytes}";
    }
}