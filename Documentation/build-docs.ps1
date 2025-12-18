<#
.SYNOPSIS
  Universal PDF Build Script.
  Combines specific project report generation with dynamic PlantUML processing.
  
.DESCRIPTION
  - Generates specific project documents (Description, Process, Report).
  - Generates a combined "FinalDocument.pdf".
  - Generates generic documents from arbitrary Markdown files.
  - Automatically generates and applies a Lua filter for PlantUML diagrams.
  - Calculates word/char counts and injects them into TeX templates.

.PARAMETER Target
  The build target. 
  Specifics: ProjectDescription | ProcessReport | ProjectReport 
  Composites: All | Combined
  Generic: Generic (requires -File)

.PARAMETER File
  The source markdown file (required only when Target is 'Generic').

.PARAMETER Title
  Overrides the document title (metadata).

.PARAMETER OutputDir
  Optional override for the output directory. Defaults to 'Final'.
#>

param(
  [ValidateSet("ProjectDescription", "ProcessReport", "ProjectReport", "Generic", "All", "Combined")]
  [string]$Target = "All",

  [string]$File,
  [string]$Title,
  [string]$OutputDir
)

$ErrorActionPreference = "Stop"

# ==========================================
# 1. SETUP & PATHING (No Hard-coded Paths)
# ==========================================

# Set root to the script's location
$ScriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location -Path $ScriptRoot

# Define Standard Paths
$StylesDir = Join-Path $ScriptRoot "Styles"
$FinalDir = if ($OutputDir) { Resolve-Path $OutputDir } else { Join-Path $ScriptRoot "Final" }

# Ensure Directories Exist
if (-not (Test-Path $FinalDir)) { New-Item -ItemType Directory -Path $FinalDir | Out-Null }
if (-not (Test-Path $StylesDir)) { New-Item -ItemType Directory -Path $StylesDir | Out-Null }

# ==========================================
# 2. VISUALS & LOGGING
# ==========================================
$E_Rocket = [char]::ConvertFromUtf32(0x1F680)
$E_Check = [char]::ConvertFromUtf32(0x2705)
$E_Warn = [char]::ConvertFromUtf32(0x26A0)
$E_X = [char]::ConvertFromUtf32(0x274C)
$E_Build = [char]::ConvertFromUtf32(0x1F3D7)
$E_Info = [char]::ConvertFromUtf32(0x2139)

function Log-Step($msg) { Write-Host "$E_Info $msg" -ForegroundColor Cyan }
function Log-Success($msg) { Write-Host "$E_Check $msg" -ForegroundColor Green }
function Log-Error($msg) { Write-Host "$E_X $msg" -ForegroundColor Red }
function Log-Build($msg) { Write-Host "$E_Build $msg" -ForegroundColor Magenta }

# ==========================================
# 3. DEPENDENCY CHECK
# ==========================================
function Require-Tool($name) {
  if (-not (Get-Command $name -ErrorAction SilentlyContinue)) {
    Log-Error "Missing tool: '$name'. Please add it to your PATH."
    throw "Missing dependency: $name"
  }
}

Log-Step "Checking environment..."
Require-Tool pandoc
Require-Tool qpdf
Require-Tool plantuml # Explicit check for puml
Require-Tool java     # Required for puml
Require-Tool dot      # Required for puml (Graphviz)

# ==========================================
# 4. PLANTUML LUA FILTER GENERATION
# ==========================================
# We generate these files on the fly to ensure they exist and work with relative paths.

$GlobalPumlFile = Join-Path $StylesDir "plantuml-config.puml"
if (-not (Test-Path $GlobalPumlFile)) {
  Set-Content -Path $GlobalPumlFile -Value "' Global PlantUML Config`n!theme plain"
}
$LuaConfigPath = $GlobalPumlFile -replace '\\', '/' # Lua needs forward slashes

