Param(
    [Parameter(Mandatory=$true, Position=0)]
    [string] $command,

    [Parameter(Mandatory=$true, Position=1)]
    [string] $moduleAssemblyFile,
    
    [Parameter(Mandatory=$false, Position=2)]
    [string] $helpFile
)

$dotnet = Get-Command 'dotnet'

$dotnetReptileAssembly = Join-Path $PSScriptRoot 'dotnet-reptile.dll'

$args = @($command, $moduleAssemblyFile)
If ($helpFile) {
    $args += $helpFile
}

Invoke-Command $dotnet -ArgumentList $args
