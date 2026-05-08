using System.Numerics;

namespace TonalSpace.Core;

/// <summary>
/// Computes a 12-point discrete Fourier transform over pitch-class values.
/// </summary>
public static class Dft12
{
    /// <summary>
    /// Computes 12 complex coefficients from a 12-element input vector.
    /// </summary>
    public static Complex[] Compute(IReadOnlyList<double> input)
    {
        if (input.Count != 12)
        {
            throw new ArgumentException("DFT12 requires exactly 12 values.", nameof(input));
        }

        var output = new Complex[12];
        const int n = 12;

        for (var k = 0; k < n; k++)
        {
            Complex sum = Complex.Zero;
            for (var t = 0; t < n; t++)
            {
                var angle = -2.0 * Math.PI * k * t / n;
                sum += input[t] * Complex.FromPolarCoordinates(1.0, angle);
            }

            output[k] = sum;
        }

        return output;
    }
}
