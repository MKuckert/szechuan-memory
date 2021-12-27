using NUnit.Framework;

namespace Szechuan.Memory;

[TestFixture]
[TestOf(typeof(ByteReader))]
[TestOf(typeof(ByteWriter))]
public sealed class FactoryMethods
{
    private readonly byte[] memory = new byte[1000];

    [Test]
    public void ByteReader_Open_CreatesByteReader()
    {
        var reader = ByteReader.Open(memory, Endian.CurrentArchitecture);

        Assert.Multiple(() =>
        {
            Assert.That(reader, Is.Not.Null);
            Assert.That(reader, Is.InstanceOf<IByteReader>());
        });
    }

    [Test]
    public void ByteWriter_Open_CreatesByteWriter()
    {
        var writer = ByteWriter.Open(memory, Endian.CurrentArchitecture);

        Assert.Multiple(() =>
        {
            Assert.That(writer, Is.Not.Null);
            Assert.That(writer, Is.InstanceOf<IByteWriter>());
        });
    }
}