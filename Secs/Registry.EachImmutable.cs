using System;
using System.Linq;

namespace Secs
{
    public partial class Registry
    {
        public delegate void EachImmutable<T0>(Registry registry, T0 v0)
            where T0 : struct;

        public unsafe void Each<T0>(EachImmutable<T0> each, Filter filter = default)
            where T0 : struct
        {
            if (!_componentIds.TryGetValue(typeof(T0), out var id0))
            {
                return;
            }

            var components0 = _components[id0];
            var sparse0 = components0._sparse;
            var dense0 = components0.GetDense<T0>();

            var include = filter.Include;
            var exclude = filter.Exclude;
            var includesCount = include?.Length + 1 ?? 1;
            var componentIdsCount = includesCount + (exclude?.Length ?? 0);
            var componentIds = stackalloc int[componentIdsCount];

            componentIds[0] = id0;

            if (include != null)
            {
                FillComponentIds(include, componentIds, 1);
            }

            QuickSort(componentIds, 0, includesCount - 1, 1);
            if (exclude != null)
            {
                FillComponentIds(exclude, componentIds, includesCount);
                QuickSort(componentIds, includesCount, componentIdsCount - 1, -1);
            }

            var compactIncludeId = FindCompactIncludeId(componentIds, includesCount);

            includesCount--;

            var components = _components[compactIncludeId];
            var entities = components._entities;
            var finalFilterCount = componentIdsCount - 1;
            var sparses = stackalloc int*[finalFilterCount];

            var x = 0;

            for (var i = 0; i < componentIdsCount; ++i)
            {
                if (componentIds[i] == compactIncludeId)
                {
                    continue;
                }

                fixed (int* sparsePointer = _components[componentIds[i]]._sparse)
                {
                    sparses[x] = sparsePointer;
                }

                x++;
            }

            var match = true;
            var entity = -1;
            var entityIndex = components._count - 1;
            var componentIndex = 0;

            MoveNextEntity:
            if (entityIndex >= 0)
            {
                entity = entities[entityIndex];
                match = true;

                MoveNextComponent:
                if (match && componentIndex < finalFilterCount)
                {
                    match = sparses[componentIndex][entity] > 0 == componentIndex < includesCount;

                    componentIndex++;
                    goto MoveNextComponent;
                }

                if (match)
                {
                    each(this, dense0[sparse0[entity] - 1]);
                }

                componentIndex = 0;
                entityIndex--;
                goto MoveNextEntity;
            }
        }

        public delegate void EachImmutable<T0, T1>(Registry registry, T0 v0, T1 v1)
            where T0 : struct
            where T1 : struct;

        public unsafe void Each<T0, T1>(EachImmutable<T0, T1> each, Filter filter = default)
            where T0 : struct
            where T1 : struct
        {
            if (!_componentIds.TryGetValue(typeof(T0), out var id0))
            {
                return;
            }

            var components0 = _components[id0];
            var sparse0 = components0._sparse;
            var dense0 = components0.GetDense<T0>();

            if (!_componentIds.TryGetValue(typeof(T1), out var id1))
            {
                return;
            }

            var components1 = _components[id1];
            var sparse1 = components1._sparse;
            var dense1 = components1.GetDense<T1>();

            var include = filter.Include;
            var exclude = filter.Exclude;
            var includesCount = include?.Length + 2 ?? 2;
            var componentIdsCount = includesCount + (exclude?.Length ?? 0);
            var componentIds = stackalloc int[componentIdsCount];

            componentIds[0] = id0;
            componentIds[1] = id1;

            if (include != null)
            {
                FillComponentIds(include, componentIds, 2);
            }

            QuickSort(componentIds, 0, includesCount - 1, 1);
            if (exclude != null)
            {
                FillComponentIds(exclude, componentIds, includesCount);
                QuickSort(componentIds, includesCount, componentIdsCount - 1, -1);
            }

            var compactIncludeId = FindCompactIncludeId(componentIds, includesCount);

            includesCount--;

            var components = _components[compactIncludeId];
            var entities = components._entities;
            var finalFilterCount = componentIdsCount - 1;
            var sparses = stackalloc int*[finalFilterCount];

            var x = 0;

            for (var i = 0; i < componentIdsCount; ++i)
            {
                if (componentIds[i] == compactIncludeId)
                {
                    continue;
                }

                fixed (int* sparsePointer = _components[componentIds[i]]._sparse)
                {
                    sparses[x] = sparsePointer;
                }

                x++;
            }

            var match = true;
            var entity = -1;
            var entityIndex = components._count - 1;
            var componentIndex = 0;

            MoveNextEntity:
            if (entityIndex >= 0)
            {
                entity = entities[entityIndex];
                match = true;

                MoveNextComponent:
                if (match && componentIndex < finalFilterCount)
                {
                    match = sparses[componentIndex][entity] > 0 == componentIndex < includesCount;

                    componentIndex++;
                    goto MoveNextComponent;
                }

                if (match)
                {
                    each(this, dense0[sparse0[entity] - 1], dense1[sparse1[entity] - 1]);
                }

                componentIndex = 0;
                entityIndex--;
                goto MoveNextEntity;
            }
        }

