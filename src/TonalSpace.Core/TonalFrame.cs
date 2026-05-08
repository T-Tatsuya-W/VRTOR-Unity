using System.Numerics;

namespace TonalSpace.Core;

/// <summary>
/// Captures a single analysis frame with pitch-class and torus-mapping features.
/// </summary>
public sealed record TonalFrame(
    IReadOnlyList<double> PitchClassBins,
    IReadOnlyList<Complex> DftCoefficients,
    double ToroidalAngle,
    double PoloidalAngle,
    double RadialEmphasis)
{
    /// <summary>
    /// Builds a tonal frame from a pitch-class vector.
    /// </summary>
    public static TonalFrame FromPitchClassVector(PitchClassVector vector)
    {
        var bins = vector.ToArray();
        var coeffs = Dft12.Compute(bins);
        var mapped = TorusMapper.MapFromCoefficients(coeffs);

        return new TonalFrame(bins, coeffs, mapped.ToroidalAngle, mapped.PoloidalAngle, mapped.RadialEmphasis);
    }
}
