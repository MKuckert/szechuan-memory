using System.Runtime.InteropServices;
using BenchmarkDotNet.Running;

T FromBytes<T>(byte[] arr) where T : struct
{
    T str = default(T);

    GCHandle h = default(GCHandle);

    try
    {
        h = GCHandle.Alloc(arr, GCHandleType.Pinned);

        str = Marshal.PtrToStructure<T>(h.AddrOfPinnedObject());
    }
    finally
    {
        if (h.IsAllocated)
        {
            h.Free();
        }
    }

    return str;
}

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);