        public delegate void EachImmutable<T0, T1, T2>(Registry registry, T0 v0, T1 v1, T2 v2)
            where T0 : struct
            where T1 : struct
            where T2 : struct;

        public unsafe void Each<T0, T1, T2>(EachImmutable<T0, T1, T2> each, Filter filter = default)
            where T0 : struct
            where T1 : struct
            where T2 : struct
        {
            if (!_componentIds.TryGetValue(typeof(T0), out var id0))
            {
                return;
            }

            var components0 = _components[id0];
            var sparse0 = components0._sparse;
            var dense0 = components0.GetDense<T0>();

            if (!_componentIds.TryGetValue(typeof(T1), out var id1))
            {
                return;
            }

            var components1 = _components[id1];
            var sparse1 = components1._sparse;
            var dense1 = components1.GetDense<T1>();

            if (!_componentIds.TryGetValue(typeof(T2), out var id2))
            {
                return;
            }

            var components2 = _components[id2];
            var sparse2 = components2._sparse;
            var dense2 = components2.GetDense<T2>();

            var include = filter.Include;
            var exclude = filter.Exclude;
            var includesCount = include?.Length + 3 ?? 3;
            var componentIdsCount = includesCount + (exclude?.Length ?? 0);
            var componentIds = stackalloc int[componentIdsCount];

            componentIds[0] = id0;
            componentIds[1] = id1;
            componentIds[2] = id2;

            if (include != null)
            {
                FillComponentIds(include, componentIds, 3);
            }

            QuickSort(componentIds, 0, includesCount - 1, 1);
            if (exclude != null)
            {
                FillComponentIds(exclude, componentIds, includesCount);
                QuickSort(componentIds, includesCount, componentIdsCount - 1, -1);
            }

            var compactIncludeId = FindCompactIncludeId(componentIds, includesCount);

            includesCount--;

            var components = _components[compactIncludeId];
            var entities = components._entities;
            var finalFilterCount = componentIdsCount - 1;
            var sparses = stackalloc int*[finalFilterCount];

            var x = 0;

            for (var i = 0; i < componentIdsCount; ++i)
            {
                if (componentIds[i] == compactIncludeId)
                {
                    continue;
                }

                fixed (int* sparsePointer = _components[componentIds[i]]._sparse)
                {
                    sparses[x] = sparsePointer;
                }

                x++;
            }

            var match = true;
            var entity = -1;
            var entityIndex = components._count - 1;
            var componentIndex = 0;

            MoveNextEntity:
            if (entityIndex >= 0)
            {
                entity = entities[entityIndex];
                match = true;

                MoveNextComponent:
                if (match && componentIndex < finalFilterCount)
                {
                    match = sparses[componentIndex][entity] > 0 == componentIndex < includesCount;

                    componentIndex++;
                    goto MoveNextComponent;
                }

                if (match)
                {
                    each(this, dense0[sparse0[entity] - 1], dense1[sparse1[entity] - 1], dense2[sparse2[entity] - 1]);
                }

                componentIndex = 0;
                entityIndex--;
                goto MoveNextEntity;
            }
        }

        public delegate void EachImmutable<T0, T1, T2, T3>(Registry registry, T0 v0, T1 v1, T2 v2, T3 v3)
            where T0 : struct
            where T1 : struct
            where T2 : struct
            where T3 : struct;

