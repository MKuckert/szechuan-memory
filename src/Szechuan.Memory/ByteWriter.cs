namespace Szechuan.Memory;

public static class ByteWriter
{
    public static IByteWriter Open(Memory<byte> memory, IEndianWriter endianWriter)
        => new MemoryByteWriter(memory, endianWriter);
}