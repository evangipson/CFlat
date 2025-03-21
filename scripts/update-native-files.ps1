function Run-DotNetPublish {
    param([Parameter(Mandatory=$true)] [string] $ProjectPath)

    dotnet publish $ProjectPath -r win-x64
    if ($LASTEXITCODE -ne 0) {
        Write-Error "$(Split-Path $ProjectPath -Leaf) publish failed."
        exit $LASTEXITCODE
    }
}

function Copy-NativeFiles {
    param([Parameter(Mandatory=$true)] [string] $SourceDirectory, [Parameter(Mandatory=$true)] [string] $TargetDirectory)
    
    if (Test-Path $SourceDirectory) {
        Get-ChildItem -Path $SourceDirectory -Filter "*.dll" | Copy-Item -Destination $TargetDirectory -Force
        Get-ChildItem -Path $SourceDirectory -Filter "*.lib" | Copy-Item -Destination $TargetDirectory -Force
    } else {
        Write-Error "$(Split-Path $SourceDirectory -Leaf) native directory not found: $SourceDirectory"
        exit 1
    }
}

function Update-NativeFiles {
    # Navigate to the root of the repository
    $repoRoot = Split-Path -Parent $PSScriptRoot

    # Define the source and destination paths
    $coreProjectPath = Join-Path $repoRoot "src\CFlat.Core"
    $applicationProjectPath = Join-Path $repoRoot "src\CFlat.Application"
    $coreNativePath = Join-Path $coreProjectPath "bin\Release\net9.0\win-x64\native"
    $applicationNativePath = Join-Path $applicationProjectPath "bin\Release\net9.0\win-x64\native"
    $exampleLibPath = Join-Path $repoRoot "src\CFlat.Example\lib"

    # Generate the native files with dotnet publish
    Run-DotNetPublish -ProjectPath $coreProjectPath
    Run-DotNetPublish -ProjectPath $applicationProjectPath

    # Copy the native files into the C++ project
    Copy-NativeFiles -SourceDirectory $coreNativePath -TargetDirectory $exampleLibPath
    Copy-NativeFiles -SourceDirectory $applicationNativePath -TargetDirectory $exampleLibPath

    Write-Host "Script completed successfully."
}

Update-NativeFiles