        public unsafe void Each<T0, T1, T2, T3>(EachImmutable<T0, T1, T2, T3> each, Filter filter = default)
            where T0 : struct
            where T1 : struct
            where T2 : struct
            where T3 : struct
        {
            if (!_componentIds.TryGetValue(typeof(T0), out var id0))
            {
                return;
            }

            var components0 = _components[id0];
            var sparse0 = components0._sparse;
            var dense0 = components0.GetDense<T0>();

            if (!_componentIds.TryGetValue(typeof(T1), out var id1))
            {
                return;
            }

            var components1 = _components[id1];
            var sparse1 = components1._sparse;
            var dense1 = components1.GetDense<T1>();

            if (!_componentIds.TryGetValue(typeof(T2), out var id2))
            {
                return;
            }

            var components2 = _components[id2];
            var sparse2 = components2._sparse;
            var dense2 = components2.GetDense<T2>();

            if (!_componentIds.TryGetValue(typeof(T3), out var id3))
            {
                return;
            }

            var components3 = _components[id3];
            var sparse3 = components3._sparse;
            var dense3 = components3.GetDense<T3>();

            var include = filter.Include;
            var exclude = filter.Exclude;
            var includesCount = include?.Length + 4 ?? 4;
            var componentIdsCount = includesCount + (exclude?.Length ?? 0);
            var componentIds = stackalloc int[componentIdsCount];

            componentIds[0] = id0;
            componentIds[1] = id1;
            componentIds[2] = id2;
            componentIds[3] = id3;

            if (include != null)
            {
                FillComponentIds(include, componentIds, 4);
            }

            QuickSort(componentIds, 0, includesCount - 1, 1);
            if (exclude != null)
            {
                FillComponentIds(exclude, componentIds, includesCount);
                QuickSort(componentIds, includesCount, componentIdsCount - 1, -1);
            }

            var compactIncludeId = FindCompactIncludeId(componentIds, includesCount);

            includesCount--;

            var components = _components[compactIncludeId];
            var entities = components._entities;
            var finalFilterCount = componentIdsCount - 1;
            var sparses = stackalloc int*[finalFilterCount];

            var x = 0;

            for (var i = 0; i < componentIdsCount; ++i)
            {
                if (componentIds[i] == compactIncludeId)
                {
                    continue;
                }

                fixed (int* sparsePointer = _components[componentIds[i]]._sparse)
                {
                    sparses[x] = sparsePointer;
                }

                x++;
            }

            var match = true;
            var entity = -1;
            var entityIndex = components._count - 1;
            var componentIndex = 0;

            MoveNextEntity:
            if (entityIndex >= 0)
            {
                entity = entities[entityIndex];
                match = true;

                MoveNextComponent:
                if (match && componentIndex < finalFilterCount)
                {
                    match = sparses[componentIndex][entity] > 0 == componentIndex < includesCount;

                    componentIndex++;
                    goto MoveNextComponent;
                }

                if (match)
                {
                    each(this, dense0[sparse0[entity] - 1], dense1[sparse1[entity] - 1], dense2[sparse2[entity] - 1],
                        dense3[sparse3[entity] - 1]);
                }

                componentIndex = 0;
                entityIndex--;
                goto MoveNextEntity;
            }
        }

        public delegate void EachImmutable<T0, T1, T2, T3, T4>(Registry registry, T0 v0, T1 v1, T2 v2, T3 v3, T4 v4)
            where T0 : struct
            where T1 : struct
            where T2 : struct
            where T3 : struct
            where T4 : struct;

