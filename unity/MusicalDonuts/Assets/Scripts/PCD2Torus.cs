using System;
using UnityEngine;
using System.Linq;

public class PCD2Torus : MonoBehaviour
{
    [SerializeField] private MIDI2PCD PCDSource;

    private void Update()
    {
        float[] distribution = PCDSource.PitchClassDistribution;
        double[] pcdDoubles = distribution.Select(f => (double)f).ToArray();
        var (fifthPhase, thirdPhase, thirdMagnitude) = PCDConverter.PCD2DFT(pcdDoubles);
        Debug.Log($"PCD2Torus DFT: FifthPhase: {fifthPhase:F2}, ThirdPhase: {thirdPhase:F2}, ThirdMagnitude: {thirdMagnitude:F2}");


        // Map DFT values to xyz coordinates in the torus.
        Vector3 torusPoint = ToroidalCoordinates.ToroidalToCartesian(
            pha5: (float)fifthPhase,
            pha3: (float)thirdPhase,
            mag3: (float)thirdMagnitude
        );

        // For demonstration, we can move this GameObject to the computed torus point.
        transform.position = torusPoint;
        
    }
}