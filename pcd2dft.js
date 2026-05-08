/**
 * PCD-DFT: Clean JavaScript functions for PCD ↔ Frequency Domain conversion
 * 
 * This module provides two main functions for working with 12-bin Pitch Class Distribution (PCD) data:
 * - pcdToFrequencyDomain: Convert 12 PCD values to amplitude/phase representation
 * - frequencyDomainToPcd: Convert amplitude/phase back to 12 PCD values
 * 
 * Usage:
 *   import { pcdToFrequencyDomain, frequencyDomainToPcd } from './pcd-dft.js';
 *   
 *   // Forward transform
 *   const { amplitudes, phases } = PCDtoDFT([1,0,0,0,1,0,0,1,0,0,0,0]);
 *   
 *   // Inverse transform
 *   const reconstructed = frequencyDomainToPcd(amplitudes, phases);
 */

/**
 * Forward DFT: Convert 12 PCD values to amplitude/phase representation
 * 
 * Takes a 12-element array of PCD (Pitch Class Distribution) values and converts them
 * to frequency domain representation using real FFT. The input is automatically normalized
 * so that the sum equals 1, which is standard for PCD data.
 * 
 * @param {number[]} pcdValues - Array of exactly 12 PCD values (will be normalized to sum=1)
 * @returns {Object} Result object containing:
 *   - amplitudes: Array of 7 amplitude values (k=0..6)
 *   - phases: Array of 7 phase values in radians (k=0..6)
 *   - normalizedInput: The normalized input array (sum=1)
 * @throws {Error} If input is not an array of exactly 12 values
 * 
 * @example
 *   const result = pcdToFrequencyDomain([1,0,0,0,1,0,0,1,0,0,0,0]);
 *   console.log(result.amplitudes); // [0.33333, 0.28868, 0, 0.28868, 0, 0.28868, 0]
 */
export function PCDtoDFT(pcdValues) {
  // Validate input
  if (!Array.isArray(pcdValues) || pcdValues.length !== 12) {
    throw new Error('Input must be an array of exactly 12 values');
  }
  
  // Normalize input so sum = 1
  const sum = pcdValues.reduce((a, b) => a + b, 0);
  const normalized = sum === 0 ? pcdValues.slice() : pcdValues.map(v => v / sum);
  
  // Compute rFFT (real FFT for 12 bins -> 7 frequency components k=0..6)
  const N = 12;
  const M = 7; // k=0..6
  const amplitudes = new Array(M);
  const phases = new Array(M);
  
  for (let k = 0; k < M; k++) {
    let re = 0, im = 0;
    const base = -2 * Math.PI * k / N;
    
    for (let t = 0; t < N; t++) {
      const angle = base * t;
      re += normalized[t] * Math.cos(angle);
      im += normalized[t] * Math.sin(angle);
    }
    
    amplitudes[k] = Math.sqrt(re * re + im * im);
    phases[k] = Math.atan2(im, re);
  }
  
  return {
    amplitudes,
    phases,
    normalizedInput: normalized
  };
}

/**
 * Inverse DFT: Convert amplitude/phase representation back to 12 PCD values
 * 
 * Takes amplitude and phase arrays from the frequency domain and reconstructs the original
 * 12-element PCD array using inverse real FFT. This is the inverse operation of 
 * pcdToFrequencyDomain().
 * 
 * @param {number[]} amplitudes - Array of exactly 7 amplitude values (k=0..6)
 * @param {number[]} phases - Array of exactly 7 phase values in radians (k=0..6)
 * @returns {number[]} Reconstructed 12-element PCD array
 * @throws {Error} If amplitudes or phases arrays are not exactly 7 elements
 * 
 * @example
 *   const amplitudes = [0.33333, 0.28868, 0, 0.28868, 0, 0.28868, 0];
 *   const phases = [0, -1.5708, 0, 1.5708, 0, 0, 0];
 *   const reconstructed = DFTtoPCD(amplitudes, phases);
 *   // Returns approximately [1,0,0,0,1,0,0,1,0,0,0,0] (normalized)
 */
export function DFTtoPCD(amplitudes, phases) {
  // Validate inputs
  if (!Array.isArray(amplitudes) || amplitudes.length !== 7) {
    throw new Error('Amplitudes must be an array of exactly 7 values');
  }
  if (!Array.isArray(phases) || phases.length !== 7) {
    throw new Error('Phases must be an array of exactly 7 values');
  }
  
  const N = 12;
  const M = 7;
  
  // Build full Hermitian spectrum for iDFT
  const spectrum = new Array(N).fill(null).map(() => ({ re: 0, im: 0 }));
  
  // Fill positive frequencies k=0..6
  for (let k = 0; k < M; k++) {
    spectrum[k].re = amplitudes[k] * Math.cos(phases[k]);
    spectrum[k].im = amplitudes[k] * Math.sin(phases[k]);
  }
  
  // Fill negative frequencies (complex conjugates) k=7..11
  for (let k = 1; k < M - 1; k++) {
    spectrum[N - k].re = spectrum[k].re;   // Real part unchanged
    spectrum[N - k].im = -spectrum[k].im;  // Imaginary part negated
  }
  
  // Inverse DFT: convert back to time domain (PCD values)
  const reconstructed = new Array(N);
  for (let t = 0; t < N; t++) {
    let re = 0;
    for (let k = 0; k < N; k++) {
      const angle = 2 * Math.PI * k * t / N;
      const cos = Math.cos(angle);
      const sin = Math.sin(angle);
      re += spectrum[k].re * cos - spectrum[k].im * sin;
    }
    reconstructed[t] = re / N; // Normalize by N
  }
  
  const normalizedReconstructed = Array.isArray(reconstructed) ? reconstructed : [];
  return thresholdPCD(normalizePCDToMax(normalizedReconstructed), 0.65);
}

/**
 * Utility function to normalize a PCD array so its sum equals 1
 * 
 * @param {number[]} arr - Array to normalize
 * @returns {number[]} Normalized array where sum = 1
 */
export function normalize12(arr) {
  const sum = arr.reduce((a, b) => a + b, 0);
  return sum === 0 ? arr.slice() : arr.map(v => v / sum);
}

/**
 * Normalize PCD values so that the maximum value becomes 1
 * @param {number[]} pcdArray - Array of PCD values
 * @returns {number[]} Normalized PCD array where max value = 1
 */
export function normalizePCDToMax(pcdArray) {
  const max = Math.max(...pcdArray);
  return max === 0 ? pcdArray.slice() : pcdArray.map(v => v / max);
}

/**
 * Threshold PCD values: set all values below threshold to 0
 * @param {number[]} pcdArray - Array of PCD values
 * @param {number} threshold - Threshold value (default 0.1)
 * @returns {number[]} PCD array with values below threshold set to 0
 */
export function thresholdPCD(pcdArray, threshold = 0.1) {
  return pcdArray.map(v => v < threshold ? 0 : v);
}


