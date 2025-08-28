using System;
using Unity.IL2CPP.CompilerServices;

namespace Secs
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public partial class Registry
    {
        internal unsafe void FillComponentIds(Type[] componentTypes, int* componentIds, int startFrom = 0)
        {
            var count = componentTypes.Length;

            for (var i = 0; i < count; ++i)
            {
                componentIds[i + startFrom] = _componentIds[componentTypes[i]];
            }
        }

        internal unsafe int FindCompactIncludeId(int* componentIds, int includesCount)
        {
            var minIncludeId = int.MaxValue;

            for (var i = 0; i < includesCount; i++)
            {
                var id = componentIds[i];

                minIncludeId = _components[id]._count < minIncludeId 
                    ? id
                    : minIncludeId;
            }
            
            return minIncludeId;
        }
        
        public static unsafe void QuickSort(int* array, int left, int right, int orderSign)
        {
            if (orderSign < 0)
            {
                QuickSortNegative(array, left, right);
            }
            else
            {
                QuickSortPositive(array, left, right);
            }
        }
        
        private static unsafe void QuickSortNegative(int* array, int left, int right)
        {
            if (left >= right)
            {
                return;
            }

            var pivot = array[(left + right) / 2];
            var i = left;
            var j = right;

            while (i <= j)
            {
                while (array[i] > pivot)
                {
                    i++;
                }

                while (array[j] < pivot)
                {
                    j--;
                }

                if (i <= j)
                {
                    (array[i], array[j]) = (array[j], array[i]);

                    i++;
                    j--;
                }
            }

            if (left < j)
            {
                QuickSortNegative(array, left, j);
            }

            if (i < right)
            {
                QuickSortNegative(array, i, right);
            }
        }
        
        private static unsafe void QuickSortPositive(int* array, int left, int right)
        {
            if (left >= right)
            {
                return;
            }

            var pivot = array[(left + right) / 2];
            var i = left;
            var j = right;

            while (i <= j)
            {
                while (array[i] < pivot)
                {
                    i++;
                }

                while (array[j] > pivot)
                {
                    j--;
                }

                if (i <= j)
                {
                    (array[i], array[j]) = (array[j], array[i]);

                    i++;
                    j--;
                }
            }
            
            if (left < j)
            {
                QuickSortPositive(array, left, j);
            }

            if (i < right)
            {
                QuickSortPositive(array, i, right);
            }
        }
    }
}