using Secs;
using NUnit.Framework;

public class AllTests
{
    [Test]
    public void Test()
    {
        var reg = new Registry();

        for (var i = 0; i < 8; i++)
        {
            var player = reg.CreateEntity();

            //reg.AddComponent(player, new Position());
            //reg.AddComponent(player, new Other());

            if (i % 2 == 0)
            {
                //reg.AddComponent(player, new Velocity());
            }
            
            if (i % 4 == 0)
            {
                //reg.AddComponent(player, new Player());
            }
        }
        
        var filter = Filter.Create([typeof(Position), typeof(Player)]);

        // foreach (var entityId in reg.Each(filter))
        // {
        //     Console.WriteLine(entityId);
        // }
        //
        // foreach (var refPlayer in reg.Each<Player>(filter))
        // {
        //     Console.WriteLine(refPlayer);
        // }
        //
        // foreach (var (entityId, refPlayer) in reg.EachWithEntity<Player>(filter))
        // {
        //     Console.WriteLine($"{entityId}: {refPlayer}");
        // }

        foreach (var refPlayer in reg.Each<Player>())
        {
            refPlayer.Value.health = 100;
        }
        
        foreach (var (entityId, refPlayer, refPosition) in reg.EachWithEntity<Player, Position>())
        {
            Console.WriteLine($"{entityId}: {refPlayer}");
            Console.WriteLine($"{entityId}: {refPosition}");
        }
        
        foreach (var (entityId, refPlayer, refPosition, _) in reg.EachWithEntity<Player, Position, Velocity>())
        {
            Console.WriteLine($"{entityId}: {refPlayer}");
            Console.WriteLine($"{entityId}: {refPosition}");
        }

        return;

        reg.Each((Registry _, Position position) =>
        {
            //reg.DeleteEntity(entity);
        }, Filter.Create([typeof(Velocity)]));
        
        reg.Each((Registry _, ref Position position) =>
        {
            //reg.DeleteEntity(entity);
        }, Filter.Create([typeof(Velocity)]));

        reg.EachWithEntity((Registry r, int entity, ref Position position) =>
        {
            Console.WriteLine($"Remove Position on entity {entity}");
            
            r.RemoveComponent<Position>(entity);
        });
        
        reg.EachWithEntity((Registry r, int entity, ref Player player) =>
        {
            Console.WriteLine($"Destroy entity {entity} with Player");

            r.DestroyEntity(entity);
        }, Filter.Create([typeof(Velocity)]));
        
        reg.EachWithEntity((Registry r, int entity, ref Velocity velocity) =>
        {
            Console.WriteLine($"Destroy entity {entity} with Velocity");

            r.DestroyEntity(entity);
        });
        
        reg.EachWithEntity((Registry r, int entity, ref Other other) =>
        {
            Console.WriteLine($"Entity[{entity}] with Other");
        }, Filter.Create(exclude: [typeof(Player), typeof(Position)]));
        
        //var group = reg.GetGroup([typeof(Position), typeof(Velocity)], [typeof(Player)]);
    }
    
    public struct Player
    {
        public int health;
    }

    public struct Velocity
    {
            
    }

    public struct Position
    {
            
    }
    
    public struct Other
    {
            
    }
}