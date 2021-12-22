namespace Szechuan.Memory;

// TODO IEndianness?
public interface IEndianWriter
{
    /// <summary>
    /// Writes a binary representation of the given <paramref name="value"/> into the <paramref name="destination"/> span.
    /// </summary>
    /// <exception cref="FormatException">Raised when value can't be written to destination</exception>
    public void Write(Span<byte> destination, sbyte value);

    /// <summary>
    /// Writes a binary representation of the given <paramref name="value"/> into the <paramref name="destination"/> span.
    /// </summary>
    /// <exception cref="FormatException">Raised when value can't be written to destination</exception>
    public void Write(Span<byte> destination, byte value);

    /// <summary>
    /// Writes a binary representation of the given <paramref name="value"/> into the <paramref name="destination"/> span.
    /// </summary>
    /// <exception cref="FormatException">Raised when value can't be written to destination</exception>
    public void Write(Span<byte> destination, short value);

    /// <summary>
    /// Writes a binary representation of the given <paramref name="value"/> into the <paramref name="destination"/> span.
    /// </summary>
    /// <exception cref="FormatException">Raised when value can't be written to destination</exception>
    public void Write(Span<byte> destination, ushort value);

    /// <summary>
    /// Writes a binary representation of the given <paramref name="value"/> into the <paramref name="destination"/> span.
    /// </summary>
    /// <exception cref="FormatException">Raised when value can't be written to destination</exception>
    public void Write(Span<byte> destination, int value);

    /// <summary>
    /// Writes a binary representation of the given <paramref name="value"/> into the <paramref name="destination"/> span.
    /// </summary>
    /// <exception cref="FormatException">Raised when value can't be written to destination</exception>
    public void Write(Span<byte> destination, uint value);

    /// <summary>
    /// Writes a binary representation of the given <paramref name="value"/> into the <paramref name="destination"/> span.
    /// </summary>
    /// <exception cref="FormatException">Raised when value can't be written to destination</exception>
    public void Write(Span<byte> destination, long value);

    /// <summary>
    /// Writes a binary representation of the given <paramref name="value"/> into the <paramref name="destination"/> span.
    /// </summary>
    /// <exception cref="FormatException">Raised when value can't be written to destination</exception>
    public void Write(Span<byte> destination, ulong value);

    /// <summary>
    /// Writes a binary representation of the given <paramref name="value"/> into the <paramref name="destination"/> span.
    /// </summary>
    /// <exception cref="FormatException">Raised when value can't be written to destination</exception>
    public void Write(Span<byte> destination, nint value);

    /// <summary>
    /// Writes a binary representation of the given <paramref name="value"/> into the <paramref name="destination"/> span.
    /// </summary>
    /// <exception cref="FormatException">Raised when value can't be written to destination</exception>
    public void Write(Span<byte> destination, nuint value);

    /// <summary>
    /// Writes a binary representation of the given <paramref name="value"/> into the <paramref name="destination"/> span.
    /// </summary>
    /// <exception cref="FormatException">Raised when value can't be written to destination</exception>
    public void Write(Span<byte> destination, float value);

    /// <summary>
    /// Writes a binary representation of the given <paramref name="value"/> into the <paramref name="destination"/> span.
    /// </summary>
    /// <exception cref="FormatException">Raised when value can't be written to destination</exception>
    public void Write(Span<byte> destination, double value);

    /// <summary>
    /// Writes a binary representation of the given <paramref name="value"/> into the <paramref name="destination"/> span.
    /// </summary>
    /// <exception cref="FormatException">Raised when value can't be written to destination</exception>
    public void Write(Span<byte> destination, bool value);
}