using NUnit.Framework;

namespace Szechuan.Memory;

[TestFixture]
[TestOf(typeof(MemoryByteWriter))]
public sealed class MemoryByteWriterWriteStructTest
{
    private readonly byte[] memory = new byte[1000];

    private MemoryByteWriter CreateSut()
        => new(memory, ByteOrder.CurrentArchitecture);

    [TestCase(100, 42u, new byte[] { 100, 0, 0, 0, 42, 0, 0, 0 })]
    public void Write_SingleValue_TestStruct1(int inputA, uint inputB, byte[] expected)
    {
        var input = new TestStruct1(inputA, inputB);
        var sut = CreateSut();
        sut.WriteStruct(ref input);
        Assertions.AssertEquivalent(sut, expected);
    }
}