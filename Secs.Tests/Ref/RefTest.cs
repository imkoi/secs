using FluentAssertions;
using NUnit.Framework;
using Secs;

namespace Secs.Tests;

public class RefTest
{
    [Test]
    public void RefValue_AccessesComponentData()
    {
        var reg = new Registry();
        var e = reg.CreateEntity();
        reg.AddComponent(e, new C0 { Value = 42 });

        foreach (var refC0 in reg.Each<C0>())
        {
            refC0.Value.Value.Should().Be(42);
        }
    }

    [Test]
    public void RefValue_MutatesComponentData()
    {
        var reg = new Registry();
        var e = reg.CreateEntity();
        reg.AddComponent(e, new C0 { Value = 1 });

        foreach (var refC0 in reg.Each<C0>())
        {
            refC0.Value.Value = 999;
        }

        reg.GetComponent<C0>(e).Value.Should().Be(999);
    }
}