$LuaFilterPath = Join-Path $StylesDir "plantuml.lua"
$LuaContentLines = @(
  "-- Generates PlantUML images and injects global config",
  "local pandoc = require 'pandoc'",
  "local system = require 'pandoc.system'",
  "",
  "function CodeBlock(block)",
  "    if block.classes:includes('plantuml') then",
  "        local content = block.text",
  "        -- 1. Read Global Config",
  "        local f = io.open('$LuaConfigPath', 'r')",
  "        local config = ''",
  "        if f then config = f:read('*all'); f:close() end",
  "        -- 2. Inject Config",
  "        if content:find('@startuml') then",
  "             content = content:gsub('@startuml', '@startuml\n' .. config)",
  "        else",
  "             content = '@startuml\n' .. config .. '\n' .. content .. '\n@enduml'",
  "        end",
  "        -- 3. Render",
  "        local hash = pandoc.sha1(content)",
  "        local img_dir = 'plantuml-images'",
  "        pcall(system.make_directory, img_dir)",
  "        local fname = img_dir .. '/' .. hash .. '.png'",
  "        local f_img = io.open(fname, 'r')",
  "        if f_img == nil then",
  "            local result = pandoc.pipe('plantuml', {'-pipe', '-tpng'}, content)",
  "            local f_out = io.open(fname, 'wb'); f_out:write(result); f_out:close()",
  "        else",
  "            f_img:close()",
  "        end",
  "",
  "        -- 4. Attributes & Manual LaTeX Generation",
  "        local attributes = block.attributes",
  "        local caption_text = attributes['caption']",
  "        local width = attributes['width'] or '60%'",
  "",
  "        -- CHECK: If generating PDF (LaTeX), write raw TeX to force centering/captions",
  "        if FORMAT:match 'latex' then",
  "            -- Convert '60%' to '0.6\\linewidth' for LaTeX",
  "            local latex_width = width",
  "            if width:match('%%') then",
  "                 local num = tonumber(width:match('(%d+)'))",
  "                 if num then latex_width = (num/100) .. '\\linewidth' end",
  "            end",
  "",
  "            -- NOTE: \\\\begin = literal \\begin. \\n = newline character.",
  "            local tex = ''",
  "            tex = tex .. '\\begin{figure}[H]\n'",
  "            tex = tex .. '\\centering\n'",
  "            tex = tex .. '\\includegraphics[width=' .. latex_width .. ']{' .. fname .. '}\n'",
  "            if caption_text then",
  "                tex = tex .. '\\caption{' .. caption_text .. '}\n'",
  "            end",
  "            tex = tex .. '\\end{figure}'",
  "",
  "            return pandoc.RawBlock('latex', tex)",
  "        else",
  "            -- Fallback for HTML/Docx (Standard Pandoc Behavior)",
  "            local img_attr = pandoc.Attr(block.identifier, block.classes, attributes)",
  "            attributes['width'] = width",
  "            ",
  "            local caption_content = {}",
  "            local img_title = ''",
  "            if caption_text then",
  "                caption_content = { pandoc.Str(caption_text) }",
  "                img_title = 'fig:'",
  "            end",
  "            return pandoc.Para({ pandoc.Image(caption_content, fname, img_title, img_attr) })",
  "        end",
  "    end",
  "end"
)
$LuaContentLines -join "`n" | Set-Content -Path $LuaFilterPath
Log-Success "Lua filter ready."

# ==========================================
# 5. CORE BUILD LOGIC
# ==========================================

# Helper to process TeX Templates with Word Counts
function Update-TeXStats {
  param($TeXFile, $MdFile)
    
  $content = Get-Content $MdFile -Raw
  $charCount = $content.Length
  $wordCount = ($content -split '\s+').Where({ $_ -ne '' }).Count
    
  $texContent = Get-Content $TeXFile -Raw
  $texContent = $texContent -replace '\$charcount\$', $charCount
  $texContent = $texContent -replace '\$wordcount\$', $wordCount
    
  $tempName = "temp-" + [System.IO.Path]::GetFileName($TeXFile)
  $tempPath = Join-Path $ScriptRoot $tempName
  $texContent | Out-File $tempPath -Encoding UTF8
    
  return $tempPath
}

# The Master Pandoc Runner
function Invoke-PandocBuild {
  param(
    [string]$InputFile,
    [string]$OutputFile,
    [string]$MetaYaml,
    [string]$TemplateTex, 
    [string]$HeaderTex,   
    [bool]$AddToc = $false,
    [string]$DocTitle
  )

  if (-not (Test-Path $InputFile)) { throw "Input file not found: $InputFile" }

  Log-Build "Processing: $InputFile -> $OutputFile"

  # 1. Update Stats in TeX
  $tempBodyTex = Update-TeXStats -TeXFile $TemplateTex -MdFile $InputFile

  # 2. Standard Header Includes
  $preamble = Join-Path $StylesDir "preamble.tex"
    
  # 3. LaTeX Header Extras (Float placement only - Removed Gin keys)
  $latexHeaderIncludes = "\usepackage{float} \floatplacement{figure}{H}"

  # 4. Construct Command
  $params = @(
    "--metadata-file=$MetaYaml",
    "--include-in-header=$preamble",
    "--include-in-header=$HeaderTex",
    "--include-before-body=$tempBodyTex",
    "-V", "geometry:margin=25mm",
    "-V", "header-includes=$latexHeaderIncludes",
    "--pdf-engine=pdflatex",
    "--number-sections",
    "--lua-filter=$LuaFilterPath",
    "-o", $OutputFile
  )

  if ($AddToc) { $params += "--include-before-body=$(Join-Path $StylesDir 'toc.tex')" }
  if ($DocTitle) { $params += "--metadata=title='$DocTitle'" }

  # 5. Run Pandoc
  pandoc $InputFile @params

  # 6. Cleanup
  Remove-Item $tempBodyTex -ErrorAction SilentlyContinue
  Log-Success "Created: $OutputFile"
}

