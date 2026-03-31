using FluentAssertions;
using NUnit.Framework;
using Secs;

namespace Secs.Tests;

public class RegistryGetComponentTest
{
    [Test]
    public void HasComponent_ReturnsFalse_ForUnregisteredType()
    {
        var reg = new Registry();
        var e = reg.CreateEntity();
        reg.HasComponent<C0>(e).Should().BeFalse();
    }

    [Test]
    public void HasComponent_ReturnsTrue_WhenPresent()
    {
        var reg = new Registry();
        var e = reg.CreateEntity();
        reg.AddComponent(e, new C0 { Value = 1 });
        reg.HasComponent<C0>(e).Should().BeTrue();
    }

    [Test]
    public void GetComponent_ReturnsMutableRef()
    {
        var reg = new Registry();
        var e = reg.CreateEntity();
        reg.AddComponent(e, new C0 { Value = 1 });
        ref var c = ref reg.GetComponent<C0>(e);
        c.Value = 999;
        reg.GetComponent<C0>(e).Value.Should().Be(999);
    }

    [Test]
    public void GetComponents_ReturnsCachedWrapper()
    {
        var reg = new Registry();
        var c1 = reg.GetComponents<C0>();
        var c2 = reg.GetComponents<C0>();
        // Both should work for the same component type
        var e = reg.CreateEntity();
        c1.AddComponent(e, new C0 { Value = 42 });
        c2.HasComponent(e).Should().BeTrue();
    }
}
