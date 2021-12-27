namespace Szechuan.Memory;

public static class BitConverters
{
    public static IEnumerable<IEndianness> LittleEndian
    {
        get
        {
            if (BitConverter.IsLittleEndian)
            {
                yield return new NativeLittleEndian();
            }

            yield return new ManagedLittleEndian();
        }
    }

    public static IEnumerable<IEndianness> BigEndian
    {
        get
        {
            if (!BitConverter.IsLittleEndian)
            {
                yield return new NativeBigEndian();
            }

            yield return new ManagedBigEndian();
        }
    }
}