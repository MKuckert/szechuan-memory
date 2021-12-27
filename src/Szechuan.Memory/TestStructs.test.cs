using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Szechuan.Memory;

public enum IntValues
{
    A,
    B,
    C = 0x7FFF
}

public enum UShortValues : ushort
{
    A,
    B,
    C = 0x7FFF
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
[ExcludeFromCodeCoverage]
public readonly struct TestStruct1 : IEquatable<TestStruct1>
{
    public int A { get; }
    public uint B { get; }

    public TestStruct1(int a, uint b)
    {
        A = a;
        B = b;
    }

    public bool Equals(TestStruct1 other) => A == other.A && B == other.B;

    public override bool Equals(object? obj) => obj is TestStruct1 other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(A, B);

    public static bool operator ==(TestStruct1 left, TestStruct1 right) => left.Equals(right);

    public static bool operator !=(TestStruct1 left, TestStruct1 right) => !left.Equals(right);
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
[ExcludeFromCodeCoverage]
public readonly ref struct TestStruct2
{
    public readonly ReadOnlySpan<byte> Content;

    public TestStruct2(ReadOnlySpan<byte> content)
        => Content = content;
}