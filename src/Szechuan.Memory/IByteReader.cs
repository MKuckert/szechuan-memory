namespace Szechuan.Memory;

public interface IByteReader
{
    sbyte ReadSByte();
    byte ReadByte();
    short ReadShort();
    ushort ReadUShort();
    int ReadInt();
    uint ReadUInt();
    long ReadLong();
    ulong ReadULong();
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