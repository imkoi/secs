using System;
using System.Collections.Generic;
using System.Diagnostics;
using Scellecs.Morpeh;

namespace DefaultNamespace
{
    public class MorpehBenchmark
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
        
        public static void CreateEntitiesWith1DenseComponents(int entitiesCount, Stopwatch stopwatch, ref BenchmarkResult result)
        {
            stopwatch.Restart();
            
            var world = World.Create();
            var localStash = world.GetStash<Local>();

            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                localStash.Add(entity);
            }
            
            stopwatch.Stop();
            result.CreateEntitiesWith1DenseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void CreateEntitiesWith1SparseComponents(int entitiesCount, Stopwatch stopwatch, ref BenchmarkResult result)
        {
            stopwatch.Restart();
            
            var world = World.Create();
            var localStash = world.GetStash<Local>();
            
            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                if (i % 2 == 0)
                {
                    localStash.Add(entity);
                }
            }
            
            stopwatch.Stop();
            result.CreateEntitiesWith1SparseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }
        
        public static void CreateEntitiesWith2DenseComponents(int entitiesCount, Stopwatch stopwatch, ref BenchmarkResult result)
        {
            stopwatch.Restart();
            
            var world = World.Create();
            var localStash = world.GetStash<Local>();
            var remoteStash = world.GetStash<Remote>();
            
            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                localStash.Add(entity);
                remoteStash.Add(entity);
            }
            
            stopwatch.Stop();
            result.CreateEntitiesWith2DenseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void CreateEntitiesWith2SparseComponents(int entitiesCount, Stopwatch stopwatch, ref BenchmarkResult result)
        {
            stopwatch.Restart();
            
            var world = World.Create();
            var localStash = world.GetStash<Local>();
            var remoteStash = world.GetStash<Remote>();
            
            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                if (i % 2 == 0)
                {
                    localStash.Add(entity);
                }

                if (i % 4 == 0)
                {
                    remoteStash.Add(entity);
                }
            }
            
            stopwatch.Stop();
            result.CreateEntitiesWith2SparseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }
        
        public static void CreateEntitiesWith3DenseComponents(int entitiesCount, Stopwatch stopwatch, ref BenchmarkResult result)
        {
            stopwatch.Restart();
            
            var world = World.Create();
            var localStash = world.GetStash<Local>();
            var remoteStash = world.GetStash<Remote>();
            var healthStash = world.GetStash<Health>();
            
            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                localStash.Add(entity);
                remoteStash.Add(entity);
                healthStash.Add(entity);
            }
            
            stopwatch.Stop();
            result.CreateEntitiesWith3DenseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void CreateEntitiesWith3SparseComponents(int entitiesCount, Stopwatch stopwatch, ref BenchmarkResult result)
        {
            stopwatch.Restart();
            
            var world = World.Create();
            var localStash = world.GetStash<Local>();
            var remoteStash = world.GetStash<Remote>();
            var healthStash = world.GetStash<Health>();
            
            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                if (i % 2 == 0)
                {
                    localStash.Add(entity);
                }

                if (i % 4 == 0)
                {
                    remoteStash.Add(entity);
                }

                if (i % 8 == 0)
                {
                    healthStash.Add(entity);
                }
            }
            
            stopwatch.Stop();
            result.CreateEntitiesWith3SparseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }
        
        public static void DestroyEntitiesWith1DenseComponents(int entitiesCount, Stopwatch stopwatch, ref BenchmarkResult result)
        {
            var world = World.Create();
            var localStash = world.GetStash<Local>();

            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                localStash.Add(entity);
            }

            world.Commit();

            var filter = world.Filter.With<Local>().Build();
            
            stopwatch.Restart();

            foreach (var entity in filter)
            {
                world.RemoveEntity(entity);
            }
            
            world.Commit();
            
            stopwatch.Stop();
            result.DestroyEntitiesWith1DenseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void DestroyEntitiesWith1SparseComponents(int entitiesCount, Stopwatch stopwatch, ref BenchmarkResult result)
        {
            var world = World.Create();
            var localStash = world.GetStash<Local>();
            
            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                if (i % 2 == 0)
                {
                    localStash.Add(entity);
                }
            }
            
            world.Commit();

            var filter = world.Filter.With<Local>().With<Remote>().Build();
            
            stopwatch.Restart();

            foreach (var entity in filter)
            {
                world.RemoveEntity(entity);
            }
            
            world.Commit();
            
            stopwatch.Stop();
            result.DestroyEntitiesWith1SparseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }
        
        public static void DestroyEntitiesWith2DenseComponents(int entitiesCount, Stopwatch stopwatch, ref BenchmarkResult result)
        {
            var world = World.Create();
            var localStash = world.GetStash<Local>();
            var remoteStash = world.GetStash<Remote>();
            
            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                localStash.Add(entity);
                remoteStash.Add(entity);
            }
            
            world.Commit();

            var filter = world.Filter.With<Local>().With<Remote>().Build();
            
            stopwatch.Restart();

            foreach (var entity in filter)
            {
                world.RemoveEntity(entity);
            }
            
            world.Commit();
            
            stopwatch.Stop();
            result.DestroyEntitiesWith2DenseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void DestroyEntitiesWith2SparseComponents(int entitiesCount, Stopwatch stopwatch, ref BenchmarkResult result)
        {
            var world = World.Create();
            var localStash = world.GetStash<Local>();
            var remoteStash = world.GetStash<Remote>();
            
            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                if (i % 2 == 0)
                {
                    localStash.Add(entity);
                }

                if (i % 4 == 0)
                {
                    remoteStash.Add(entity);
                }
            }
            
            world.Commit();

            var filter = world.Filter.With<Local>().With<Remote>().Build();
            
            stopwatch.Restart();

            foreach (var entity in filter)
            {
                world.RemoveEntity(entity);
            }
            
            world.Commit();
            
            stopwatch.Stop();
            result.DestroyEntitiesWith2SparseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }
        
        public static void DestroyEntitiesWith3DenseComponents(int entitiesCount, Stopwatch stopwatch, ref BenchmarkResult result)
        {
            var world = World.Create();
            var localStash = world.GetStash<Local>();
            var remoteStash = world.GetStash<Remote>();
            var healthStash = world.GetStash<Health>();
            
            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                localStash.Add(entity);
                remoteStash.Add(entity);
                healthStash.Add(entity);
            }
            
            world.Commit();

            var filter = world.Filter.With<Local>().With<Remote>().With<Health>().Build();
            
            stopwatch.Restart();

            foreach (var entity in filter)
            {
                world.RemoveEntity(entity);
            }
            
            world.Commit();
            
            stopwatch.Stop();
            result.DestroyEntitiesWith3DenseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void DestroyEntitiesWith3SparseComponents(int entitiesCount, Stopwatch stopwatch, ref BenchmarkResult result)
        {
            var world = World.Create();
            var localStash = world.GetStash<Local>();
            var remoteStash = world.GetStash<Remote>();
            var healthStash = world.GetStash<Health>();
            
            for (var i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                if (i % 2 == 0)
                {
                    localStash.Add(entity);
                }

                if (i % 4 == 0)
                {
                    remoteStash.Add(entity);
                }

                if (i % 8 == 0)
                {
                    healthStash.Add(entity);
                }
            }
            
            world.Commit();

            var filter = world.Filter.With<Local>().With<Remote>().With<Health>().Build();
            
            stopwatch.Restart();

            foreach (var entity in filter)
            {
                world.RemoveEntity(entity);
            }
            
            world.Commit();
            
            stopwatch.Stop();
            result.DestroyEntitiesWith3SparseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }
        
        public static void AddNewComponentWith1DenseComponents(int entitiesCount, Stopwatch stopwatch, ref BenchmarkResult result)
        {
            var world = World.Create();
            var localStash = world.GetStash<Local>();
            var remoteStash = world.GetStash<Remote>();

            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                localStash.Add(entity);
            }
            
            world.Commit();

            var filter = world.Filter.With<Local>().Build();
            
            stopwatch.Restart();

            foreach (var entity in filter)
            {
                remoteStash.Add(entity);
            }
            
            world.Commit();
            
            stopwatch.Stop();
            result.AddNewComponentWith1DenseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void AddNewComponentWith1SparseComponents(int entitiesCount, Stopwatch stopwatch, ref BenchmarkResult result)
        {
            var world = World.Create();
            var localStash = world.GetStash<Local>();
            var remoteStash = world.GetStash<Remote>();
            
            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                if (i % 2 == 0)
                {
                    localStash.Add(entity);
                }
            }
            
            world.Commit();

            var filter = world.Filter.With<Local>().Build();
            
            stopwatch.Restart();

            foreach (var entity in filter)
            {
                if (entity.Id % 4 == 0)
                {
                    remoteStash.Add(entity);
                }
            }
            
            world.Commit();
            
            stopwatch.Stop();
            result.AddNewComponentWith1SparseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }
        
        public static void AddNewComponentWith2DenseComponents(int entitiesCount, Stopwatch stopwatch, ref BenchmarkResult result)
        {
            var world = World.Create();
            var localStash = world.GetStash<Local>();
            var remoteStash = world.GetStash<Remote>();
            var healthStash = world.GetStash<Health>();
            
            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                localStash.Add(entity);
                remoteStash.Add(entity);
            }
            
            world.Commit();

            var filter = world.Filter.With<Local>().With<Remote>().Build();
            
            stopwatch.Restart();

            foreach (var entity in filter)
            {
                healthStash.Add(entity);
            }
            
            world.Commit();
            
            stopwatch.Stop();
            result.AddNewComponentWith2DenseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void AddNewComponentWith2SparseComponents(int entitiesCount, Stopwatch stopwatch, ref BenchmarkResult result)
        {
            var world = World.Create();
            var localStash = world.GetStash<Local>();
            var remoteStash = world.GetStash<Remote>();
            var healthStash = world.GetStash<Health>();
            
            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                if (i % 2 == 0)
                {
                    localStash.Add(entity);
                }

                if (i % 4 == 0)
                {
                    remoteStash.Add(entity);
                }
            }
            
            world.Commit();

            var filter = world.Filter.With<Local>().With<Remote>().Build();
            
            stopwatch.Restart();

            foreach (var entity in filter)
            {
                if (entity.Id % 8 == 0)
                {
                    healthStash.Add(entity);
                }
            }
            
            world.Commit();
            
            stopwatch.Stop();
            result.AddNewComponentWith2SparseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }
        
        public static void AddNewComponentWith3DenseComponents(int entitiesCount, Stopwatch stopwatch, ref BenchmarkResult result)
        {
            var world = World.Create();
            var localStash = world.GetStash<Local>();
            var remoteStash = world.GetStash<Remote>();
            var healthStash = world.GetStash<Health>();
            var velocityStash = world.GetStash<Velocity>();
            
            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                localStash.Add(entity);
                remoteStash.Add(entity);
                healthStash.Add(entity);
            }
            
            world.Commit();

            var filter = world.Filter.With<Local>().With<Remote>().With<Health>().Build();
            
            stopwatch.Restart();

            foreach (var entity in filter)
            {
                velocityStash.Add(entity);
            }
            
            world.Commit();
            
            stopwatch.Stop();
            result.AddNewComponentWith3DenseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void AddNewComponentWith3SparseComponents(int entitiesCount, Stopwatch stopwatch, ref BenchmarkResult result)
        {
            var world = World.Create();
            var localStash = world.GetStash<Local>();
            var remoteStash = world.GetStash<Remote>();
            var healthStash = world.GetStash<Health>();
            var velocityStash = world.GetStash<Velocity>();
            
            for (var i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                if (i % 2 == 0)
                {
                    localStash.Add(entity);
                }

                if (i % 4 == 0)
                {
                    remoteStash.Add(entity);
                }

                if (i % 8 == 0)
                {
                    healthStash.Add(entity);
                }
            }
            
            world.Commit();

            var filter = world.Filter.With<Local>().With<Remote>().With<Health>().Build();
            
            stopwatch.Restart();

            foreach (var entity in filter)
            {
                if (entity.Id % 16 == 0)
                {
                    velocityStash.Add(entity);
                }
            }
            
            world.Commit();
            
            stopwatch.Stop();
            result.AddNewComponentWith3SparseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void IterateWith1DenseComponents(int entitiesCount, Stopwatch stopwatch, ref BenchmarkResult result)
        {
            var world = World.Create();
            var localStash = world.GetStash<Local>();

            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                localStash.Add(entity);
            }
            
            world.Commit();

            var filter = world.Filter.With<Local>().Build();
            
            stopwatch.Restart();

            foreach (var entity in filter)
            {
                ref var local = ref localStash.Get(entity);

                local.i++;
            }
            
            stopwatch.Stop();
            result.IterateWith1DenseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void IterateWith1SparseComponents(int entitiesCount, Stopwatch stopwatch, ref BenchmarkResult result)
        {
            var world = World.Create();
            var localStash = world.GetStash<Local>();
            
            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                if (i % 2 == 0)
                {
                    localStash.Add(entity);
                }
            }
            
            world.Commit();

            var filter = world.Filter.With<Local>().Build();
            
            stopwatch.Restart();

            foreach (var entity in filter)
            {
                ref var local = ref localStash.Get(entity);

                local.i++;
            }
            
            world.Commit();
            
            stopwatch.Stop();
            result.IterateWith1SparseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }
        
        public static void IterateWith2DenseComponents(int entitiesCount, Stopwatch stopwatch, ref BenchmarkResult result)
        {
            var world = World.Create();
            var localStash = world.GetStash<Local>();
            var remoteStash = world.GetStash<Remote>();

            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                localStash.Add(entity);
                remoteStash.Add(entity);
            }
            
            world.Commit();

            var filter = world.Filter.With<Local>().With<Remote>().Build();
            
            stopwatch.Restart();

            foreach (var entity in filter)
            {
                ref var local = ref localStash.Get(entity);
                ref var remote = ref remoteStash.Get(entity);

                local.i++;
                remote.i++;
            }
            
            world.Commit();
            
            stopwatch.Stop();
            result.IterateWith2DenseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void IterateWith2SparseComponents(int entitiesCount, Stopwatch stopwatch, ref BenchmarkResult result)
        {
            var world = World.Create();
            var localStash = world.GetStash<Local>();
            var remoteStash = world.GetStash<Remote>();
            
            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                if (i % 2 == 0)
                {
                    localStash.Add(entity);
                }

                if (i % 4 == 0)
                {
                    remoteStash.Add(entity);
                }
            }
            
            world.Commit();

            var filter = world.Filter.With<Local>().With<Remote>().Build();
            
            stopwatch.Restart();

            foreach (var entity in filter)
            {
                ref var local = ref localStash.Get(entity);
                ref var remote = ref remoteStash.Get(entity);

                local.i++;
                remote.i++;
            }
            
            world.Commit();
            
            stopwatch.Stop();
            result.IterateWith2SparseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }
        
        public static void IterateWith3DenseComponents(int entitiesCount, Stopwatch stopwatch, ref BenchmarkResult result)
        {
            var world = World.Create();
            var localStash = world.GetStash<Local>();
            var remoteStash = world.GetStash<Remote>();
            var healthStash = world.GetStash<Health>();
            
            for (int i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                localStash.Add(entity);
                remoteStash.Add(entity);
                healthStash.Add(entity);
            }
            
            world.Commit();

            var filter = world.Filter.With<Local>().With<Remote>().With<Health>().Build();
            
            stopwatch.Restart();

            foreach (var entity in filter)
            {
                ref var local = ref localStash.Get(entity);
                ref var remote = ref remoteStash.Get(entity);
                ref var health = ref healthStash.Get(entity);

                local.i++;
                remote.i++;
                health.i++;
            }
            
            world.Commit();
            
            stopwatch.Stop();
            result.IterateWith3DenseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void IterateWith3SparseComponents(int entitiesCount, Stopwatch stopwatch, ref BenchmarkResult result)
        {
            var world = World.Create();
            var localStash = world.GetStash<Local>();
            var remoteStash = world.GetStash<Remote>();
            var healthStash = world.GetStash<Health>();
            
            for (var i = 0; i < entitiesCount; i++)
            {
                var entity = world.CreateEntity();

                if (i % 2 == 0)
                {
                    localStash.Add(entity);
                }

                if (i % 4 == 0)
                {
                    remoteStash.Add(entity);
                }

                if (i % 8 == 0)
                {
                    healthStash.Add(entity);
                }
            }
            
            world.Commit();

            var filter = world.Filter.With<Local>().With<Remote>().With<Health>().Build();
            
            stopwatch.Restart();

            foreach (var entity in filter)
            {
                ref var local = ref localStash.Get(entity);
                ref var remote = ref remoteStash.Get(entity);
                ref var health = ref healthStash.Get(entity);

                local.i++;
                remote.i++;
                health.i++;
            }
            
            world.Commit();
            
            stopwatch.Stop();
            result.IterateWith3SparseComponents += stopwatch.Elapsed.TotalMilliseconds;
        }
    }
}