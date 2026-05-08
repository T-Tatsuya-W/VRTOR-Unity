# Development Roadmap

## Phase 0 - Foundation (current)

- Establish repository structure.
- Implement/test core musical primitives in pure C#.
- Add console smoke test for end-to-end sanity check.

## Phase 1 - Desktop analysis loop

- Add MIDI input adapter using DryWetMIDI.
- Maintain rolling pitch-class distribution over time windows.
- Feed analysis snapshots to a simple desktop visualizer.

## Phase 2 - Unity desktop visualizer

- Add Unity project under `/unity`.
- Render tonal torus and trajectory/history.
- Add UI controls for scaling, damping, and coloring.

## Phase 3 - VR enablement

- Add OpenXR integration.
- Tune interaction and performance for PCVR.
- Support controller + keyboard workflows.

## Phase 4 - Advanced analysis (optional)

- Add audio/chroma fallback when MIDI unavailable.
- Explore additional Fourier coefficients and smoothing.
- Evaluate key-estimation overlays and progression detection.
