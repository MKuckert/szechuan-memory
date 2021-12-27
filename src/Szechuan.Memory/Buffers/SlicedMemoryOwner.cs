using System.Buffers;

namespace Szechuan.Memory.Buffers
{
    internal readonly struct SlicedMemoryOwner<T> : IMemoryOwner<T>, IEquatable<SlicedMemoryOwner<T>>
    {
        private readonly IMemoryOwner<T> owner;

        public SlicedMemoryOwner(IMemoryOwner<T> owner, int start)
        {
            this.owner = owner;
            Memory = owner.Memory[start..];
        }

        public SlicedMemoryOwner(IMemoryOwner<T> owner, int start, int length)
        {
            this.owner = owner;
            Memory = owner.Memory.Slice(start, length);
        }

        public void Dispose()
            => owner.Dispose();

        public Memory<T> Memory { get; }

        #region Equality

        public bool Equals(SlicedMemoryOwner<T> other)
            => owner.Equals(other.owner) && Memory.Equals(other.Memory);

        public override bool Equals(object? obj)
            => obj is SlicedMemoryOwner<T> other && Equals(other);

        public override int GetHashCode()
            => HashCode.Combine(owner, Memory);

        public static bool operator ==(SlicedMemoryOwner<T> left, SlicedMemoryOwner<T> right)
            => left.Equals(right);

        public static bool operator !=(SlicedMemoryOwner<T> left, SlicedMemoryOwner<T> right)
            => !left.Equals(right);

        #endregion
    }
}