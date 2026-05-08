using System.Numerics;

namespace TonalSpace.Core;

/// <summary>
/// Maps selected DFT coefficients into torus-related coordinates.
/// </summary>
public static class TorusMapper
{
    /// <summary>
    /// Derives toroidal angle, poloidal angle, and radial emphasis from DFT coefficients.
    /// </summary>
    public static (double ToroidalAngle, double PoloidalAngle, double RadialEmphasis) MapFromCoefficients(IReadOnlyList<Complex> coefficients)
    {
        if (coefficients.Count != 12)
        {
            throw new ArgumentException("Expected 12 DFT coefficients.", nameof(coefficients));
        }

        var fifth = coefficients[5];
        var third = coefficients[3];

        return (fifth.Phase, third.Phase, third.Magnitude);
    }
}
