using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Secs;

namespace Secs.Tests;

public class IntegrationTest
{
    // --- MultiEntityIterationTests ---

    [Test]
    public void Each_MultipleEntities_IteratesAll()
    {
        var reg = new Registry();
        for (var i = 0; i < 10; i++)
        {
            var e = reg.CreateEntity();
            reg.AddComponent(e, new C0 { Value = i });
        }

        var sum = 0;
        reg.Each((Registry _, C0 c0) => { sum += c0.Value; });
        sum.Should().Be(45);
    }

    [Test]
    public void Each_PartialMatch_OnlyIteratesMatching()
    {
        var reg = new Registry();
        for (var i = 0; i < 10; i++)
        {
            var e = reg.CreateEntity();
            reg.AddComponent(e, new C0 { Value = i });
            if (i % 2 == 0)
                reg.AddComponent(e, new C1 { Value = i * 10 });
        }

        var count = 0;
        reg.Each((Registry _, C0 c0, C1 c1) => { count++; });
        count.Should().Be(5);
    }

    [Test]
    public void EachMutable_MultipleEntities_ModifiesAll()
    {
        var reg = new Registry();
        for (var i = 0; i < 5; i++)
        {
            var e = reg.CreateEntity();
            reg.AddComponent(e, new C0 { Value = i });
        }

        reg.Each((Registry _, ref C0 c0) => { c0.Value += 100; });

        for (var i = 0; i < 5; i++)
            reg.GetComponent<C0>(i).Value.Should().Be(i + 100);
    }

    [Test]
    public void EachWithEntity_CanRemoveComponents()
    {
        var reg = new Registry();
        for (var i = 0; i < 5; i++)
        {
            var e = reg.CreateEntity();
            reg.AddComponent(e, new C0 { Value = i });
            reg.AddComponent(e, new C1 { Value = i * 10 });
        }

        reg.EachWithEntity((Registry r, int entity, ref C0 c0) =>
        {
            if (c0.Value % 2 == 0)
                r.RemoveComponent<C0>(entity);
        });

        var remaining = 0;
        reg.Each((Registry _, C0 c0) => { remaining++; });
        remaining.Should().Be(2);
    }

    [Test]
    public void EachWithEntity_CanDestroyEntities()
    {
        var reg = new Registry();
        for (var i = 0; i < 5; i++)
        {
            var e = reg.CreateEntity();
            reg.AddComponent(e, new C0 { Value = i });
        }

        reg.EachWithEntity((Registry r, int entity, ref C0 c0) =>
        {
            if (c0.Value >= 3)
                r.DestroyEntity(entity);
        });

        var remaining = 0;
        reg.Each((Registry _, C0 c0) => { remaining++; });
        remaining.Should().Be(3);
    }

    [Test]
    public void DestroyEntity_WithZeroComponents_EnqueuesForReuse()
    {
        var reg = new Registry();
        var e = reg.CreateEntity();
        reg.DestroyEntity(e);
        var reused = reg.CreateEntity();
        reused.Should().Be(e);
    }

    // --- StressAndIntegrationTests ---

    [Test]
    public void MassCreateDestroy_EntityIdsAreReusedCorrectly()
    {
        var reg = new Registry();
        var ids = new int[100];
        for (var i = 0; i < 100; i++)
        {
            ids[i] = reg.CreateEntity();
            reg.AddComponent(ids[i], new C0 { Value = i });
        }

        for (var i = 0; i < 100; i++)
            reg.DestroyEntity(ids[i]);

        var reused = new HashSet<int>();
        for (var i = 0; i < 100; i++)
        {
            var e = reg.CreateEntity();
            reused.Add(e).Should().BeTrue($"entity {e} was returned twice");
        }

        reused.Should().BeEquivalentTo(ids);
    }

    [Test]
    public void AddRemoveAdd_SameComponent_DataIsCorrect()
    {
        var reg = new Registry();
        var e = reg.CreateEntity();

        reg.AddComponent(e, new C0 { Value = 10 });
        reg.GetComponent<C0>(e).Value.Should().Be(10);

        reg.RemoveComponent<C0>(e);
        reg.HasComponent<C0>(e).Should().BeFalse();

        reg.AddComponent(e, new C0 { Value = 99 });
        reg.GetComponent<C0>(e).Value.Should().Be(99);
    }

    [Test]
    public void DestroyAndReuse_NewEntityHasNoStaleComponents()
    {
        var reg = new Registry();
        var e0 = reg.CreateEntity();
        reg.AddComponent(e0, new C0 { Value = 42 });
        reg.AddComponent(e0, new C1 { Value = 100 });
        reg.DestroyEntity(e0);

        var e1 = reg.CreateEntity();
        e1.Should().Be(e0);
        reg.HasComponent<C0>(e1).Should().BeFalse();
        reg.HasComponent<C1>(e1).Should().BeFalse();
    }

