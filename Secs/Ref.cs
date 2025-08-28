using System.Runtime.CompilerServices;

namespace Secs
{
    public struct Ref<T> where T : struct
    {
        public ref T Value => ref GetValue();

        internal unsafe T* _dense;
        internal int[] _sparse;
        internal int _entity;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe ref T GetValue()
        {
            return ref _dense[_sparse[_entity] - 1];
        }
    }
}