using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Szechuan.Memory.Benchmark;

[SimpleJob(RuntimeMoniker.Net60, baseline: true)]
[MemoryDiagnoser]
public class ManagedLittleEndianBenchmark
{
    private readonly ManagedLittleEndian converter = new();
    private readonly Memory<byte> memory = new byte[1000];

    [Benchmark(Baseline = true)]
    public void WriteInts()
        => converter.Write(memory.Span, 100);
}