        public unsafe void Each<T0, T1, T2, T3, T4>(EachImmutable<T0, T1, T2, T3, T4> each, Filter filter = default)
            where T0 : struct
            where T1 : struct
            where T2 : struct
            where T3 : struct
            where T4 : struct
        {
            if (!_componentIds.TryGetValue(typeof(T0), out var id0))
            {
                return;
            }

            var components0 = _components[id0];
            var sparse0 = components0._sparse;
            var dense0 = components0.GetDense<T0>();

            if (!_componentIds.TryGetValue(typeof(T1), out var id1))
            {
                return;
            }

            var components1 = _components[id1];
            var sparse1 = components1._sparse;
            var dense1 = components1.GetDense<T1>();

            if (!_componentIds.TryGetValue(typeof(T2), out var id2))
            {
                return;
            }

            var components2 = _components[id2];
            var sparse2 = components2._sparse;
            var dense2 = components2.GetDense<T2>();

            if (!_componentIds.TryGetValue(typeof(T3), out var id3))
            {
                return;
            }

            var components3 = _components[id3];
            var sparse3 = components3._sparse;
            var dense3 = components3.GetDense<T3>();

            if (!_componentIds.TryGetValue(typeof(T4), out var id4))
            {
                return;
            }

            var components4 = _components[id4];
            var sparse4 = components4._sparse;
            var dense4 = components4.GetDense<T4>();

            var include = filter.Include;
            var exclude = filter.Exclude;
            var includesCount = include?.Length + 5 ?? 5;
            var componentIdsCount = includesCount + (exclude?.Length ?? 0);
            var componentIds = stackalloc int[componentIdsCount];

            componentIds[0] = id0;
            componentIds[1] = id1;
            componentIds[2] = id2;
            componentIds[3] = id3;
            componentIds[4] = id4;

            if (include != null)
            {
                FillComponentIds(include, componentIds, 5);
            }

            QuickSort(componentIds, 0, includesCount - 1, 1);
            if (exclude != null)
            {
                FillComponentIds(exclude, componentIds, includesCount);
                QuickSort(componentIds, includesCount, componentIdsCount - 1, -1);
            }

            var compactIncludeId = FindCompactIncludeId(componentIds, includesCount);

            includesCount--;

            var components = _components[compactIncludeId];
            var entities = components._entities;
            var finalFilterCount = componentIdsCount - 1;
            var sparses = stackalloc int*[finalFilterCount];

            var x = 0;

            for (var i = 0; i < componentIdsCount; ++i)
            {
                if (componentIds[i] == compactIncludeId)
                {
                    continue;
                }

                fixed (int* sparsePointer = _components[componentIds[i]]._sparse)
                {
                    sparses[x] = sparsePointer;
                }

                x++;
            }

            var match = true;
            var entity = -1;
            var entityIndex = components._count - 1;
            var componentIndex = 0;

            MoveNextEntity:
            if (entityIndex >= 0)
            {
                entity = entities[entityIndex];
                match = true;

                MoveNextComponent:
                if (match && componentIndex < finalFilterCount)
                {
                    match = sparses[componentIndex][entity] > 0 == componentIndex < includesCount;

                    componentIndex++;
                    goto MoveNextComponent;
                }

                if (match)
                {
                    each(this, dense0[sparse0[entity] - 1], dense1[sparse1[entity] - 1], dense2[sparse2[entity] - 1],
                        dense3[sparse3[entity] - 1], dense4[sparse4[entity] - 1]);
                }

                componentIndex = 0;
                entityIndex--;
                goto MoveNextEntity;
            }
        }

        public delegate void EachImmutable<T0, T1, T2, T3, T4, T5>(Registry registry, T0 v0, T1 v1, T2 v2, T3 v3, T4 v4,
            T5 v5)
            where T0 : struct
            where T1 : struct
            where T2 : struct
            where T3 : struct
            where T4 : struct
            where T5 : struct;

