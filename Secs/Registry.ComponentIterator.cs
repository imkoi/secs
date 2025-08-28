using System.Runtime.CompilerServices;

namespace Secs
{
    public partial class Registry
    {
        public ComponentIterator<T0> Each<T0>(Filter filter = default)
            where T0 : struct
        {
            return new ComponentIterator<T0>(this, filter);
        }

        public unsafe struct ComponentIterator<T0>
            where T0 : struct
        {
            private readonly int _finalFilterCount;
            private readonly int _includesCount;
            private readonly int[] _entities;
            private readonly int[][] _sparses;

            private int _entityIndex;
            public Ref<T0> Current { get; set; }

            private Ref<T0> _current;

            public ComponentIterator(Registry registry, Filter filter)
            {
                var registryComponentIds = registry._componentIds;
                var registryComponents = registry._components;

                var include = filter.Include;
                var exclude = filter.Exclude;
                var includesCount = include?.Length + 1 ?? 1;
                var componentIdsCount = includesCount + (exclude?.Length ?? 0);
                var componentIds = stackalloc int[componentIdsCount];

                var sparse0 = default(int[]);
                var dense0 = default(T0*);
                var id0 = -1;
                var hasAllQueryComponents = registryComponentIds.TryGetValue(typeof(T0), out id0);
                if (hasAllQueryComponents)
                {
                    var components0 = registryComponents[id0];
                    sparse0 = components0._sparse;
                    dense0 = components0.GetDense<T0>();
                    componentIds[0] = id0;
                }

                if (include != null)
                {
                    registry.FillComponentIds(include, componentIds, 1);
                }

                QuickSort(componentIds, 0, includesCount - 1, 1);
                if (exclude != null)
                {
                    registry.FillComponentIds(exclude, componentIds, includesCount);
                    QuickSort(componentIds, includesCount, componentIdsCount - 1, -1);
                }

                var compactIncludeId = registry.FindCompactIncludeId(componentIds, includesCount);

                var components = registryComponents[compactIncludeId];
                var entities = components._entities;
                var finalFilterCount = componentIdsCount - 1;
                var sparses = _stack.Count > 0 ? _stack.Pop() : new int[64][];

                var x = 0;

                for (var i = 0; i < componentIdsCount; ++i)
                {
                    if (componentIds[i] == compactIncludeId)
                    {
                        continue;
                    }

                    sparses[x] = registryComponents[componentIds[i]]._sparse;
                    x++;
                }

                _entities = entities;
                _sparses = sparses;
                _entityIndex = components._count - 1;
                _includesCount = includesCount - 1;
                _finalFilterCount = finalFilterCount;
                _current = new Ref<T0> { _sparse = sparse0, _dense = dense0, };
                Current = _current;
            }

            public ComponentIterator<T0> GetEnumerator() => this;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                var match = true;
                var entity = -1;
                var entityIndex = _entityIndex;
                var sparses = _sparses;
                var includesCount = _includesCount;
                var finalFilterCount = _finalFilterCount;
                var entities = _entities;
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

                    componentIndex = 0;
                    entityIndex--;

                    if (!match)
                    {
                        goto MoveNextEntity;
                    }
                }

                _entityIndex = entityIndex;

                _current._entity = entity;

                Current = _current;

                if (entity >= 0)
                {
                    return true;
                }

                Dispose();
                return false;
            }

