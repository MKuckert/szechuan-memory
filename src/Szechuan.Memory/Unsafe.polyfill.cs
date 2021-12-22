#if !HAS_UNSAFE
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Szechuan.Memory;

public static class Unsafe
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int SizeOf<T>()
        => Marshal.SizeOf<T>();
}
#endif