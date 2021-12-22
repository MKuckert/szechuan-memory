using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Szechuan.Memory;

internal sealed class MemoryByteWriter : IByteWriter, IMemoryHolder
{
    private readonly Memory<byte> memory;
    private readonly IEndianWriter endianWriter;
    private Memory<byte> buffer;

    public MemoryByteWriter(Memory<byte> memory, IEndianWriter endianWriter)
    {
        this.memory = buffer = memory;
        this.endianWriter = endianWriter;
    }

    public IByteWriter Write(sbyte value)
    {
        const int SIZE = sizeof(sbyte);
        EnsureBufferSize(SIZE);
        endianWriter.Write(buffer.Span, value);
        Advance(SIZE);
        return this;
    }

    public IByteWriter Write(byte value)
    {
        const int SIZE = sizeof(byte);
        EnsureBufferSize(SIZE);
        endianWriter.Write(buffer.Span, value);
        Advance(SIZE);
        return this;
    }

    public IByteWriter Write(short value)
    {
        const int SIZE = sizeof(short);
        EnsureBufferSize(SIZE);
        endianWriter.Write(buffer.Span, value);
        Advance(SIZE);
        return this;
    }

    public IByteWriter Write(ushort value)
    {
        const int SIZE = sizeof(ushort);
        EnsureBufferSize(SIZE);
        endianWriter.Write(buffer.Span, value);
        Advance(SIZE);
        return this;
    }

    public IByteWriter Write(int value)
    {
        const int SIZE = sizeof(int);
        EnsureBufferSize(SIZE);
        endianWriter.Write(buffer.Span, value);
        Advance(SIZE);
        return this;
    }

    public IByteWriter Write(uint value)
    {
        const int SIZE = sizeof(uint);
        EnsureBufferSize(SIZE);
        endianWriter.Write(buffer.Span, value);
        Advance(SIZE);
        return this;
    }

    public IByteWriter Write(long value)
    {
        const int SIZE = sizeof(long);
        EnsureBufferSize(SIZE);
        endianWriter.Write(buffer.Span, value);
        Advance(SIZE);
        return this;
    }

    public IByteWriter Write(ulong value)
    {
        const int SIZE = sizeof(ulong);
        EnsureBufferSize(SIZE);
        endianWriter.Write(buffer.Span, value);
        Advance(SIZE);
        return this;
    }

    public IByteWriter Write(nint value)
    {
        // We're using IntPtr.Size instead of sizeof(nint) as the later one is unsafe code
        var size = IntPtr.Size;
        EnsureBufferSize(size);
        endianWriter.Write(buffer.Span, value);
        Advance(size);
        return this;
    }

    public IByteWriter Write(nuint value)
    {
        // We're using UIntPtr.Size instead of sizeof(nuint) as the later one is unsafe code
        var size = UIntPtr.Size;
        EnsureBufferSize(size);
        endianWriter.Write(buffer.Span, value);
        Advance(size);
        return this;
    }

    public IByteWriter Write(float value)
    {
        const int SIZE = sizeof(float);
        EnsureBufferSize(SIZE);
        endianWriter.Write(buffer.Span, value);
        Advance(SIZE);
        return this;
    }

    public IByteWriter Write(double value)
    {
        const int SIZE = sizeof(double);
        EnsureBufferSize(SIZE);
        endianWriter.Write(buffer.Span, value);
        Advance(SIZE);
        return this;
    }

    public IByteWriter Write(bool value)
    {
        const int SIZE = sizeof(bool);
        EnsureBufferSize(SIZE);
        endianWriter.Write(buffer.Span, value);
        Advance(SIZE);
        return this;
    }

    public IByteWriter Write(ReadOnlySpan<byte> value)
    {
        var size = value.Length;
        EnsureBufferSize(size);
        if (!value.TryCopyTo(buffer.Span))
        {
            throw new FormatException($"Can't write given span. Failed to copy to destination buffer");
        }

        Advance(size);
        return this;
    }

    public IByteWriter WriteEnum<TEnum>(TEnum value)
        where TEnum : struct, Enum, IConvertible
    {
        var underlyingType = Enum.GetUnderlyingType(typeof(TEnum));
        if (underlyingType == typeof(sbyte))
        {
            return Write(value.ToSByte(null));
        }

        if (underlyingType == typeof(byte))
        {
            return Write(value.ToByte(null));
        }

        if (underlyingType == typeof(short))
        {
            return Write(value.ToInt16(null));
        }

        if (underlyingType == typeof(ushort))
        {
            return Write(value.ToUInt16(null));
        }

        if (underlyingType == typeof(int))
        {
            return Write(value.ToInt32(null));
        }

        if (underlyingType == typeof(uint))
        {
            return Write(value.ToUInt32(null));
        }

        if (underlyingType == typeof(long))
        {
            return Write(value.ToInt64(null));
        }

        if (underlyingType == typeof(ulong))
        {
            return Write(value.ToUInt64(null));
        }

        if (underlyingType == typeof(nint))
        {
            return Write((nint)(object)value);
        }

        if (underlyingType == typeof(nuint))
        {
            return Write((nuint)(object)value);
        }

        throw new FormatException($"Can't convert given enum {typeof(TEnum).Name} value {value}: Unknown underlying type {underlyingType.Name}");
    }

    public IByteWriter WriteStruct<TStruct>(ref TStruct value)
        where TStruct : struct
    {
        var type = typeof(TStruct);
        if (RuntimeHelpers.IsReferenceOrContainsReferences<TStruct>())
        {
            throw new FormatException($"Can't convert struct {type.Name}. The type contains reference types");
        }

        // One would like to verify the StructLayoutAttribute here but it's not part of the assembly anymore.
        var size = Unsafe.SizeOf<TStruct>();
        EnsureBufferSize(size);

        MemoryMarshal.Write(buffer.Span, ref value);

        Advance(size);
        return this;
    }

    private void EnsureBufferSize(int expectedSize)
    {
        if (buffer.Length < expectedSize)
        {
            throw new InsufficientMemoryException($"Can't write a given value. Expected to have at least {expectedSize} more bytes but there are only {buffer.Length} bytes left");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Advance(int by)
        => buffer = buffer[by..];

    public int Length
        => memory.Length - buffer.Length;

    public Memory<byte> Memory
    {
        get
        {
            var length = Length;
            if (length == 0)
            {
                return Memory<byte>.Empty;
            }

            return memory[..length];
        }
    }
}