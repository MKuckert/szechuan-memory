using NUnit.Framework;

namespace Szechuan.Memory;

[TestFixture]
[TestOf(typeof(MemoryByteReader))]
public sealed class MemoryByteReaderReadStructTest
{
    private readonly byte[] memory = new byte[1000];

    private MemoryByteReader CreateSut()
        => new(memory, Endian.CurrentArchitecture);

    [TestCase(new byte[] { 100, 0, 0, 0, 42, 0, 0, 0 }, 100, 42u)]
    public void Read_SingleValue_TestStruct1(byte[] actual, int expectedA, uint expectedB)
    {
        Array.Copy(actual, memory, actual.Length);
        var sut = CreateSut();
        var result = sut.ReadStruct<TestStruct1>();

        Assert.Multiple(() =>
        {
            Assert.That(result.A, Is.EqualTo(expectedA));
            Assert.That(result.B, Is.EqualTo(expectedB));
        });
    }
}