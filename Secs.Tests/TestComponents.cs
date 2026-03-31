using Secs;

namespace Secs.Tests;

public struct C0 { public int Value; }
public struct C1 { public int Value; }
public struct C2 { public int Value; }
public struct C3 { public int Value; }
public struct C4 { public int Value; }
public struct C5 { public int Value; }
public struct C6 { public int Value; }
public struct C7 { public int Value; }
public struct Tag { }
public struct ExcludeTag { }

public static class TestHelper
{
    public static Registry CreateRegistryWithComponents(int count, int entityValue = 10)
    {
        var reg = new Registry();
        var e = reg.CreateEntity();
        AddComponents(reg, e, count, entityValue);
        return reg;
    }

    public static void AddComponents(Registry reg, int entity, int count, int baseValue = 10)
    {
        if (count >= 1) reg.AddComponent(entity, new C0 { Value = baseValue });
        if (count >= 2) reg.AddComponent(entity, new C1 { Value = baseValue + 1 });
        if (count >= 3) reg.AddComponent(entity, new C2 { Value = baseValue + 2 });
        if (count >= 4) reg.AddComponent(entity, new C3 { Value = baseValue + 3 });
        if (count >= 5) reg.AddComponent(entity, new C4 { Value = baseValue + 4 });
        if (count >= 6) reg.AddComponent(entity, new C5 { Value = baseValue + 5 });
        if (count >= 7) reg.AddComponent(entity, new C6 { Value = baseValue + 6 });
        if (count >= 8) reg.AddComponent(entity, new C7 { Value = baseValue + 7 });
    }
}
