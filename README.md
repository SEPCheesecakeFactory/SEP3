# Description

This repository contains a project developed for the Semester Project 3 (SEP3) course at VIA University College. The project is a collaborative effort by a team of five members.

# Team Members

- Guillermo Sanchez Martinez (355442)
- Piotr Wiktor Junosz (355502)
- Halil Ibrahim Aygun (355770)
- Alexandru Savin (354790)
- Eduard Fekete (355323)

# Timeline

**8th September 2025** - Project proposal submission

**15th-ish September 2025** - Project kickoff

**1st-20th December 2025** - Project submission

# Requirements

## Version Control

- Git must be used
- the project must be under version control from start to finish
- hand-in must include a link to a GitHub repository

## The System

- must be a distributed system
- must be heterogeneous
- must have two servers
- must have server to server communication
- must use at least two different network technologies (REST, RabbitMQ, gRPC, Websockets, SignalR, GraphQL, etc.)
- must have at least one database (Postgres, MySQL, MongoDB, EFC, etc.)

# Examination

20 min group presentation of the project

20 min / per person group examination (including evaluation)

# Tools

The following tools are needed for working on the project:

- IDE (Visual Studio, IntelliJ, etc.) - VS Code is recommended
- Markdown Preview extension of choice
- PlantUML Preview extension of choice
- PDF Preview extension of choice
- Pandoc
- MikTeX
- Graphviz
- PlantUML
- Dotnet SDK

# Setup

Make sure to have [Chocolatey](https://chocolatey.org/install) installed, then run the following commands in an elevated (admin) PowerShell/CMD terminal:

```powershell
choco install pandoc
choco install miktex
choco install graphviz
choco install openjdk

choco install qpdf -y
```

Verify your installations (except QPDF) by running the following commands:

```powershell
pandoc -v
pdflatex --version
dot -V
java -jar plantuml.jar -version
```

Then, install the following VSCode extensions:
- Markdown Preview Enhanced
- PlantUML

# Building the documentation

Make sure that you are in the Documentation directory, probably you need to run
```powershell
cd Documentation
```

## Project Description PDF

```powershell
  pandoc --metadata-file=Styles\ptd-meta.yaml ProjectDescription.md `
  --include-before-body=Styles\ptd.tex `
  --include-before-body=Styles\toc.tex `
  --include-in-header=Styles\ptd-hdr-ftr.tex `
  -V geometry:margin=25mm `
  --pdf-engine=pdflatex `
  --number-sections `
  -o Final\ProjectDescription.pdf
  ```

## Process Report PDF

```powershell
  pandoc --metadata-file=Styles\psr-meta.yaml ProcessReport.md `
  --include-before-body=Styles\psr.tex `
  --include-before-body=Styles\toc.tex `
  --include-in-header=Styles\psr-hdr-ftr.tex `
  -V geometry:margin=25mm `
  --pdf-engine=pdflatex `
  --number-sections `
  -o Final\ProcessReport.pdf
  ```
  
## Project Report PDF

```powershell
  pandoc --metadata-file=Styles\ptr-meta.yaml ProjectReport.md `
  --include-before-body=Styles\ptr.tex `
  --include-before-body=Styles\toc.tex `
  --include-in-header=Styles\ptr-hdr-ftr.tex `
  -V geometry:margin=25mm `
  --pdf-engine=pdflatex `
  --number-sections `
  -o Final\ProjectReport.pdf
  ```

## Final Combined PDF

Make sure to export the documents individually before, and then run:

```powershell
  qpdf --empty --pages Final\ProjectDescription.pdf 1-z Final\ProcessReport.pdf 1-z Final\ProjectReport.pdf 1-z -- Final\FinalDocument.pdf
  ```

## Full Source Code ZIP

For this one, don't be inside the Documentation directory, but rather in the root of the repository, then run:

```powershell
$zip = "$PWD\$(Split-Path -Leaf $PWD).zip"; if (Test-Path $zip) {Remove-Item $zip -Force}; Compress-Archive * -DestinationPath $zip
  ```

## General Document PDF

Include --toc if needed for table of contents

```powershell
  pandoc <doc-name>.md `
  --include-in-header=Styles\general-hdr-ftr.tex `
  -V geometry:margin=25mm `
  --pdf-engine=pdflatex `
  --number-sections `
  -o Final\<doc-name>.pdf
  ```