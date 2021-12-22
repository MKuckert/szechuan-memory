using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Szechuan.Memory;

internal sealed class MemoryByteReader : IByteReader, IMemoryHolder
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
        throw new NotImplementedException();
    }

    public byte ReadByte()
    {
        throw new NotImplementedException();
    }

    public short ReadShort()
    {
        throw new NotImplementedException();
    }

    public ushort ReadUShort()
    {
        throw new NotImplementedException();
    }

    public int ReadInt()
    {
        throw new NotImplementedException();
    }

    public uint ReadUInt()
    {
        throw new NotImplementedException();
    }

    public long ReadLong()
    {
        throw new NotImplementedException();
    }

    public ulong ReadULong()
    {
        throw new NotImplementedException();
    }

    public nint ReadNInt()
    {
        throw new NotImplementedException();
    }

    public nuint ReadNUInt()
    {
        throw new NotImplementedException();
    }

    public float ReadFloat()
    {
        throw new NotImplementedException();
    }

    public double ReadDouble()
    {
        throw new NotImplementedException();
    }

    public bool ReadBool()
    {
        throw new NotImplementedException();
    }

    public ReadOnlySpan<byte> Read(int count)
    {
        throw new NotImplementedException();
    }

    public TEnum ReadEnum<TEnum>()
        where TEnum : struct, Enum, IConvertible
    {
        throw new NotImplementedException();
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