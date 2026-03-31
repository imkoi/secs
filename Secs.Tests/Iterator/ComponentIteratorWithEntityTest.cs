using FluentAssertions;
using NUnit.Framework;
using Secs;

namespace Secs.Tests;

public class ComponentIteratorWithEntityTest
{
    // --- Basic iteration (1-3) ---

    [Test]
    public void ComponentIteratorWithEntity1_IteratesWithEntityId()
    {
        var reg = new Registry();
        var e = reg.CreateEntity();
        reg.AddComponent(e, new C0 { Value = 42 });

        var count = 0;
        foreach (var (entityId, refC0) in reg.EachWithEntity<C0>())
        {
            entityId.Should().Be(e);
            count++;
        }
        count.Should().Be(1);
    }

    [Test]
    public void ComponentIteratorWithEntity2_Iterates()
    {
        var reg = new Registry();
        var e = reg.CreateEntity();
        reg.AddComponent(e, new C0 { Value = 10 });
        reg.AddComponent(e, new C1 { Value = 20 });

        var count = 0;
        foreach (var (entityId, _, _) in reg.EachWithEntity<C0, C1>())
        {
            entityId.Should().Be(e);
            count++;
        }
        count.Should().Be(1);
    }

    [Test]
    public void ComponentIteratorWithEntity3_Iterates()
    {
        var reg = new Registry();
        var e = reg.CreateEntity();
        reg.AddComponent(e, new C0 { Value = 10 });
        reg.AddComponent(e, new C1 { Value = 20 });
        reg.AddComponent(e, new C2 { Value = 30 });

        var count = 0;
        foreach (var (entityId, _, _, _) in reg.EachWithEntity<C0, C1, C2>())
        {
            entityId.Should().Be(e);
            count++;
        }
        count.Should().Be(1);
    }

    // --- Unregistered / partial (1-3) ---

    [Test]
    public void ComponentIteratorWithEntity1_UnregisteredType_NoIteration()
    {
        var reg = new Registry();
        reg.CreateEntity();
        var count = 0;
        foreach (var _ in reg.EachWithEntity<C0>())
            count++;
        count.Should().Be(0);
    }

    [Test]
    public void ComponentIteratorWithEntity2_PartiallyRegistered_NoIteration()
    {
        var reg = new Registry();
        var e = reg.CreateEntity();
        reg.AddComponent(e, new C0 { Value = 1 });
        var count = 0;
        foreach (var _ in reg.EachWithEntity<C0, C1>())
            count++;
        count.Should().Be(0);
    }

    [Test]
    public void ComponentIteratorWithEntity3_PartiallyRegistered_NoIteration()
    {
        var reg = new Registry();
        var e = reg.CreateEntity();
        reg.AddComponent(e, new C0 { Value = 1 });
        reg.AddComponent(e, new C1 { Value = 2 });
        var count = 0;
        foreach (var _ in reg.EachWithEntity<C0, C1, C2>())
            count++;
        count.Should().Be(0);
    }

    // --- Filter ---

    [Test]
    public void ComponentIteratorWithEntity_WithFilter()
    {
        var reg = new Registry();
        var e0 = reg.CreateEntity();
        reg.AddComponent(e0, new C0 { Value = 10 });
        reg.AddComponent(e0, new ExcludeTag());

        var e1 = reg.CreateEntity();
        reg.AddComponent(e1, new C0 { Value = 20 });

        var count = 0;
        foreach (var (entityId, _) in reg.EachWithEntity<C0>(Filter.Create(exclude: new[] { typeof(ExcludeTag) })))
        {
            entityId.Should().Be(e1);
            count++;
        }
        count.Should().Be(1);
    }

    // --- High arity (4-8) ---

    [Test]
    public void ComponentIteratorWithEntity4_Iterates()
    {
        var reg = TestHelper.CreateRegistryWithComponents(4);
        var count = 0;
        foreach (var (entityId, _, _, _, _) in reg.EachWithEntity<C0, C1, C2, C3>())
        {
            entityId.Should().Be(0);
            count++;
        }
        count.Should().Be(1);
    }

    [Test]
    public void ComponentIteratorWithEntity5_Iterates()
    {
        var reg = TestHelper.CreateRegistryWithComponents(5);
        var count = 0;
        foreach (var (_, _, _, _, _, _) in reg.EachWithEntity<C0, C1, C2, C3, C4>())
            count++;
        count.Should().Be(1);
    }

    [Test]
    public void ComponentIteratorWithEntity6_Iterates()
    {
        var reg = TestHelper.CreateRegistryWithComponents(6);
        var count = 0;
        foreach (var (_, _, _, _, _, _, _) in reg.EachWithEntity<C0, C1, C2, C3, C4, C5>())
            count++;
        count.Should().Be(1);
    }

