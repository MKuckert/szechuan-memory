namespace Szechuan.Memory;

public static class ByteReader
{
    public static IByteReader Open(ReadOnlyMemory<byte> memory, IEndianReader endianReader)
        => new MemoryByteReader(memory, endianReader);
}