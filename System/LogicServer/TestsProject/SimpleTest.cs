namespace TestsProject;

public class SimpleTest
{
    [Fact]
    public void Test1()
    {
        var result = Helper();
        Assert.True(result);
    }

    private bool Helper()
    {
        return true;
    }
}
