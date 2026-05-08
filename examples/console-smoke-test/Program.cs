using TonalSpace.Core;

var pcdInput = new double[] { 1, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0 };

var (fifthPhase, thirdPhase, thirdMagnitude) = PCDConverter.PCD2DFT(pcdInput);
var reconstructedPcd = PCDConverter.DFT2PCD(fifthPhase, thirdPhase, thirdMagnitude);

Console.WriteLine("PCD->DFT->PCD Smoke Test");
Console.WriteLine("------------------");
Console.WriteLine($"PC input: {string.Join(", ", pcdInput.Select(v => v.ToString("0.###")))}");
Console.WriteLine($"PCD2DFT : (P5: {fifthPhase:0.###}, P3: {thirdPhase:0.###}, M3: {thirdMagnitude:0.###})");
Console.WriteLine($"DFT2PCD : {string.Join(", ", reconstructedPcd.Select(v => v.ToString("0.###")))}");
