namespace Szechuan.Memory;

internal sealed class ManagedLittleEndian : IEndianness
{
    public ByteOrder ByteOrder { get; } = ByteOrder.LITTLE_ENDIAN;

    public void Write(Span<byte> destination, sbyte value)
    {
        EnsureSize(destination, value, sizeof(sbyte));
        destination[0] = (byte)value;
    }

    public void Write(Span<byte> destination, byte value)
    {
        EnsureSize(destination, value, sizeof(byte));
        destination[0] = value;
    }

    public void Write(Span<byte> destination, short value)
    {
        EnsureSize(destination, value, sizeof(short));
        destination[0] = (byte)value;
        destination[1] = (byte)(value >> 8);
    }

    public void Write(Span<byte> destination, ushort value)
    {
        EnsureSize(destination, value, sizeof(ushort));
        destination[0] = (byte)value;
        destination[1] = (byte)(value >> 8);
    }

    public void Write(Span<byte> destination, int value)
    {
        EnsureSize(destination, value, sizeof(int));
        destination[0] = (byte)value;
        destination[1] = (byte)(value >> 8);
        destination[2] = (byte)(value >> 16);
        destination[3] = (byte)(value >> 24);
    }

    public void Write(Span<byte> destination, uint value)
    {
        EnsureSize(destination, value, sizeof(uint));
        destination[0] = (byte)value;
        destination[1] = (byte)(value >> 8);
        destination[2] = (byte)(value >> 16);
        destination[3] = (byte)(value >> 24);
    }

    public void Write(Span<byte> destination, long value)
    {
        EnsureSize(destination, value, sizeof(long));
        destination[0] = (byte)value;
        destination[1] = (byte)(value >> 8);
        destination[2] = (byte)(value >> 16);
        destination[3] = (byte)(value >> 24);
        destination[4] = (byte)(value >> 32);
        destination[5] = (byte)(value >> 40);
        destination[6] = (byte)(value >> 48);
        destination[7] = (byte)(value >> 56);
    }

