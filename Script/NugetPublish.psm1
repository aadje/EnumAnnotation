function Start-MygetPublish{
 <#
   .Synopsis
    Searches recursively for a single Nuspec Xml file
   .Description
    And pushed
   .Example
    Set-PackageSemVer
   .Parameter 
    MygetApiKey 
    Source
   .Notes
    NAME:  Start-MygetPublish
    AUTHOR: Arjan van Rijn
 #>
    param(
        [string] $MygetApiKey = $MygetApiKey,
        [string] $Source = "http://www.myget.org/F/2efd78dad5f24a71b64b69e807a8b1de/api/v2/package"
    )
	
    if(!$MygetApiKey) { throw "MygetApiKey not found. Set global in variable:MygetApiKey " }
	$nuget = (Get-ChildItem -Path .\packages\NuGet.CommandLine*.*\tools\NuGet.exe).ToString()
    
    Remove-Item -Path .\PublishPackage\content -Recurse
    Copy-Item -Path .\MontfoortIT.EnumAnnotation\ComponentModel -Destination .\PublishPackage\content\ComponentModel -Recurse
    Update-NugetNamespace -Path .\PublishPackage -RootNameSpace MontfoortIT.EnumAnnotation

    $spec = Get-ChildItem -Path .\PublishPackage -Recurse -Filter *.nuspec
    Set-PackageSemVer -Path .\PublishPackage
    & $nuget pack $spec.FullName
    $package = Get-ChildItem *.nupkg | Sort-Object CreationTime | Select-Object -Last 1
    & $nuget push $package $MygetApiKey -Source $Source
}

function Update-NugetNamespace{
 <#
   .Synopsis
    Recusivly Search for .cs file below the specified Path, and Update the files with $rootnamespace$ voor the rootnamespace
 #>
    param(
        [string] $RootNamespace,
        [string] $Path = $PWD
    )
	
	Get-ChildItem -Path $Path -Recurse -Filter *.cs | ForEach-Object {
        $file = Get-Content $_.FullName
        $file | ForEach-Object {
            $_.Replace($RootNamespace, '$rootnamespace$')
        } | 
        Set-Content $_.FullName
        Rename-Item -Path $_.FullName -NewName "$($_.Name).pp"
    }
}

function Set-PackageSemVer{
 <#
   .Synopsis
    Updates the semantic version number in the Nuspec Xml file
   .Description
    Updates the Semantic version #Major.Minor.Patch
   .Example
    Set-PackageSemVer
   .Parameter 
    No parameters
   .Notes
    NAME:  Set-PackageSemVer
    AUTHOR: Arjan van Rijn
 #>

    param(
        [string] $Path = $PWD
    )

    $spec = Get-ChildItem -Path $Path -Recurse -Filter *.nuspec
    $xml = [xml](get-content $spec.FullName)
    $package = $xml.get_DocumentElement()
    
    $versionArray = $package.metadata.version.Split('.')
    if($versionArray.Length -ne 3){ throw "Package has no semantic version" }

    $patchVersion = [int]$versionArray[2]
    $versionArray[2] = ++$patchVersion

    $package.metadata.version = [string]::Join('.', $versionArray)
    
    $xml.Save((Resolve-Path $spec.FullName))
}

Set-Alias -Name publish -Value Start-MygetPublish
Export-ModuleMember -alias * -function * -variable *