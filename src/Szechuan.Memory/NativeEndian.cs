namespace Szechuan.Memory;

internal abstract class NativeEndian : IEndianness
{
    protected NativeEndian(ByteOrder supportedByteOrder)
    {
        var isCorrectEndianness = supportedByteOrder == ByteOrder.LITTLE_ENDIAN && BitConverter.IsLittleEndian;
        if (!isCorrectEndianness)
        {
            throw new InvalidOperationException(
                $"Can use {GetType().Name} on a system with {supportedByteOrder} architecture only. Use a managed variant instead");
        }

        ByteOrder = supportedByteOrder;
    }

    public ByteOrder ByteOrder { get; }

    public void Write(Span<byte> destination, sbyte value)
    {
        if (destination.Length < 1)
        {
            throw CantWrite(destination, value, sizeof(sbyte));
        }

        destination[0] = (byte)value;
    }

    public void Write(Span<byte> destination, byte value)
    {
        if (destination.Length < 1)
        {
            throw CantWrite(destination, value, sizeof(sbyte));
        }

        destination[0] = value;
    }

    public void Write(Span<byte> destination, short value)
    {
        if (!BitConverter.TryWriteBytes(destination, value))
        {
            throw CantWrite(destination, value, sizeof(short));
        }
    }

    public void Write(Span<byte> destination, ushort value)
    {
        if (!BitConverter.TryWriteBytes(destination, value))
        {
            throw CantWrite(destination, value, sizeof(ushort));
        }
    }

    public void Write(Span<byte> destination, int value)
    {
        if (!BitConverter.TryWriteBytes(destination, value))
        {
            throw CantWrite(destination, value, sizeof(int));
        }
    }

    public void Write(Span<byte> destination, uint value)
    {
        if (!BitConverter.TryWriteBytes(destination, value))
        {
            throw CantWrite(destination, value, sizeof(uint));
        }
    }

    public void Write(Span<byte> destination, long value)
    {
        if (!BitConverter.TryWriteBytes(destination, value))
        {
            throw CantWrite(destination, value, sizeof(long));
        }
    }

    public void Write(Span<byte> destination, ulong value)
    {
        if (!BitConverter.TryWriteBytes(destination, value))
        {
            throw CantWrite(destination, value, sizeof(ulong));
        }
    }

    public void Write(Span<byte> destination, nint value)
    {
        // We're using IntPtr.Size instead of sizeof(nint) as the later one is unsafe code
        switch (IntPtr.Size)
        {
            case sizeof(int):
                Write(destination, (int)value);
                break;
            case sizeof(long):
                Write(destination, (long)value);
                break;
            default:
                throw new FormatException(
                    $"Can't write nint value {value}: Unable to decide whether it's a 32 or 64 bit type");
        }
    }

    public void Write(Span<byte> destination, nuint value)
    {
        // We're using IntPtr.Size instead of sizeof(nuint) as the later one is unsafe code
        switch (UIntPtr.Size)
        {
            case sizeof(uint):
                Write(destination, (uint)value);
                break;
            case sizeof(ulong):
                Write(destination, (ulong)value);
                break;
            default:
                throw new FormatException(
                    $"Can't write nuint value {value}: Unable to decide whether it's a 32 or 64 bit type");
        }
    }

    public void Write(Span<byte> destination, float value)
    {
        if (!BitConverter.TryWriteBytes(destination, value))
        {
            throw CantWrite(destination, value, sizeof(float));
        }
    }

    public void Write(Span<byte> destination, double value)
    {
        if (!BitConverter.TryWriteBytes(destination, value))
        {
            throw CantWrite(destination, value, sizeof(double));
        }
    }

    public void Write(Span<byte> destination, bool value)
    {
        if (!BitConverter.TryWriteBytes(destination, value))
        {
            throw CantWrite(destination, value, sizeof(bool));
        }
    }

    private static FormatException CantWrite<T>(Span<byte> destination, T value, int expectedSize)
        => new(
            $"Can't write {typeof(T).Name} value {value}: destination is too small. Expected at least {expectedSize} bytes but got only {destination.Length}");

    public sbyte ReadSByte(ReadOnlySpan<byte> source)
        => (sbyte)source[0];

    public byte ReadByte(ReadOnlySpan<byte> source)
        => source[0];

    public short ReadInt16(ReadOnlySpan<byte> source)
        => BitConverter.ToInt16(source);

    public ushort ReadUInt16(ReadOnlySpan<byte> source)
        => BitConverter.ToUInt16(source);

    public int ReadInt32(ReadOnlySpan<byte> source)
        => BitConverter.ToInt32(source);

    public uint ReadUInt32(ReadOnlySpan<byte> source)
        => BitConverter.ToUInt32(source);

    public long ReadInt64(ReadOnlySpan<byte> source)
        => BitConverter.ToInt64(source);

    public ulong ReadUInt64(ReadOnlySpan<byte> source)
        => BitConverter.ToUInt64(source);

    public nint ReadNInt(ReadOnlySpan<byte> source)

        // We're using IntPtr.Size instead of sizeof(nint) as the later one is unsafe code
        => IntPtr.Size switch
        {
            sizeof(int) => ReadInt32(source),
            sizeof(long) => (nint)ReadInt64(source),
            _ => throw new FormatException($"Can't read nint value: Unable to decide whether it's a 32 or 64 bit type")
        };

    public nuint ReadNUInt(ReadOnlySpan<byte> source)

        // We're using IntPtr.Size instead of sizeof(nuint) as the later one is unsafe code
        => UIntPtr.Size switch
        {
            sizeof(uint) => ReadUInt32(source),
            sizeof(ulong) => (nuint)ReadUInt64(source),
            _ => throw new FormatException($"Can't read nuint value: Unable to decide whether it's a 32 or 64 bit type")
        };

    public float ReadFloat(ReadOnlySpan<byte> source)
        => BitConverter.ToSingle(source);

    public double ReadDouble(ReadOnlySpan<byte> source)
        => BitConverter.ToDouble(source);

    public bool ReadBool(ReadOnlySpan<byte> source)
        => source[0] != 0;

    public ReadOnlySpan<byte> Read(ReadOnlySpan<byte> source, int count)
        => source[..count];
}

internal sealed class NativeBigEndian() : NativeEndian(ByteOrder.BIG_ENDIAN);

internal sealed class NativeLittleEndian() : NativeEndian(ByteOrder.LITTLE_ENDIAN);