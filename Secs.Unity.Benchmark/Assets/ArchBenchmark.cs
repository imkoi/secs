using System.Diagnostics;
using Arch.Core;
using UnityEngine;

namespace DefaultNamespace
{
    public class ArchBenchmark
    {
        public static void Run(Stopwatch stopwatch, out BenchmarkResult result)
        {
            result = default;
            
            stopwatch.Restart();
            var world = World.Create();

            for (var i = 0; i < 1000000; ++i)
            {
                var entity = world.Create();
                
                world.Add<Player>(entity);
                world.Add<Position>(entity);

                if (i % 2 == 0)
                {
                    world.Add<Local>(entity);
                }

                if (i % 4 == 0)
                {
                    world.Add<Remote>(entity);
                }

                if (i % 8 == 0)
                {
                    world.Add<Health>(entity);
                }
            }
            
            stopwatch.Stop();
            result.CreateEntitiesTime = stopwatch.Elapsed.TotalMilliseconds;
            stopwatch.Restart();

            world.Query(QueryDescription.Null, (ref Player player) =>
            {
                player.age = 24;
            });
            
            stopwatch.Stop();
            result.IterateOneComponentTime = stopwatch.Elapsed.TotalMilliseconds;
            stopwatch.Restart();
            
            world.Query(QueryDescription.Null, (ref Player player, ref Position position) =>
            {
                player.age = 24;
                position.x = 100;
                position.y = 100;
                position.z = 100;
            });
            
            stopwatch.Stop();
            result.IterateTwoComponentTime = stopwatch.Elapsed.TotalMilliseconds;
            stopwatch.Restart();
            
            world.Query(QueryDescription.Null, (ref Player player, ref Position position) =>
            {
                player.age = 24;
                position.x = 100;
                position.y = 100;
                position.z = 100;
            });
            
            world.Query(QueryDescription.Null, (ref Player player, ref Position position) =>
            {
                player.age = 24;
                position.x = 100;
                position.y = 100;
                position.z = 100;
            });
            
            world.Query(QueryDescription.Null, (ref Player player, ref Position position) =>
            {
                player.age = 24;
                position.x = 100;
                position.y = 100;
                position.z = 100;
            });
            
            stopwatch.Stop();
            result.IterateTwoComponent3Time = stopwatch.Elapsed.TotalMilliseconds;
            stopwatch.Restart();
            
            world.Query(QueryDescription.Null, (ref Local local, ref Remote remote, ref Health health) =>
            {
                local.i = -1;
                remote.i = -1;
                health.i = -1;
            });
            world.Query(QueryDescription.Null, (ref Local local, ref Remote remote, ref Health health) =>
            {
                local.i = -1;
                remote.i = -1;
                health.i = -1;
            });
            world.Query(QueryDescription.Null, (ref Local local, ref Remote remote, ref Health health) =>
            {
                local.i = -1;
                remote.i = -1;
                health.i = -1;
            });
            
            stopwatch.Stop();
            result.IterateThreeSparseComponent3Time = stopwatch.Elapsed.TotalMilliseconds;
            stopwatch.Restart();
        }
    }
}