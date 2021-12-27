using System.Buffers;

namespace Szechuan.Memory.Buffers;

public static class MemoryOwnerExtensions
{
    public static IMemoryOwner<T> Slice<T>(this IMemoryOwner<T> owner, int start)
        => new SlicedMemoryOwner<T>(owner, start);

    public static IMemoryOwner<T> Slice<T>(this IMemoryOwner<T> owner, int start, int length)
        => new SlicedMemoryOwner<T>(owner, start, length);
}