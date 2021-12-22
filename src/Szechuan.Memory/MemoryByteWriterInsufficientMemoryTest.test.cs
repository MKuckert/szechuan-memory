using NUnit.Framework;

namespace Szechuan.Memory;

[TestFixture]
[TestOf(typeof(MemoryByteWriter))]
public sealed class MemoryByteWriterInsufficientMemoryTest
{
    private readonly byte[] memory = new byte[1];

    private MemoryByteWriter CreateSut()
        => new(memory, ByteOrder.CurrentArchitecture);

    [Test]
    public void Write_WithNoMoreMemory_ThrowsInsufficientMemoryException()
    {
        var sut = CreateSut();
        Assert.Throws<InsufficientMemoryException>(() => sut.Write(100));
    }
}