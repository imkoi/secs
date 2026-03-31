using FluentAssertions;
using NUnit.Framework;
using Secs;

namespace Secs.Tests;

public class FilterTest
{
    [Test]
    public void Create_Default_NullArrays()
    {
        var f = Filter.Create();
        f.Include.Should().BeNull();
        f.Exclude.Should().BeNull();
    }

    [Test]
    public void Create_WithInclude()
    {
        var f = Filter.Create(include: new[] { typeof(C0) });
        f.Include.Should().HaveCount(1);
        f.Exclude.Should().BeNull();
    }

    [Test]
    public void Create_WithExclude()
    {
        var f = Filter.Create(exclude: new[] { typeof(C1) });
        f.Include.Should().BeNull();
        f.Exclude.Should().HaveCount(1);
    }

    [Test]
    public void Create_WithBoth()
    {
        var f = Filter.Create(new[] { typeof(C0) }, new[] { typeof(C1) });
        f.Include.Should().HaveCount(1);
        f.Exclude.Should().HaveCount(1);
    }
}
