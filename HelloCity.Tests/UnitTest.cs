using HelloCity.Services;
using Xunit;

namespace HelloCity.Tests;

public class TestUserServiceTest
{
    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(-1, -2, -3)]
    [InlineData(0, 0, 0)]
    [InlineData(1000, 2000, 3000)]
    public void SumInt_ReturnsCorrectSum(int a, int b, int expected)
    {
        var unitTest = new UnitTestService();
        var result = unitTest.SumInt(a, b);
        Assert.Equal(expected,result);
    }
}