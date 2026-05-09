using System;
using UnityEngine;

public class PCDVisualiser : MonoBehaviour
{
    [SerializeField] private MIDI2PCD PCDSource;

    // value to choose which pitch class to visualise, 0-11 for C-B
    [SerializeField] private int pitchClassToVisualise = 0;

    // value for starting cube sizes    
    [SerializeField] private float baseScale = 0.2f;

    //x position offset, multiplied by pcd value, to spread out the cubes
    [SerializeField] private float xOffsetMultiplier = 0.2f;

    private void Start()
    {
        // position the cube based on its pitch class
        transform.localPosition = new Vector3(pitchClassToVisualise * xOffsetMultiplier, 0f, 0f);
    }

    private void Update()
    {
        float cValue = PCDSource.PitchClassDistribution[pitchClassToVisualise];
        transform.localScale = new Vector3(baseScale, baseScale + cValue, baseScale);
    }
}