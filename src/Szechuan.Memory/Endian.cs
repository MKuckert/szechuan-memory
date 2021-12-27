using System.Diagnostics.CodeAnalysis;

namespace Szechuan.Memory;

[ExcludeFromCodeCoverage]
public static class Endian
{
    static Endian()
    {
        if (BitConverter.IsLittleEndian)
        {
            Little = new NativeLittleEndian();
            Big = new ManagedBigEndian();
            CurrentArchitecture = Little;
        }
        else
        {
            Little = new ManagedLittleEndian();
            Big = new NativeBigEndian();
            CurrentArchitecture = Big;
        }
    }

    public static IEndianness CurrentArchitecture { get; }
    public static IEndianness Little { get; }
    public static IEndianness Big { get; }
}