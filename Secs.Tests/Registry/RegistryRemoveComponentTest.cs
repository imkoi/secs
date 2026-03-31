using FluentAssertions;
using NUnit.Framework;
using Secs;

namespace Secs.Tests;

public class RegistryRemoveComponentTest
{
    [Test]
    public void RemoveComponent_Generic_RemovesComponent()
    {
        var reg = new Registry();
        var e = reg.CreateEntity();
        reg.AddComponent(e, new C0 { Value = 1 });
        reg.AddComponent(e, new C1 { Value = 2 });
        reg.RemoveComponent<C0>(e);
        reg.HasComponent<C0>(e).Should().BeFalse();
        reg.HasComponent<C1>(e).Should().BeTrue();
    }

    [Test]
    public void RemoveComponent_LastComponent_MovesToPool()
    {
        var reg = new Registry();
        var e = reg.CreateEntity();
        reg.AddComponent(e, new C0 { Value = 1 });
        reg.RemoveComponent<C0>(e);
        var e2 = reg.CreateEntity();
        e2.Should().Be(e);
    }

    [Test]
    public void RemoveComponent_SwapAndRemove_KeepsOtherEntitiesIntact()
    {
        var reg = new Registry();
        var e0 = reg.CreateEntity();
        var e1 = reg.CreateEntity();
        var e2 = reg.CreateEntity();
        reg.AddComponent(e0, new C0 { Value = 10 });
        reg.AddComponent(e0, new C1 { Value = 100 });
        reg.AddComponent(e1, new C0 { Value = 20 });
        reg.AddComponent(e1, new C1 { Value = 200 });
        reg.AddComponent(e2, new C0 { Value = 30 });
        reg.AddComponent(e2, new C1 { Value = 300 });

        reg.RemoveComponent<C0>(e0);
        reg.HasComponent<C0>(e0).Should().BeFalse();
        reg.GetComponent<C0>(e1).Value.Should().Be(20);
        reg.GetComponent<C0>(e2).Value.Should().Be(30);
    }

    [Test]
    public void RemoveComponent_LastInDense_DirectRemoval()
    {
        var reg = new Registry();
        var e0 = reg.CreateEntity();
        var e1 = reg.CreateEntity();
        reg.AddComponent(e0, new C0 { Value = 10 });
        reg.AddComponent(e0, new C1 { Value = 100 });
        reg.AddComponent(e1, new C0 { Value = 20 });
        reg.AddComponent(e1, new C1 { Value = 200 });

        reg.RemoveComponent<C0>(e1);
        reg.HasComponent<C0>(e1).Should().BeFalse();
        reg.GetComponent<C0>(e0).Value.Should().Be(10);
    }
}
