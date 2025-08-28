using System.Collections.Generic;
using System.Diagnostics;
using Ecs;

namespace DefaultNamespace
{
    public class SecsEntityIteratorBenchmark
    {
        public static Dictionary<string, BenchmarkRunner.ScenarioDelegate> GetScenarios()
        {
            var scenarios = new Dictionary<string, BenchmarkRunner.ScenarioDelegate>();

            scenarios[nameof(CreateEntitiesWith1DenseComponents)] = CreateEntitiesWith1DenseComponents;
            scenarios[nameof(CreateEntitiesWith1SparseComponents)] = CreateEntitiesWith1SparseComponents;
            scenarios[nameof(CreateEntitiesWith2DenseComponents)] = CreateEntitiesWith2DenseComponents;
            scenarios[nameof(CreateEntitiesWith2SparseComponents)] = CreateEntitiesWith2SparseComponents;
            scenarios[nameof(CreateEntitiesWith3DenseComponents)] = CreateEntitiesWith3DenseComponents;
            scenarios[nameof(CreateEntitiesWith3SparseComponents)] = CreateEntitiesWith3SparseComponents;

            scenarios[nameof(DestroyEntitiesWith1DenseComponents)] = DestroyEntitiesWith1DenseComponents;
            scenarios[nameof(DestroyEntitiesWith1SparseComponents)] = DestroyEntitiesWith1SparseComponents;
            scenarios[nameof(DestroyEntitiesWith2DenseComponents)] = DestroyEntitiesWith2DenseComponents;
            scenarios[nameof(DestroyEntitiesWith2SparseComponents)] = DestroyEntitiesWith2SparseComponents;
            scenarios[nameof(DestroyEntitiesWith3DenseComponents)] = DestroyEntitiesWith3DenseComponents;
            scenarios[nameof(DestroyEntitiesWith3SparseComponents)] = DestroyEntitiesWith3SparseComponents;

            scenarios[nameof(AddNewComponentWith1DenseComponents)] = AddNewComponentWith1DenseComponents;
            scenarios[nameof(AddNewComponentWith1SparseComponents)] = AddNewComponentWith1SparseComponents;
            scenarios[nameof(AddNewComponentWith2DenseComponents)] = AddNewComponentWith2DenseComponents;
            scenarios[nameof(AddNewComponentWith2SparseComponents)] = AddNewComponentWith2SparseComponents;
            scenarios[nameof(AddNewComponentWith3DenseComponents)] = AddNewComponentWith3DenseComponents;
            scenarios[nameof(AddNewComponentWith3SparseComponents)] = AddNewComponentWith3SparseComponents;

            scenarios[nameof(IterateWith1DenseComponents)] = IterateWith1DenseComponents;
            scenarios[nameof(IterateWith1SparseComponents)] = IterateWith1SparseComponents;
            scenarios[nameof(IterateWith2DenseComponents)] = IterateWith2DenseComponents;
            scenarios[nameof(IterateWith2SparseComponents)] = IterateWith2SparseComponents;
            scenarios[nameof(IterateWith3DenseComponents)] = IterateWith3DenseComponents;
            scenarios[nameof(IterateWith3SparseComponents)] = IterateWith3SparseComponents;

            return scenarios;
        }

        public static void CreateEntitiesWith1DenseComponents(int entitiesCount, Stopwatch stopwatch,
            ref BenchmarkResult result)
        {
            stopwatch.Restart();

            var world = new Registry();

            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                world.AddComponent(entity, new Local());
            }

            stopwatch.Stop();
            result.CreateEntitiesWith1DenseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void CreateEntitiesWith1SparseComponents(int entitiesCount, Stopwatch stopwatch,
            ref BenchmarkResult result)
        {
            stopwatch.Restart();

            var world = new Registry();

            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                if (i % 2 == 0)
                {
                    world.AddComponent(entity, new Local());
                }
            }

            stopwatch.Stop();
            result.CreateEntitiesWith1SparseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void CreateEntitiesWith2DenseComponents(int entitiesCount, Stopwatch stopwatch,
            ref BenchmarkResult result)
        {
            stopwatch.Restart();

            var world = new Registry();

            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                world.AddComponent(entity, new Local());
                world.AddComponent(entity, new Remote());
            }

