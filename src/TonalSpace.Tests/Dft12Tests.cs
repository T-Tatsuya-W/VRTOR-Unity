using TonalSpace.Core;

namespace TonalSpace.Tests;

public sealed class Dft12Tests
{
    [Fact]
    public void Compute_Returns12Coefficients()
    {
        var input = new double[12];
        input[0] = 1;

        var coeffs = Dft12.Compute(input);

        Assert.Equal(12, coeffs.Length);
    }
}
