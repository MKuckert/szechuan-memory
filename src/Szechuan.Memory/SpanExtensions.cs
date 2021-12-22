namespace Szechuan.Memory;

internal static class SpanExtensions
{
    public static void Swap<T>(this Span<T> destination, int a, int b)
    {
        (destination[a], destination[b]) = (destination[b], destination[a]);
    }

    public static void Reverse<T>(this Span<T> destination, int start, int length)
    {
        if (length % 2 == 1)
        {
            throw new ArgumentException($"Has to be a multiple of 2 but was {length}", nameof(length));
        }

        var n = length / 2;
        for (var i = 0; i < n; i++)
        {
            destination.Swap(i, length - i - 1);
        }
    }
}