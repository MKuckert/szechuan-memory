using System.Runtime.InteropServices;

namespace Szechuan.Memory;

// TODO Write(string, encoding)
public interface IByteWriter
{
    Memory<byte> Memory { get; }
    int Length { get; }

    IByteWriter Write(sbyte value);
    IByteWriter Write(byte value);
    IByteWriter Write(short value);
    IByteWriter Write(ushort value);
    IByteWriter Write(int value);
    IByteWriter Write(uint value);
    IByteWriter Write(long value);
    IByteWriter Write(ulong value);
    IByteWriter Write(nint value);
    IByteWriter Write(nuint value);
    IByteWriter Write(float value);
    IByteWriter Write(double value);
    IByteWriter Write(bool value);
    IByteWriter Write(ReadOnlySpan<byte> value);

    IByteWriter WriteEnum<TEnum>(TEnum value)
        where TEnum : struct, Enum, IConvertible;

    /// <summary>
    /// Writes the given struct into the byte writers underlying memory.
    /// This implementation may ignore the endianness of this writer and fallback to the platform specific endianness.
    ///
    /// The type has to be a struct and can't contain any fields of reference types, e.g. no strings, no arrays, no
    /// objects.
    /// The type also has to be attributed with the <see cref="StructLayoutAttribute"/>. One has to specify either the
    /// layout kind <see cref="LayoutKind.Explicit"/> or <see cref="LayoutKind.Sequential"/> to ensure a properly
    /// defined alignment of all fields.
    /// </summary>
    /// <exception cref="FormatException">The given struct can't be written e.g. because it contains fields of reference
    /// types.</exception>
    /// <exception cref="InsufficientMemoryException">There's not enough memory to write the given struct into the
    /// underlying memory.</exception>
#pragma warning disable CA1045 // It's expected design to have this struct as a ref
    IByteWriter WriteStruct<TStruct>(ref TStruct value)
        where TStruct : struct;
#pragma warning restore CA1045
}