            private void Dispose()
            {
                for (var i = 0; i < _sparses.Length; ++i)
                {
                    if (_sparses[i] != null)
                    {
                        _sparses[i] = null;
                    }

                    else
                    {
                        break;
                    }
                }

                _stack.Push(_sparses);
            }
        }

        public ComponentIterator<T0, T1> Each<T0, T1>(Filter filter = default)
            where T0 : struct
            where T1 : struct
        {
            return new ComponentIterator<T0, T1>(this, filter);
        }

        public unsafe struct ComponentIterator<T0, T1>
            where T0 : struct
            where T1 : struct
        {
            private readonly int _finalFilterCount;
            private readonly int _includesCount;
            private readonly int[] _entities;
            private readonly int[][] _sparses;

            private int _entityIndex;
            public (Ref<T0>, Ref<T1>) Current { get; set; }

            private (Ref<T0>, Ref<T1>) _current;

            public ComponentIterator(Registry registry, Filter filter)
            {
                var registryComponentIds = registry._componentIds;
                var registryComponents = registry._components;

                var include = filter.Include;
                var exclude = filter.Exclude;
                var includesCount = include?.Length + 2 ?? 2;
                var componentIdsCount = includesCount + (exclude?.Length ?? 0);
                var componentIds = stackalloc int[componentIdsCount];

                var sparse0 = default(int[]);
                var dense0 = default(T0*);
                var id0 = -1;
                var sparse1 = default(int[]);
                var dense1 = default(T1*);
                var id1 = -1;
                var hasAllQueryComponents = registryComponentIds.TryGetValue(typeof(T0), out id0) &&
                                            registryComponentIds.TryGetValue(typeof(T1), out id1);
                if (hasAllQueryComponents)
                {
                    var components0 = registryComponents[id0];
                    sparse0 = components0._sparse;
                    dense0 = components0.GetDense<T0>();
                    componentIds[0] = id0;

                    var components1 = registryComponents[id1];
                    sparse1 = components1._sparse;
                    dense1 = components1.GetDense<T1>();
                    componentIds[1] = id1;
                }

                if (include != null)
                {
                    registry.FillComponentIds(include, componentIds, 2);
                }

                QuickSort(componentIds, 0, includesCount - 1, 1);
                if (exclude != null)
                {
                    registry.FillComponentIds(exclude, componentIds, includesCount);
                    QuickSort(componentIds, includesCount, componentIdsCount - 1, -1);
                }

                var compactIncludeId = registry.FindCompactIncludeId(componentIds, includesCount);

                var components = registryComponents[compactIncludeId];
                var entities = components._entities;
                var finalFilterCount = componentIdsCount - 1;
                var sparses = _stack.Count > 0 ? _stack.Pop() : new int[64][];

                var x = 0;

                for (var i = 0; i < componentIdsCount; ++i)
                {
                    if (componentIds[i] == compactIncludeId)
                    {
                        continue;
                    }

                    sparses[x] = registryComponents[componentIds[i]]._sparse;
                    x++;
                }

                _entities = entities;
                _sparses = sparses;
                _entityIndex = components._count - 1;
                _includesCount = includesCount - 1;
                _finalFilterCount = finalFilterCount;
                _current = (new Ref<T0> { _sparse = sparse0, _dense = dense0, },
                    new Ref<T1> { _sparse = sparse1, _dense = dense1, });
                Current = _current;
            }

            public ComponentIterator<T0, T1> GetEnumerator() => this;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                var match = true;
                var entity = -1;
                var entityIndex = _entityIndex;
                var sparses = _sparses;
                var includesCount = _includesCount;
                var finalFilterCount = _finalFilterCount;
                var entities = _entities;
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

                    componentIndex = 0;
                    entityIndex--;

                    if (!match)
                    {
                        goto MoveNextEntity;
                    }
                }

                _entityIndex = entityIndex;


                Current = _current;

                if (entity >= 0)
                {
                    return true;
                }

                Dispose();
                return false;
            }

            private void Dispose()
            {
                for (var i = 0; i < _sparses.Length; ++i)
                {
                    if (_sparses[i] != null)
                    {
                        _sparses[i] = null;
                    }

                    else
                    {
                        break;
                    }
                }

                _stack.Push(_sparses);
            }
        }

        public ComponentIterator<T0, T1, T2> Each<T0, T1, T2>(Filter filter = default)
            where T0 : struct
            where T1 : struct
            where T2 : struct
        {
            return new ComponentIterator<T0, T1, T2>(this, filter);
        }

        public unsafe struct ComponentIterator<T0, T1, T2>
            where T0 : struct
            where T1 : struct
            where T2 : struct
        {
            private readonly int _finalFilterCount;
            private readonly int _includesCount;
            private readonly int[] _entities;
            private readonly int[][] _sparses;

            private int _entityIndex;
            public (Ref<T0>, Ref<T1>, Ref<T2>) Current { get; set; }

            private (Ref<T0>, Ref<T1>, Ref<T2>) _current;

            public ComponentIterator(Registry registry, Filter filter)
            {
                var registryComponentIds = registry._componentIds;
                var registryComponents = registry._components;

                var include = filter.Include;
                var exclude = filter.Exclude;
                var includesCount = include?.Length + 3 ?? 3;
                var componentIdsCount = includesCount + (exclude?.Length ?? 0);
                var componentIds = stackalloc int[componentIdsCount];

                var sparse0 = default(int[]);
                var dense0 = default(T0*);
                var id0 = -1;
                var sparse1 = default(int[]);
                var dense1 = default(T1*);
                var id1 = -1;
                var sparse2 = default(int[]);
                var dense2 = default(T2*);
                var id2 = -1;
                var hasAllQueryComponents = registryComponentIds.TryGetValue(typeof(T0), out id0) &&
                                            registryComponentIds.TryGetValue(typeof(T1), out id1) &&
                                            registryComponentIds.TryGetValue(typeof(T2), out id2);
                if (hasAllQueryComponents)
                {
                    var components0 = registryComponents[id0];
                    sparse0 = components0._sparse;
                    dense0 = components0.GetDense<T0>();
                    componentIds[0] = id0;

                    var components1 = registryComponents[id1];
                    sparse1 = components1._sparse;
                    dense1 = components1.GetDense<T1>();
                    componentIds[1] = id1;

                    var components2 = registryComponents[id2];
                    sparse2 = components2._sparse;
                    dense2 = components2.GetDense<T2>();
                    componentIds[2] = id2;
                }

                if (include != null)
                {
                    registry.FillComponentIds(include, componentIds, 3);
                }

                QuickSort(componentIds, 0, includesCount - 1, 1);
                if (exclude != null)
                {
                    registry.FillComponentIds(exclude, componentIds, includesCount);
                    QuickSort(componentIds, includesCount, componentIdsCount - 1, -1);
                }

                var compactIncludeId = registry.FindCompactIncludeId(componentIds, includesCount);

                var components = registryComponents[compactIncludeId];
                var entities = components._entities;
                var finalFilterCount = componentIdsCount - 1;
                var sparses = _stack.Count > 0 ? _stack.Pop() : new int[64][];

                var x = 0;

                for (var i = 0; i < componentIdsCount; ++i)
                {
                    if (componentIds[i] == compactIncludeId)
                    {
                        continue;
                    }

                    sparses[x] = registryComponents[componentIds[i]]._sparse;
                    x++;
                }

                _entities = entities;
                _sparses = sparses;
                _entityIndex = components._count - 1;
                _includesCount = includesCount - 1;
                _finalFilterCount = finalFilterCount;
                _current = (new Ref<T0> { _sparse = sparse0, _dense = dense0, },
                    new Ref<T1> { _sparse = sparse1, _dense = dense1, },
                    new Ref<T2> { _sparse = sparse2, _dense = dense2, });
                Current = _current;
            }

            public ComponentIterator<T0, T1, T2> GetEnumerator() => this;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                var match = true;
                var entity = -1;
                var entityIndex = _entityIndex;
                var sparses = _sparses;
                var includesCount = _includesCount;
                var finalFilterCount = _finalFilterCount;
                var entities = _entities;
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

                    componentIndex = 0;
                    entityIndex--;

                    if (!match)
                    {
                        goto MoveNextEntity;
                    }
                }

                _entityIndex = entityIndex;


                Current = _current;

                if (entity >= 0)
                {
                    return true;
                }

                Dispose();
                return false;
            }

            private void Dispose()
            {
                for (var i = 0; i < _sparses.Length; ++i)
                {
                    if (_sparses[i] != null)
                    {
                        _sparses[i] = null;
                    }

                    else
                    {
                        break;
                    }
                }

                _stack.Push(_sparses);
            }
        }

        public ComponentIterator<T0, T1, T2, T3> Each<T0, T1, T2, T3>(Filter filter = default)
            where T0 : struct
            where T1 : struct
            where T2 : struct
            where T3 : struct
        {
            return new ComponentIterator<T0, T1, T2, T3>(this, filter);
        }

        public unsafe struct ComponentIterator<T0, T1, T2, T3>
            where T0 : struct
            where T1 : struct
            where T2 : struct
            where T3 : struct
        {
            private readonly int _finalFilterCount;
            private readonly int _includesCount;
            private readonly int[] _entities;
            private readonly int[][] _sparses;

            private int _entityIndex;
            public (Ref<T0>, Ref<T1>, Ref<T2>, Ref<T3>) Current { get; set; }

            private (Ref<T0>, Ref<T1>, Ref<T2>, Ref<T3>) _current;

            public ComponentIterator(Registry registry, Filter filter)
            {
                var registryComponentIds = registry._componentIds;
                var registryComponents = registry._components;

                var include = filter.Include;
                var exclude = filter.Exclude;
                var includesCount = include?.Length + 4 ?? 4;
                var componentIdsCount = includesCount + (exclude?.Length ?? 0);
                var componentIds = stackalloc int[componentIdsCount];

                var sparse0 = default(int[]);
                var dense0 = default(T0*);
                var id0 = -1;
                var sparse1 = default(int[]);
                var dense1 = default(T1*);
                var id1 = -1;
                var sparse2 = default(int[]);
                var dense2 = default(T2*);
                var id2 = -1;
                var sparse3 = default(int[]);
                var dense3 = default(T3*);
                var id3 = -1;
                var hasAllQueryComponents = registryComponentIds.TryGetValue(typeof(T0), out id0) &&
                                            registryComponentIds.TryGetValue(typeof(T1), out id1) &&
                                            registryComponentIds.TryGetValue(typeof(T2), out id2) &&
                                            registryComponentIds.TryGetValue(typeof(T3), out id3);
                if (hasAllQueryComponents)
                {
                    var components0 = registryComponents[id0];
                    sparse0 = components0._sparse;
                    dense0 = components0.GetDense<T0>();
                    componentIds[0] = id0;

                    var components1 = registryComponents[id1];
                    sparse1 = components1._sparse;
                    dense1 = components1.GetDense<T1>();
                    componentIds[1] = id1;

                    var components2 = registryComponents[id2];
                    sparse2 = components2._sparse;
                    dense2 = components2.GetDense<T2>();
                    componentIds[2] = id2;

                    var components3 = registryComponents[id3];
                    sparse3 = components3._sparse;
                    dense3 = components3.GetDense<T3>();
                    componentIds[3] = id3;
                }

                if (include != null)
                {
                    registry.FillComponentIds(include, componentIds, 4);
                }

                QuickSort(componentIds, 0, includesCount - 1, 1);
                if (exclude != null)
                {
                    registry.FillComponentIds(exclude, componentIds, includesCount);
                    QuickSort(componentIds, includesCount, componentIdsCount - 1, -1);
                }

                var compactIncludeId = registry.FindCompactIncludeId(componentIds, includesCount);

                var components = registryComponents[compactIncludeId];
                var entities = components._entities;
                var finalFilterCount = componentIdsCount - 1;
                var sparses = _stack.Count > 0 ? _stack.Pop() : new int[64][];

                var x = 0;

                for (var i = 0; i < componentIdsCount; ++i)
                {
                    if (componentIds[i] == compactIncludeId)
                    {
                        continue;
                    }

                    sparses[x] = registryComponents[componentIds[i]]._sparse;
                    x++;
                }

                _entities = entities;
                _sparses = sparses;
                _entityIndex = components._count - 1;
                _includesCount = includesCount - 1;
                _finalFilterCount = finalFilterCount;
                _current = (new Ref<T0> { _sparse = sparse0, _dense = dense0, },
                    new Ref<T1> { _sparse = sparse1, _dense = dense1, },
                    new Ref<T2> { _sparse = sparse2, _dense = dense2, },
                    new Ref<T3> { _sparse = sparse3, _dense = dense3, });
                Current = _current;
            }

            public ComponentIterator<T0, T1, T2, T3> GetEnumerator() => this;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                var match = true;
                var entity = -1;
                var entityIndex = _entityIndex;
                var sparses = _sparses;
                var includesCount = _includesCount;
                var finalFilterCount = _finalFilterCount;
                var entities = _entities;
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

                    componentIndex = 0;
                    entityIndex--;

                    if (!match)
                    {
                        goto MoveNextEntity;
                    }
                }

                _entityIndex = entityIndex;


                Current = _current;

                if (entity >= 0)
                {
                    return true;
                }

                Dispose();
                return false;
            }

            private void Dispose()
            {
                for (var i = 0; i < _sparses.Length; ++i)
                {
                    if (_sparses[i] != null)
                    {
                        _sparses[i] = null;
                    }

                    else
                    {
                        break;
                    }
                }

                _stack.Push(_sparses);
            }
        }

        public ComponentIterator<T0, T1, T2, T3, T4> Each<T0, T1, T2, T3, T4>(Filter filter = default)
            where T0 : struct
            where T1 : struct
            where T2 : struct
            where T3 : struct
            where T4 : struct
        {
            return new ComponentIterator<T0, T1, T2, T3, T4>(this, filter);
        }

        public unsafe struct ComponentIterator<T0, T1, T2, T3, T4>
            where T0 : struct
            where T1 : struct
            where T2 : struct
            where T3 : struct
            where T4 : struct
        {
            private readonly int _finalFilterCount;
            private readonly int _includesCount;
            private readonly int[] _entities;
            private readonly int[][] _sparses;

            private int _entityIndex;
            public (Ref<T0>, Ref<T1>, Ref<T2>, Ref<T3>, Ref<T4>) Current { get; set; }

            private (Ref<T0>, Ref<T1>, Ref<T2>, Ref<T3>, Ref<T4>) _current;

            public ComponentIterator(Registry registry, Filter filter)
            {
                var registryComponentIds = registry._componentIds;
                var registryComponents = registry._components;

                var include = filter.Include;
                var exclude = filter.Exclude;
                var includesCount = include?.Length + 5 ?? 5;
                var componentIdsCount = includesCount + (exclude?.Length ?? 0);
                var componentIds = stackalloc int[componentIdsCount];

                var sparse0 = default(int[]);
                var dense0 = default(T0*);
                var id0 = -1;
                var sparse1 = default(int[]);
                var dense1 = default(T1*);
                var id1 = -1;
                var sparse2 = default(int[]);
                var dense2 = default(T2*);
                var id2 = -1;
                var sparse3 = default(int[]);
                var dense3 = default(T3*);
                var id3 = -1;
                var sparse4 = default(int[]);
                var dense4 = default(T4*);
                var id4 = -1;
                var hasAllQueryComponents = registryComponentIds.TryGetValue(typeof(T0), out id0) &&
                                            registryComponentIds.TryGetValue(typeof(T1), out id1) &&
                                            registryComponentIds.TryGetValue(typeof(T2), out id2) &&
                                            registryComponentIds.TryGetValue(typeof(T3), out id3) &&
                                            registryComponentIds.TryGetValue(typeof(T4), out id4);
                if (hasAllQueryComponents)
                {
                    var components0 = registryComponents[id0];
                    sparse0 = components0._sparse;
                    dense0 = components0.GetDense<T0>();
                    componentIds[0] = id0;

                    var components1 = registryComponents[id1];
                    sparse1 = components1._sparse;
                    dense1 = components1.GetDense<T1>();
                    componentIds[1] = id1;

                    var components2 = registryComponents[id2];
                    sparse2 = components2._sparse;
                    dense2 = components2.GetDense<T2>();
                    componentIds[2] = id2;

                    var components3 = registryComponents[id3];
                    sparse3 = components3._sparse;
                    dense3 = components3.GetDense<T3>();
                    componentIds[3] = id3;

                    var components4 = registryComponents[id4];
                    sparse4 = components4._sparse;
                    dense4 = components4.GetDense<T4>();
                    componentIds[4] = id4;
                }

                if (include != null)
                {
                    registry.FillComponentIds(include, componentIds, 5);
                }

                QuickSort(componentIds, 0, includesCount - 1, 1);
                if (exclude != null)
                {
                    registry.FillComponentIds(exclude, componentIds, includesCount);
                    QuickSort(componentIds, includesCount, componentIdsCount - 1, -1);
                }

                var compactIncludeId = registry.FindCompactIncludeId(componentIds, includesCount);

                var components = registryComponents[compactIncludeId];
                var entities = components._entities;
                var finalFilterCount = componentIdsCount - 1;
                var sparses = _stack.Count > 0 ? _stack.Pop() : new int[64][];

                var x = 0;

                for (var i = 0; i < componentIdsCount; ++i)
                {
                    if (componentIds[i] == compactIncludeId)
                    {
                        continue;
                    }

                    sparses[x] = registryComponents[componentIds[i]]._sparse;
                    x++;
                }

                _entities = entities;
                _sparses = sparses;
                _entityIndex = components._count - 1;
                _includesCount = includesCount - 1;
                _finalFilterCount = finalFilterCount;
                _current = (new Ref<T0> { _sparse = sparse0, _dense = dense0, },
                    new Ref<T1> { _sparse = sparse1, _dense = dense1, },
                    new Ref<T2> { _sparse = sparse2, _dense = dense2, },
                    new Ref<T3> { _sparse = sparse3, _dense = dense3, },
                    new Ref<T4> { _sparse = sparse4, _dense = dense4, });
                Current = _current;
            }

            public ComponentIterator<T0, T1, T2, T3, T4> GetEnumerator() => this;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                var match = true;
                var entity = -1;
                var entityIndex = _entityIndex;
                var sparses = _sparses;
                var includesCount = _includesCount;
                var finalFilterCount = _finalFilterCount;
                var entities = _entities;
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

                    componentIndex = 0;
                    entityIndex--;

                    if (!match)
                    {
                        goto MoveNextEntity;
                    }
                }

                _entityIndex = entityIndex;


                Current = _current;

                if (entity >= 0)
                {
                    return true;
                }

                Dispose();
                return false;
            }

            private void Dispose()
            {
                for (var i = 0; i < _sparses.Length; ++i)
                {
                    if (_sparses[i] != null)
                    {
                        _sparses[i] = null;
                    }

                    else
                    {
                        break;
                    }
                }

                _stack.Push(_sparses);
            }
        }

        public ComponentIterator<T0, T1, T2, T3, T4, T5> Each<T0, T1, T2, T3, T4, T5>(Filter filter = default)
            where T0 : struct
            where T1 : struct
            where T2 : struct
            where T3 : struct
            where T4 : struct
            where T5 : struct
        {
            return new ComponentIterator<T0, T1, T2, T3, T4, T5>(this, filter);
        }

        public unsafe struct ComponentIterator<T0, T1, T2, T3, T4, T5>
            where T0 : struct
            where T1 : struct
            where T2 : struct
            where T3 : struct
            where T4 : struct
            where T5 : struct
        {
            private readonly int _finalFilterCount;
            private readonly int _includesCount;
            private readonly int[] _entities;
            private readonly int[][] _sparses;

            private int _entityIndex;
            public (Ref<T0>, Ref<T1>, Ref<T2>, Ref<T3>, Ref<T4>, Ref<T5>) Current { get; set; }

            private (Ref<T0>, Ref<T1>, Ref<T2>, Ref<T3>, Ref<T4>, Ref<T5>) _current;

            public ComponentIterator(Registry registry, Filter filter)
            {
                var registryComponentIds = registry._componentIds;
                var registryComponents = registry._components;

                var include = filter.Include;
                var exclude = filter.Exclude;
                var includesCount = include?.Length + 6 ?? 6;
                var componentIdsCount = includesCount + (exclude?.Length ?? 0);
                var componentIds = stackalloc int[componentIdsCount];

                var sparse0 = default(int[]);
                var dense0 = default(T0*);
                var id0 = -1;
                var sparse1 = default(int[]);
                var dense1 = default(T1*);
                var id1 = -1;
                var sparse2 = default(int[]);
                var dense2 = default(T2*);
                var id2 = -1;
                var sparse3 = default(int[]);
                var dense3 = default(T3*);
                var id3 = -1;
                var sparse4 = default(int[]);
                var dense4 = default(T4*);
                var id4 = -1;
                var sparse5 = default(int[]);
                var dense5 = default(T5*);
                var id5 = -1;
                var hasAllQueryComponents = registryComponentIds.TryGetValue(typeof(T0), out id0) &&
                                            registryComponentIds.TryGetValue(typeof(T1), out id1) &&
                                            registryComponentIds.TryGetValue(typeof(T2), out id2) &&
                                            registryComponentIds.TryGetValue(typeof(T3), out id3) &&
                                            registryComponentIds.TryGetValue(typeof(T4), out id4) &&
                                            registryComponentIds.TryGetValue(typeof(T5), out id5);
                if (hasAllQueryComponents)
                {
                    var components0 = registryComponents[id0];
                    sparse0 = components0._sparse;
                    dense0 = components0.GetDense<T0>();
                    componentIds[0] = id0;

                    var components1 = registryComponents[id1];
                    sparse1 = components1._sparse;
                    dense1 = components1.GetDense<T1>();
                    componentIds[1] = id1;

                    var components2 = registryComponents[id2];
                    sparse2 = components2._sparse;
                    dense2 = components2.GetDense<T2>();
                    componentIds[2] = id2;

                    var components3 = registryComponents[id3];
                    sparse3 = components3._sparse;
                    dense3 = components3.GetDense<T3>();
                    componentIds[3] = id3;

                    var components4 = registryComponents[id4];
                    sparse4 = components4._sparse;
                    dense4 = components4.GetDense<T4>();
                    componentIds[4] = id4;

                    var components5 = registryComponents[id5];
                    sparse5 = components5._sparse;
                    dense5 = components5.GetDense<T5>();
                    componentIds[5] = id5;
                }

                if (include != null)
                {
                    registry.FillComponentIds(include, componentIds, 6);
                }

                QuickSort(componentIds, 0, includesCount - 1, 1);
                if (exclude != null)
                {
                    registry.FillComponentIds(exclude, componentIds, includesCount);
                    QuickSort(componentIds, includesCount, componentIdsCount - 1, -1);
                }

                var compactIncludeId = registry.FindCompactIncludeId(componentIds, includesCount);

                var components = registryComponents[compactIncludeId];
                var entities = components._entities;
                var finalFilterCount = componentIdsCount - 1;
                var sparses = _stack.Count > 0 ? _stack.Pop() : new int[64][];

                var x = 0;

                for (var i = 0; i < componentIdsCount; ++i)
                {
                    if (componentIds[i] == compactIncludeId)
                    {
                        continue;
                    }

                    sparses[x] = registryComponents[componentIds[i]]._sparse;
                    x++;
                }

                _entities = entities;
                _sparses = sparses;
                _entityIndex = components._count - 1;
                _includesCount = includesCount - 1;
                _finalFilterCount = finalFilterCount;
                _current = (new Ref<T0> { _sparse = sparse0, _dense = dense0, },
                    new Ref<T1> { _sparse = sparse1, _dense = dense1, },
                    new Ref<T2> { _sparse = sparse2, _dense = dense2, },
                    new Ref<T3> { _sparse = sparse3, _dense = dense3, },
                    new Ref<T4> { _sparse = sparse4, _dense = dense4, },
                    new Ref<T5> { _sparse = sparse5, _dense = dense5, });
                Current = _current;
            }

            public ComponentIterator<T0, T1, T2, T3, T4, T5> GetEnumerator() => this;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                var match = true;
                var entity = -1;
                var entityIndex = _entityIndex;
                var sparses = _sparses;
                var includesCount = _includesCount;
                var finalFilterCount = _finalFilterCount;
                var entities = _entities;
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

                    componentIndex = 0;
                    entityIndex--;

                    if (!match)
                    {
                        goto MoveNextEntity;
                    }
                }

                _entityIndex = entityIndex;


                Current = _current;

                if (entity >= 0)
                {
                    return true;
                }

                Dispose();
                return false;
            }

            private void Dispose()
            {
                for (var i = 0; i < _sparses.Length; ++i)
                {
                    if (_sparses[i] != null)
                    {
                        _sparses[i] = null;
                    }

                    else
                    {
                        break;
                    }
                }

                _stack.Push(_sparses);
            }
        }

        public ComponentIterator<T0, T1, T2, T3, T4, T5, T6> Each<T0, T1, T2, T3, T4, T5, T6>(Filter filter = default)
            where T0 : struct
            where T1 : struct
            where T2 : struct
            where T3 : struct
            where T4 : struct
            where T5 : struct
            where T6 : struct
        {
            return new ComponentIterator<T0, T1, T2, T3, T4, T5, T6>(this, filter);
        }

        public unsafe struct ComponentIterator<T0, T1, T2, T3, T4, T5, T6>
            where T0 : struct
            where T1 : struct
            where T2 : struct
            where T3 : struct
            where T4 : struct
            where T5 : struct
            where T6 : struct
        {
            private readonly int _finalFilterCount;
            private readonly int _includesCount;
            private readonly int[] _entities;
            private readonly int[][] _sparses;

            private int _entityIndex;
            public (Ref<T0>, Ref<T1>, Ref<T2>, Ref<T3>, Ref<T4>, Ref<T5>, Ref<T6>) Current { get; set; }

            private (Ref<T0>, Ref<T1>, Ref<T2>, Ref<T3>, Ref<T4>, Ref<T5>, Ref<T6>) _current;

            public ComponentIterator(Registry registry, Filter filter)
            {
                var registryComponentIds = registry._componentIds;
                var registryComponents = registry._components;

                var include = filter.Include;
                var exclude = filter.Exclude;
                var includesCount = include?.Length + 7 ?? 7;
                var componentIdsCount = includesCount + (exclude?.Length ?? 0);
                var componentIds = stackalloc int[componentIdsCount];

                var sparse0 = default(int[]);
                var dense0 = default(T0*);
                var id0 = -1;
                var sparse1 = default(int[]);
                var dense1 = default(T1*);
                var id1 = -1;
                var sparse2 = default(int[]);
                var dense2 = default(T2*);
                var id2 = -1;
                var sparse3 = default(int[]);
                var dense3 = default(T3*);
                var id3 = -1;
                var sparse4 = default(int[]);
                var dense4 = default(T4*);
                var id4 = -1;
                var sparse5 = default(int[]);
                var dense5 = default(T5*);
                var id5 = -1;
                var sparse6 = default(int[]);
                var dense6 = default(T6*);
                var id6 = -1;
                var hasAllQueryComponents = registryComponentIds.TryGetValue(typeof(T0), out id0) &&
                                            registryComponentIds.TryGetValue(typeof(T1), out id1) &&
                                            registryComponentIds.TryGetValue(typeof(T2), out id2) &&
                                            registryComponentIds.TryGetValue(typeof(T3), out id3) &&
                                            registryComponentIds.TryGetValue(typeof(T4), out id4) &&
                                            registryComponentIds.TryGetValue(typeof(T5), out id5) &&
                                            registryComponentIds.TryGetValue(typeof(T6), out id6);
                if (hasAllQueryComponents)
                {
                    var components0 = registryComponents[id0];
                    sparse0 = components0._sparse;
                    dense0 = components0.GetDense<T0>();
                    componentIds[0] = id0;

                    var components1 = registryComponents[id1];
                    sparse1 = components1._sparse;
                    dense1 = components1.GetDense<T1>();
                    componentIds[1] = id1;

                    var components2 = registryComponents[id2];
                    sparse2 = components2._sparse;
                    dense2 = components2.GetDense<T2>();
                    componentIds[2] = id2;

                    var components3 = registryComponents[id3];
                    sparse3 = components3._sparse;
                    dense3 = components3.GetDense<T3>();
                    componentIds[3] = id3;

                    var components4 = registryComponents[id4];
                    sparse4 = components4._sparse;
                    dense4 = components4.GetDense<T4>();
                    componentIds[4] = id4;

                    var components5 = registryComponents[id5];
                    sparse5 = components5._sparse;
                    dense5 = components5.GetDense<T5>();
                    componentIds[5] = id5;

                    var components6 = registryComponents[id6];
                    sparse6 = components6._sparse;
                    dense6 = components6.GetDense<T6>();
                    componentIds[6] = id6;
                }

                if (include != null)
                {
                    registry.FillComponentIds(include, componentIds, 7);
                }

                QuickSort(componentIds, 0, includesCount - 1, 1);
                if (exclude != null)
                {
                    registry.FillComponentIds(exclude, componentIds, includesCount);
                    QuickSort(componentIds, includesCount, componentIdsCount - 1, -1);
                }

                var compactIncludeId = registry.FindCompactIncludeId(componentIds, includesCount);

                var components = registryComponents[compactIncludeId];
                var entities = components._entities;
                var finalFilterCount = componentIdsCount - 1;
                var sparses = _stack.Count > 0 ? _stack.Pop() : new int[64][];

                var x = 0;

                for (var i = 0; i < componentIdsCount; ++i)
                {
                    if (componentIds[i] == compactIncludeId)
                    {
                        continue;
                    }

                    sparses[x] = registryComponents[componentIds[i]]._sparse;
                    x++;
                }

                _entities = entities;
                _sparses = sparses;
                _entityIndex = components._count - 1;
                _includesCount = includesCount - 1;
                _finalFilterCount = finalFilterCount;
                _current = (new Ref<T0> { _sparse = sparse0, _dense = dense0, },
                    new Ref<T1> { _sparse = sparse1, _dense = dense1, },
                    new Ref<T2> { _sparse = sparse2, _dense = dense2, },
                    new Ref<T3> { _sparse = sparse3, _dense = dense3, },
                    new Ref<T4> { _sparse = sparse4, _dense = dense4, },
                    new Ref<T5> { _sparse = sparse5, _dense = dense5, },
                    new Ref<T6> { _sparse = sparse6, _dense = dense6, });
                Current = _current;
            }

            public ComponentIterator<T0, T1, T2, T3, T4, T5, T6> GetEnumerator() => this;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                var match = true;
                var entity = -1;
                var entityIndex = _entityIndex;
                var sparses = _sparses;
                var includesCount = _includesCount;
                var finalFilterCount = _finalFilterCount;
                var entities = _entities;
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

                    componentIndex = 0;
                    entityIndex--;

                    if (!match)
                    {
                        goto MoveNextEntity;
                    }
                }

                _entityIndex = entityIndex;


                Current = _current;

                if (entity >= 0)
                {
                    return true;
                }

                Dispose();
                return false;
            }

            private void Dispose()
            {
                for (var i = 0; i < _sparses.Length; ++i)
                {
                    if (_sparses[i] != null)
                    {
                        _sparses[i] = null;
                    }

                    else
                    {
                        break;
                    }
                }

                _stack.Push(_sparses);
            }
        }

        public ComponentIterator<T0, T1, T2, T3, T4, T5, T6, T7> Each<T0, T1, T2, T3, T4, T5, T6, T7>(
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
            return new ComponentIterator<T0, T1, T2, T3, T4, T5, T6, T7>(this, filter);
        }

        public unsafe struct ComponentIterator<T0, T1, T2, T3, T4, T5, T6, T7>
            where T0 : struct
            where T1 : struct
            where T2 : struct
            where T3 : struct
            where T4 : struct
            where T5 : struct
            where T6 : struct
            where T7 : struct
        {
            private readonly int _finalFilterCount;
            private readonly int _includesCount;
            private readonly int[] _entities;
            private readonly int[][] _sparses;

            private int _entityIndex;
            public (Ref<T0>, Ref<T1>, Ref<T2>, Ref<T3>, Ref<T4>, Ref<T5>, Ref<T6>, Ref<T7>) Current { get; set; }

            private (Ref<T0>, Ref<T1>, Ref<T2>, Ref<T3>, Ref<T4>, Ref<T5>, Ref<T6>, Ref<T7>) _current;

            public ComponentIterator(Registry registry, Filter filter)
            {
                var registryComponentIds = registry._componentIds;
                var registryComponents = registry._components;

                var include = filter.Include;
                var exclude = filter.Exclude;
                var includesCount = include?.Length + 8 ?? 8;
                var componentIdsCount = includesCount + (exclude?.Length ?? 0);
                var componentIds = stackalloc int[componentIdsCount];

                var sparse0 = default(int[]);
                var dense0 = default(T0*);
                var id0 = -1;
                var sparse1 = default(int[]);
                var dense1 = default(T1*);
                var id1 = -1;
                var sparse2 = default(int[]);
                var dense2 = default(T2*);
                var id2 = -1;
                var sparse3 = default(int[]);
                var dense3 = default(T3*);
                var id3 = -1;
                var sparse4 = default(int[]);
                var dense4 = default(T4*);
                var id4 = -1;
                var sparse5 = default(int[]);
                var dense5 = default(T5*);
                var id5 = -1;
                var sparse6 = default(int[]);
                var dense6 = default(T6*);
                var id6 = -1;
                var sparse7 = default(int[]);
                var dense7 = default(T7*);
                var id7 = -1;
                var hasAllQueryComponents = registryComponentIds.TryGetValue(typeof(T0), out id0) &&
                                            registryComponentIds.TryGetValue(typeof(T1), out id1) &&
                                            registryComponentIds.TryGetValue(typeof(T2), out id2) &&
                                            registryComponentIds.TryGetValue(typeof(T3), out id3) &&
                                            registryComponentIds.TryGetValue(typeof(T4), out id4) &&
                                            registryComponentIds.TryGetValue(typeof(T5), out id5) &&
                                            registryComponentIds.TryGetValue(typeof(T6), out id6) &&
                                            registryComponentIds.TryGetValue(typeof(T7), out id7);
                if (hasAllQueryComponents)
                {
                    var components0 = registryComponents[id0];
                    sparse0 = components0._sparse;
                    dense0 = components0.GetDense<T0>();
                    componentIds[0] = id0;

                    var components1 = registryComponents[id1];
                    sparse1 = components1._sparse;
                    dense1 = components1.GetDense<T1>();
                    componentIds[1] = id1;

                    var components2 = registryComponents[id2];
                    sparse2 = components2._sparse;
                    dense2 = components2.GetDense<T2>();
                    componentIds[2] = id2;

                    var components3 = registryComponents[id3];
                    sparse3 = components3._sparse;
                    dense3 = components3.GetDense<T3>();
                    componentIds[3] = id3;

                    var components4 = registryComponents[id4];
                    sparse4 = components4._sparse;
                    dense4 = components4.GetDense<T4>();
                    componentIds[4] = id4;

                    var components5 = registryComponents[id5];
                    sparse5 = components5._sparse;
                    dense5 = components5.GetDense<T5>();
                    componentIds[5] = id5;

                    var components6 = registryComponents[id6];
                    sparse6 = components6._sparse;
                    dense6 = components6.GetDense<T6>();
                    componentIds[6] = id6;

                    var components7 = registryComponents[id7];
                    sparse7 = components7._sparse;
                    dense7 = components7.GetDense<T7>();
                    componentIds[7] = id7;
                }

                if (include != null)
                {
                    registry.FillComponentIds(include, componentIds, 8);
                }

                QuickSort(componentIds, 0, includesCount - 1, 1);
                if (exclude != null)
                {
                    registry.FillComponentIds(exclude, componentIds, includesCount);
                    QuickSort(componentIds, includesCount, componentIdsCount - 1, -1);
                }

                var compactIncludeId = registry.FindCompactIncludeId(componentIds, includesCount);

                var components = registryComponents[compactIncludeId];
                var entities = components._entities;
                var finalFilterCount = componentIdsCount - 1;
                var sparses = _stack.Count > 0 ? _stack.Pop() : new int[64][];

                var x = 0;

                for (var i = 0; i < componentIdsCount; ++i)
                {
                    if (componentIds[i] == compactIncludeId)
                    {
                        continue;
                    }

                    sparses[x] = registryComponents[componentIds[i]]._sparse;
                    x++;
                }

                _entities = entities;
                _sparses = sparses;
                _entityIndex = components._count - 1;
                _includesCount = includesCount - 1;
                _finalFilterCount = finalFilterCount;
                _current = (new Ref<T0> { _sparse = sparse0, _dense = dense0, },
                    new Ref<T1> { _sparse = sparse1, _dense = dense1, },
                    new Ref<T2> { _sparse = sparse2, _dense = dense2, },
                    new Ref<T3> { _sparse = sparse3, _dense = dense3, },
                    new Ref<T4> { _sparse = sparse4, _dense = dense4, },
                    new Ref<T5> { _sparse = sparse5, _dense = dense5, },
                    new Ref<T6> { _sparse = sparse6, _dense = dense6, },
                    new Ref<T7> { _sparse = sparse7, _dense = dense7, });
                Current = _current;
            }

            public ComponentIterator<T0, T1, T2, T3, T4, T5, T6, T7> GetEnumerator() => this;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                var match = true;
                var entity = -1;
                var entityIndex = _entityIndex;
                var sparses = _sparses;
                var includesCount = _includesCount;
                var finalFilterCount = _finalFilterCount;
                var entities = _entities;
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

                    componentIndex = 0;
                    entityIndex--;

                    if (!match)
                    {
                        goto MoveNextEntity;
                    }
                }

                _entityIndex = entityIndex;


                Current = _current;

                if (entity >= 0)
                {
                    return true;
                }

                Dispose();
                return false;
            }

            private void Dispose()
            {
                for (var i = 0; i < _sparses.Length; ++i)
                {
                    if (_sparses[i] != null)
                    {
                        _sparses[i] = null;
                    }

                    else
                    {
                        break;
                    }
                }

                _stack.Push(_sparses);
            }
        }
    }
}