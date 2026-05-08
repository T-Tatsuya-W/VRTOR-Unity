using TonalSpace.Core;

var pcdInput = new double[] { 1, 0.2, 0.1, 0.4, 0.8, 0.6, 0.3, 0.9, 0.5, 0.7, 0.05, 0.15 };

var (fifthPhase, thirdPhase, thirdMagnitude) = PCDConverter.PCD2DFT(pcdInput);
var reconstructedPcd = PCDConverter.DFT2PCD(fifthPhase, thirdPhase, thirdMagnitude);

Console.WriteLine("PCD/DFT Smoke Test");
Console.WriteLine("------------------");
Console.WriteLine($"PCD input (12 values): {string.Join(", ", pcdInput.Select(v => v.ToString("0.###")))}");
Console.WriteLine($"PCD2DFT output: fifthPhase={fifthPhase:0.###}, thirdPhase={thirdPhase:0.###}, thirdMagnitude={thirdMagnitude:0.###}");
Console.WriteLine($"DFT2PCD output (12 values): {string.Join(", ", reconstructedPcd.Select(v => v.ToString("0.###")))}");