            stopwatch.Stop();
            result.CreateEntitiesWith2DenseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void CreateEntitiesWith2SparseComponents(int entitiesCount, Stopwatch stopwatch,
            ref BenchmarkResult result)
        {
            stopwatch.Restart();

            var world = new Registry();

            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                if (i % 2 == 0)
                {
                    world.AddComponent(entity, new Local());
                }

                if (i % 4 == 0)
                {
                    world.AddComponent(entity, new Remote());
                }
            }

            stopwatch.Stop();
            result.CreateEntitiesWith2SparseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void CreateEntitiesWith3DenseComponents(int entitiesCount, Stopwatch stopwatch,
            ref BenchmarkResult result)
        {
            stopwatch.Restart();

            var world = new Registry();

            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                world.AddComponent(entity, new Local());
                world.AddComponent(entity, new Remote());
                world.AddComponent(entity, new Health());
            }

            stopwatch.Stop();
            result.CreateEntitiesWith3DenseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void CreateEntitiesWith3SparseComponents(int entitiesCount, Stopwatch stopwatch,
            ref BenchmarkResult result)
        {
            stopwatch.Restart();

            var world = new Registry();

            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                if (i % 2 == 0)
                {
                    world.AddComponent(entity, new Local());
                }

                if (i % 4 == 0)
                {
                    world.AddComponent(entity, new Remote());
                }

                if (i % 8 == 0)
                {
                    world.AddComponent(entity, new Health());
                }
            }

            stopwatch.Stop();
            result.CreateEntitiesWith3SparseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void DestroyEntitiesWith1DenseComponents(int entitiesCount, Stopwatch stopwatch,
            ref BenchmarkResult result)
        {
            var world = new Registry();

            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                world.AddComponent(entity, new Local());
            }

            stopwatch.Restart();

            foreach (var entity in world.Each(Filter.Create(new[] { typeof(Local) })))
            {
                world.DestroyEntity(entity);
            }

            stopwatch.Stop();
            result.DestroyEntitiesWith1DenseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void DestroyEntitiesWith1SparseComponents(int entitiesCount, Stopwatch stopwatch,
            ref BenchmarkResult result)
        {
            var world = new Registry();

            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                if (i % 2 == 0)
                {
                    world.AddComponent(entity, new Local());
                }
            }

            stopwatch.Restart();

            foreach (var entity in world.Each(Filter.Create(
                         new[] { typeof(Local) })))
            {
                world.DestroyEntity(entity);
            }

            stopwatch.Stop();
            result.DestroyEntitiesWith1SparseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void DestroyEntitiesWith2DenseComponents(int entitiesCount, Stopwatch stopwatch,
            ref BenchmarkResult result)
        {
            var world = new Registry();

            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                world.AddComponent(entity, new Local());
                world.AddComponent(entity, new Remote());
            }

            stopwatch.Restart();

            foreach (var entity in world.Each(Filter.Create(
                         new[] { typeof(Local), typeof(Remote) })))
            {
                world.DestroyEntity(entity);
            }

            stopwatch.Stop();
            result.DestroyEntitiesWith2DenseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void DestroyEntitiesWith2SparseComponents(int entitiesCount, Stopwatch stopwatch,
            ref BenchmarkResult result)
        {
            var world = new Registry();

            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                if (i % 2 == 0)
                {
                    world.AddComponent(entity, new Local());
                }

                if (i % 4 == 0)
                {
                    world.AddComponent(entity, new Remote());
                }
            }

            stopwatch.Restart();

            foreach (var entity in world.Each(Filter.Create(
                         new[] { typeof(Local), typeof(Remote) })))
            {
                world.DestroyEntity(entity);
            }

            stopwatch.Stop();
            result.DestroyEntitiesWith2SparseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void DestroyEntitiesWith3DenseComponents(int entitiesCount, Stopwatch stopwatch,
            ref BenchmarkResult result)
        {
            var world = new Registry();

            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                world.AddComponent(entity, new Local());
                world.AddComponent(entity, new Remote());
                world.AddComponent(entity, new Health());
            }

            stopwatch.Restart();

            foreach (var entity in world.Each(Filter.Create(
                         new[] { typeof(Local), typeof(Remote), typeof(Health) })))
            {
                world.DestroyEntity(entity);
            }
            
