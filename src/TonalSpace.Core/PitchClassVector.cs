namespace TonalSpace.Core;

/// <summary>
/// Represents a 12-bin pitch class distribution.
/// </summary>
public sealed class PitchClassVector
{
    private readonly double[] _bins = new double[12];

    /// <summary>
    /// Gets a read-only view of the internal bins.
    /// </summary>
    public IReadOnlyList<double> Bins => _bins;

    /// <summary>
    /// Adds weight to the bin that corresponds to the given MIDI note.
    /// </summary>
    public void AddMidiNote(int midiNote, double weight = 1.0)
    {
        var pitchClass = MidiNoteMapper.ToPitchClass(midiNote);
        _bins[pitchClass] += weight;
    }

    /// <summary>
    /// Adds weight directly to a pitch class bin.
    /// </summary>
    public void AddPitchClass(int pitchClass, double weight = 1.0)
    {
        var normalized = MidiNoteMapper.ToPitchClass(pitchClass);
        _bins[normalized] += weight;
    }

    /// <summary>
    /// Creates a copy of the bins as a mutable array.
    /// </summary>
    public double[] ToArray() => (double[])_bins.Clone();
}
