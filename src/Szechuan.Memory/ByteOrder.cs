namespace Szechuan.Memory;

public static class ByteOrder
{
    static ByteOrder()
    {
        if (BitConverter.IsLittleEndian)
        {
            LittleEndian = new NativeLittleEndian();
            BigEndian = new ManagedBigEndian();
            CurrentArchitecture = LittleEndian;
        }
        else
        {
            LittleEndian = new ManagedLittleEndian();
            BigEndian = new NativeBigEndian();
            CurrentArchitecture = BigEndian;
        }
    }

    public static IEndianness CurrentArchitecture { get; }
    public static IEndianness LittleEndian { get; }
    public static IEndianness BigEndian { get; }
}