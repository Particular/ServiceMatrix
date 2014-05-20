$packageName = "ServiceMatrix.VS2013"

$url = gci -path "c:\ChocolateyResourceCache" -Filter "Particular.ServiceMatrix.12.0.vsix" -ErrorAction SilentlyContinue | select -first 1

if($url){
	$url = $url | Select -expandProperty FullName
}
else{
	$url = "https://github.com/Particular/ServiceMatrix/releases/download/{{ReleaseName}}/Particular.ServiceMatrix.12.0.vsix"
}


try {

    $chocTempDir = Join-Path $env:TEMP "chocolatey"

    $tempDir = Join-Path $chocTempDir "$packageName"
    
    if (![System.IO.Directory]::Exists($tempDir)) {[System.IO.Directory]::CreateDirectory($tempDir) | Out-Null}
    
    $vsixOnLocalDisk = Join-Path $tempDir "Particular.ServiceMatrix.12.0.vsix"

    Get-ChocolateyWebFile $packageName $vsixOnLocalDisk $url
  
    $vs2013ToolsPath = $Env:VS120COMNTOOLS

    Write-Host "VS2013 Tools Path: $vs2013ToolsPath"

    if($vs2013ToolsPath -eq $null)
    {
    	throw "Visual Studio 2013 not found on this machine"
    }

	$vs2013Dir = New-Object System.IO.DirectoryInfo($vs2013ToolsPath)
 	$pathToVsixInstaller = [io.path]::Combine($vs2013Dir.Parent.FullName, "IDE")
 	$pathToVsixInstaller = [io.path]::Combine($pathToVsixInstaller, "VSIXInstaller.exe")
 	
 	Write-Host "Path to VsixInstaller: $pathToVsixInstaller"


	$arguments  ="`"$vsixOnLocalDisk`" /quiet"
	
	Write-Host "Invoking vsix installer with arguments: $arguments"
    
	$validExitCodes = @(0,1001)  #1001 means extention already installed

    Start-ChocolateyProcessAsAdmin "$arguments" "$pathToVsixInstaller" -validExitCodes $validExitCodes

    Write-ChocolateySuccess $packageName
    Remove-Item $pathToVsixInstaller -ErrorAction SilentlyContinue 
} catch {
	Write-ChocolateyFailure $packageName $($_.Exception.Message)
	throw
}