        public unsafe void Each<T0, T1, T2, T3, T4, T5>(EachImmutable<T0, T1, T2, T3, T4, T5> each,
            Filter filter = default)
            where T0 : struct
            where T1 : struct
            where T2 : struct
            where T3 : struct
            where T4 : struct
            where T5 : struct
        {
            if (!_componentIds.TryGetValue(typeof(T0), out var id0))
            {
                return;
            }

            var components0 = _components[id0];
            var sparse0 = components0._sparse;
            var dense0 = components0.GetDense<T0>();

            if (!_componentIds.TryGetValue(typeof(T1), out var id1))
            {
                return;
            }

            var components1 = _components[id1];
            var sparse1 = components1._sparse;
            var dense1 = components1.GetDense<T1>();

            if (!_componentIds.TryGetValue(typeof(T2), out var id2))
            {
                return;
            }

            var components2 = _components[id2];
            var sparse2 = components2._sparse;
            var dense2 = components2.GetDense<T2>();

            if (!_componentIds.TryGetValue(typeof(T3), out var id3))
            {
                return;
            }

            var components3 = _components[id3];
            var sparse3 = components3._sparse;
            var dense3 = components3.GetDense<T3>();

            if (!_componentIds.TryGetValue(typeof(T4), out var id4))
            {
                return;
            }

            var components4 = _components[id4];
            var sparse4 = components4._sparse;
            var dense4 = components4.GetDense<T4>();

            if (!_componentIds.TryGetValue(typeof(T5), out var id5))
            {
                return;
            }

            var components5 = _components[id5];
            var sparse5 = components5._sparse;
            var dense5 = components5.GetDense<T5>();

            var include = filter.Include;
            var exclude = filter.Exclude;
            var includesCount = include?.Length + 6 ?? 6;
            var componentIdsCount = includesCount + (exclude?.Length ?? 0);
            var componentIds = stackalloc int[componentIdsCount];

            componentIds[0] = id0;
            componentIds[1] = id1;
            componentIds[2] = id2;
            componentIds[3] = id3;
            componentIds[4] = id4;
            componentIds[5] = id5;

            if (include != null)
            {
                FillComponentIds(include, componentIds, 6);
            }

            QuickSort(componentIds, 0, includesCount - 1, 1);
            if (exclude != null)
            {
                FillComponentIds(exclude, componentIds, includesCount);
                QuickSort(componentIds, includesCount, componentIdsCount - 1, -1);
            }

            var compactIncludeId = FindCompactIncludeId(componentIds, includesCount);

            includesCount--;

            var components = _components[compactIncludeId];
            var entities = components._entities;
            var finalFilterCount = componentIdsCount - 1;
            var sparses = stackalloc int*[finalFilterCount];

            var x = 0;

            for (var i = 0; i < componentIdsCount; ++i)
            {
                if (componentIds[i] == compactIncludeId)
                {
                    continue;
                }

                fixed (int* sparsePointer = _components[componentIds[i]]._sparse)
                {
                    sparses[x] = sparsePointer;
                }

                x++;
            }

            var match = true;
            var entity = -1;
            var entityIndex = components._count - 1;
            var componentIndex = 0;

            MoveNextEntity:
            if (entityIndex >= 0)
            {
                entity = entities[entityIndex];
                match = true;

                MoveNextComponent:
                if (match && componentIndex < finalFilterCount)
                {
                    match = sparses[componentIndex][entity] > 0 == componentIndex < includesCount;

                    componentIndex++;
                    goto MoveNextComponent;
                }

                if (match)
                {
                    each(this, dense0[sparse0[entity] - 1], dense1[sparse1[entity] - 1], dense2[sparse2[entity] - 1],
                        dense3[sparse3[entity] - 1], dense4[sparse4[entity] - 1], dense5[sparse5[entity] - 1]);
                }

                componentIndex = 0;
                entityIndex--;
                goto MoveNextEntity;
            }
        }

        public delegate void EachImmutable<T0, T1, T2, T3, T4, T5, T6>(Registry registry, T0 v0, T1 v1, T2 v2, T3 v3,
            T4 v4, T5 v5, T6 v6)
            where T0 : struct
            where T1 : struct
            where T2 : struct
            where T3 : struct
            where T4 : struct
            where T5 : struct
            where T6 : struct;

