using FluentAssertions;
using NUnit.Framework;
using Secs;

namespace Secs.Tests;

public class RegistryEachFilterTest
{
    private Registry SetupFilterRegistry()
    {
        var reg = new Registry();
        var e0 = reg.CreateEntity();
        TestHelper.AddComponents(reg, e0, 8, 10);
        reg.AddComponent(e0, new ExcludeTag());

        var e1 = reg.CreateEntity();
        TestHelper.AddComponents(reg, e1, 8, 100);
        reg.AddComponent(e1, new Tag());
        return reg;
    }

    [Test]
    public void EachImmutable_WithIncludeFilter()
    {
        var reg = new Registry();
        var e0 = reg.CreateEntity();
        reg.AddComponent(e0, new C0 { Value = 10 });
        reg.AddComponent(e0, new Tag());

        var e1 = reg.CreateEntity();
        reg.AddComponent(e1, new C0 { Value = 20 });

        var count = 0;
        reg.Each((Registry _, C0 c0) =>
        {
            c0.Value.Should().Be(10);
            count++;
        }, Filter.Create(include: new[] { typeof(Tag) }));
        count.Should().Be(1);
    }

    [Test]
    public void EachImmutable_WithExcludeFilter()
    {
        var reg = new Registry();
        var e0 = reg.CreateEntity();
        reg.AddComponent(e0, new C0 { Value = 10 });
        reg.AddComponent(e0, new ExcludeTag());

        var e1 = reg.CreateEntity();
        reg.AddComponent(e1, new C0 { Value = 20 });

        var count = 0;
        reg.Each((Registry _, C0 c0) =>
        {
            c0.Value.Should().Be(20);
            count++;
        }, Filter.Create(exclude: new[] { typeof(ExcludeTag) }));
        count.Should().Be(1);
    }

    [Test]
    public void EachImmutable_WithIncludeAndExcludeFilter()
    {
        var reg = new Registry();
        var e0 = reg.CreateEntity();
        reg.AddComponent(e0, new C0 { Value = 10 });
        reg.AddComponent(e0, new Tag());
        reg.AddComponent(e0, new ExcludeTag());

        var e1 = reg.CreateEntity();
        reg.AddComponent(e1, new C0 { Value = 20 });
        reg.AddComponent(e1, new Tag());

        var count = 0;
        reg.Each((Registry _, C0 c0) =>
        {
            c0.Value.Should().Be(20);
            count++;
        }, Filter.Create(include: new[] { typeof(Tag) }, exclude: new[] { typeof(ExcludeTag) }));
        count.Should().Be(1);
    }

    [Test]
    public void EachMutable_WithFilter()
    {
        var reg = new Registry();
        var e0 = reg.CreateEntity();
        reg.AddComponent(e0, new C0 { Value = 10 });
        reg.AddComponent(e0, new Tag());

        var e1 = reg.CreateEntity();
        reg.AddComponent(e1, new C0 { Value = 20 });

        var count = 0;
        reg.Each((Registry _, ref C0 c0) =>
        {
            c0.Value.Should().Be(10);
            count++;
        }, Filter.Create(include: new[] { typeof(Tag) }));
        count.Should().Be(1);
    }

    [Test]
    public void EachImmutableWithEntity_WithFilter()
    {
        var reg = new Registry();
        var e0 = reg.CreateEntity();
        reg.AddComponent(e0, new C0 { Value = 10 });
        reg.AddComponent(e0, new ExcludeTag());

        var e1 = reg.CreateEntity();
        reg.AddComponent(e1, new C0 { Value = 20 });

        var count = 0;
        reg.EachWithEntity((Registry _, int entity, C0 c0) =>
        {
            entity.Should().Be(e1);
            count++;
        }, Filter.Create(exclude: new[] { typeof(ExcludeTag) }));
        count.Should().Be(1);
    }

    [Test]
    public void EachMutableWithEntity_WithFilter()
    {
        var reg = new Registry();
        var e0 = reg.CreateEntity();
        reg.AddComponent(e0, new C0 { Value = 10 });
        reg.AddComponent(e0, new Tag());
        reg.AddComponent(e0, new ExcludeTag());

        var e1 = reg.CreateEntity();
        reg.AddComponent(e1, new C0 { Value = 20 });
        reg.AddComponent(e1, new Tag());

        var count = 0;
        reg.EachWithEntity((Registry _, int entity, ref C0 c0) =>
        {
            entity.Should().Be(e1);
            count++;
        }, Filter.Create(include: new[] { typeof(Tag) }, exclude: new[] { typeof(ExcludeTag) }));
        count.Should().Be(1);
    }

