using UnityEngine;

/// <summary>
/// Converts between toroidal coordinates and Cartesian coordinates on a torus surface.
/// </summary>
public static class ToroidalCoordinates
{
    /// <summary>
    /// Major radius of the torus (distance from center to tube center)
    /// </summary>
    public const float TORUS_MAJOR_RADIUS = 1f;

    /// <summary>
    /// Minor radius of the torus (radius of the tube)
    /// </summary>
    public const float TORUS_MINOR_RADIUS = 0.5f;

    /// <summary>
    /// Converts toroidal coordinates to Cartesian coordinates on the torus surface.
    /// </summary>
    /// <param name="pha5">Primary angle around major radius (0 to 2π)</param>
    /// <param name="pha3">Secondary angle around minor radius (0 to 2π)</param>
    /// <param name="mag3">Radial distance from tube center (0 to 1, scaled to minor radius).
    /// 0 means at the centerline of the tube, 1 means at the edge of the tube.</param>
    /// <returns>A Vector3 representing the Cartesian coordinates on the torus</returns>
    public static Vector3 ToroidalToCartesian(float pha5, float pha3, float mag3)
    {
        // Scale the magnitude to the actual minor radius
        float tubeRadius = mag3 * TORUS_MINOR_RADIUS;

        // Standard torus parameterization
        // First, find the position on the major circle
        float majorX = TORUS_MAJOR_RADIUS * Mathf.Cos(pha5);
        float majorZ = TORUS_MAJOR_RADIUS * Mathf.Sin(pha5);

        // Then offset by the tube radius in the direction defined by pha3
        float z = majorX + tubeRadius * Mathf.Cos(pha3) * Mathf.Cos(pha5);
        float y = tubeRadius * Mathf.Sin(pha3);
        float x = majorZ + tubeRadius * Mathf.Cos(pha3) * Mathf.Sin(pha5);

        return new Vector3(-x, y, -z);
    }
}
