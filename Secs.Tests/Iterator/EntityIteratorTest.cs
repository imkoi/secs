using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Secs;

namespace Secs.Tests;

public class EntityIteratorTest
{
    [Test]
    public void EntityIterator_IteratesMatchingEntities()
    {
        var reg = new Registry();
        var e0 = reg.CreateEntity();
        reg.AddComponent(e0, new C0 { Value = 10 });
        reg.AddComponent(e0, new C1 { Value = 20 });

        var e1 = reg.CreateEntity();
        reg.AddComponent(e1, new C0 { Value = 30 });

        var entities = new List<int>();
        foreach (var entityId in reg.Each(Filter.Create(include: new[] { typeof(C0), typeof(C1) })))
            entities.Add(entityId);

        entities.Should().HaveCount(1);
        entities.Should().Contain(e0);
    }

    [Test]
    public void EntityIterator_WithExclude()
    {
        var reg = new Registry();
        var e0 = reg.CreateEntity();
        reg.AddComponent(e0, new C0 { Value = 10 });
        reg.AddComponent(e0, new ExcludeTag());

        var e1 = reg.CreateEntity();
        reg.AddComponent(e1, new C0 { Value = 20 });

        var entities = new List<int>();
        foreach (var entityId in reg.Each(Filter.Create(include: new[] { typeof(C0) }, exclude: new[] { typeof(ExcludeTag) })))
            entities.Add(entityId);

        entities.Should().HaveCount(1);
        entities.Should().Contain(e1);
    }

    [Test]
    public void EntityIterator_MultipleEntities()
    {
        var reg = new Registry();
        for (var i = 0; i < 5; i++)
        {
            var e = reg.CreateEntity();
            reg.AddComponent(e, new C0 { Value = i });
        }

        var entities = new List<int>();
        foreach (var entityId in reg.Each(Filter.Create(include: new[] { typeof(C0) })))
            entities.Add(entityId);

        entities.Should().HaveCount(5);
    }

    [Test]
    public void EntityIterator_WithIncludeAndExcludeFilter()
    {
        var reg = new Registry();
        var e0 = reg.CreateEntity();
        TestHelper.AddComponents(reg, e0, 8, 10);
        reg.AddComponent(e0, new ExcludeTag());

        var e1 = reg.CreateEntity();
        TestHelper.AddComponents(reg, e1, 8, 100);
        reg.AddComponent(e1, new Tag());

        var filter = Filter.Create(include: new[] { typeof(C0), typeof(Tag) }, exclude: new[] { typeof(ExcludeTag) });
        var count = 0;
        foreach (var _ in reg.Each(filter))
            count++;
        count.Should().Be(1);
    }
}
