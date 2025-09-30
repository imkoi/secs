using System;

namespace Secs
{
    public readonly struct Components<T> where T : struct
    {
        private readonly int _id;
        private readonly Registry _registry;

        public Components(Registry registry, int id)
        {
            _registry = registry;
            _id = id;
        }

        public ref T AddComponent(int entityId, T component)
        {
            var componentId = _id;
            ref var components = ref _registry._components[componentId];

            var denseIndex = components._count;

            if (entityId >= components._sparse.Length)
            {
                Array.Resize(ref components._sparse, entityId * 2);
            }
            
            components._sparse[entityId] = denseIndex + 1;
            
            var concreteComponents = (T[])components._components;

            if (denseIndex >= components._entities.Length)
            {
                _registry.ResizeComponents(ref components, ref concreteComponents);
            }
            
            components._entities[denseIndex] = entityId;
            concreteComponents[denseIndex] = component;

            components._count++;

            const int bits = 20;
            const long mask = (1L << bits) - 1;

            var entityInfos = _registry._entityInfos;
            var info = entityInfos[entityId];

            var count = (info & mask) + 1;
            var right = (info >> bits) & mask;
            var left = (info >> (2 * bits)) & mask;

            var idU = (long)componentId;
            right = idU > right ? idU : right;
            left = idU < left ? idU : left;

            entityInfos[entityId] =
                ((left & mask) << (2 * bits)) |
                ((right & mask) << bits) |
                (count & mask);

            return ref concreteComponents[denseIndex];
        }
        
        public unsafe int RemoveComponent(int entity)
        {
            var componentId = _id;
            ref var components = ref _registry._components[componentId];
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

            var entityInfos = _registry._entityInfos;
            var data = entityInfos[entity];
            var count = (data & mask) - 1;
            
            if (count == 0)
            {
                _registry.MoveEntityToPool(entity);
            }
            else
            {
                entityInfos[entity] = (data & ~mask) | (count & mask);
            }

            return 1;
        }
        
        public unsafe ref T GetComponent(int entity)
        {
            var components = _registry._components[_id];
            
            return ref ((T*) components._componentsPtr)[components._sparse[entity]];
        }

        public bool HasComponent(int entity)
        {
            return _registry._components[_id]._sparse[entity] != 0;
        }
    }
}