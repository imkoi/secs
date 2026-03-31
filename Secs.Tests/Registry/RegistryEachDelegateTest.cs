using FluentAssertions;
using NUnit.Framework;
using Secs;

namespace Secs.Tests;

public class RegistryEachDelegateTest
{
    // --- Immutable ---

    [Test]
    public void Each1_Immutable_IteratesMatchingEntities()
    {
        var reg = TestHelper.CreateRegistryWithComponents(8);
        var count = 0;
        reg.Each((Registry _, C0 c0) => { c0.Value.Should().Be(10); count++; });
        count.Should().Be(1);
    }

    [Test]
    public void Each2_Immutable()
    {
        var reg = TestHelper.CreateRegistryWithComponents(8);
        var count = 0;
        reg.Each((Registry _, C0 c0, C1 c1) => { count++; });
        count.Should().Be(1);
    }

    [Test]
    public void Each3_Immutable()
    {
        var reg = TestHelper.CreateRegistryWithComponents(8);
        var count = 0;
        reg.Each((Registry _, C0 c0, C1 c1, C2 c2) => { count++; });
        count.Should().Be(1);
    }

    [Test]
    public void Each4_Immutable()
    {
        var reg = TestHelper.CreateRegistryWithComponents(8);
        var count = 0;
        reg.Each((Registry _, C0 c0, C1 c1, C2 c2, C3 c3) => { count++; });
        count.Should().Be(1);
    }

    [Test]
    public void Each5_Immutable()
    {
        var reg = TestHelper.CreateRegistryWithComponents(8);
        var count = 0;
        reg.Each((Registry _, C0 c0, C1 c1, C2 c2, C3 c3, C4 c4) => { count++; });
        count.Should().Be(1);
    }

    [Test]
    public void Each6_Immutable()
    {
        var reg = TestHelper.CreateRegistryWithComponents(8);
        var count = 0;
        reg.Each((Registry _, C0 c0, C1 c1, C2 c2, C3 c3, C4 c4, C5 c5) => { count++; });
        count.Should().Be(1);
    }

    [Test]
    public void Each7_Immutable()
    {
        var reg = TestHelper.CreateRegistryWithComponents(8);
        var count = 0;
        reg.Each((Registry _, C0 c0, C1 c1, C2 c2, C3 c3, C4 c4, C5 c5, C6 c6) => { count++; });
        count.Should().Be(1);
    }

    [Test]
    public void Each8_Immutable()
    {
        var reg = TestHelper.CreateRegistryWithComponents(8);
        var count = 0;
        reg.Each((Registry _, C0 c0, C1 c1, C2 c2, C3 c3, C4 c4, C5 c5, C6 c6, C7 c7) => { count++; });
        count.Should().Be(1);
    }

    [Test]
    public void EachImmutable_EarlyReturn_NoTypesRegistered()
    {
        var reg = new Registry();
        reg.CreateEntity();
        var called = false;
        reg.Each((Registry _, C0 c0) => { called = true; });
        reg.Each((Registry _, C0 c0, C1 c1) => { called = true; });
        reg.Each((Registry _, C0 c0, C1 c1, C2 c2) => { called = true; });
        reg.Each((Registry _, C0 c0, C1 c1, C2 c2, C3 c3) => { called = true; });
        reg.Each((Registry _, C0 c0, C1 c1, C2 c2, C3 c3, C4 c4) => { called = true; });
        reg.Each((Registry _, C0 c0, C1 c1, C2 c2, C3 c3, C4 c4, C5 c5) => { called = true; });
        reg.Each((Registry _, C0 c0, C1 c1, C2 c2, C3 c3, C4 c4, C5 c5, C6 c6) => { called = true; });
        reg.Each((Registry _, C0 c0, C1 c1, C2 c2, C3 c3, C4 c4, C5 c5, C6 c6, C7 c7) => { called = true; });
        called.Should().BeFalse();
    }

    [Test]
    public void EachImmutable_EarlyReturn_PartialTypesRegistered()
    {
        var called = false;

        for (var n = 1; n <= 7; n++)
        {
            var reg = TestHelper.CreateRegistryWithComponents(n);
            called = false;
            if (n < 2) { reg.Each((Registry _, C0 c0, C1 c1) => { called = true; }); called.Should().BeFalse(); }
            if (n < 3) { called = false; reg.Each((Registry _, C0 c0, C1 c1, C2 c2) => { called = true; }); called.Should().BeFalse(); }
            if (n < 4) { called = false; reg.Each((Registry _, C0 c0, C1 c1, C2 c2, C3 c3) => { called = true; }); called.Should().BeFalse(); }
            if (n < 5) { called = false; reg.Each((Registry _, C0 c0, C1 c1, C2 c2, C3 c3, C4 c4) => { called = true; }); called.Should().BeFalse(); }
            if (n < 6) { called = false; reg.Each((Registry _, C0 c0, C1 c1, C2 c2, C3 c3, C4 c4, C5 c5) => { called = true; }); called.Should().BeFalse(); }
            if (n < 7) { called = false; reg.Each((Registry _, C0 c0, C1 c1, C2 c2, C3 c3, C4 c4, C5 c5, C6 c6) => { called = true; }); called.Should().BeFalse(); }
            if (n < 8) { called = false; reg.Each((Registry _, C0 c0, C1 c1, C2 c2, C3 c3, C4 c4, C5 c5, C6 c6, C7 c7) => { called = true; }); called.Should().BeFalse(); }
        }
    }