    [Test]
    public void ComponentIteratorWithEntity7_Iterates()
    {
        var reg = TestHelper.CreateRegistryWithComponents(7);
        var count = 0;
        foreach (var (_, _, _, _, _, _, _, _) in reg.EachWithEntity<C0, C1, C2, C3, C4, C5, C6>())
            count++;
        count.Should().Be(1);
    }

    [Test]
    public void ComponentIteratorWithEntity8_Iterates()
    {
        var reg = TestHelper.CreateRegistryWithComponents(8);
        var count = 0;
        foreach (var (_, _, _, _, _, _, _, _, _) in reg.EachWithEntity<C0, C1, C2, C3, C4, C5, C6, C7>())
            count++;
        count.Should().Be(1);
    }

    [Test]
    public void ComponentIteratorWithEntity4to8_PartiallyRegistered_NoIteration()
    {
        var count = 0;
        for (var n = 3; n <= 7; n++)
        {
            var reg = TestHelper.CreateRegistryWithComponents(n);
            if (n < 4) { count = 0; foreach (var _ in reg.EachWithEntity<C0, C1, C2, C3>()) count++; count.Should().Be(0); }
            if (n < 5) { count = 0; foreach (var _ in reg.EachWithEntity<C0, C1, C2, C3, C4>()) count++; count.Should().Be(0); }
            if (n < 6) { count = 0; foreach (var _ in reg.EachWithEntity<C0, C1, C2, C3, C4, C5>()) count++; count.Should().Be(0); }
            if (n < 7) { count = 0; foreach (var _ in reg.EachWithEntity<C0, C1, C2, C3, C4, C5, C6>()) count++; count.Should().Be(0); }
            if (n < 8) { count = 0; foreach (var _ in reg.EachWithEntity<C0, C1, C2, C3, C4, C5, C6, C7>()) count++; count.Should().Be(0); }
        }
    }

    // --- All overloads with filter ---

    [Test]
    public void ComponentIteratorWithEntity_AllOverloads_WithFilter()
    {
        var reg = new Registry();
        var e0 = reg.CreateEntity();
        TestHelper.AddComponents(reg, e0, 8, 10);
        reg.AddComponent(e0, new ExcludeTag());

        var e1 = reg.CreateEntity();
        TestHelper.AddComponents(reg, e1, 8, 100);
        reg.AddComponent(e1, new Tag());

        var filter = Filter.Create(include: new[] { typeof(Tag) }, exclude: new[] { typeof(ExcludeTag) });
        int c;
        c = 0; foreach (var _ in reg.EachWithEntity<C0>(filter)) c++; c.Should().Be(1);
        c = 0; foreach (var _ in reg.EachWithEntity<C0, C1>(filter)) c++; c.Should().Be(1);
        c = 0; foreach (var _ in reg.EachWithEntity<C0, C1, C2>(filter)) c++; c.Should().Be(1);
        c = 0; foreach (var _ in reg.EachWithEntity<C0, C1, C2, C3>(filter)) c++; c.Should().Be(1);
        c = 0; foreach (var _ in reg.EachWithEntity<C0, C1, C2, C3, C4>(filter)) c++; c.Should().Be(1);
        c = 0; foreach (var _ in reg.EachWithEntity<C0, C1, C2, C3, C4, C5>(filter)) c++; c.Should().Be(1);
        c = 0; foreach (var _ in reg.EachWithEntity<C0, C1, C2, C3, C4, C5, C6>(filter)) c++; c.Should().Be(1);
        c = 0; foreach (var _ in reg.EachWithEntity<C0, C1, C2, C3, C4, C5, C6, C7>(filter)) c++; c.Should().Be(1);
    }

    // --- Individual filter tests ---

    [Test]
    public void ComponentIteratorWithEntity2_WithFilter()
    {
        var reg = new Registry();
        var e0 = reg.CreateEntity();
        TestHelper.AddComponents(reg, e0, 8, 10);
        reg.AddComponent(e0, new ExcludeTag());

        var e1 = reg.CreateEntity();
        TestHelper.AddComponents(reg, e1, 8, 100);
        reg.AddComponent(e1, new Tag());

        var count = 0;
        foreach (var (entityId, _, _) in reg.EachWithEntity<C0, C1>(Filter.Create(include: new[] { typeof(Tag) })))
        {
            entityId.Should().Be(1);
            count++;
        }
        count.Should().Be(1);
    }

    [Test]
    public void ComponentIteratorWithEntity3_WithFilter()
    {
        var reg = new Registry();
        var e0 = reg.CreateEntity();
        TestHelper.AddComponents(reg, e0, 8, 10);
        reg.AddComponent(e0, new ExcludeTag());

        var e1 = reg.CreateEntity();
        TestHelper.AddComponents(reg, e1, 8, 100);
        reg.AddComponent(e1, new Tag());

        var count = 0;
        foreach (var _ in reg.EachWithEntity<C0, C1, C2>(Filter.Create(exclude: new[] { typeof(ExcludeTag) })))
            count++;
        count.Should().Be(1);
    }
}
