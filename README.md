# Tonal Space VR

Tonal Space VR is a C#-first project for visualizing musical harmony in a 3D tonal space.

The long-term goal is a **PCVR/OpenXR visualizer** that reacts to live MIDI performance from a digital piano over USB-MIDI. The short-term goal is a **desktop visualizer and analysis pipeline** that is easy to build, test, and iterate.

## Why desktop first, then VR

Starting with a non-Unity, non-VR core library keeps the musical analysis logic independent from rendering and hardware concerns. This gives us:

- Faster iteration on harmony math and data models.
- Clear unit-test coverage without Unity runtime overhead.
- A reusable core that can later drive both desktop and VR front ends.

## Planned architecture

The system is organized into layers:

1. **Input layer** (eventually USB-MIDI, later optional audio/chroma):
   - Parse incoming note events.
   - Convert MIDI note numbers to pitch classes (0-11).
2. **Analysis layer** (`TonalSpace.Core`):
   - Build a 12-bin pitch-class vector.
   - Compute a 12-point DFT.
   - Extract key harmonic features:
     - 5th coefficient phase -> dominant/fifths-related toroidal angle.
     - 3rd coefficient phase -> minor-thirds-related poloidal angle.
     - 3rd coefficient magnitude -> optional radial/minor-mode emphasis.
3. **Visualization layer** (Unity):
   - Desktop 3D visualizer first.
   - Later migrated/extended to OpenXR VR.

## Development tracks

This repo is structured around three parallel tracks:

1. **USB-MIDI input**
   - Device handling and note event stream ingestion.
   - Planned integration with libraries such as DryWetMIDI.
2. **C# tonal-space analysis library**
   - Pure .NET project (`TonalSpace.Core`) with no Unity dependency.
   - Pitch-class, DFT, and torus-mapping primitives.
3. **Unity visualizer**
   - Desktop visualizer first.
   - OpenXR/VR support later.

## Repository layout

- `src/TonalSpace.Core`: pure C# analysis library.
- `src/TonalSpace.Tests`: unit tests for core behavior.
- `examples/console-smoke-test`: small executable to validate analysis flow.
- `docs`: architecture, roadmap, and MIDI notes.
- `unity`: reserved for the Unity project that will be added later.

## Build and test

From repository root:

```bash
dotnet build src/TonalSpace.Core/TonalSpace.Core.csproj
dotnet test src/TonalSpace.Tests/TonalSpace.Tests.csproj
```

Run the smoke test:

```bash
dotnet run --project examples/console-smoke-test/TonalSpace.ConsoleSmokeTest.csproj
```

## Unity status

No Unity project is committed yet. Unity assets/scenes/scripts will be added later under `/unity` once the analysis pipeline is stable.
