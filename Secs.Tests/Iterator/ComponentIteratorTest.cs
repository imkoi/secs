using FluentAssertions;
using NUnit.Framework;
using Secs;

namespace Secs.Tests;

public class ComponentIteratorTest
{
    // --- Basic iteration (1-4) ---

    [Test]
    public void ComponentIterator1_IteratesAndAccessesRef()
    {
        var reg = new Registry();
        var e = reg.CreateEntity();
        reg.AddComponent(e, new C0 { Value = 42 });

        var count = 0;
        foreach (var refC0 in reg.Each<C0>())
        {
            refC0.Value.Value.Should().Be(42);
            count++;
        }
        count.Should().Be(1);
    }

    [Test]
    public void ComponentIterator2_Iterates()
    {
        var reg = new Registry();
        var e = reg.CreateEntity();
        reg.AddComponent(e, new C0 { Value = 10 });
        reg.AddComponent(e, new C1 { Value = 20 });

        var count = 0;
        foreach (var (_, _) in reg.Each<C0, C1>())
            count++;
        count.Should().Be(1);
    }

    [Test]
    public void ComponentIterator3_Iterates()
    {
        var reg = new Registry();
        var e = reg.CreateEntity();
        reg.AddComponent(e, new C0 { Value = 10 });
        reg.AddComponent(e, new C1 { Value = 20 });
        reg.AddComponent(e, new C2 { Value = 30 });

        var count = 0;
        foreach (var (_, _, _) in reg.Each<C0, C1, C2>())
            count++;
        count.Should().Be(1);
    }

    [Test]
    public void ComponentIterator4_Iterates()
    {
        var reg = new Registry();
        var e = reg.CreateEntity();
        reg.AddComponent(e, new C0 { Value = 10 });
        reg.AddComponent(e, new C1 { Value = 20 });
        reg.AddComponent(e, new C2 { Value = 30 });
        reg.AddComponent(e, new C3 { Value = 40 });

        var count = 0;
        foreach (var (_, _, _, _) in reg.Each<C0, C1, C2, C3>())
            count++;
        count.Should().Be(1);
    }

    // --- Unregistered / partial (1-4) ---

    [Test]
    public void ComponentIterator1_UnregisteredType_NoIteration()
    {
        var reg = new Registry();
        reg.CreateEntity();
        var count = 0;
        foreach (var _ in reg.Each<C0>())
            count++;
        count.Should().Be(0);
    }

    [Test]
    public void ComponentIterator2_PartiallyRegistered_NoIteration()
    {
        var reg = new Registry();
        var e = reg.CreateEntity();
        reg.AddComponent(e, new C0 { Value = 1 });
        var count = 0;
        foreach (var _ in reg.Each<C0, C1>())
            count++;
        count.Should().Be(0);
    }

    [Test]
    public void ComponentIterator3_PartiallyRegistered_NoIteration()
    {
        var reg = new Registry();
        var e = reg.CreateEntity();
        reg.AddComponent(e, new C0 { Value = 1 });
        reg.AddComponent(e, new C1 { Value = 2 });
        var count = 0;
        foreach (var _ in reg.Each<C0, C1, C2>())
            count++;
        count.Should().Be(0);
    }

    [Test]
    public void ComponentIterator4_PartiallyRegistered_NoIteration()
    {
        var reg = new Registry();
        var e = reg.CreateEntity();
        reg.AddComponent(e, new C0 { Value = 1 });
        reg.AddComponent(e, new C1 { Value = 2 });
        reg.AddComponent(e, new C2 { Value = 3 });
        var count = 0;
        foreach (var _ in reg.Each<C0, C1, C2, C3>())
            count++;
        count.Should().Be(0);
    }

    // --- Filter ---

    [Test]
    public void ComponentIterator_WithFilter()
    {
        var reg = new Registry();
        var e0 = reg.CreateEntity();
        reg.AddComponent(e0, new C0 { Value = 10 });
        reg.AddComponent(e0, new ExcludeTag());

        var e1 = reg.CreateEntity();
        reg.AddComponent(e1, new C0 { Value = 20 });

        var count = 0;
        foreach (var refC0 in reg.Each<C0>(Filter.Create(exclude: new[] { typeof(ExcludeTag) })))
        {
            refC0.Value.Value.Should().Be(20);
            count++;
        }
        count.Should().Be(1);
    }

    // --- Multiple entities ---

    [Test]
    public void ComponentIterator_MultipleEntities()
    {
        var reg = new Registry();
        for (var i = 0; i < 5; i++)
        {
            var e = reg.CreateEntity();
            reg.AddComponent(e, new C0 { Value = i * 10 });
        }

        var count = 0;
        foreach (var _ in reg.Each<C0>())
            count++;
        count.Should().Be(5);
    }

    // --- High arity (5-8) ---

    [Test]
    public void ComponentIterator5_Iterates()
    {
        var reg = TestHelper.CreateRegistryWithComponents(5);
        var count = 0;
        foreach (var (_, _, _, _, _) in reg.Each<C0, C1, C2, C3, C4>())
            count++;
        count.Should().Be(1);
    }

    [Test]
    public void ComponentIterator6_Iterates()
    {
        var reg = TestHelper.CreateRegistryWithComponents(6);
        var count = 0;
        foreach (var (_, _, _, _, _, _) in reg.Each<C0, C1, C2, C3, C4, C5>())
            count++;
        count.Should().Be(1);
    }

