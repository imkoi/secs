using NUnit.Framework;

namespace Secs.Tests;

public class GroupEachComponentsTest
{
    [Test]
    public void Test()
    {
        var reg = new Registry();
        
        reg.Each<int>(static (r, component) =>
        {
            
        });
    }
}