using FluentAssertions;
using NUnit.Framework;
using Secs;

namespace Secs.Tests;

public class ComponentsWrapperTest
{
    [Test]
    public void AddComponent_StoresAndReturnsRef()
    {
        var reg = new Registry();
        var components = reg.GetComponents<C0>();
        var e = reg.CreateEntity();
        ref var c = ref components.AddComponent(e, new C0 { Value = 42 });
        c.Value.Should().Be(42);
        c.Value = 99;
        components.GetComponent(e).Value.Should().Be(99);
    }

    [Test]
    public void AddComponent_ResizesSparseArray()
    {
        var reg = new Registry();
        var e0 = reg.CreateEntity();
        var components = reg.GetComponents<C0>();
        components.AddComponent(e0, new C0 { Value = 1 });
        var e1 = reg.CreateEntity();
        components.AddComponent(e1, new C0 { Value = 2 });
        components.GetComponent(e1).Value.Should().Be(2);
    }

    [Test]
    public void AddComponent_ResizesDenseArray()
    {
        var reg = new Registry();
        var components = reg.GetComponents<C0>();
        for (var i = 0; i < 40; i++)
        {
            var e = reg.CreateEntity();
            components.AddComponent(e, new C0 { Value = i });
        }

        for (var i = 0; i < 40; i++)
            components.GetComponent(i).Value.Should().Be(i);
    }

    [Test]
    public void RemoveComponent_NotPresent_ReturnsZero()
    {
        var reg = new Registry();
        var e = reg.CreateEntity();
        var components = reg.GetComponents<C0>();
        components.RemoveComponent(e).Should().Be(0);
    }

    [Test]
    public void RemoveComponent_LastInDense_ReturnsOne()
    {
        var reg = new Registry();
        var e = reg.CreateEntity();
        var components = reg.GetComponents<C0>();
        reg.AddComponent(e, new C1 { Value = 1 });
        components.AddComponent(e, new C0 { Value = 1 });
        components.RemoveComponent(e).Should().Be(1);
        components.HasComponent(e).Should().BeFalse();
    }

    [Test]
    public void RemoveComponent_SwapAndRemove()
    {
        var reg = new Registry();
        var e0 = reg.CreateEntity();
        var e1 = reg.CreateEntity();
        var e2 = reg.CreateEntity();
        var components = reg.GetComponents<C0>();
        reg.AddComponent(e0, new C1 { Value = 100 });
        reg.AddComponent(e1, new C1 { Value = 200 });
        reg.AddComponent(e2, new C1 { Value = 300 });
        components.AddComponent(e0, new C0 { Value = 10 });
        components.AddComponent(e1, new C0 { Value = 20 });
        components.AddComponent(e2, new C0 { Value = 30 });

        components.RemoveComponent(e0).Should().Be(1);
        components.HasComponent(e0).Should().BeFalse();
        components.GetComponent(e1).Value.Should().Be(20);
        components.GetComponent(e2).Value.Should().Be(30);
    }

    [Test]
    public void RemoveComponent_LastComponent_MovesToPool()
    {
        var reg = new Registry();
        var e = reg.CreateEntity();
        var components = reg.GetComponents<C0>();
        components.AddComponent(e, new C0 { Value = 1 });
        components.RemoveComponent(e);
        var e2 = reg.CreateEntity();
        e2.Should().Be(e);
    }

    [Test]
    public void RemoveComponent_NotLastComponent_UpdatesCount()
    {
        var reg = new Registry();
        var e = reg.CreateEntity();
        reg.AddComponent(e, new C0 { Value = 1 });
        reg.AddComponent(e, new C1 { Value = 2 });
        var components = reg.GetComponents<C0>();
        components.RemoveComponent(e).Should().Be(1);
        reg.HasComponent<C1>(e).Should().BeTrue();
    }

    [Test]
    public void HasComponent_ReturnsTrueWhenPresent()
    {
        var reg = new Registry();
        var e = reg.CreateEntity();
        var components = reg.GetComponents<C0>();
        components.AddComponent(e, new C0 { Value = 1 });
        components.HasComponent(e).Should().BeTrue();
    }

    [Test]
    public void GetComponent_ReturnsMutableRef()
    {
        var reg = new Registry();
        var e = reg.CreateEntity();
        var components = reg.GetComponents<C0>();
        components.AddComponent(e, new C0 { Value = 1 });
        ref var c = ref components.GetComponent(e);
        c.Value = 999;
        components.GetComponent(e).Value.Should().Be(999);
    }
}