    [Test]
    public void EachImmutable_AllOverloads_WithIncludeAndExcludeFilter()
    {
        var reg = SetupFilterRegistry();
        var filter = Filter.Create(include: new[] { typeof(Tag) }, exclude: new[] { typeof(ExcludeTag) });
        int c;
        c = 0; reg.Each((Registry _, C0 a) => { c++; }, filter); c.Should().Be(1);
        c = 0; reg.Each((Registry _, C0 a, C1 b) => { c++; }, filter); c.Should().Be(1);
        c = 0; reg.Each((Registry _, C0 a, C1 b, C2 d) => { c++; }, filter); c.Should().Be(1);
        c = 0; reg.Each((Registry _, C0 a, C1 b, C2 d, C3 e) => { c++; }, filter); c.Should().Be(1);
        c = 0; reg.Each((Registry _, C0 a, C1 b, C2 d, C3 e, C4 f) => { c++; }, filter); c.Should().Be(1);
        c = 0; reg.Each((Registry _, C0 a, C1 b, C2 d, C3 e, C4 f, C5 g) => { c++; }, filter); c.Should().Be(1);
        c = 0; reg.Each((Registry _, C0 a, C1 b, C2 d, C3 e, C4 f, C5 g, C6 h) => { c++; }, filter); c.Should().Be(1);
        c = 0; reg.Each((Registry _, C0 a, C1 b, C2 d, C3 e, C4 f, C5 g, C6 h, C7 i) => { c++; }, filter); c.Should().Be(1);
    }

    [Test]
    public void EachMutable_AllOverloads_WithIncludeAndExcludeFilter()
    {
        var reg = SetupFilterRegistry();
        var filter = Filter.Create(include: new[] { typeof(Tag) }, exclude: new[] { typeof(ExcludeTag) });
        int c;
        c = 0; reg.Each((Registry _, ref C0 a) => { c++; }, filter); c.Should().Be(1);
        c = 0; reg.Each((Registry _, ref C0 a, ref C1 b) => { c++; }, filter); c.Should().Be(1);
        c = 0; reg.Each((Registry _, ref C0 a, ref C1 b, ref C2 d) => { c++; }, filter); c.Should().Be(1);
        c = 0; reg.Each((Registry _, ref C0 a, ref C1 b, ref C2 d, ref C3 e) => { c++; }, filter); c.Should().Be(1);
        c = 0; reg.Each((Registry _, ref C0 a, ref C1 b, ref C2 d, ref C3 e, ref C4 f) => { c++; }, filter); c.Should().Be(1);
        c = 0; reg.Each((Registry _, ref C0 a, ref C1 b, ref C2 d, ref C3 e, ref C4 f, ref C5 g) => { c++; }, filter); c.Should().Be(1);
        c = 0; reg.Each((Registry _, ref C0 a, ref C1 b, ref C2 d, ref C3 e, ref C4 f, ref C5 g, ref C6 h) => { c++; }, filter); c.Should().Be(1);
        c = 0; reg.Each((Registry _, ref C0 a, ref C1 b, ref C2 d, ref C3 e, ref C4 f, ref C5 g, ref C6 h, ref C7 i) => { c++; }, filter); c.Should().Be(1);
    }

