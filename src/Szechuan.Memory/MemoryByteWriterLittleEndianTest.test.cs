using NUnit.Framework;

namespace Szechuan.Memory;

[TestFixtureSource(nameof(BitConverters))]
[TestOf(typeof(MemoryByteWriter))]
[TestOf(typeof(ManagedLittleEndian))]
[TestOf(typeof(NativeLittleEndian))]
public sealed class MemoryByteWriterLittleEndianTest
{
    private const int MINUS_SIGN = 0b1000_0000;

    private readonly IEndianWriter endianWriter;
    private readonly byte[] memory = new byte[1000];

    public MemoryByteWriterLittleEndianTest(IEndianWriter endianWriter)
        => this.endianWriter = endianWriter;

    private MemoryByteWriter CreateSut()
        => new(memory, endianWriter);

    private MemoryByteWriter Do(Action<MemoryByteWriter> callback)
    {
        var sut = CreateSut();
        callback(sut);
        return sut;
    }

    private static IEnumerable<IEndianWriter> BitConverters
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

    [TestCase(sbyte.MinValue, new byte[] { MINUS_SIGN })]
    [TestCase(sbyte.MaxValue, new byte[] { 0x7F })]
    public void Write_SingleValue_sbyte(sbyte input, byte[] expected)
        => AssertEquivalent(w => w.Write(input), expected);

    [TestCase(byte.MinValue, new byte[] { 0 })]
    [TestCase(byte.MaxValue, new byte[] { 0xFF })]
    public void Write_SingleValue_byte(byte input, byte[] expected)
        => AssertEquivalent(w => w.Write(input), expected);

    [TestCase(short.MinValue, new byte[] { 0, MINUS_SIGN })]
    [TestCase(short.MaxValue, new byte[] { 0xFF, 0x7F })]
    public void Write_SingleValue_short(short input, byte[] expected)
        => AssertEquivalent(w => w.Write(input), expected);

    [TestCase(ushort.MinValue, new byte[] { 0, 0 })]
    [TestCase(ushort.MaxValue, new byte[] { 0xFF, 0xFF })]
    public void Write_SingleValue_ushort(ushort input, byte[] expected)
        => AssertEquivalent(w => w.Write(input), expected);

    [TestCase(0, new byte[] { 0, 0, 0, 0 })]
    [TestCase(100, new byte[] { 100, 0, 0, 0 })]
    [TestCase(int.MinValue, new byte[] { 0, 0, 0, 0x80 })]
    [TestCase(int.MaxValue, new byte[] { 0xFF, 0xFF, 0xFF, 0x7F })]
    public void Write_SingleValue_int(int input, byte[] expected)
        => AssertEquivalent(w => w.Write(input), expected);

    [TestCase(100u, new byte[] { 100, 0, 0, 0 })]
    [TestCase(uint.MinValue, new byte[] { 0, 0, 0, 0 })]
    [TestCase(uint.MaxValue, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF })]
    public void Write_SingleValue_uint(uint input, byte[] expected)
        => AssertEquivalent(w => w.Write(input), expected);

    [TestCase(long.MinValue, new byte[] { 0, 0, 0, 0, 0, 0, 0, MINUS_SIGN })]
    [TestCase(long.MaxValue, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F })]
    public void Write_SingleValue_long(long input, byte[] expected)
        => AssertEquivalent(w => w.Write(input), expected);

    [TestCase(ulong.MinValue, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 })]
    [TestCase(ulong.MaxValue, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF })]
    public void Write_SingleValue_ulong(ulong input, byte[] expected)
        => AssertEquivalent(w => w.Write(input), expected);

    [TestCase(float.MinValue, new byte[] { 0xFF, 0xFF, 0x7F, 0xFF })]
    [TestCase(float.MaxValue, new byte[] { 0xFF, 0xFF, 0x7F, 0x7F })]
    public void Write_SingleValue_float(float input, byte[] expected)
        => AssertEquivalent(w => w.Write(input), expected);

    [TestCase(double.MinValue, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xEF, 0xFF })]
    [TestCase(double.MaxValue, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xEF, 0x7F })]
    public void Write_SingleValue_double(double input, byte[] expected)
        => AssertEquivalent(w => w.Write(input), expected);

    [TestCase(false, new byte[] { 0 })]
    [TestCase(true, new byte[] { 1 })]
    public void Write_SingleValue_bool(bool input, byte[] expected)
        => AssertEquivalent(w => w.Write(input), expected);

    [TestCase(IntValues.A, new byte[] { 0, 0, 0, 0 })]
    [TestCase(IntValues.C, new byte[] { 0xFF, 0x7F, 0, 0 })]
    public void Write_SingleValue_IntEnum(IntValues input, byte[] expected)
        => AssertEquivalent(w => w.WriteEnum(input), expected);

    [TestCase(UShortValues.A, new byte[] { 0, 0 })]
    [TestCase(UShortValues.C, new byte[] { 0xFF, 0x7F })]
    public void Write_SingleValue_UShortEnum(UShortValues input, byte[] expected)
        => AssertEquivalent(w => w.WriteEnum(input), expected);

    [TestCase(0, 100, new byte[] { 0, 0, 0, 0, 100, 0, 0, 0 })]
    [TestCase(int.MaxValue, int.MinValue, new byte[] { 0xFF, 0xFF, 0xFF, 0x7F, 0, 0, 0, MINUS_SIGN })]
    public void Write_MultipleValue_Int32(int input1, int input2, byte[] expected)
    {
        AssertEquivalent(
            w => w
                .Write(input1)
                .Write(input2),
            expected);
    }

    [TestCase(100, 42u, new byte[] { 100, 0, 0, 0, 42, 0, 0, 0 })]
    public void Write_SingleValue_TestStruct1(int inputA, uint inputB, byte[] expected)
    {
        var input = new TestStruct1(inputA, inputB);
        var sut = CreateSut();
        sut.WriteStruct(ref input);
        Assertions.AssertEquivalent(sut, expected);
    }

    private void AssertEquivalent(Action<MemoryByteWriter> actual, IEnumerable<byte> expected)
        => Assertions.AssertEquivalent(Do(actual), expected);
}