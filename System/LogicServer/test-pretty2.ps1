# test-pretty.ps1

# --- CONFIGURATION & SETUP ---
$baseDir = ".\TestResults"
if (!(Test-Path $baseDir)) { New-Item -ItemType Directory -Path $baseDir | Out-Null }

# 1. Get Context (Git Branch & Timestamp)
try {
    $branch = git rev-parse --abbrev-ref HEAD
    # Sanitize branch name for file system (e.g., convert 'feature/login' to 'feature-login')
    $branch = $branch -replace '[\\/:*?"<>|]', '-'
}
catch {
    $branch = "NoGit"
}

$timestamp = Get-Date -Format "yyyy-MM-dd_HH-mm-ss"
$logFileName = "TestRun_${branch}_${timestamp}.txt"
$logPath = Join-Path -Path $baseDir -ChildPath $logFileName

# Helper function to write to both Host (Color) and File (Plain text)
function Write-Log {
    param (
        [string]$Message,
        [ConsoleColor]$Color = "White",
        [switch]$NoNewline
    )
    
    # 1. Console Output
    Write-Host $Message -ForegroundColor $Color -NoNewline:$NoNewline
    
    # 2. File Output (Strip formatting, handle newlines)
    if ($NoNewline) {
        $Message | Out-File -FilePath $logPath -Append -NoNewline -Encoding utf8
    } else {
        $Message | Out-File -FilePath $logPath -Append -Encoding utf8
    }
}

Write-Log "🚀 Starting Tests..." -Color Cyan
Write-Log "📝 Saving log to: $logPath" -Color DarkGray

# --- EXECUTION ---

# 2. Run the tests quietly, generate TRX
# Note: Ensure the path matches your specific project structure or use a variable
dotnet test --nologo --verbosity quiet --logger "trx;LogFileName=TestResults.xml"

# 3. Check if the file was created
$trxPath = ".\TestsProject\TestResults\TestResults.xml" 

if (-not (Test-Path $trxPath)) {
    Write-Log "Build failed or no results found at $trxPath" -Color Red
    exit
}

# 4. Parse the XML
[xml]$xml = Get-Content $trxPath
$ns = @{ ns = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010" }

# 5. Loop through results and print nicely
$results = Select-Xml -Xml $xml -XPath "//ns:UnitTestResult" -Namespace $ns

foreach ($r in $results) {
    $node = $r.Node
    $testName = $node.testName.Split('.')[-1] 
    $duration = [math]::Round([TimeSpan]::Parse($node.duration).TotalMilliseconds)
    $outcome = $node.outcome
    
    if ($outcome -eq "Passed") {
        Write-Log "✅ $testName " -Color Green -NoNewline
        Write-Log "(${duration}ms)" -Color DarkGray
    }
    elseif ($outcome -eq "Failed") {
        Write-Log "❌ $testName " -Color Red -NoNewline
        Write-Log "(${duration}ms)" -Color DarkGray
        
        # Log error details
        $errMsg = $node.Output.ErrorInfo.Message
        if ($errMsg) {
            Write-Log "   ERROR: $errMsg" -Color Red
        }
    }
}

# 6. Summary
$counters = (Select-Xml -Xml $xml -XPath "//ns:ResultSummary/ns:Counters" -Namespace $ns).Node
Write-Log "--------------------------------------------------" -Color Gray
Write-Log "🎉 Total: $($counters.total)  |  ✅ Passed: $($counters.passed)  |  ❌ Failed: $($counters.failed)" -Color White
Write-Log "--------------------------------------------------" -Color Gray

# 7. Cleanup
Remove-Item $trxPath -ErrorAction SilentlyContinue

Write-Host -NoNewLine 'Press any key to continue...';
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');