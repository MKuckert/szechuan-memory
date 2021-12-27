using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Szechuan.Memory;

internal sealed class MemoryByteReader : IByteReader
{
    private readonly Memory<byte> memory;
    private readonly IEndianReader endianReader;
    private Memory<byte> buffer;

    public MemoryByteReader(Memory<byte> memory, IEndianReader endianReader)
    {
        this.memory = buffer = memory;
        this.endianReader = endianReader;
    }

    public sbyte ReadSByte()
    {
        const int SIZE = sizeof(sbyte);
        EnsureBufferSize(SIZE);
        var result = endianReader.ReadSByte(buffer.Span);
        Advance(SIZE);
        return result;
    }

    public byte ReadByte()
    {
        const int SIZE = sizeof(byte);
        EnsureBufferSize(SIZE);
        var result = endianReader.ReadByte(buffer.Span);
        Advance(SIZE);
        return result;
    }

    public short ReadInt16()
    {
        const int SIZE = sizeof(short);
        EnsureBufferSize(SIZE);
        var result = endianReader.ReadInt16(buffer.Span);
        Advance(SIZE);
        return result;
    }

    public ushort ReadUInt16()
    {
        const int SIZE = sizeof(ushort);
        EnsureBufferSize(SIZE);
        var result = endianReader.ReadUInt16(buffer.Span);
        Advance(SIZE);
        return result;
    }

    public int ReadInt32()
    {
        const int SIZE = sizeof(int);
        EnsureBufferSize(SIZE);
        var result = endianReader.ReadInt32(buffer.Span);
        Advance(SIZE);
        return result;
    }

    public uint ReadUInt32()
    {
        const int SIZE = sizeof(uint);
        EnsureBufferSize(SIZE);
        var result = endianReader.ReadUInt32(buffer.Span);
        Advance(SIZE);
        return result;
    }

    public long ReadInt64()
    {
        const int SIZE = sizeof(long);
        EnsureBufferSize(SIZE);
        var result = endianReader.ReadInt64(buffer.Span);
        Advance(SIZE);
        return result;
    }

    public ulong ReadUInt64()
    {
        const int SIZE = sizeof(ulong);
        EnsureBufferSize(SIZE);
        var result = endianReader.ReadUInt64(buffer.Span);
        Advance(SIZE);
        return result;
    }

    public nint ReadNInt()
    {
        // We're using IntPtr.Size instead of sizeof(nint) as the later one is unsafe code
        var size = IntPtr.Size;
        EnsureBufferSize(size);
        var result = endianReader.ReadNInt(buffer.Span);
        Advance(size);
        return result;
    }

    public nuint ReadNUInt()
    {
        // We're using UIntPtr.Size instead of sizeof(nuint) as the later one is unsafe code
        var size = UIntPtr.Size;
        EnsureBufferSize(size);
        var result = endianReader.ReadNUInt(buffer.Span);
        Advance(size);
        return result;
    }

    public float ReadFloat()
    {
        const int SIZE = sizeof(float);
        EnsureBufferSize(SIZE);
        var result = endianReader.ReadFloat(buffer.Span);
        Advance(SIZE);
        return result;
    }

    public double ReadDouble()
    {
        const int SIZE = sizeof(double);
        EnsureBufferSize(SIZE);
        var result = endianReader.ReadDouble(buffer.Span);
        Advance(SIZE);
        return result;
    }

    public bool ReadBool()
    {
        const int SIZE = sizeof(bool);
        EnsureBufferSize(SIZE);
        var result = endianReader.ReadBool(buffer.Span);
        Advance(SIZE);
        return result;
    }

    public ReadOnlySpan<byte> Read(int count)
    {
        EnsureBufferSize(count);
        var result = endianReader.Read(buffer.Span, count);
        Advance(count);
        return result;
    }

    public TEnum ReadEnum<TEnum>()
        where TEnum : struct, Enum, IConvertible
    {
        var type = typeof(TEnum);
        var underlyingType = Enum.GetUnderlyingType(type);
        if (underlyingType == typeof(sbyte))
        {
            return (TEnum)Enum.ToObject(type, ReadSByte());
        }

        if (underlyingType == typeof(byte))
        {
            return (TEnum)Enum.ToObject(type, ReadByte());
        }

        if (underlyingType == typeof(short))
        {
            return (TEnum)Enum.ToObject(type, ReadInt16());
        }

        if (underlyingType == typeof(ushort))
        {
            return (TEnum)Enum.ToObject(type, ReadUInt16());
        }

        if (underlyingType == typeof(int))
        {
            return (TEnum)Enum.ToObject(type, ReadInt32());
        }

        if (underlyingType == typeof(uint))
        {
            return (TEnum)Enum.ToObject(type, ReadUInt32());
        }

        if (underlyingType == typeof(long))
        {
            return (TEnum)Enum.ToObject(type, ReadInt64());
        }

        if (underlyingType == typeof(ulong))
        {
            return (TEnum)Enum.ToObject(type, ReadUInt64());
        }

        if (underlyingType == typeof(nint))
        {
            return (TEnum)(object)ReadNInt();
        }

        if (underlyingType == typeof(nuint))
        {
            return (TEnum)(object)ReadNUInt();
        }

        throw new FormatException($"Can't read enum {type.Name} value from source: Unknown underlying type {underlyingType.Name}");
    }

    public TStruct ReadStruct<TStruct>()
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

        var value = MemoryMarshal.Read<TStruct>(buffer.Span);

        Advance(size);
        return value;
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

    public ReadOnlyMemory<byte> UnreadMemory
        => buffer;
}