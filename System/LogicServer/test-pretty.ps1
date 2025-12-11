# test-pretty.ps1

# 1. Run the tests quietly, but generate a machine-readable TRX file
Write-Host "🚀 Starting Tests..." -ForegroundColor Cyan
dotnet test --nologo --verbosity quiet --logger "trx;LogFileName=TestResults.xml"

# 2. Check if the file was created (in case build failed)
$trxPath = ".\TestsProject\TestResults\TestResults.xml"
if (-not (Test-Path $trxPath)) {
    Write-Error "Build failed or no results found."
    exit
}

# 3. Parse the XML
[xml]$xml = Get-Content $trxPath
$ns = @{ ns = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010" }

# 4. Loop through results and print nicely
$results = Select-Xml -Xml $xml -XPath "//ns:UnitTestResult" -Namespace $ns

foreach ($r in $results) {
    $node = $r.Node
    $testName = $node.testName.Split('.')[-1] # Get just the method name
    $duration = [math]::Round([TimeSpan]::Parse($node.duration).TotalMilliseconds)
    $outcome = $node.outcome
    
    if ($outcome -eq "Passed") {
        Write-Host "✅ $testName " -NoNewline -ForegroundColor Green
        Write-Host "(${duration}ms)" -ForegroundColor DarkGray
    }
    elseif ($outcome -eq "Failed") {
        Write-Host "❌ $testName " -NoNewline -ForegroundColor Red
        Write-Host "(${duration}ms)" -ForegroundColor DarkGray
        # Optional: Print error message if failed
        Write-Host "   ERROR: $($node.Output.ErrorInfo.Message)" -ForegroundColor Red
    }
}

# 5. Summary
$counters = (Select-Xml -Xml $xml -XPath "//ns:ResultSummary/ns:Counters" -Namespace $ns).Node
Write-Host "--------------------------------------------------" -ForegroundColor Gray
Write-Host "🎉 Total: $($counters.total)  |  ✅ Passed: $($counters.passed)  |  ❌ Failed: $($counters.failed)" -ForegroundColor White
Write-Host "--------------------------------------------------" -ForegroundColor Gray

# 6. Cleanup (Optional)
Remove-Item $trxPath -ErrorAction SilentlyContinue

Write-Host -NoNewLine 'Press any key to continue...';
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');