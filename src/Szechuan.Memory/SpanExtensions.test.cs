using NUnit.Framework;

namespace Szechuan.Memory;

[TestFixture]
[TestOf(typeof(SpanExtensions))]
public sealed class SpanExtensionsTest
{
    [Test]
    public void Swap_SwapsBytes()
    {
        Memory<byte> memory = new byte[] { 1, 2, 3, 4 };
        memory.Span.Swap(0, 2);
        Assert.That(memory.ToArray(), Is.EqualTo(new byte[] { 3, 2, 1, 4 }));
    }

    [Test]
    public void Reverse_ReversesBytes()
    {
        Memory<byte> memory = new byte[] { 1, 2, 3, 4 };
        memory.Span.Reverse();
        Assert.That(memory.ToArray(), Is.EqualTo(new byte[] { 4, 3, 2, 1 }));
    }

    [Test]
    public void Reverse_WithRange_ReversesSelectedBytes()
    {
        Memory<byte> memory = new byte[] { 1, 2, 3, 4 };
        memory.Span.Reverse(1, 2);
        Assert.That(memory.ToArray(), Is.EqualTo(new byte[] { 1, 3, 2, 4 }));
    }
}