using TonalSpace.Core;

var vector = new PitchClassVector();
var midiNotes = new[] { 60, 64, 67, 71 }; // Cmaj7

foreach (var note in midiNotes)
{
    vector.AddMidiNote(note);
}

var frame = TonalFrame.FromPitchClassVector(vector);

Console.WriteLine("Tonal Space Smoke Test");
Console.WriteLine("----------------------");
Console.WriteLine($"Input MIDI notes: {string.Join(", ", midiNotes)}");
Console.WriteLine($"Pitch class bins: {string.Join(", ", frame.PitchClassBins.Select(v => v.ToString("0.##")))}");
Console.WriteLine($"DFT coefficient count: {frame.DftCoefficients.Count}");
Console.WriteLine($"Toroidal angle (k=5 phase): {frame.ToroidalAngle:0.000}");
Console.WriteLine($"Poloidal angle (k=3 phase): {frame.PoloidalAngle:0.000}");
Console.WriteLine($"Radial emphasis (|k=3|): {frame.RadialEmphasis:0.000}");
