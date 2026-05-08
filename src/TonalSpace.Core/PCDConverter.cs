using System.Numerics;

namespace TonalSpace.Core;

/// <summary>
/// Converts between 12-bin PCD vectors and compact DFT descriptors.
/// </summary>
public static class PCDConverter
{
    private const int BinCount = 12;

    /// <summary>
    /// Converts 12 PCD values to the compact DFT representation:
    /// fifth phase, third phase, and third magnitude.
    /// </summary>
    public static (double FifthPhase, double ThirdPhase, double ThirdMagnitude) PCD2DFT(IReadOnlyList<double> pcdValues)
    {
        if (pcdValues.Count != BinCount)
        {
            throw new ArgumentException("PCD input must contain exactly 12 values.", nameof(pcdValues));
        }

        var normalized = NormalizeToUnitSum(pcdValues);
        var coefficients = Dft12.Compute(normalized);

        var fifth = coefficients[5];
        var third = coefficients[3];

        return (fifth.Phase, third.Phase, third.Magnitude);
    }

    /// <summary>
    /// Reconstructs a 12-bin PCD vector from compact DFT values:
    /// fifth phase, third phase, and third magnitude.
    /// </summary>
    public static double[] DFT2PCD(double fifthPhase, double thirdPhase, double thirdMagnitude)
    {
        if (thirdMagnitude < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(thirdMagnitude), "Magnitude must be non-negative.");
        }

        var coefficients = new Complex[BinCount];

        // DC component keeps the reconstruction centered around positive values.
        coefficients[0] = new Complex(1.0, 0.0);

        var third = Complex.FromPolarCoordinates(thirdMagnitude, thirdPhase);
        coefficients[3] = third;
        coefficients[9] = Complex.Conjugate(third);

        var fifth = Complex.FromPolarCoordinates(1.0, fifthPhase);
        coefficients[5] = fifth;
        coefficients[7] = Complex.Conjugate(fifth);

        var reconstructed = new double[BinCount];
        for (var t = 0; t < BinCount; t++)
        {
            Complex sum = Complex.Zero;
            for (var k = 0; k < BinCount; k++)
            {
                var angle = 2.0 * Math.PI * k * t / BinCount;
                sum += coefficients[k] * Complex.FromPolarCoordinates(1.0, angle);
            }

            reconstructed[t] = sum.Real / BinCount;
        }

        return NormalizeToMaxOne(reconstructed);
    }

    private static double[] NormalizeToUnitSum(IReadOnlyList<double> values)
    {
        var sum = 0.0;
        foreach (var value in values)
        {
            sum += value;
        }

        var output = new double[values.Count];
        if (sum == 0)
        {
            for (var i = 0; i < values.Count; i++)
            {
                output[i] = values[i];
            }
            return output;
        }

        for (var i = 0; i < values.Count; i++)
        {
            output[i] = values[i] / sum;
        }

        return output;
    }

    private static double[] NormalizeToMaxOne(IReadOnlyList<double> values)
    {
        var max = values.Max();
        if (max <= 0)
        {
            return values.Select(v => 0.0).ToArray();
        }

        return values.Select(v => Math.Max(0.0, v / max)).ToArray();
    }
}
