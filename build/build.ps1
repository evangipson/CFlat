# Publishes a project's libraries to a destination path
function Publish-LibraryToProject([string] $SolutionRoot, [string] $Project, [string] $Destination)
{
    $dotnetPublishCommand = "dotnet publish $Project -c Release --use-current-runtime --property:PublishDir=$Destination"
    Start-Process powershell.exe -Wait -WorkingDirectory $SolutionRoot -ArgumentList "-Command", $dotnetPublishCommand
}

workflow Deploy-CFlat([string] $ScriptRoot)
{
    parallel
    {
        Publish-LibraryToProject -Project "src\Application\src\Core" -Destination "..\..\..\Shared\src\Native\bin\Release\net9.0" -SolutionRoot $ScriptRoot
        Publish-LibraryToProject -Project "src\Application\src\Application" -Destination "..\..\..\Shared\src\Native\bin\Release\net9.0" -SolutionRoot $ScriptRoot
    }

    Publish-LibraryToProject -Project "src\Shared\src\Native" -Destination "..\..\..\..\src\Examples\cpp\lib" -SolutionRoot $ScriptRoot
}

Deploy-CFlat -ScriptRoot (Split-Path -Parent $PSScriptRoot)