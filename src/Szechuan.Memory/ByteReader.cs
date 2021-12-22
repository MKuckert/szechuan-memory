namespace Szechuan.Memory;

public static class ByteReader
{
    public static IByteReader Open<T>(Memory<byte> memory, IEndianReader endianReader)
        => new MemoryByteReader(memory, endianReader);
}