using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.IL2CPP.CompilerServices;

namespace Secs
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public partial class Registry
    {
        private long[] _entityInfos = new long[4096];
        internal Queue<int> _destroyedEntities = new Queue<int>();
        private int _lastEntityId;
        
        private TypeIdProvider _componentIds = new TypeIdProvider(256);
        private Components[] _components = new Components[32];
        private int _componentCount;

        public int CreateEntity()
        {
            var entityId = _destroyedEntities.Count == 0 
                ? _lastEntityId++ 
                : _destroyedEntities.Dequeue();

            if (_entityInfos.Length <= entityId)
            {
                Array.Resize(ref _entityInfos, entityId * 2);
            }
            
            const int bits = 20;
            const long mask = (1L << bits) - 1;

            _entityInfos[entityId] =
                ((-1L & mask) << (2 * bits)) |
                ((-1L & mask) << bits) |
                (0L & mask);
            
            return entityId;
        }
        
        public void DestroyEntity(int entity)
        {
            const int bits = 20;
            const long mask = (1L << bits) - 1;

            var entityInfo = _entityInfos[entity];

            var componentsCount = (int)(entityInfo & mask);
            var right = (int)((entityInfo >> bits) & mask);
            var left = (int)((entityInfo >> (2 * bits)) & mask);

            for (var i = left; i <= right && componentsCount > 0; i++)
            {
                componentsCount -= RemoveComponent(entity, i);
            }

            _destroyedEntities.Enqueue(entity);
        }

        public unsafe ref T AddComponent<T>(int entityId, T component)
            where T : struct
        {
            var componentType = component.GetType();
            
            if(!_componentIds.TryGetValue(componentType, out var id))
            {
                RegisterComponent<T>(componentType, out id);
            }

            ref var components = ref _components[id];

            var denseIndex = components._count;

            if (entityId >= components._sparse.Length)
            {
                Array.Resize(ref components._sparse, entityId * 2);
            }
            
            components._sparse[entityId] = denseIndex + 1;
            
            var concreteComponents = (T[])components._components;

            if (denseIndex >= components._entities.Length)
            {
                ResizeComponents(ref components, ref concreteComponents);
            }
            
            components._entities[denseIndex] = entityId;
            concreteComponents[denseIndex] = component;

            components._count++;

            const int bits = 20;
            const long mask = (1L << bits) - 1;

            var info = _entityInfos[entityId];

            var count = (info & mask) + 1;
            var right = (info >> bits) & mask;
            var left = (info >> (2 * bits)) & mask;

            var idU = (long)id;
            right = idU > right ? idU : right;
            left = idU < left ? idU : left;

            _entityInfos[entityId] =
                ((left & mask) << (2 * bits)) |
                ((right & mask) << bits) |
                (count & mask);

            return ref concreteComponents[denseIndex];
        }
        
        public void RemoveComponent<T>(int entity)
        {
            _componentIds.TryGetValue(typeof(T), out var id);
            
            RemoveComponent(entity, id);
        }
        
        public unsafe ref T GetComponent<T>(int entity) 
            where T : struct
        {
            _componentIds.TryGetValue(typeof(T), out var id);
            var components = _components[id];
            
            return ref components.GetDense<T>()[components._sparse[entity]];
        }

        public Group GetGroup(Type[] include = null, Type[] exclude = null)
        {
            throw new NotImplementedException();
            
            return default;
        }

        private unsafe void RegisterComponent<T>(Type componentType, out int id) where T : struct
        {
            _componentIds.TryAdd(componentType, out id);
                
            _componentCount++;
                
            if (_components.Length <= id)
            {
                Array.Resize(ref _components, id * 2);
            }
                
            var array = new T[32];
                
            var newComponents = new Components
            {
                _sparse = new int[_lastEntityId],
                _entities = new int[32],
                _components = array,
                _size = sizeof(T)
            };

            fixed (T* arrayPtr = array)
            {
                newComponents._componentsPtr = arrayPtr;
            }

            _components[id] = newComponents;
        }
        
        private unsafe void ResizeComponents<T>(ref Components components, ref T[] concreteComponents) where T : struct
        {
            var newDenseSize = components._count * 2;
            
            Array.Resize(ref components._entities, newDenseSize);
            Array.Resize(ref concreteComponents, newDenseSize);
                
            components._components = concreteComponents;

            fixed (T* arrayPtr = concreteComponents)
            {
                components._componentsPtr = arrayPtr;
            }
        }
        
        private unsafe int RemoveComponent(int entity, int componentId)
        {
            ref var components = ref _components[componentId];
            var sparse = components._sparse;
            var denseIndex = sparse[entity] - 1;

            if (denseIndex < 0)
            {
                return 0;
            }
            
            var lastIndex = components._count - 1;

            if (denseIndex == lastIndex)
            {
                sparse[entity] = 0;

                goto Finalize;
            }

            components._entities[denseIndex] = components._entities[lastIndex];
            sparse[components._entities[denseIndex]] = denseIndex + 1;
            sparse[entity] = 0;

            var arrayPtr = components._componentsPtr;
            
            var basePtr = (byte*)arrayPtr;
            var size = components._size;
            
            var a = basePtr + denseIndex * size;
            var b = basePtr + lastIndex * size;
            
            for (var i = 0; i < size; i++)
            {
                (a[i], b[i]) = (b[i], a[i]);
            }

            Finalize:
            components._count--;
            
            const int bits = 20;
            const long mask = (1L << bits) - 1;

            var data = _entityInfos[entity];
            var count = (data & mask) - 1;
            
            if (count == 0)
            {
                MoveEntityToPool(entity);
            }
            else
            {
                _entityInfos[entity] = (data & ~mask) | (count & mask);
            }

            return 1;
        }

        private void MoveEntityToPool(int entity)
        {
            const int bits = 20;
            const long mask = (1L << bits) - 1;
            
            _entityInfos[entity] =
                ((-1L & mask) << (2 * bits)) |
                ((-1L & mask) << bits) |
                (0L & mask);
                
            _destroyedEntities.Enqueue(entity);
        }
        
        private unsafe struct Components
        {
            internal int[] _sparse;
            internal int[] _entities;
            internal Array _components;
            internal void* _componentsPtr;
            internal int _count;
            internal int _size;

            public unsafe T* GetDense<T>() where T : struct
            {
                return (T*) _componentsPtr;
            }
        }
    }
}