using TonalSpace.Core;

namespace TonalSpace.Tests;

public sealed class PCDConverterTests
{
    [Fact]
    public void PCD2DFT_ReturnsThreeValuesWithoutNaN()
    {
        var pcd = new double[] { 1, 0.2, 0.1, 0.4, 0.8, 0.6, 0.3, 0.9, 0.5, 0.7, 0.05, 0.15 };

        var (fifthPhase, thirdPhase, thirdMagnitude) = PCDConverter.PCD2DFT(pcd);

        Assert.False(double.IsNaN(fifthPhase));
        Assert.False(double.IsNaN(thirdPhase));
        Assert.False(double.IsNaN(thirdMagnitude));
        Assert.True(thirdMagnitude >= 0);
    }

    [Fact]
    public void DFT2PCD_Returns12ValuesBoundedToZeroAndOne()
    {
        var pcd = PCDConverter.DFT2PCD(0.5, 1.0, 0.75);

        Assert.Equal(12, pcd.Length);
        Assert.All(pcd, value => Assert.InRange(value, 0.0, 1.0));
    }

    [Fact]
    public void PCD2DFT_Then_DFT2PCD_ReturnsValidReconstruction()
    {
        var input = new double[] { 1, 0.2, 0.1, 0.4, 0.8, 0.6, 0.3, 0.9, 0.5, 0.7, 0.05, 0.15 };

        var (fifthPhase, thirdPhase, thirdMagnitude) = PCDConverter.PCD2DFT(input);
        var output = PCDConverter.DFT2PCD(fifthPhase, thirdPhase, thirdMagnitude);

        Assert.Equal(12, output.Length);
        Assert.All(output, value => Assert.InRange(value, 0.0, 1.0));
    }
}
