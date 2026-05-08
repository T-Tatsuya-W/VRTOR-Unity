namespace TonalSpace.Core;

/// <summary>
/// Converts MIDI note numbers into 12-tone pitch classes.
/// </summary>
public static class MidiNoteMapper
{
    /// <summary>
    /// Maps a MIDI note number to pitch class in range 0..11.
    /// </summary>
    public static int ToPitchClass(int midiNote) => ((midiNote % 12) + 12) % 12;
}