    [Test]
    public void RemoveComponent_DuringIteration_DoesNotCorruptOtherEntities()
    {
        var reg = new Registry();
        for (var i = 0; i < 20; i++)
        {
            var e = reg.CreateEntity();
            reg.AddComponent(e, new C0 { Value = i });
            reg.AddComponent(e, new C1 { Value = i * 10 });
        }

        reg.EachWithEntity((Registry r, int entity, ref C0 c0) =>
        {
            if (c0.Value % 3 == 0)
                r.RemoveComponent<C0>(entity);
        });

        var values = new List<int>();
        reg.Each((Registry _, C0 c0) => { values.Add(c0.Value); });

        values.Should().NotContain(v => v % 3 == 0);
        values.Should().HaveCount(13);
    }

    [Test]
    public void DestroyEntity_DuringIteration_DoesNotCorruptOtherEntities()
    {
        var reg = new Registry();
        for (var i = 0; i < 20; i++)
        {
            var e = reg.CreateEntity();
            reg.AddComponent(e, new C0 { Value = i });
        }

        reg.EachWithEntity((Registry r, int entity, ref C0 c0) =>
        {
            if (c0.Value >= 15)
                r.DestroyEntity(entity);
        });

        var values = new List<int>();
        reg.Each((Registry _, C0 c0) => { values.Add(c0.Value); });

        values.Should().HaveCount(15);
        values.Should().OnlyContain(v => v < 15);
    }

    [Test]
    public void AddComponent_DuringMutableIteration_ToSameEntity()
    {
        var reg = new Registry();
        var e0 = reg.CreateEntity();
        reg.AddComponent(e0, new C0 { Value = 1 });
        var e1 = reg.CreateEntity();
        reg.AddComponent(e1, new C0 { Value = 2 });

        reg.EachWithEntity((Registry r, int entity, ref C0 c0) =>
        {
            if (!r.HasComponent<C1>(entity))
                r.AddComponent(entity, new C1 { Value = c0.Value * 100 });
        });

        reg.GetComponent<C1>(e0).Value.Should().Be(100);
        reg.GetComponent<C1>(e1).Value.Should().Be(200);
    }

    [Test]
    public void SparseSetConsistency_AfterManyAddRemoveCycles()
    {
        var reg = new Registry();
        var entities = new int[50];
        for (var i = 0; i < 50; i++)
        {
            entities[i] = reg.CreateEntity();
            reg.AddComponent(entities[i], new C0 { Value = i });
        }

        for (var cycle = 0; cycle < 5; cycle++)
        {
            for (var i = 0; i < 50; i += 2)
                reg.RemoveComponent<C0>(entities[i]);

            for (var i = 0; i < 50; i += 2)
                reg.AddComponent(entities[i], new C0 { Value = (cycle + 1) * 100 + i });
        }

        for (var i = 0; i < 50; i++)
        {
            reg.HasComponent<C0>(entities[i]).Should().BeTrue();
            var expected = i % 2 == 0 ? 500 + i : i;
            reg.GetComponent<C0>(entities[i]).Value.Should().Be(expected);
        }
    }

    [Test]
    public void CreateDestroyCreate_ThreeGenerations_ComponentDataIsolation()
    {
        var reg = new Registry();

        var gen1 = reg.CreateEntity();
        reg.AddComponent(gen1, new C0 { Value = 111 });
        reg.AddComponent(gen1, new C1 { Value = 222 });
        reg.DestroyEntity(gen1);

        var gen2 = reg.CreateEntity();
        gen2.Should().Be(gen1);
        reg.HasComponent<C0>(gen2).Should().BeFalse();
        reg.HasComponent<C1>(gen2).Should().BeFalse();

        reg.AddComponent(gen2, new C0 { Value = 333 });
        reg.GetComponent<C0>(gen2).Value.Should().Be(333);

        reg.DestroyEntity(gen2);

        var gen3 = reg.CreateEntity();
        gen3.Should().Be(gen1);
        reg.HasComponent<C0>(gen3).Should().BeFalse();
    }

    [Test]
    public void SwapRemove_ManyEntities_DataIntegrity()
    {
        var reg = new Registry();
        var entities = new int[100];
        for (var i = 0; i < 100; i++)
        {
            entities[i] = reg.CreateEntity();
            reg.AddComponent(entities[i], new C0 { Value = i });
            reg.AddComponent(entities[i], new C1 { Value = i * 10 });
        }

        for (var i = 0; i < 100; i += 3)
            reg.RemoveComponent<C0>(entities[i]);

        for (var i = 0; i < 100; i++)
        {
            if (i % 3 == 0)
            {
                reg.HasComponent<C0>(entities[i]).Should().BeFalse();
            }
            else
            {
                reg.GetComponent<C0>(entities[i]).Value.Should().Be(i);
            }

            reg.GetComponent<C1>(entities[i]).Value.Should().Be(i * 10);
        }
    }

    [Test]
    public void MixedIterationStyles_SameResults()
    {
        var reg = new Registry();
        for (var i = 0; i < 30; i++)
        {
            var e = reg.CreateEntity();
            reg.AddComponent(e, new C0 { Value = i });
            if (i % 2 == 0)
                reg.AddComponent(e, new C1 { Value = i * 10 });
        }

        var delegateEntities = new List<int>();
        reg.EachWithEntity((Registry _, int entity, C0 c0, C1 c1) =>
        {
            delegateEntities.Add(entity);
        });

        var iteratorEntities = new List<int>();
        foreach (var (entityId, _, _) in reg.EachWithEntity<C0, C1>())
            iteratorEntities.Add(entityId);

        delegateEntities.Sort();
        iteratorEntities.Sort();
        iteratorEntities.Should().BeEquivalentTo(delegateEntities);
        iteratorEntities.Should().HaveCount(15);
    }