    // --- Mutable ---

    [Test]
    public void Each1_Mutable_ModifiesComponent()
    {
        var reg = TestHelper.CreateRegistryWithComponents(8);
        reg.Each((Registry _, ref C0 c0) => { c0.Value = 999; });
        reg.GetComponent<C0>(0).Value.Should().Be(999);
    }

    [Test]
    public void Each2_Mutable()
    {
        var reg = TestHelper.CreateRegistryWithComponents(8);
        var count = 0;
        reg.Each((Registry _, ref C0 c0, ref C1 c1) => { count++; });
        count.Should().Be(1);
    }

    [Test]
    public void Each3_Mutable()
    {
        var reg = TestHelper.CreateRegistryWithComponents(8);
        var count = 0;
        reg.Each((Registry _, ref C0 c0, ref C1 c1, ref C2 c2) => { count++; });
        count.Should().Be(1);
    }

    [Test]
    public void Each4_Mutable()
    {
        var reg = TestHelper.CreateRegistryWithComponents(8);
        var count = 0;
        reg.Each((Registry _, ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3) => { count++; });
        count.Should().Be(1);
    }

    [Test]
    public void Each5_Mutable()
    {
        var reg = TestHelper.CreateRegistryWithComponents(8);
        var count = 0;
        reg.Each((Registry _, ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4) => { count++; });
        count.Should().Be(1);
    }

    [Test]
    public void Each6_Mutable()
    {
        var reg = TestHelper.CreateRegistryWithComponents(8);
        var count = 0;
        reg.Each((Registry _, ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5) => { count++; });
        count.Should().Be(1);
    }

    [Test]
    public void Each7_Mutable()
    {
        var reg = TestHelper.CreateRegistryWithComponents(8);
        var count = 0;
        reg.Each((Registry _, ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5, ref C6 c6) => { count++; });
        count.Should().Be(1);
    }

    [Test]
    public void Each8_Mutable()
    {
        var reg = TestHelper.CreateRegistryWithComponents(8);
        var count = 0;
        reg.Each((Registry _, ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5, ref C6 c6, ref C7 c7) => { count++; });
        count.Should().Be(1);
    }

    [Test]
    public void EachMutable_EarlyReturn_NoTypesRegistered()
    {
        var reg = new Registry();
        reg.CreateEntity();
        var called = false;
        reg.Each((Registry _, ref C0 c0) => { called = true; });
        reg.Each((Registry _, ref C0 c0, ref C1 c1) => { called = true; });
        reg.Each((Registry _, ref C0 c0, ref C1 c1, ref C2 c2) => { called = true; });
        reg.Each((Registry _, ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3) => { called = true; });
        reg.Each((Registry _, ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4) => { called = true; });
        reg.Each((Registry _, ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5) => { called = true; });
        reg.Each((Registry _, ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5, ref C6 c6) => { called = true; });
        reg.Each((Registry _, ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5, ref C6 c6, ref C7 c7) => { called = true; });
        called.Should().BeFalse();
    }

    [Test]
    public void EachMutable_EarlyReturn_PartialTypesRegistered()
    {
        var called = false;

        for (var n = 1; n <= 7; n++)
        {
            var reg = TestHelper.CreateRegistryWithComponents(n);
            called = false;
            if (n < 2) { reg.Each((Registry _, ref C0 c0, ref C1 c1) => { called = true; }); called.Should().BeFalse(); }
            if (n < 3) { called = false; reg.Each((Registry _, ref C0 c0, ref C1 c1, ref C2 c2) => { called = true; }); called.Should().BeFalse(); }
            if (n < 4) { called = false; reg.Each((Registry _, ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3) => { called = true; }); called.Should().BeFalse(); }
            if (n < 5) { called = false; reg.Each((Registry _, ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4) => { called = true; }); called.Should().BeFalse(); }
            if (n < 6) { called = false; reg.Each((Registry _, ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5) => { called = true; }); called.Should().BeFalse(); }
            if (n < 7) { called = false; reg.Each((Registry _, ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5, ref C6 c6) => { called = true; }); called.Should().BeFalse(); }
            if (n < 8) { called = false; reg.Each((Registry _, ref C0 c0, ref C1 c1, ref C2 c2, ref C3 c3, ref C4 c4, ref C5 c5, ref C6 c6, ref C7 c7) => { called = true; }); called.Should().BeFalse(); }
        }
    }
}
