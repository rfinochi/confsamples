param($installPath, $toolsPath, $package, $project)

$p = Get-Project

$analyzerPath = join-path $toolsPath "analyzers"
$analyzerFilePath = join-path $analyzerPath "EmptyCatchCodeFix.Demo.dll"

$p.Object.AnalyzerReferences.Remove("$analyzerFilePath")