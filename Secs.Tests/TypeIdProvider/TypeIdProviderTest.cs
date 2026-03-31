using System;
using FluentAssertions;
using NUnit.Framework;
using Secs;

namespace Secs.Tests;

public class TypeIdProviderTest
{
    [Test]
    public void Constructor_SmallCapacity_ClampsToMinimum()
    {
        var provider = new TypeIdProvider(1);
        provider.Count.Should().Be(0);
    }

    [Test]
    public void Constructor_CapacityExceedsMax_Throws()
    {
        var act = () => new TypeIdProvider(TypeIdProvider.MaxCapacity);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Test]
    public void TryAdd_NewType_ReturnsTrueAndAssignsId()
    {
        var provider = new TypeIdProvider();
        provider.TryAdd(typeof(C0), out var id).Should().BeTrue();
        id.Should().Be(0);
        provider.Count.Should().Be(1);
    }

    [Test]
    public void TryAdd_DuplicateType_ReturnsFalse()
    {
        var provider = new TypeIdProvider();
        provider.TryAdd(typeof(C0), out _);
        provider.TryAdd(typeof(C0), out var id).Should().BeFalse();
        id.Should().Be(0);
        provider.Count.Should().Be(1);
    }

    [Test]
    public void TryAdd_NullKey_Throws()
    {
        var provider = new TypeIdProvider();
        var act = () => provider.TryAdd(null!, out _);
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void TryGetValue_Existing_ReturnsTrueWithId()
    {
        var provider = new TypeIdProvider();
        provider.TryAdd(typeof(C0), out _);
        provider.TryGetValue(typeof(C0), out var id).Should().BeTrue();
        id.Should().Be(0);
    }

    [Test]
    public void TryGetValue_Missing_ReturnsFalse()
    {
        var provider = new TypeIdProvider();
        provider.TryGetValue(typeof(C0), out var id).Should().BeFalse();
        id.Should().Be(-1);
    }

    [Test]
    public void TryGetValue_NullKey_Throws()
    {
        var provider = new TypeIdProvider();
        var act = () => provider.TryGetValue(null!, out _);
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Indexer_ReturnsCorrectId()
    {
        var provider = new TypeIdProvider();
        provider.TryAdd(typeof(C0), out _);
        provider.TryAdd(typeof(C1), out _);
        provider[typeof(C0)].Should().Be(0);
        provider[typeof(C1)].Should().Be(1);
    }

    [Test]
    public void TryAdd_ManyTypes_TriggersResize()
    {
        var provider = new TypeIdProvider(8, 0.5f);
        var types = new[]
        {
            typeof(C0), typeof(C1), typeof(C2), typeof(C3),
            typeof(C4), typeof(C5), typeof(C6), typeof(C7),
            typeof(Tag), typeof(ExcludeTag), typeof(int), typeof(float),
            typeof(double), typeof(long), typeof(short), typeof(byte)
        };

        for (var i = 0; i < types.Length; i++)
        {
            provider.TryAdd(types[i], out var id);
            id.Should().Be(i);
        }

        provider.Count.Should().Be(types.Length);
        for (var i = 0; i < types.Length; i++)
            provider.TryGetValue(types[i], out var id).Should().BeTrue();
    }
}
