using System;
using FluentAssertions;
using NUnit.Framework;
using Secs;

namespace Secs.Tests;

public class RegistryAddComponentTest
{
    [Test]
    public void AddComponent_StoresValue()
    {
        var reg = new Registry();
        var e = reg.CreateEntity();
        reg.AddComponent(e, new C0 { Value = 42 });
        reg.GetComponent<C0>(e).Value.Should().Be(42);
    }

    [Test]
    public void AddComponent_ReturnsMutableRef()
    {
        var reg = new Registry();
        var e = reg.CreateEntity();
        ref var c = ref reg.AddComponent(e, new C0 { Value = 1 });
        c.Value = 99;
        reg.GetComponent<C0>(e).Value.Should().Be(99);
    }

    [Test]
    public void AddComponent_ResizesSparseArray()
    {
        var reg = new Registry();
        var e0 = reg.CreateEntity();
        reg.AddComponent(e0, new C0 { Value = 1 });
        var e1 = reg.CreateEntity();
        reg.AddComponent(e1, new C0 { Value = 2 });
        reg.GetComponent<C0>(e1).Value.Should().Be(2);
    }

    [Test]
    public void AddComponent_ResizesDenseArray()
    {
        var reg = new Registry();
        for (var i = 0; i < 40; i++)
        {
            var e = reg.CreateEntity();
            reg.AddComponent(e, new C0 { Value = i });
        }

        for (var i = 0; i < 40; i++)
            reg.GetComponent<C0>(i).Value.Should().Be(i);
    }

    [Test]
    public void RegisterComponent_ResizesComponentsArray_WhenExceeding32Types()
    {
        var reg = new Registry();
        var e = reg.CreateEntity();
        reg.AddComponent(e, new C0());
        reg.AddComponent(e, new C1());
        reg.AddComponent(e, new C2());
        reg.AddComponent(e, new C3());
        reg.AddComponent(e, new C4());
        reg.AddComponent(e, new C5());
        reg.AddComponent(e, new C6());
        reg.AddComponent(e, new C7());
        reg.AddComponent(e, new Tag());
        reg.AddComponent(e, new ExcludeTag());
        reg.AddComponent<int>(e, 0);
        reg.AddComponent<float>(e, 0);
        reg.AddComponent<double>(e, 0);
        reg.AddComponent<long>(e, 0);
        reg.AddComponent<short>(e, 0);
        reg.AddComponent<byte>(e, 0);
        reg.AddComponent<bool>(e, false);
        reg.AddComponent<char>(e, ' ');
        reg.AddComponent<decimal>(e, 0);
        reg.AddComponent<uint>(e, 0);
        reg.AddComponent<ulong>(e, 0);
        reg.AddComponent<ushort>(e, 0);
        reg.AddComponent<sbyte>(e, 0);
        reg.AddComponent<nint>(e, 0);
        reg.AddComponent<nuint>(e, 0);
        reg.AddComponent<TimeSpan>(e, TimeSpan.Zero);
        reg.AddComponent<DateTime>(e, DateTime.MinValue);
        reg.AddComponent<DateOnly>(e, DateOnly.MinValue);
        reg.AddComponent<TimeOnly>(e, TimeOnly.MinValue);
        reg.AddComponent<Guid>(e, Guid.Empty);
        reg.AddComponent<Half>(e, Half.Zero);
        reg.AddComponent<Int128>(e, Int128.Zero);
        reg.AddComponent<UInt128>(e, UInt128.Zero);
        reg.HasComponent<UInt128>(e).Should().BeTrue();
    }
}
