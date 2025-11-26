Write-Host "Starting DataServer (Java)..." -ForegroundColor Cyan
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd 'System/DataServer/DataServer'; mvn exec:java"

Write-Host "Starting LogicServer (.NET)..." -ForegroundColor Cyan
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd 'System/LogicServer/RESTAPI'; dotnet watch run"

Write-Host "Starting ClientApp (Blazor)..." -ForegroundColor Cyan
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd 'System/ClientApp/BlazorApp'; dotnet watch run"

Write-Host "All services attempted. Check the new windows for output." -ForegroundColor Green