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

namespace Enbrea.Konsoli;

/// <summary>
/// Represents a file size.
/// </summary>
public readonly struct FileSize : IEquatable<FileSize>
{
    private const long BytesPerGigabyte = BytesPerMegabyte * 1024L;
    private const long BytesPerKilobyte = 1024L;
    private const long BytesPerMegabyte = BytesPerKilobyte * 1024L;
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

    /// <summary>
    /// Converts a byte value to a <see cref="FileSize"/>.
    /// </summary>
    /// <param name="value">The file size in bytes.</param>
    public static implicit operator FileSize(long value)
    {
        return new FileSize(value);
    }

    /// <summary>
    /// Converts a <see cref="FileSize"/> to its raw byte value.
    /// </summary>
    /// <param name="fileSize">The file size.</param>
    public static implicit operator long(FileSize fileSize)
    {
        return fileSize.Value;
    }

    /// <summary>
    /// Determines whether two file sizes are not equal.
    /// </summary>
    public static bool operator !=(FileSize left, FileSize right)
    {
        return !left.Equals(right);
    }

    /// <summary>
    /// Determines whether two file sizes are equal.
    /// </summary>
    public static bool operator ==(FileSize left, FileSize right)
    {
        return left.Equals(right);
    }

    /// <inheritdoc />
    public bool Equals(FileSize other)
    {
        return Value == other.Value;
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
        return obj is FileSize other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    /// <summary>
    /// Returns the file size using the most appropriate unit.
    /// </summary>
    /// <returns>The formatted file size.</returns>
    public override string ToString()
    {
        return ToString(FileSizeUnit.Auto);
    }

    /// <summary>
    /// Returns the formatted file size.
    /// </summary>
    /// <param name="unit">The file size formatting mode.</param>
    /// <returns>The formatted file size.</returns>
    public string ToString(FileSizeUnit unit)
    {
        return unit switch
        {
            FileSizeUnit.Auto => GenerateAutoString(),
            FileSizeUnit.BytesOnly => $"{Value} {Strings.Bytes}",
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
    /// Generates a string representation of the file size using the most appropriate unit (Bytes, KB, MB, GB, TB).
    /// </summary>
    private string GenerateAutoString()
    {
        if (Value >= BytesPerTerabyte)
        {
            return Format(Value, BytesPerTerabyte, Strings.TB);
        }

        if (Value >= BytesPerGigabyte)
        {
            return Format(Value, BytesPerGigabyte, Strings.GB);
        }

        if (Value >= BytesPerMegabyte)
        {
            return Format(Value, BytesPerMegabyte, Strings.MB);
        }

        if (Value >= BytesPerKilobyte)
        {
            return Format(Value, BytesPerKilobyte, Strings.KB);
        }

        return $"{Value} {Strings.Bytes}";
    }
}