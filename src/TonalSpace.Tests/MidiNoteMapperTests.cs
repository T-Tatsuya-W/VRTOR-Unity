using TonalSpace.Core;

namespace TonalSpace.Tests;

public sealed class MidiNoteMapperTests
{
    [Fact]
    public void Midi60_MapsToPitchClass0() => Assert.Equal(0, MidiNoteMapper.ToPitchClass(60));

    [Fact]
    public void Midi61_MapsToPitchClass1() => Assert.Equal(1, MidiNoteMapper.ToPitchClass(61));

    [Fact]
    public void Midi69_MapsToPitchClass9() => Assert.Equal(9, MidiNoteMapper.ToPitchClass(69));
}
