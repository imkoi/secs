using FluentAssertions;
using NUnit.Framework;
using Secs;

namespace Secs.Tests;

public class RegistryEntityTest
{
    [Test]
    public void CreateEntity_ReturnsSequentialIds()
    {
        var reg = new Registry();
        reg.CreateEntity().Should().Be(0);
        reg.CreateEntity().Should().Be(1);
        reg.CreateEntity().Should().Be(2);
    }

    [Test]
    public void DestroyEntity_EnablesReuse()
    {
        var reg = new Registry();
        var e0 = reg.CreateEntity();
        reg.AddComponent(e0, new C0 { Value = 1 });
        reg.DestroyEntity(e0);

        var e1 = reg.CreateEntity();
        e1.Should().Be(e0);
    }

    [Test]
    public void CreateEntity_ResizesEntityInfos_WhenExceedingInitialCapacity()
    {
        var reg = new Registry();
        for (var i = 0; i < 5000; i++)
            reg.CreateEntity();
    }

    [Test]
    public void DestroyEntity_RemovesAllComponents()
    {
        var reg = new Registry();
        var e = reg.CreateEntity();
        reg.AddComponent(e, new C0 { Value = 1 });
        reg.AddComponent(e, new C1 { Value = 2 });

        reg.DestroyEntity(e);
        reg.HasComponent<C0>(e).Should().BeFalse();
        reg.HasComponent<C1>(e).Should().BeFalse();
    }

    [Test]
    public void DestroyEntity_SkipsUnregisteredComponentSlots()
    {
        var reg = new Registry();
        var e = reg.CreateEntity();
        reg.AddComponent(e, new C0 { Value = 1 });
        reg.AddComponent(e, new C2 { Value = 3 });
        reg.DestroyEntity(e);
        reg.HasComponent<C0>(e).Should().BeFalse();
        reg.HasComponent<C2>(e).Should().BeFalse();
    }

    [Test]
    public void DestroyEntity_Twice_DoesNotDoubleEnqueue()
    {
        var reg = new Registry();
        var e0 = reg.CreateEntity();
        reg.AddComponent(e0, new C0 { Value = 1 });
        reg.DestroyEntity(e0);
        reg.DestroyEntity(e0);

        var reused1 = reg.CreateEntity();
        reused1.Should().Be(e0);
        var next = reg.CreateEntity();
        next.Should().NotBe(e0);
    }

    [Test]
    public void DestroyEntity_Twice_NoComponents_DoesNotDoubleEnqueue()
    {
        var reg = new Registry();
        var e0 = reg.CreateEntity();
        reg.DestroyEntity(e0);
        reg.DestroyEntity(e0);

        var reused1 = reg.CreateEntity();
        reused1.Should().Be(e0);
        var next = reg.CreateEntity();
        next.Should().NotBe(e0);
    }

    [Test]
    public void DestroyEntity_WithGapInComponentIds_SkipsMissingSlots()
    {
        var reg = new Registry();
        var setup = reg.CreateEntity();
        reg.AddComponent(setup, new C0());
        reg.AddComponent(setup, new C1());
        reg.AddComponent(setup, new C2());

        var e = reg.CreateEntity();
        reg.AddComponent(e, new C0 { Value = 10 });
        reg.AddComponent(e, new C2 { Value = 30 });

        reg.DestroyEntity(e);
        reg.HasComponent<C0>(e).Should().BeFalse();
        reg.HasComponent<C2>(e).Should().BeFalse();
    }
}