        public unsafe void Each<T0, T1, T2, T3, T4, T5, T6>(EachImmutable<T0, T1, T2, T3, T4, T5, T6> each,
            Filter filter = default)
            where T0 : struct
            where T1 : struct
            where T2 : struct
            where T3 : struct
            where T4 : struct
            where T5 : struct
            where T6 : struct
        {
            if (!_componentIds.TryGetValue(typeof(T0), out var id0))
            {
                return;
            }

            var components0 = _components[id0];
            var sparse0 = components0._sparse;
            var dense0 = components0.GetDense<T0>();

            if (!_componentIds.TryGetValue(typeof(T1), out var id1))
            {
                return;
            }

            var components1 = _components[id1];
            var sparse1 = components1._sparse;
            var dense1 = components1.GetDense<T1>();

            if (!_componentIds.TryGetValue(typeof(T2), out var id2))
            {
                return;
            }

            var components2 = _components[id2];
            var sparse2 = components2._sparse;
            var dense2 = components2.GetDense<T2>();

            if (!_componentIds.TryGetValue(typeof(T3), out var id3))
            {
                return;
            }

            var components3 = _components[id3];
            var sparse3 = components3._sparse;
            var dense3 = components3.GetDense<T3>();

            if (!_componentIds.TryGetValue(typeof(T4), out var id4))
            {
                return;
            }

            var components4 = _components[id4];
            var sparse4 = components4._sparse;
            var dense4 = components4.GetDense<T4>();

            if (!_componentIds.TryGetValue(typeof(T5), out var id5))
            {
                return;
            }

            var components5 = _components[id5];
            var sparse5 = components5._sparse;
            var dense5 = components5.GetDense<T5>();

            if (!_componentIds.TryGetValue(typeof(T6), out var id6))
            {
                return;
            }

            var components6 = _components[id6];
            var sparse6 = components6._sparse;
            var dense6 = components6.GetDense<T6>();

            var include = filter.Include;
            var exclude = filter.Exclude;
            var includesCount = include?.Length + 7 ?? 7;
            var componentIdsCount = includesCount + (exclude?.Length ?? 0);
            var componentIds = stackalloc int[componentIdsCount];

            componentIds[0] = id0;
            componentIds[1] = id1;
            componentIds[2] = id2;
            componentIds[3] = id3;
            componentIds[4] = id4;
            componentIds[5] = id5;
            componentIds[6] = id6;

            if (include != null)
            {
                FillComponentIds(include, componentIds, 7);
            }

            QuickSort(componentIds, 0, includesCount - 1, 1);
            if (exclude != null)
            {
                FillComponentIds(exclude, componentIds, includesCount);
                QuickSort(componentIds, includesCount, componentIdsCount - 1, -1);
            }

            var compactIncludeId = FindCompactIncludeId(componentIds, includesCount);

            includesCount--;

            var components = _components[compactIncludeId];
            var entities = components._entities;
            var finalFilterCount = componentIdsCount - 1;
            var sparses = stackalloc int*[finalFilterCount];

            var x = 0;

            for (var i = 0; i < componentIdsCount; ++i)
            {
                if (componentIds[i] == compactIncludeId)
                {
                    continue;
                }

                fixed (int* sparsePointer = _components[componentIds[i]]._sparse)
                {
                    sparses[x] = sparsePointer;
                }

                x++;
            }

            var match = true;
            var entity = -1;
            var entityIndex = components._count - 1;
            var componentIndex = 0;

            MoveNextEntity:
            if (entityIndex >= 0)
            {
                entity = entities[entityIndex];
                match = true;

                MoveNextComponent:
                if (match && componentIndex < finalFilterCount)
                {
                    match = sparses[componentIndex][entity] > 0 == componentIndex < includesCount;

                    componentIndex++;
                    goto MoveNextComponent;
                }

                if (match)
                {
                    each(this, dense0[sparse0[entity] - 1], dense1[sparse1[entity] - 1], dense2[sparse2[entity] - 1],
                        dense3[sparse3[entity] - 1], dense4[sparse4[entity] - 1], dense5[sparse5[entity] - 1],
                        dense6[sparse6[entity] - 1]);
                }

                componentIndex = 0;
                entityIndex--;
                goto MoveNextEntity;
            }
        }

        public delegate void EachImmutable<T0, T1, T2, T3, T4, T5, T6, T7>(Registry registry, T0 v0, T1 v1, T2 v2,
            T3 v3, T4 v4, T5 v5, T6 v6, T7 v7)
            where T0 : struct
            where T1 : struct
            where T2 : struct
            where T3 : struct
            where T4 : struct
            where T5 : struct
            where T6 : struct
            where T7 : struct;