# ==========================================
# 6. SPECIFIC REPORT DEFINITIONS
# ==========================================

function Build-ProjectDescription {
  Invoke-PandocBuild `
    -InputFile (Join-Path $ScriptRoot "ProjectDescription.md") `
    -OutputFile (Join-Path $FinalDir "ProjectDescription.pdf") `
    -MetaYaml (Join-Path $StylesDir "ptd-meta.yaml") `
    -TemplateTex (Join-Path $StylesDir "ptd.tex") `
    -HeaderTex (Join-Path $StylesDir "ptd-hdr-ftr.tex") `
    -AddToc $true
}

function Build-ProcessReport {
  Invoke-PandocBuild `
    -InputFile (Join-Path $ScriptRoot "ProcessReport.md") `
    -OutputFile (Join-Path $FinalDir "ProcessReport.pdf") `
    -MetaYaml (Join-Path $StylesDir "psr-meta.yaml") `
    -TemplateTex (Join-Path $StylesDir "psr.tex") `
    -HeaderTex (Join-Path $StylesDir "psr-hdr-ftr.tex") `
    -AddToc $true
}

function Build-ProjectReport {
  Invoke-PandocBuild `
    -InputFile (Join-Path $ScriptRoot "ProjectReport.md") `
    -OutputFile (Join-Path $FinalDir "ProjectReport.pdf") `
    -MetaYaml (Join-Path $StylesDir "ptr-meta.yaml") `
    -TemplateTex (Join-Path $StylesDir "ptr.tex") `
    -HeaderTex (Join-Path $StylesDir "ptr-hdr-ftr.tex") `
    -AddToc $true
}

function Build-Generic {
  if (-not $File) { throw "Parameter -File is required for Generic build." }
    
  $baseName = [System.IO.Path]::GetFileNameWithoutExtension($File)
  $outPath = Join-Path $FinalDir "$baseName.pdf"
    
  Invoke-PandocBuild `
    -InputFile (Resolve-Path $File) `
    -OutputFile $outPath `
    -MetaYaml (Join-Path $StylesDir "general-meta.yaml") `
    -TemplateTex (Join-Path $StylesDir "general.tex") `
    -HeaderTex (Join-Path $StylesDir "general-hdr-ftr.tex") `
    -DocTitle $Title
}

function Build-Combined {
  Log-Build "Combining PDFs..."
  $pd = Join-Path $FinalDir "ProjectDescription.pdf"
  $ps = Join-Path $FinalDir "ProcessReport.pdf"
  $pr = Join-Path $FinalDir "ProjectReport.pdf"
  $out = Join-Path $FinalDir "FinalDocument.pdf"

  foreach ($f in @($pd, $ps, $pr)) {
    if (-not (Test-Path $f)) { throw "Missing input for combined PDF: $f. Please build 'All' first." }
  }
    
  qpdf --empty --pages $pd 1-z $ps 1-z $pr 1-z -- $out
  Log-Success "Combined PDF created at: $out"
}

# ==========================================
# 7. EXECUTION
# ==========================================

Write-Host "`n$E_Rocket STARTING BUILD: $Target`n" -ForegroundColor Magenta

switch ($Target) {
  "ProjectDescription" { Build-ProjectDescription }
  "ProcessReport" { Build-ProcessReport }
  "ProjectReport" { Build-ProjectReport }
  "Combined" { Build-Combined }
  "Generic" { Build-Generic }
  "All" {
    Build-ProjectDescription
    Build-ProcessReport
    Build-ProjectReport
    Build-Combined
  }
}

Write-Host "`n$E_Check DONE`n" -ForegroundColor Green