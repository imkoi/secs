using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Secs
{
    public partial class Registry
    {
        private static Stack<int[][]> _stack = new Stack<int[][]>();
        
        public EntityIterator Each(Filter filter)
        {
            return new EntityIterator(this, filter);
        }
        
        public unsafe struct EntityIterator
        {
            private readonly int _finalFilterCount;
            private readonly int _includesCount;
            private readonly int[] _entities;
            private readonly int[][] _sparses;
            
            private int _entityIndex;
            public int Current { get; set; }

            public EntityIterator(Registry registry, Filter filter)
            {
                Current = -1;

                var registryComponents = registry._components;
                
                var include = filter.Include;
                var exclude = filter.Exclude;
                var includesCount = include?.Length + 0 ??  0;
                var componentIdsCount = includesCount + (exclude?.Length ?? 0);
                var componentIds = stackalloc int[componentIdsCount];

                if (include != null)
                {
                    registry.FillComponentIds(include, componentIds);
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
                _includesCount = includesCount - 1; // -1 ???
                _finalFilterCount = finalFilterCount;
            }
            
            public EntityIterator GetEnumerator() => this;

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
                Current = entity;

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