    [Test]
    public void Filter_ExcludeMultipleTypes()
    {
        var reg = new Registry();
        for (var i = 0; i < 10; i++)
        {
            var e = reg.CreateEntity();
            reg.AddComponent(e, new C0 { Value = i });
            if (i % 2 == 0) reg.AddComponent(e, new Tag());
            if (i % 3 == 0) reg.AddComponent(e, new ExcludeTag());
        }

        var matched = new List<int>();
        reg.Each((Registry _, C0 c0) => { matched.Add(c0.Value); },
            Filter.Create(exclude: new[] { typeof(Tag), typeof(ExcludeTag) }));

        matched.Should().BeEquivalentTo(new[] { 1, 5, 7 });
    }

    [Test]
    public void DenseResize_UnderLoad_DataPreserved()
    {
        var reg = new Registry();
        for (var i = 0; i < 100; i++)
        {
            var e = reg.CreateEntity();
            reg.AddComponent(e, new C0 { Value = i });
        }

        for (var i = 0; i < 100; i++)
            reg.GetComponent<C0>(i).Value.Should().Be(i);

        var count = 0;
        reg.Each((Registry _, C0 c0) => { count++; });
        count.Should().Be(100);
    }

    [Test]
    public void DestroyEntity_Twice_WithComponents_IsIdempotent()
    {
        var reg = new Registry();
        var e = reg.CreateEntity();
        reg.AddComponent(e, new C0 { Value = 1 });
        reg.AddComponent(e, new C1 { Value = 2 });

        reg.DestroyEntity(e);
        reg.DestroyEntity(e);

        var reused = reg.CreateEntity();
        reused.Should().Be(e);

        var noMoreReuse = reg.CreateEntity();
        noMoreReuse.Should().NotBe(e);
    }

    [Test]
    public void FullLifecycle_CreateAddIterateRemoveDestroyReuse()
    {
        var reg = new Registry();

        var e0 = reg.CreateEntity();
        var e1 = reg.CreateEntity();
        var e2 = reg.CreateEntity();

        reg.AddComponent(e0, new C0 { Value = 10 });
        reg.AddComponent(e0, new C1 { Value = 100 });
        reg.AddComponent(e1, new C0 { Value = 20 });
        reg.AddComponent(e2, new C0 { Value = 30 });
        reg.AddComponent(e2, new C1 { Value = 300 });

        var sum = 0;
        reg.Each((Registry _, C0 c0, C1 c1) => { sum += c0.Value + c1.Value; });
        sum.Should().Be(10 + 100 + 30 + 300);

        reg.RemoveComponent<C1>(e0);
        reg.HasComponent<C1>(e0).Should().BeFalse();
        reg.HasComponent<C0>(e0).Should().BeTrue();

        reg.DestroyEntity(e1);

        var remaining = new List<int>();
        reg.EachWithEntity((Registry _, int entity, ref C0 c0) =>
        {
            remaining.Add(entity);
        });
        remaining.Should().BeEquivalentTo(new[] { e0, e2 });

        var e3 = reg.CreateEntity();
        e3.Should().Be(e1);

        reg.AddComponent(e3, new C0 { Value = 50 });
        reg.AddComponent(e3, new C1 { Value = 500 });

        sum = 0;
        reg.Each((Registry _, C0 c0, C1 c1) => { sum += c0.Value + c1.Value; });
        sum.Should().Be(30 + 300 + 50 + 500);
    }

    [Test]
    public void RemoveLastComponent_ThenReAddAndIterate()
    {
        var reg = new Registry();
        var e = reg.CreateEntity();
        reg.AddComponent(e, new C0 { Value = 1 });
        reg.RemoveComponent<C0>(e);

        var e2 = reg.CreateEntity();
        e2.Should().Be(e);

        reg.AddComponent(e2, new C0 { Value = 99 });

        var count = 0;
        reg.Each((Registry _, C0 c0) =>
        {
            c0.Value.Should().Be(99);
            count++;
        });
        count.Should().Be(1);
    }

    [Test]
    public void ComponentsWrapper_ParallelWithRegistry_DataConsistent()
    {
        var reg = new Registry();
        var comps = reg.GetComponents<C0>();

        var e0 = reg.CreateEntity();
        comps.AddComponent(e0, new C0 { Value = 10 });

        var e1 = reg.CreateEntity();
        reg.AddComponent(e1, new C0 { Value = 20 });

        comps.GetComponent(e0).Value.Should().Be(10);
        reg.GetComponent<C0>(e1).Value.Should().Be(20);

        comps.RemoveComponent(e0);
        reg.HasComponent<C0>(e0).Should().BeFalse();
        reg.GetComponent<C0>(e1).Value.Should().Be(20);
    }
}
