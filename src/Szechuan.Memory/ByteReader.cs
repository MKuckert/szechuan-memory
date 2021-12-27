namespace Szechuan.Memory;

public static class ByteReader
{
    public static IByteReader Open(Memory<byte> memory, IEndianReader endianReader)
        => new MemoryByteReader(memory, endianReader);
}