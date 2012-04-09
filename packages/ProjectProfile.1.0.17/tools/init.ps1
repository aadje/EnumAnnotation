param($installPath, $toolsPath, $package, $project)

function global:Load-ProjectProfile()
{		
    $projectName = $PWD.Path.SubString($PWD.Path.LastIndexOf('\') + 1)
	$profileName = "$($projectName)_profile.ps1"

	if(-not (Test-Path $profileName)) 
	{
		New-Item $profileName -ItemType file
	}
    
    $path = (Resolve-Path $profileName).Path
	. $path
}

Load-ProjectProfile