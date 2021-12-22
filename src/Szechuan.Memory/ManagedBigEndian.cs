namespace Szechuan.Memory;

internal sealed class ManagedBigEndian : IEndianness
{
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
        destination[0] = (byte)(value >> 8);
        destination[1] = (byte)value;
    }

    public void Write(Span<byte> destination, ushort value)
    {
        EnsureSize(destination, value, sizeof(ushort));
        destination[0] = (byte)(value >> 8);
        destination[1] = (byte)value;
    }

    public void Write(Span<byte> destination, int value)
    {
        EnsureSize(destination, value, sizeof(int));
        destination[0] = (byte)(value >> 24);
        destination[1] = (byte)(value >> 16);
        destination[2] = (byte)(value >> 8);
        destination[3] = (byte)value;
    }

    public void Write(Span<byte> destination, uint value)
    {
        EnsureSize(destination, value, sizeof(uint));
        destination[0] = (byte)(value >> 24);
        destination[1] = (byte)(value >> 16);
        destination[2] = (byte)(value >> 8);
        destination[3] = (byte)value;
    }

    public void Write(Span<byte> destination, long value)
    {
        EnsureSize(destination, value, sizeof(long));
        destination[0] = (byte)(value >> 56);
        destination[1] = (byte)(value >> 48);
        destination[2] = (byte)(value >> 40);
        destination[3] = (byte)(value >> 32);
        destination[4] = (byte)(value >> 24);
        destination[5] = (byte)(value >> 16);
        destination[6] = (byte)(value >> 8);
        destination[7] = (byte)value;
    }

    public void Write(Span<byte> destination, ulong value)
    {
        EnsureSize(destination, value, sizeof(ulong));
        destination[0] = (byte)(value >> 56);
        destination[1] = (byte)(value >> 48);
        destination[2] = (byte)(value >> 40);
        destination[3] = (byte)(value >> 32);
        destination[4] = (byte)(value >> 24);
        destination[5] = (byte)(value >> 16);
        destination[6] = (byte)(value >> 8);
        destination[7] = (byte)value;
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

        if (BitConverter.IsLittleEndian)
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

        if (BitConverter.IsLittleEndian)
        {
            destination.Reverse(0, sizeof(double));
        }
    }

    public void Write(Span<byte> destination, bool value)
    {
        EnsureSize(destination, value, sizeof(bool));
        destination[0] = value ? (byte)1 : (byte)0;
    }

    private static void EnsureSize<T>(Span<byte> destination, T value, int expectedSize)
    {
        if (destination.Length < expectedSize)
        {
            throw new FormatException($"Can't write {typeof(T).Name} value {value}: destination is too small. Expected at least {expectedSize} bytes but got only {destination.Length}");
        }
    }
}