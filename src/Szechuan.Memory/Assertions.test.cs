using NUnit.Framework;

namespace Szechuan.Memory;

internal static class Assertions
{
    public static void AssertEquivalent(IMemoryHolder actual, IEnumerable<byte> expected)
        => Assert.That(
            BitConverter.ToString(actual.Memory.ToArray()),
            Is.EqualTo(BitConverter.ToString(expected.ToArray())));
}