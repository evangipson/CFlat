# The root folder for the project, used in multiple functions
$projectRootPath = Split-Path -Parent $PSScriptRoot

# Runs `dotnet publish` for the project defined by `$ProjectPath`
function Run-DotNetPublish([string] $ProjectPath)
{
    dotnet publish $ProjectPath -r win-x64

    if ($LASTEXITCODE -ne 0)
    {
        Write-Error "$(Split-Path $ProjectPath -Leaf) publish failed."
        exit $LASTEXITCODE
    }
}

# Copies all native files from the `$SourceDirectory` to the `$TargetDirectory`
function Copy-NativeFiles([string] $SourceDirectory, [string] $TargetDirectory)
{
    if (!(Test-Path $SourceDirectory))
    {
        Write-Error "$(Split-Path $SourceDirectory -Leaf) native directory not found: $SourceDirectory"
        exit 1
    }

    if (!(Test-Path $TargetDirectory))
    {
        Write-Error "$(Split-Path $TargetDirectory -Leaf) target directory not found: $TargetDirectory"
        exit 1
    }

    Get-ChildItem -Path $SourceDirectory -Filter "*.dll" | Copy-Item -Destination $TargetDirectory -Force
    Get-ChildItem -Path $SourceDirectory -Filter "*.lib" | Copy-Item -Destination $TargetDirectory -Force
}

# Uses `Run-DotNetPublish` and `Copy-NativeFiles` to deploy expected dependencies to the CFlat.example library path
function Update-NativeFiles
{
    $coreProjectPath = Join-Path $projectRootPath "src\CFlat.Core"
    $applicationProjectPath = Join-Path $projectRootPath "src\CFlat.Application"
    $coreNativePath = Join-Path $coreProjectPath "bin\Release\net9.0\win-x64\native"
    $applicationNativePath = Join-Path $applicationProjectPath "bin\Release\net9.0\win-x64\native"
    $exampleLibPath = Join-Path $projectRootPath "src\CFlat.Example\lib"

    Run-DotNetPublish -ProjectPath $coreProjectPath
    Run-DotNetPublish -ProjectPath $applicationProjectPath

    Copy-NativeFiles -SourceDirectory $coreNativePath -TargetDirectory $exampleLibPath
    Copy-NativeFiles -SourceDirectory $applicationNativePath -TargetDirectory $exampleLibPath
}

# Runs all necessary functions to deploy the CFlat application
function Deploy-CFlat
{
    Write-Host "CFlat deploy starting."

    Update-NativeFiles

    Write-Host "CFlat deploy successful."
}

Deploy-CFlat