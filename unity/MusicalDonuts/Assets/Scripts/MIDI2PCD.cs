using Minis;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class MIDI2PCD : MonoBehaviour
{
    private readonly float[] activeNoteVelocities = new float[128];
    private readonly float[] pitchClassDistribution = new float[12];
    public float[] PitchClassDistribution => pitchClassDistribution;
    void Start()
    {
        Debug.Log("All InputSystem devices:");

        foreach (var device in InputSystem.devices)
        {
            Debug.Log($"{device.displayName} | {device.GetType().FullName} | layout={device.layout}");
        }

        Debug.Log("MIDI devices via Minis:");
        foreach (var midi in InputSystem.devices)
        {
            Debug.Log($"MIDI: {midi.displayName}");
        }
    }
    private void Awake()
    {
        Application.runInBackground = true;
    }
    
    private void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
        AttachToExistingMidiDevices();
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;

        foreach (var device in InputSystem.devices)
        {
            if (device is MidiDevice midi)
            {
                midi.onWillNoteOn -= OnNoteOn;
                midi.onWillNoteOff -= OnNoteOff;
            }
        }
    }

    private void AttachToExistingMidiDevices()
    {
        foreach (var device in InputSystem.devices)
        {
            Debug.Log($"Input device found: {device.displayName} / {device.GetType().Name}");

            if (device is MidiDevice midi)
            {
                midi.onWillNoteOn += OnNoteOn;
                midi.onWillNoteOff += OnNoteOff;

                Debug.Log($"Attached MIDI device: {midi.displayName}");
            }
        }
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        // Debug.Log($"Device change: {device.displayName} / {change}");

        if (device is MidiDevice midi && change == InputDeviceChange.Added)
        {
            midi.onWillNoteOn += OnNoteOn;
            midi.onWillNoteOff += OnNoteOff;

            Debug.Log($"Attached new MIDI device: {midi.displayName}");
        }
    }

    private void OnNoteOn(MidiNoteControl note, float velocity)
    {
        activeNoteVelocities[note.noteNumber] = velocity;
        rebuildPCD();
        // Debug.Log($"NOTE ON  note={note.noteNumber}, velocity={velocity:F2}");
    }

    private void OnNoteOff(MidiNoteControl note)
    {
        activeNoteVelocities[note.noteNumber] = 0f;
        rebuildPCD();
        // Debug.Log($"NOTE OFF note={note.noteNumber}");
    }

    private void rebuildPCD()
    {
        System.Array.Clear(pitchClassDistribution, 0, pitchClassDistribution.Length);

        for (int note = 0; note < activeNoteVelocities.Length; note++)
        {
            float velocity = activeNoteVelocities[note];
            if (velocity <= 0f) continue;

            int pitchClass = note % 12;
            pitchClassDistribution[pitchClass] += velocity;
        }

        // Debug.Log(
        //     $"PCD: C={pitchClassDistribution[0]:F2}, C#={pitchClassDistribution[1]:F2}, D={pitchClassDistribution[2]:F2}, D#={pitchClassDistribution[3]:F2}, " +
        //     $"E={pitchClassDistribution[4]:F2}, F={pitchClassDistribution[5]:F2}, F#={pitchClassDistribution[6]:F2}, G={pitchClassDistribution[7]:F2}, " +
        //     $"G#={pitchClassDistribution[8]:F2}, A={pitchClassDistribution[9]:F2}, A#={pitchClassDistribution[10]:F2}, B={pitchClassDistribution[11]:F2}"
        // );
    }
}