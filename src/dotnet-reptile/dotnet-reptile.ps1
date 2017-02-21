Param(
    [Parameter(Mandatory=$true, Position=0)]
    [string] $moduleAssemblyFile,
    
    [Parameter(Mandatory=$false, Position=1)]
    [string] $helpFile
)

$dotnet = Get-Command 'dotnet'

$dotnetReptileAssembly = Join-Path $PSScriptRoot 'dotnet-reptile.dll'

If ($helpFile) {
    & $dotnet $dotnetReptileAssembly $moduleAssemblyFile $helpFile
} Else {
    & $dotnet $dotnetReptileAssembly $moduleAssemblyFile
}

