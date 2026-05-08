# Architecture Overview

## Goal

Tonal Space VR transforms note input into harmonic coordinates that can be rendered in 3D (desktop first, VR later).

## Data flow

1. **MIDI/event input**
   - Note-on/off events (future: USB-MIDI via DryWetMIDI).
2. **Pitch-class mapping**
   - MIDI note number (0-127) -> pitch class (0-11).
3. **Pitch-class vector**
   - 12 bins representing class salience/energy.
4. **12-point DFT**
   - Frequency-domain harmonic descriptors from the pitch-class vector.
5. **Torus mapping**
   - DFT coefficient phases/magnitudes mapped to toroidal + poloidal coordinates.
6. **Renderer**
   - Unity desktop 3D view (future OpenXR view).

## Boundaries

- `TonalSpace.Core` must remain Unity-free and deterministic.
- Unity integration consumes frame outputs from `TonalSpace.Core`.
- Input adapters (MIDI/audio) should be swappable.

## Initial class responsibilities

- `MidiNoteMapper`: MIDI note -> pitch class.
- `PitchClassVector`: manages the 12-bin vector.
- `Dft12`: computes 12-point discrete Fourier transform.
- `TorusMapper`: maps DFT features to torus coordinates.
- `TonalFrame`: bundles one analysis snapshot.
