namespace Szechuan.Memory;

internal static class SpanExtensions
{
    public static void Swap<T>(this Span<T> destination, int a, int b)
        => (destination[a], destination[b]) = (destination[b], destination[a]);

    public static void Reverse<T>(this Span<T> destination, int start, int length)
        => destination.Slice(start, length).Reverse();
}