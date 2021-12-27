namespace Szechuan.Memory;

public interface IByteReader
{
    sbyte ReadSByte();
    byte ReadByte();
    short ReadInt16();
    ushort ReadUInt16();
    int ReadInt32();
    uint ReadUInt32();
    long ReadInt64();
    ulong ReadUInt64();
    nint ReadNInt();
    nuint ReadNUInt();
    float ReadFloat();
    double ReadDouble();
    bool ReadBool();
    ReadOnlySpan<byte> Read(int count);

    TEnum ReadEnum<TEnum>()
        where TEnum : struct, Enum, IConvertible;

    TStruct ReadStruct<TStruct>()
        where TStruct : struct;
}