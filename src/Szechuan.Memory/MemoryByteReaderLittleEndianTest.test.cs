using NUnit.Framework;

namespace Szechuan.Memory;

[TestFixtureSource(typeof(BitConverters), nameof(BitConverters.LittleEndian))]
[TestOf(typeof(MemoryByteReader))]
[TestOf(typeof(ManagedLittleEndian))]
[TestOf(typeof(NativeLittleEndian))]
public sealed class MemoryByteReaderLittleEndianTest
{
    private const int MINUS_SIGN = 0b1000_0000;

    private readonly IEndianReader endianReader;
    private readonly byte[] memory = new byte[1000];

    public MemoryByteReaderLittleEndianTest(IEndianReader endianReader)
        => this.endianReader = endianReader;

    private MemoryByteReader CreateSut(byte[]? mem = null)
        => new(mem ?? memory, endianReader);

    [TestCase(sbyte.MinValue, new byte[] { MINUS_SIGN })]
    [TestCase(sbyte.MaxValue, new byte[] { 0x7F })]
    public void Read_SingleValue_sbyte(sbyte expected, byte[] actual)
        => AssertEqual(actual, expected, r => r.ReadSByte());

    [TestCase(byte.MinValue, new byte[] { 0 })]
    [TestCase(byte.MaxValue, new byte[] { 0xFF })]
    public void Read_SingleValue_byte(byte expected, byte[] actual)
        => AssertEqual(actual, expected, r => r.ReadByte());

    [TestCase(short.MinValue, new byte[] { 0, MINUS_SIGN })]
    [TestCase(short.MaxValue, new byte[] { 0xFF, 0x7F })]
    public void Read_SingleValue_short(short expected, byte[] actual)
        => AssertEqual(actual, expected, r => r.ReadInt16());

    [TestCase(ushort.MinValue, new byte[] { 0, 0 })]
    [TestCase(ushort.MaxValue, new byte[] { 0xFF, 0xFF })]
    public void Read_SingleValue_ushort(ushort expected, byte[] actual)
        => AssertEqual(actual, expected, r => r.ReadUInt16());

    [TestCase(0, new byte[] { 0, 0, 0, 0 })]
    [TestCase(100, new byte[] { 100, 0, 0, 0 })]
    [TestCase(int.MinValue, new byte[] { 0, 0, 0, 0x80 })]
    [TestCase(int.MaxValue, new byte[] { 0xFF, 0xFF, 0xFF, 0x7F })]
    public void Read_SingleValue_int(int expected, byte[] actual)
        => AssertEqual(actual, expected, r => r.ReadInt32());

    [TestCase(100u, new byte[] { 100, 0, 0, 0 })]
    [TestCase(uint.MinValue, new byte[] { 0, 0, 0, 0 })]
    [TestCase(uint.MaxValue, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF })]
    public void Read_SingleValue_uint(uint expected, byte[] actual)
        => AssertEqual(actual, expected, r => r.ReadUInt32());

    [TestCase(long.MinValue, new byte[] { 0, 0, 0, 0, 0, 0, 0, MINUS_SIGN })]
    [TestCase(long.MaxValue, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F })]
    public void Read_SingleValue_long(long expected, byte[] actual)
        => AssertEqual(actual, expected, r => r.ReadInt64());

    [TestCase(ulong.MinValue, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 })]
    [TestCase(ulong.MaxValue, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF })]
    public void Read_SingleValue_ulong(ulong expected, byte[] actual)
        => AssertEqual(actual, expected, r => r.ReadUInt64());

    [TestCase(float.MinValue, new byte[] { 0xFF, 0xFF, 0x7F, 0xFF })]
    [TestCase(float.MaxValue, new byte[] { 0xFF, 0xFF, 0x7F, 0x7F })]
    public void Read_SingleValue_float(float expected, byte[] actual)
        => AssertEqual(actual, expected, r => r.ReadFloat());

    [TestCase(double.MinValue, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xEF, 0xFF })]
    [TestCase(double.MaxValue, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xEF, 0x7F })]
    public void Read_SingleValue_double(double expected, byte[] actual)
        => AssertEqual(actual, expected, r => r.ReadDouble());

    [TestCase(false, new byte[] { 0 })]
    [TestCase(true, new byte[] { 1 })]
    public void Read_SingleValue_bool(bool expected, byte[] actual)
        => AssertEqual(actual, expected, r => r.ReadBool());

    [TestCase(IntValues.A, new byte[] { 0, 0, 0, 0 })]
    [TestCase(IntValues.C, new byte[] { 0xFF, 0x7F, 0, 0 })]
    public void Read_SingleValue_IntEnum(IntValues expected, byte[] actual)
        => AssertEqual(actual, expected, r => r.ReadEnum<IntValues>());

    [TestCase(UShortValues.A, new byte[] { 0, 0 })]
    [TestCase(UShortValues.C, new byte[] { 0xFF, 0x7F })]
    public void Read_SingleValue_UShortEnum(UShortValues expected, byte[] actual)
        => AssertEqual(actual, expected, r => r.ReadEnum<UShortValues>());

    [TestCase(0, 100, new byte[] { 0, 0, 0, 0, 100, 0, 0, 0 })]
    [TestCase(int.MaxValue, int.MinValue, new byte[] { 0xFF, 0xFF, 0xFF, 0x7F, 0, 0, 0, MINUS_SIGN })]
    public void Read_MultipleValue_Int32(int expected1, int expected2, byte[] actual)
    {
        Array.Copy(actual, memory, actual.Length);
        var sut = CreateSut();
        var actual1 = sut.ReadInt32();
        var actual2 = sut.ReadInt32();

        Assert.Multiple(() =>
        {
            Assert.That(actual1, Is.EqualTo(expected1));
            Assert.That(actual2, Is.EqualTo(expected2));
        });
    }

    [TestCase(new byte[] { 0xFF, 0xFF, 0xFF, 0x7F, 0, 0 }, new byte[] { 0xFF, 0xFF, 0xFF, 0x7F, 0, 0, 0, MINUS_SIGN })]
    public void Read_MultipleBytes(byte[] expected, byte[] actual)
        => AssertEqual(actual, expected, r => r.Read(6).ToArray());

    [Test]
    public void Read_WithSourceTooSmall_Fails()
    {
        var sut = CreateSut(new byte[2]);
        Assert.Throws<InsufficientMemoryException>(() => sut.ReadInt32());
    }

    private void AssertEqual<T>(byte[] actual, T expected, Func<MemoryByteReader, T> callback)
    {
        Array.Copy(actual, memory, actual.Length);
        var sut = CreateSut();
        Assert.That(callback(sut), Is.EqualTo(expected));
    }
}