# MIDI Notes and Pitch-Class Mapping

## MIDI note basics

- MIDI note numbers are integers 0-127.
- Pitch class is modulo 12:
  - 0 = C
  - 1 = C#/Db
  - ...
  - 9 = A
  - 11 = B

Formula:

`pitchClass = ((midiNote % 12) + 12) % 12`

This keeps results valid even if non-standard negative values appear in test data.

## Early handling strategy

- Start with note-on accumulation into a 12-bin vector.
- Later incorporate velocity weighting and note-off decay.
- Consider temporal smoothing windows for stable visuals.

## Fourier emphasis

For a 12-bin pitch-class vector `x[n]`, compute DFT `X[k]` for `k=0..11`.

- `k=5` phase -> toroidal angle (fifths cycle structure).
- `k=3` phase -> poloidal angle (minor-third symmetry).
- `|X[3]|` -> optional radial modulation (minor/augmented emphasis).

These mappings are heuristic and can be calibrated during visual iteration.