    public void Write(Span<byte> destination, ulong value)
    {
        EnsureSize(destination, value, sizeof(ulong));
        destination[0] = (byte)value;
        destination[1] = (byte)(value >> 8);
        destination[2] = (byte)(value >> 16);
        destination[3] = (byte)(value >> 24);
        destination[4] = (byte)(value >> 32);
        destination[5] = (byte)(value >> 40);
        destination[6] = (byte)(value >> 48);
        destination[7] = (byte)(value >> 56);
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
                throw new FormatException($"Can't write nint value {value}: Unable to decide whether it's a 32 or 64 bit type");
        }
    }

    public void Write(Span<byte> destination, nuint value)
    {
        // We're using UIntPtr.Size instead of sizeof(nuint) as the later one is unsafe code
        switch (UIntPtr.Size)
        {
            case sizeof(uint):
                Write(destination, (uint)value);
                break;
            case sizeof(ulong):
                Write(destination, (ulong)value);
                break;
            default:
                throw new FormatException($"Can't write nuint value {value}: Unable to decide whether it's a 32 or 64 bit type");
        }
    }

    public void Write(Span<byte> destination, float value)
    {
        EnsureSize(destination, value, sizeof(float));

        // We have no better way to get the bytes for a double yet so we're falling back to using the BitConverter and reversing if it's the wrong byte order
        if (!BitConverter.TryWriteBytes(destination, value))
        {
            throw new FormatException($"Can't write float value {value}: Bit conversion failed");
        }

        if (!BitConverter.IsLittleEndian)
        {
            destination.Reverse(0, sizeof(float));
        }
    }

    public void Write(Span<byte> destination, double value)
    {
        EnsureSize(destination, value, sizeof(double));

        // We have no better way to get the bytes for a double yet so we're falling back to using the BitConverter and reversing if it's the wrong byte order
        if (!BitConverter.TryWriteBytes(destination, value))
        {
            throw new FormatException($"Can't write float value {value}: Bit conversion failed");
        }

        if (!BitConverter.IsLittleEndian)
        {
            destination.Reverse(0, sizeof(double));
        }
    }

    public void Write(Span<byte> destination, bool value)
    {
        EnsureSize(destination, value, sizeof(bool));
        destination[0] = value ? (byte)1 : (byte)0;
    }

    public sbyte ReadSByte(ReadOnlySpan<byte> source)
    {
        EnsureSize<sbyte>(source, sizeof(sbyte));
        return (sbyte)source[0];
    }

    public byte ReadByte(ReadOnlySpan<byte> source)
    {
        EnsureSize<byte>(source, sizeof(byte));
        return source[0];
    }

    public short ReadInt16(ReadOnlySpan<byte> source)
    {
        EnsureSize<short>(source, sizeof(short));
        return (short)(
            source[0]
            | source[1] << 8);
    }

    public ushort ReadUInt16(ReadOnlySpan<byte> source)
    {
        EnsureSize<ushort>(source, sizeof(ushort));
        return (ushort)(
            source[0]
            | source[1] << 8);
    }

    public int ReadInt32(ReadOnlySpan<byte> source)
    {
        EnsureSize<int>(source, sizeof(int));
        return source[0]
               | source[1] << 8
               | source[2] << 16
               | source[3] << 24;
    }

    public uint ReadUInt32(ReadOnlySpan<byte> source)
    {
        EnsureSize<uint>(source, sizeof(uint));
        return (uint)(
            source[0]
            | source[1] << 8
            | source[2] << 16
            | source[3] << 24);
    }

    public long ReadInt64(ReadOnlySpan<byte> source)
    {
        EnsureSize<long>(source, sizeof(long));
        return source[0]
               | (long)source[1] << 8
               | (long)source[2] << 16
               | (long)source[3] << 24
               | (long)source[4] << 32
               | (long)source[5] << 40
               | (long)source[6] << 48
               | (long)source[7] << 56;
    }

    public ulong ReadUInt64(ReadOnlySpan<byte> source)
    {
        EnsureSize<ulong>(source, sizeof(ulong));
        return source[0]
               | (ulong)source[1] << 8
               | (ulong)source[2] << 16
               | (ulong)source[3] << 24
               | (ulong)source[4] << 32
               | (ulong)source[5] << 40
               | (ulong)source[6] << 48
               | (ulong)source[7] << 56;
    }

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
    {
        const int SIZE = sizeof(float);
        EnsureSize<float>(source, SIZE);

        // We have no better way to get the bytes for a float yet so we're falling back to using the BitConverter and reversing if it's the wrong byte order
        if (BitConverter.IsLittleEndian)
        {
            return BitConverter.ToSingle(source);
        }

        Span<byte> workbench = stackalloc byte[SIZE];
        source[..SIZE].CopyTo(workbench);
        workbench.Reverse();
        return BitConverter.ToSingle(source);
    }

    public double ReadDouble(ReadOnlySpan<byte> source)
    {
        const int SIZE = sizeof(double);
        EnsureSize<double>(source, SIZE);

        // We have no better way to get the bytes for a float yet so we're falling back to using the BitConverter and reversing if it's the wrong byte order
        if (BitConverter.IsLittleEndian)
        {
            return BitConverter.ToDouble(source);
        }

        Span<byte> workbench = stackalloc byte[SIZE];
        source[..SIZE].CopyTo(workbench);
        workbench.Reverse();
        return BitConverter.ToDouble(source);
    }

    public bool ReadBool(ReadOnlySpan<byte> source)
    {
        EnsureSize<bool>(source, sizeof(bool));
        return source[0] != 0;
    }

    public ReadOnlySpan<byte> Read(ReadOnlySpan<byte> source, int count)
    {
        EnsureSize<byte[]>(source, count);
        return source[..count];
    }

    private static void EnsureSize<T>(ReadOnlySpan<byte> source, int expectedSize)
    {
        if (source.Length < expectedSize)
        {
            throw new FormatException($"Can't read {typeof(T).Name} value: source is too small. Expected at least {expectedSize} bytes but got only {source.Length}");
        }
    }

    private static void EnsureSize<T>(Span<byte> destination, T value, int expectedSize)
    {
        if (destination.Length < expectedSize)
        {
            throw new FormatException($"Can't write {typeof(T).Name} value {value}: destination is too small. Expected at least {expectedSize} bytes but got only {destination.Length}");
        }
    }
}