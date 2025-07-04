using System;
using AridityTeam.Text;

namespace AridityTeam.Platform.Tests.Text;

public class UtlStringTests
{
    [Fact]
    public void UtlString_StringContains()
    {
        UtlString str = "Hello, World!";
        string substring = "World";
        bool result = str.Contains(substring);
        Assert.True(result, $"Expected '{str}' to contain '{substring}'");
    }

    [Fact]
    public void UtlString_ReserveIncreasesCapacity()
    {
        UtlString str = new UtlString(10);
        str.Append("Hello");
        int initialLength = str.ToString().Length;
        str.Reserve(20);
        str.Append(", World!");
        
        Assert.True(str.ToString().Length > initialLength, "Expected string length to increase after reserve and append.");
    }
}
