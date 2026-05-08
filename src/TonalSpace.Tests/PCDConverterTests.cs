using TonalSpace.Core;

namespace TonalSpace.Tests;

public sealed class PCDConverterTests
{
    public static void Main()
    {
        Console.WriteLine("Running PCD Converter Tests");
        Console.WriteLine("===========================\n");

        try
        {
            Test_PCD2DFT_ReturnsThreeValuesWithoutNaN();
            Console.WriteLine("✓ Test_PCD2DFT_ReturnsThreeValuesWithoutNaN PASSED\n");
        }
        catch (AssertionException ex)
        {
            Console.WriteLine($"✗ Test_PCD2DFT_ReturnsThreeValuesWithoutNaN FAILED: {ex.Message}\n");
        }

        try
        {
            Test_DFT2PCD_Returns12ValuesBoundedToZeroAndOne();
            Console.WriteLine("✓ Test_DFT2PCD_Returns12ValuesBoundedToZeroAndOne PASSED\n");
        }
        catch (AssertionException ex)
        {
            Console.WriteLine($"✗ Test_DFT2PCD_Returns12ValuesBoundedToZeroAndOne FAILED: {ex.Message}\n");
        }

        try
        {
            Test_PCD2DFT_Then_DFT2PCD_ReturnsValidReconstruction();
            Console.WriteLine("✓ Test_PCD2DFT_Then_DFT2PCD_ReturnsValidReconstruction PASSED\n");
        }
        catch (AssertionException ex)
        {
            Console.WriteLine($"✗ Test_PCD2DFT_Then_DFT2PCD_ReturnsValidReconstruction FAILED: {ex.Message}\n");
        }

        Console.WriteLine("===========================");
        Console.WriteLine("Test run complete");
    }

    private static void Test_PCD2DFT_ReturnsThreeValuesWithoutNaN()
    {
        var pcd = new double[] { 1, 0.2, 0.1, 0.4, 0.8, 0.6, 0.3, 0.9, 0.5, 0.7, 0.05, 0.15 };

        var (fifthPhase, thirdPhase, thirdMagnitude) = PCDConverter.PCD2DFT(pcd);

        Assert(
            !double.IsNaN(fifthPhase) && !double.IsNaN(thirdPhase) && !double.IsNaN(thirdMagnitude) && thirdMagnitude >= 0,
            "Values should not be NaN and magnitude should be >= 0"
        );
    }

    private static void Test_DFT2PCD_Returns12ValuesBoundedToZeroAndOne()
    {
        var pcd = PCDConverter.DFT2PCD(0.5, 1.0, 0.75);

        Assert(pcd.Length == 12, $"Expected 12 values, got {pcd.Length}");
        Assert(
            pcd.All(value => value >= 0.0 && value <= 1.0),
            "All values should be bounded between 0 and 1"
        );
    }

    private static void Test_PCD2DFT_Then_DFT2PCD_ReturnsValidReconstruction()
    {
        var input = new double[] { 1, 0.2, 0.1, 0.4, 0.8, 0.6, 0.3, 0.9, 0.5, 0.7, 0.05, 0.15 };

        var (fifthPhase, thirdPhase, thirdMagnitude) = PCDConverter.PCD2DFT(input);
        var output = PCDConverter.DFT2PCD(fifthPhase, thirdPhase, thirdMagnitude);

        Assert(output.Length == 12, $"Expected 12 values, got {output.Length}");
        Assert(
            output.All(value => value >= 0.0 && value <= 1.0),
            "All reconstructed values should be bounded between 0 and 1"
        );
    }

    private static void Assert(bool condition, string message)
    {
        if (!condition)
            throw new AssertionException(message);
    }
}

public sealed class AssertionException : Exception
{
    public AssertionException(string message) : base(message) { }
}
