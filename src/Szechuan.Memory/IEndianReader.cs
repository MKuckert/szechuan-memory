namespace Szechuan.Memory;

public interface IEndianReader
{
    /// <summary>
    /// The byte order of this reader.
    /// </summary>
    ByteOrder ByteOrder { get; }

    /// <summary>
    /// Reads a raw value from its binary representation from the given <paramref name="source"/> span.
    /// </summary>
    /// <exception cref="FormatException">Raised when a value can't be read from the source</exception>
    sbyte ReadSByte(ReadOnlySpan<byte> source);

    /// <summary>
    /// Reads a raw value from its binary representation from the given <paramref name="source"/> span.
    /// </summary>
    /// <exception cref="FormatException">Raised when a value can't be read from the source</exception>
    byte ReadByte(ReadOnlySpan<byte> source);

    /// <summary>
    /// Reads a raw value from its binary representation from the given <paramref name="source"/> span.
    /// </summary>
    /// <exception cref="FormatException">Raised when a value can't be read from the source</exception>
    short ReadInt16(ReadOnlySpan<byte> source);

    /// <summary>
    /// Reads a raw value from its binary representation from the given <paramref name="source"/> span.
    /// </summary>
    /// <exception cref="FormatException">Raised when a value can't be read from the source</exception>
    ushort ReadUInt16(ReadOnlySpan<byte> source);

    /// <summary>
    /// Reads a raw value from its binary representation from the given <paramref name="source"/> span.
    /// </summary>
    /// <exception cref="FormatException">Raised when a value can't be read from the source</exception>
    int ReadInt32(ReadOnlySpan<byte> source);

    /// <summary>
    /// Reads a raw value from its binary representation from the given <paramref name="source"/> span.
    /// </summary>
    /// <exception cref="FormatException">Raised when a value can't be read from the source</exception>
    uint ReadUInt32(ReadOnlySpan<byte> source);

    /// <summary>
    /// Reads a raw value from its binary representation from the given <paramref name="source"/> span.
    /// </summary>
    /// <exception cref="FormatException">Raised when a value can't be read from the source</exception>
    long ReadInt64(ReadOnlySpan<byte> source);

    /// <summary>
    /// Reads a raw value from its binary representation from the given <paramref name="source"/> span.
    /// </summary>
    /// <exception cref="FormatException">Raised when a value can't be read from the source</exception>
    ulong ReadUInt64(ReadOnlySpan<byte> source);

    /// <summary>
    /// Reads a raw value from its binary representation from the given <paramref name="source"/> span.
    /// </summary>
    /// <exception cref="FormatException">Raised when a value can't be read from the source</exception>
    nint ReadNInt(ReadOnlySpan<byte> source);

    /// <summary>
    /// Reads a raw value from its binary representation from the given <paramref name="source"/> span.
    /// </summary>
    /// <exception cref="FormatException">Raised when a value can't be read from the source</exception>
    nuint ReadNUInt(ReadOnlySpan<byte> source);

    /// <summary>
    /// Reads a raw value from its binary representation from the given <paramref name="source"/> span.
    /// </summary>
    /// <exception cref="FormatException">Raised when a value can't be read from the source</exception>
    float ReadFloat(ReadOnlySpan<byte> source);

    /// <summary>
    /// Reads a raw value from its binary representation from the given <paramref name="source"/> span.
    /// </summary>
    /// <exception cref="FormatException">Raised when a value can't be read from the source</exception>
    double ReadDouble(ReadOnlySpan<byte> source);

    /// <summary>
    /// Reads a raw value from its binary representation from the given <paramref name="source"/> span.
    /// </summary>
    /// <exception cref="FormatException">Raised when a value can't be read from the source</exception>
    bool ReadBool(ReadOnlySpan<byte> source);

    /// <summary>
    /// Reads <paramref name="count"/> raw bytes from the given <paramref name="source"/> span.
    /// </summary>
    /// <exception cref="FormatException">Raised when a value can't be read from the source</exception>
    ReadOnlySpan<byte> Read(ReadOnlySpan<byte> source, int count);
}