    [Test]
    public void EachImmutableWithEntity_AllOverloads_WithIncludeAndExcludeFilter()
    {
        var reg = SetupFilterRegistry();
        var filter = Filter.Create(include: new[] { typeof(Tag) }, exclude: new[] { typeof(ExcludeTag) });
        int c;
        c = 0; reg.EachWithEntity((Registry _, int entity, C0 a) => { c++; }, filter); c.Should().Be(1);
        c = 0; reg.EachWithEntity((Registry _, int entity, C0 a, C1 b) => { c++; }, filter); c.Should().Be(1);
        c = 0; reg.EachWithEntity((Registry _, int entity, C0 a, C1 b, C2 d) => { c++; }, filter); c.Should().Be(1);
        c = 0; reg.EachWithEntity((Registry _, int entity, C0 a, C1 b, C2 d, C3 e) => { c++; }, filter); c.Should().Be(1);
        c = 0; reg.EachWithEntity((Registry _, int entity, C0 a, C1 b, C2 d, C3 e, C4 f) => { c++; }, filter); c.Should().Be(1);
        c = 0; reg.EachWithEntity((Registry _, int entity, C0 a, C1 b, C2 d, C3 e, C4 f, C5 g) => { c++; }, filter); c.Should().Be(1);
        c = 0; reg.EachWithEntity((Registry _, int entity, C0 a, C1 b, C2 d, C3 e, C4 f, C5 g, C6 h) => { c++; }, filter); c.Should().Be(1);
        c = 0; reg.EachWithEntity((Registry _, int entity, C0 a, C1 b, C2 d, C3 e, C4 f, C5 g, C6 h, C7 i) => { c++; }, filter); c.Should().Be(1);
    }

    [Test]
    public void EachMutableWithEntity_AllOverloads_WithIncludeAndExcludeFilter()
    {
        var reg = SetupFilterRegistry();
        var filter = Filter.Create(include: new[] { typeof(Tag) }, exclude: new[] { typeof(ExcludeTag) });
        int c;
        c = 0; reg.EachWithEntity((Registry _, int entity, ref C0 a) => { c++; }, filter); c.Should().Be(1);
        c = 0; reg.EachWithEntity((Registry _, int entity, ref C0 a, ref C1 b) => { c++; }, filter); c.Should().Be(1);
        c = 0; reg.EachWithEntity((Registry _, int entity, ref C0 a, ref C1 b, ref C2 d) => { c++; }, filter); c.Should().Be(1);
        c = 0; reg.EachWithEntity((Registry _, int entity, ref C0 a, ref C1 b, ref C2 d, ref C3 e) => { c++; }, filter); c.Should().Be(1);
        c = 0; reg.EachWithEntity((Registry _, int entity, ref C0 a, ref C1 b, ref C2 d, ref C3 e, ref C4 f) => { c++; }, filter); c.Should().Be(1);
        c = 0; reg.EachWithEntity((Registry _, int entity, ref C0 a, ref C1 b, ref C2 d, ref C3 e, ref C4 f, ref C5 g) => { c++; }, filter); c.Should().Be(1);
        c = 0; reg.EachWithEntity((Registry _, int entity, ref C0 a, ref C1 b, ref C2 d, ref C3 e, ref C4 f, ref C5 g, ref C6 h) => { c++; }, filter); c.Should().Be(1);
        c = 0; reg.EachWithEntity((Registry _, int entity, ref C0 a, ref C1 b, ref C2 d, ref C3 e, ref C4 f, ref C5 g, ref C6 h, ref C7 i) => { c++; }, filter); c.Should().Be(1);
    }

    [Test]
    public void EachImmutable2_WithFilter()
    {
        var reg = SetupFilterRegistry();
        var count = 0;
        reg.Each((Registry _, C0 c0, C1 c1) => { count++; },
            Filter.Create(include: new[] { typeof(Tag) }, exclude: new[] { typeof(ExcludeTag) }));
        count.Should().Be(1);
    }

    [Test]
    public void EachMutable2_WithFilter()
    {
        var reg = SetupFilterRegistry();
        var count = 0;
        reg.Each((Registry _, ref C0 c0, ref C1 c1) => { count++; },
            Filter.Create(include: new[] { typeof(Tag) }, exclude: new[] { typeof(ExcludeTag) }));
        count.Should().Be(1);
    }

    [Test]
    public void EachImmutableWithEntity2_WithFilter()
    {
        var reg = SetupFilterRegistry();
        var count = 0;
        reg.EachWithEntity((Registry _, int e, C0 c0, C1 c1) => { count++; },
            Filter.Create(include: new[] { typeof(Tag) }, exclude: new[] { typeof(ExcludeTag) }));
        count.Should().Be(1);
    }

    [Test]
    public void EachMutableWithEntity2_WithFilter()
    {
        var reg = SetupFilterRegistry();
        var count = 0;
        reg.EachWithEntity((Registry _, int e, ref C0 c0, ref C1 c1) => { count++; },
            Filter.Create(include: new[] { typeof(Tag) }, exclude: new[] { typeof(ExcludeTag) }));
        count.Should().Be(1);
    }
}
