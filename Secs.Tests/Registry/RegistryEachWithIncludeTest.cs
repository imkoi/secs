using NUnit.Framework;

namespace Secs.Tests;

public class RegistryEachWithIncludeTest
{
    [Test]
    public unsafe void Test1()
    {
        var reg = new Registry();

        var ent = reg.CreateEntity();
        reg.AddComponent(ent, 10);

        // var group = reg.GetGroup([typeof(int)]);
        //
        // foreach (var entityId in group)
        // {
        //     ref var val = ref reg.GetComponent<int>(entityId);
        // }

        // group.Each(static (int val) =>
        // {
        //     Console.WriteLine(val);
        // });
    }
}