    [Test]
    public void ComponentIterator7_Iterates()
    {
        var reg = TestHelper.CreateRegistryWithComponents(7);
        var count = 0;
        foreach (var (_, _, _, _, _, _, _) in reg.Each<C0, C1, C2, C3, C4, C5, C6>())
            count++;
        count.Should().Be(1);
    }

    [Test]
    public void ComponentIterator8_Iterates()
    {
        var reg = TestHelper.CreateRegistryWithComponents(8);
        var count = 0;
        foreach (var (_, _, _, _, _, _, _, _) in reg.Each<C0, C1, C2, C3, C4, C5, C6, C7>())
            count++;
        count.Should().Be(1);
    }

    [Test]
    public void ComponentIterator5_PartiallyRegistered_NoIteration()
    {
        var reg = TestHelper.CreateRegistryWithComponents(4);
        var count = 0;
        foreach (var _ in reg.Each<C0, C1, C2, C3, C4>())
            count++;
        count.Should().Be(0);
    }

    [Test]
    public void ComponentIterator6_PartiallyRegistered_NoIteration()
    {
        var reg = TestHelper.CreateRegistryWithComponents(5);
        var count = 0;
        foreach (var _ in reg.Each<C0, C1, C2, C3, C4, C5>())
            count++;
        count.Should().Be(0);
    }

    [Test]
    public void ComponentIterator7_PartiallyRegistered_NoIteration()
    {
        var reg = TestHelper.CreateRegistryWithComponents(6);
        var count = 0;
        foreach (var _ in reg.Each<C0, C1, C2, C3, C4, C5, C6>())
            count++;
        count.Should().Be(0);
    }

    [Test]
    public void ComponentIterator8_PartiallyRegistered_NoIteration()
    {
        var reg = TestHelper.CreateRegistryWithComponents(7);
        var count = 0;
        foreach (var _ in reg.Each<C0, C1, C2, C3, C4, C5, C6, C7>())
            count++;
        count.Should().Be(0);
    }

    // --- All overloads with filter ---

    [Test]
    public void ComponentIterator_AllOverloads_WithFilter()
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
        c = 0; foreach (var _ in reg.Each<C0>(filter)) c++; c.Should().Be(1);
        c = 0; foreach (var _ in reg.Each<C0, C1>(filter)) c++; c.Should().Be(1);
        c = 0; foreach (var _ in reg.Each<C0, C1, C2>(filter)) c++; c.Should().Be(1);
        c = 0; foreach (var _ in reg.Each<C0, C1, C2, C3>(filter)) c++; c.Should().Be(1);
        c = 0; foreach (var _ in reg.Each<C0, C1, C2, C3, C4>(filter)) c++; c.Should().Be(1);
        c = 0; foreach (var _ in reg.Each<C0, C1, C2, C3, C4, C5>(filter)) c++; c.Should().Be(1);
        c = 0; foreach (var _ in reg.Each<C0, C1, C2, C3, C4, C5, C6>(filter)) c++; c.Should().Be(1);
        c = 0; foreach (var _ in reg.Each<C0, C1, C2, C3, C4, C5, C6, C7>(filter)) c++; c.Should().Be(1);
    }

    // --- Individual filter tests ---

    [Test]
    public void ComponentIterator2_WithFilter()
    {
        var reg = new Registry();
        var e0 = reg.CreateEntity();
        TestHelper.AddComponents(reg, e0, 8, 10);
        reg.AddComponent(e0, new ExcludeTag());

        var e1 = reg.CreateEntity();
        TestHelper.AddComponents(reg, e1, 8, 100);
        reg.AddComponent(e1, new Tag());

        var count = 0;
        foreach (var _ in reg.Each<C0, C1>(Filter.Create(include: new[] { typeof(Tag) })))
            count++;
        count.Should().Be(1);
    }

    [Test]
    public void ComponentIterator3_WithFilter()
    {
        var reg = new Registry();
        var e0 = reg.CreateEntity();
        TestHelper.AddComponents(reg, e0, 8, 10);
        reg.AddComponent(e0, new ExcludeTag());

        var e1 = reg.CreateEntity();
        TestHelper.AddComponents(reg, e1, 8, 100);
        reg.AddComponent(e1, new Tag());

        var count = 0;
        foreach (var _ in reg.Each<C0, C1, C2>(Filter.Create(exclude: new[] { typeof(ExcludeTag) })))
            count++;
        count.Should().Be(1);
    }

    [Test]
    public void ComponentIterator4_WithFilter()
    {
        var reg = new Registry();
        var e0 = reg.CreateEntity();
        TestHelper.AddComponents(reg, e0, 8, 10);
        reg.AddComponent(e0, new ExcludeTag());

        var e1 = reg.CreateEntity();
        TestHelper.AddComponents(reg, e1, 8, 100);
        reg.AddComponent(e1, new Tag());

        var count = 0;
        foreach (var _ in reg.Each<C0, C1, C2, C3>(Filter.Create(include: new[] { typeof(Tag) }, exclude: new[] { typeof(ExcludeTag) })))
            count++;
        count.Should().Be(1);
    }
}
