using TonalSpace.Core;

namespace TonalSpace.Tests;

public sealed class PCDConverterTests
{
    [Fact]
    public void PCD2DFT_ReturnsCompactTuple()
    {
        var pcd = new double[12];
        pcd[0] = 1;
        pcd[4] = 1;
        pcd[7] = 1;

        var (fifthPhase, thirdPhase, thirdMagnitude) = PCDConverter.PCD2DFT(pcd);

        Assert.False(double.IsNaN(fifthPhase));
        Assert.False(double.IsNaN(thirdPhase));
        Assert.True(thirdMagnitude >= 0);
    }

    [Fact]
    public void DFT2PCD_Returns12ValuesBoundedToZeroAndOne()
    {
        var pcd = PCDConverter.DFT2PCD(0.5, 1.0, 0.75);

        Assert.Equal(12, pcd.Length);
        Assert.All(pcd, value => Assert.InRange(value, 0.0, 1.0));
    }
}
