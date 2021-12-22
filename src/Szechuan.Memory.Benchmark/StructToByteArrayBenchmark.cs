using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Szechuan.Memory.Benchmark;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct X
{
    public int Foo { get; }

    public X(int foo)
        => Foo = foo;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct Y
{
    public int V0 { get; }
    public int V1 { get; }
    public int V2 { get; }
    public int V3 { get; }
    public int V4 { get; }
    public int V5 { get; }
    public int V6 { get; }
    public int V7 { get; }
    public int V8 { get; }
    public int V9 { get; }
    public int V10 { get; }
    public int V11 { get; }
    public int V12 { get; }
    public int V13 { get; }
    public int V14 { get; }
    public int V15 { get; }
    public int V16 { get; }
    public int V17 { get; }
    public int V18 { get; }
    public int V19 { get; }
    public int V20 { get; }
    public int V21 { get; }
    public int V22 { get; }
    public int V23 { get; }
    public int V24 { get; }
    public int V25 { get; }
    public int V26 { get; }
    public int V27 { get; }
    public int V28 { get; }
    public int V29 { get; }
    public int V30 { get; }
    public int V31 { get; }
    public int V32 { get; }
    public int V33 { get; }
    public int V34 { get; }
    public int V35 { get; }
    public int V36 { get; }
    public int V37 { get; }
    public int V38 { get; }
    public int V39 { get; }
    public int V40 { get; }
    public int V41 { get; }
    public int V42 { get; }
    public int V43 { get; }
    public int V44 { get; }
    public int V45 { get; }
    public int V46 { get; }
    public int V47 { get; }
    public int V48 { get; }
    public int V49 { get; }
    public int V50 { get; }
    public int V51 { get; }
    public int V52 { get; }
    public int V53 { get; }
    public int V54 { get; }
    public int V55 { get; }
    public int V56 { get; }
    public int V57 { get; }
    public int V58 { get; }
    public int V59 { get; }
    public int V60 { get; }
    public int V61 { get; }
    public int V62 { get; }
    public int V63 { get; }
    public int V64 { get; }
    public int V65 { get; }
    public int V66 { get; }
    public int V67 { get; }
    public int V68 { get; }
    public int V69 { get; }
    public int V70 { get; }
    public int V71 { get; }
    public int V72 { get; }
    public int V73 { get; }
    public int V74 { get; }
    public int V75 { get; }
    public int V76 { get; }
    public int V77 { get; }
    public int V78 { get; }
    public int V79 { get; }
    public int V80 { get; }
    public int V81 { get; }
    public int V82 { get; }
    public int V83 { get; }
    public int V84 { get; }
    public int V85 { get; }
    public int V86 { get; }
    public int V87 { get; }
    public int V88 { get; }
    public int V89 { get; }
    public int V90 { get; }
    public int V91 { get; }
    public int V92 { get; }
    public int V93 { get; }
    public int V94 { get; }
    public int V95 { get; }
    public int V96 { get; }
    public int V97 { get; }
    public int V98 { get; }
    public int V99 { get; }
}

[SimpleJob(RuntimeMoniker.NetCoreApp31)]
[SimpleJob(RuntimeMoniker.Net60, baseline: true)]
[MemoryDiagnoser]
public class StructToByteArrayBenchmark
{
    private readonly X x = new(100);
    private readonly Y y = new();

    [Benchmark(Baseline = true)]
    public byte[] UseSpanCast_X() => UseSpanCast(x);

    [Benchmark]
    public byte[] UseSpanAsBytes_X() => UseSpanAsBytes(x);

    [Benchmark]
    public byte[] UseMarshalStructureToPtrCopy_X() => UseMarshalStructureToPtrCopy(x);

    [Benchmark]
    public byte[] UseMarshalStructureToPtrDirectly_X() => UseMarshalStructureToPtrDirectly(x);

    [Benchmark]
    public byte[] UseSpanCast_Y() => UseSpanCast(y);

    [Benchmark]
    public byte[] UseSpanAsBytes_Y() => UseSpanAsBytes(y);

    [Benchmark]
    public byte[] UseMarshalStructureToPtrCopy_Y() => UseMarshalStructureToPtrCopy(y);

    [Benchmark]
    public byte[] UseMarshalStructureToPtrDirectly_Y() => UseMarshalStructureToPtrDirectly(y);

    private byte[] UseSpanCast<T>(T x)
        where T : struct
    {
        Span<T> r = new[] { x };
        var buf = MemoryMarshal.Cast<T, byte>(r);
        return buf.ToArray();
    }

    private byte[] UseSpanAsBytes<T>(T x)
        where T : struct
    {
        Span<T> r = new[] { x };
        var buf = MemoryMarshal.AsBytes(r);
        return buf.ToArray();
    }

    public byte[] UseMarshalStructureToPtrCopy<T>(T x)
        where T : struct
    {
        var size = Marshal.SizeOf(x);
        var buffer = new byte[size];
        var ptr = Marshal.AllocHGlobal(size);
        Marshal.StructureToPtr(x, ptr, true);
        Marshal.Copy(ptr, buffer, 0, size);
        Marshal.FreeHGlobal(ptr);
        return buffer;
    }

    public unsafe byte[] UseMarshalStructureToPtrDirectly<T>(T x)
        where T : struct
    {
        var n = Marshal.SizeOf(x);
        var buffer = new byte[n];
        fixed (byte* pointer = buffer)
        {
            Marshal.StructureToPtr(x, new IntPtr(pointer), false);
        }

        return buffer;
    }
}