using FluentAssertions;
using NUnit.Framework;

namespace Secs.Tests;

public class ComponentsCreateEntitiesTest
{
    [Test]
    public void Test()
    {
        var reg = new Registry();

        var playerComponents = reg.GetComponents<Player>();

        for (var i = 0; i < 100_000; i++)
        {
            var entity = reg.CreateEntity();
            playerComponents.AddComponent(entity, new Player());
        }

        var amount = 0;

        foreach (var (entityId, _) in reg.EachWithEntity<Player>())
        {
            playerComponents.RemoveComponent(entityId);
            
            amount++;
        }
        
        foreach (var _ in reg.EachWithEntity<Player>())
        {
            amount++;
        }

        amount.Should().Be(100_000);
    }

    private struct Player
    {
        
    }
}