            stopwatch.Stop();
            result.DestroyEntitiesWith3DenseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void DestroyEntitiesWith3SparseComponents(int entitiesCount, Stopwatch stopwatch,
            ref BenchmarkResult result)
        {
            var world = new Registry();

            for (var i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                if (i % 2 == 0)
                {
                    world.AddComponent(entity, new Local());
                }

                if (i % 4 == 0)
                {
                    world.AddComponent(entity, new Remote());
                }

                if (i % 8 == 0)
                {
                    world.AddComponent(entity, new Health());
                }
            }

            stopwatch.Restart();

            foreach (var entity in world.Each(Filter.Create(
                         new[] { typeof(Local), typeof(Remote), typeof(Health) })))
            {
                world.DestroyEntity(entity);
            }
            
            stopwatch.Stop();
            result.DestroyEntitiesWith3SparseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void AddNewComponentWith1DenseComponents(int entitiesCount, Stopwatch stopwatch,
            ref BenchmarkResult result)
        {
            var world = new Registry();

            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                world.AddComponent(entity, new Local());
            }

            stopwatch.Restart();

            foreach (var entity in world.Each(Filter.Create(
                         new []{ typeof(Local) })))
            {
                world.AddComponent(entity, new Remote());
            }

            stopwatch.Stop();
            result.AddNewComponentWith1DenseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void AddNewComponentWith1SparseComponents(int entitiesCount, Stopwatch stopwatch,
            ref BenchmarkResult result)
        {
            var world = new Registry();

            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                if (i % 2 == 0)
                {
                    world.AddComponent(entity, new Local());
                }
            }

            stopwatch.Restart();

            foreach (var entity in world.Each(Filter.Create(
                         new []{ typeof(Local) })))
            {
                if (entity % 4 == 0)
                {
                    world.AddComponent(entity, new Remote());
                }
            }

            stopwatch.Stop();
            result.AddNewComponentWith1SparseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void AddNewComponentWith2DenseComponents(int entitiesCount, Stopwatch stopwatch,
            ref BenchmarkResult result)
        {
            var world = new Registry();

            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                world.AddComponent(entity, new Local());
                world.AddComponent(entity, new Remote());
            }

            stopwatch.Restart();

            foreach (var entity in world.Each(Filter.Create(
                         new []{ typeof(Local), typeof(Remote) })))
            {
                world.AddComponent(entity, new Health());
            }

            stopwatch.Stop();
            result.AddNewComponentWith2DenseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void AddNewComponentWith2SparseComponents(int entitiesCount, Stopwatch stopwatch,
            ref BenchmarkResult result)
        {
            var world = new Registry();

            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                if (i % 2 == 0)
                {
                    world.AddComponent(entity, new Local());
                }

                if (i % 4 == 0)
                {
                    world.AddComponent(entity, new Remote());
                }
            }

            stopwatch.Restart();

            foreach (var entity in world.Each(Filter.Create(
                         new []{ typeof(Local), typeof(Remote) })))
            {
                if (entity % 8 == 0)
                {
                    world.AddComponent(entity, new Health());
                }
            }

            stopwatch.Stop();
            result.AddNewComponentWith2SparseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void AddNewComponentWith3DenseComponents(int entitiesCount, Stopwatch stopwatch,
            ref BenchmarkResult result)
        {
            var world = new Registry();

            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                world.AddComponent(entity, new Local());
                world.AddComponent(entity, new Remote());
                world.AddComponent(entity, new Health());
            }

            stopwatch.Restart();

            foreach (var entity in world.Each(Filter.Create(
                         new[] { typeof(Local), typeof(Remote), typeof(Health) })))
            {
                world.AddComponent(entity, new Velocity());
            }

            stopwatch.Stop();
            result.AddNewComponentWith3DenseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void AddNewComponentWith3SparseComponents(int entitiesCount, Stopwatch stopwatch,
            ref BenchmarkResult result)
        {
            var world = new Registry();

            for (var i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                if (i % 2 == 0)
                {
                    world.AddComponent(entity, new Local());
                }

                if (i % 4 == 0)
                {
                    world.AddComponent(entity, new Remote());
                }

                if (i % 8 == 0)
                {
                    world.AddComponent(entity, new Health());
                }
            }

            stopwatch.Restart();

            foreach (var entity in world.Each(Filter.Create(
                         new[] { typeof(Local), typeof(Remote), typeof(Health) })))
            {
                if (entity % 16 == 0)
                {
                    world.AddComponent(entity, new Velocity());
                }
            }

            stopwatch.Stop();
            result.AddNewComponentWith3SparseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void IterateWith1DenseComponents(int entitiesCount, Stopwatch stopwatch,
            ref BenchmarkResult result)
        {
            var world = new Registry();

            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                world.AddComponent(entity, new Local());
            }

            stopwatch.Restart();

            foreach (var entity in world.Each(Filter.Create(
                         new[] { typeof(Local) })))
            {
                ref var local = ref world.GetComponent<Local>(entity);

                local.i++;
            }

            stopwatch.Stop();
            result.IterateWith1DenseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void IterateWith1SparseComponents(int entitiesCount, Stopwatch stopwatch,
            ref BenchmarkResult result)
        {
            var world = new Registry();

            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                if (i % 2 == 0)
                {
                    world.AddComponent(entity, new Local());
                }
            }

            stopwatch.Restart();

            foreach (var entity in world.Each(Filter.Create(
                         new[] { typeof(Local) })))
            {
                ref var local = ref world.GetComponent<Local>(entity);

                local.i++;
            }

            stopwatch.Stop();
            result.IterateWith1SparseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void IterateWith2DenseComponents(int entitiesCount, Stopwatch stopwatch,
            ref BenchmarkResult result)
        {
            var world = new Registry();

            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                world.AddComponent(entity, new Local());
                world.AddComponent(entity, new Remote());
            }

            stopwatch.Restart();

            foreach (var entity in world.Each(Filter.Create(
                         new[] { typeof(Local), typeof(Remote) })))
            {
                ref var local = ref world.GetComponent<Local>(entity);
                ref var remote = ref world.GetComponent<Remote>(entity);

                local.i++;
                remote.i++;
            }

            stopwatch.Stop();
            result.IterateWith2DenseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void IterateWith2SparseComponents(int entitiesCount, Stopwatch stopwatch,
            ref BenchmarkResult result)
        {
            var world = new Registry();

            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                if (i % 2 == 0)
                {
                    world.AddComponent(entity, new Local());
                }

                if (i % 4 == 0)
                {
                    world.AddComponent(entity, new Remote());
                }
            }

            stopwatch.Restart();

            foreach (var entity in world.Each(Filter.Create(
                         new[] { typeof(Local), typeof(Remote) })))
            {
                ref var local = ref world.GetComponent<Local>(entity);
                ref var remote = ref world.GetComponent<Remote>(entity);

                local.i++;
                remote.i++;
            }

            stopwatch.Stop();
            result.IterateWith2SparseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void IterateWith3DenseComponents(int entitiesCount, Stopwatch stopwatch,
            ref BenchmarkResult result)
        {
            var world = new Registry();

            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                world.AddComponent(entity, new Local());
                world.AddComponent(entity, new Remote());
                world.AddComponent(entity, new Health());
            }

            stopwatch.Restart();

            foreach (var entity in world.Each(Filter.Create(
                         new[] { typeof(Local), typeof(Remote), typeof(Health) })))
            {
                ref var local = ref world.GetComponent<Local>(entity);
                ref var remote = ref world.GetComponent<Remote>(entity);
                ref var health = ref world.GetComponent<Health>(entity);

                local.i++;
                remote.i++;
                health.i++;
            }

            stopwatch.Stop();
            result.IterateWith3DenseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void IterateWith3SparseComponents(int entitiesCount, Stopwatch stopwatch,
            ref BenchmarkResult result)
        {
            var world = new Registry();

            for (var i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                if (i % 2 == 0)
                {
                    world.AddComponent(entity, new Local());
                }

                if (i % 4 == 0)
                {
                    world.AddComponent(entity, new Remote());
                }

                if (i % 8 == 0)
                {
                    world.AddComponent(entity, new Health());
                }
            }

            stopwatch.Restart();

            foreach (var entity in world.Each(Filter.Create(
                         new[] { typeof(Local), typeof(Remote), typeof(Health) })))
            {
                ref var local = ref world.GetComponent<Local>(entity);
                ref var remote = ref world.GetComponent<Remote>(entity);
                ref var health = ref world.GetComponent<Health>(entity);

                local.i++;
                remote.i++;
                health.i++;
            }

            stopwatch.Stop();
            result.IterateWith3SparseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }
    }
}