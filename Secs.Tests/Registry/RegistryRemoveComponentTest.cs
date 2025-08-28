using Secs;
using FluentAssertions;
using NUnit.Framework;

public class RegistryRemoveComponentTest
{
    [Test]
    public void AllComponentsRemoved_EntityDeleted()
    {
        var reg = new Registry();
        
        for (var i = 0; i < 8; i++)
        {
            var player = reg.CreateEntity();
                
            reg.AddComponent(player, new Position());
            reg.AddComponent(player, new Other());

            if (i % 2 == 0)
            {
                reg.AddComponent(player, new Velocity());
            }
            
            if (i % 4 == 0)
            {
                reg.AddComponent(player, new Player());
            }
        }

        int[] entitiesToDestroy = [1, 3, 5, 7];

        foreach (var entityId in entitiesToDestroy)
        {
            reg.RemoveComponent<Position>(entityId);
            reg.RemoveComponent<Other>(entityId);
        }

        foreach (var entityId in entitiesToDestroy)
        {
            reg._destroyedEntities.Contains(entityId).Should().BeTrue();
        }
    }
    
    [Test]
    public void Test()
    {
        var reg = new Registry();
        
        for (var i = 0; i < 100000; i++)
        {
            var player = reg.CreateEntity();
                
            reg.AddComponent(player, new Position());
        }
    }
    
    public struct Player
    {
            
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