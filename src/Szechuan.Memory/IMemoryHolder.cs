namespace Szechuan.Memory;

internal interface IMemoryHolder
{
    Memory<byte> Memory { get; }
    int Length { get; }
}