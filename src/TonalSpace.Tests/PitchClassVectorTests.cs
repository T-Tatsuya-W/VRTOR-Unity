using TonalSpace.Core;

namespace TonalSpace.Tests;

public sealed class PitchClassVectorTests
{
    [Fact]
    public void CMajorChord_ProducesNonEmptyPitchClassValues()
    {
        var vector = new PitchClassVector();
        vector.AddMidiNote(60); // C
        vector.AddMidiNote(64); // E
        vector.AddMidiNote(67); // G

        var bins = vector.Bins;
        Assert.True(bins[0] > 0);
        Assert.True(bins[4] > 0);
        Assert.True(bins[7] > 0);
    }
}
