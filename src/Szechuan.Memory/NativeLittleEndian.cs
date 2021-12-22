namespace Szechuan.Memory;

internal sealed class NativeLittleEndian : IEndianness
{
    public NativeLittleEndian()
    {
        if (!BitConverter.IsLittleEndian)
        {
            throw new InvalidOperationException($"Can't use {GetType().Name} on a system with big endian architecture. Use {nameof(ManagedLittleEndian)} instead");
        }
    }

    public void Write(Span<byte> destination, sbyte value)
    {
        if (!BitConverter.TryWriteBytes(destination, value))
        {
            throw CantWrite(destination, value, sizeof(sbyte));
        }
    }

    public void Write(Span<byte> destination, byte value)
    {
        if (!BitConverter.TryWriteBytes(destination, value))
        {
            throw CantWrite(destination, value, sizeof(byte));
        }
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
        if (!BitConverter.TryWriteBytes(destination, value))
        {
            // We're using IntPtr.Size instead of sizeof(nint) as the later one is unsafe code
            throw CantWrite(destination, value, IntPtr.Size);
        }
    }

    public void Write(Span<byte> destination, nuint value)
    {
        if (!BitConverter.TryWriteBytes(destination, value))
        {
            // We're using UIntPtr.Size instead of sizeof(nuint) as the later one is unsafe code
            throw CantWrite(destination, value, UIntPtr.Size);
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
        => new($"Can't write {typeof(T).Name} value {value}: destination is too small. Expected at least {expectedSize} bytes but got only {destination.Length}");
}