        public unsafe void Each<T0, T1, T2, T3, T4, T5, T6, T7>(EachImmutable<T0, T1, T2, T3, T4, T5, T6, T7> each,
            Filter filter = default)
            where T0 : struct
            where T1 : struct
            where T2 : struct
            where T3 : struct
            where T4 : struct
            where T5 : struct
            where T6 : struct
            where T7 : struct
        {
            if (!_componentIds.TryGetValue(typeof(T0), out var id0))
            {
                return;
            }

            var components0 = _components[id0];
            var sparse0 = components0._sparse;
            var dense0 = components0.GetDense<T0>();

            if (!_componentIds.TryGetValue(typeof(T1), out var id1))
            {
                return;
            }

            var components1 = _components[id1];
            var sparse1 = components1._sparse;
            var dense1 = components1.GetDense<T1>();

            if (!_componentIds.TryGetValue(typeof(T2), out var id2))
            {
                return;
            }

            var components2 = _components[id2];
            var sparse2 = components2._sparse;
            var dense2 = components2.GetDense<T2>();

            if (!_componentIds.TryGetValue(typeof(T3), out var id3))
            {
                return;
            }

            var components3 = _components[id3];
            var sparse3 = components3._sparse;
            var dense3 = components3.GetDense<T3>();

            if (!_componentIds.TryGetValue(typeof(T4), out var id4))
            {
                return;
            }

            var components4 = _components[id4];
            var sparse4 = components4._sparse;
            var dense4 = components4.GetDense<T4>();

            if (!_componentIds.TryGetValue(typeof(T5), out var id5))
            {
                return;
            }

            var components5 = _components[id5];
            var sparse5 = components5._sparse;
            var dense5 = components5.GetDense<T5>();

            if (!_componentIds.TryGetValue(typeof(T6), out var id6))
            {
                return;
            }

            var components6 = _components[id6];
            var sparse6 = components6._sparse;
            var dense6 = components6.GetDense<T6>();

            if (!_componentIds.TryGetValue(typeof(T7), out var id7))
            {
                return;
            }

            var components7 = _components[id7];
            var sparse7 = components7._sparse;
            var dense7 = components7.GetDense<T7>();

            var include = filter.Include;
            var exclude = filter.Exclude;
            var includesCount = include?.Length + 8 ?? 8;
            var componentIdsCount = includesCount + (exclude?.Length ?? 0);
            var componentIds = stackalloc int[componentIdsCount];

            componentIds[0] = id0;
            componentIds[1] = id1;
            componentIds[2] = id2;
            componentIds[3] = id3;
            componentIds[4] = id4;
            componentIds[5] = id5;
            componentIds[6] = id6;
            componentIds[7] = id7;

            if (include != null)
            {
                FillComponentIds(include, componentIds, 8);
            }

            QuickSort(componentIds, 0, includesCount - 1, 1);
            if (exclude != null)
            {
                FillComponentIds(exclude, componentIds, includesCount);
                QuickSort(componentIds, includesCount, componentIdsCount - 1, -1);
            }

            var compactIncludeId = FindCompactIncludeId(componentIds, includesCount);

            includesCount--;

            var components = _components[compactIncludeId];
            var entities = components._entities;
            var finalFilterCount = componentIdsCount - 1;
            var sparses = stackalloc int*[finalFilterCount];

            var x = 0;

            for (var i = 0; i < componentIdsCount; ++i)
            {
                if (componentIds[i] == compactIncludeId)
                {
                    continue;
                }

                fixed (int* sparsePointer = _components[componentIds[i]]._sparse)
                {
                    sparses[x] = sparsePointer;
                }

                x++;
            }

            var match = true;
            var entity = -1;
            var entityIndex = components._count - 1;
            var componentIndex = 0;

            MoveNextEntity:
            if (entityIndex >= 0)
            {
                entity = entities[entityIndex];
                match = true;

                MoveNextComponent:
                if (match && componentIndex < finalFilterCount)
                {
                    match = sparses[componentIndex][entity] > 0 == componentIndex < includesCount;

                    componentIndex++;
                    goto MoveNextComponent;
                }

                if (match)
                {
                    each(this, dense0[sparse0[entity] - 1], dense1[sparse1[entity] - 1], dense2[sparse2[entity] - 1],
                        dense3[sparse3[entity] - 1], dense4[sparse4[entity] - 1], dense5[sparse5[entity] - 1],
                        dense6[sparse6[entity] - 1], dense7[sparse7[entity] - 1]);
                }

                componentIndex = 0;
                entityIndex--;
                goto MoveNextEntity;
            }
        }
    }
}