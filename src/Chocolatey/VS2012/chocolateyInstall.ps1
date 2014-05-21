$packageName = "ServiceMatrix.VS2012"

$url = gci -path "c:\ChocolateyResourceCache" -Filter "Particular.ServiceMatrix.11.0.vsix" -ErrorAction SilentlyContinue | select -first 1

if($url){
	$url = $url | Select -expandProperty FullName
}
else{
	$url = "https://github.com/Particular/ServiceMatrix/releases/download/{{ReleaseName}}/Particular.ServiceMatrix.11.0.vsix"
}


try {

    $chocTempDir = Join-Path $env:TEMP "chocolatey"

    $tempDir = Join-Path $chocTempDir "$packageName"
    
    if (![System.IO.Directory]::Exists($tempDir)) {[System.IO.Directory]::CreateDirectory($tempDir) | Out-Null}
    
    $vsixOnLocalDisk = Join-Path $tempDir "Particular.ServiceMatrix.11.0.vsix"

    Get-ChocolateyWebFile $packageName $vsixOnLocalDisk $url
  
    $vs2012ToolsPath = $Env:VS110COMNTOOLS

    Write-Host "VS2012 Tools Path: $vs2012ToolsPath"

    if($vs2012ToolsPath -eq $null)
    {
    	throw "Visual Studio 2012 not found on this machine"
    }

	$vs2012Dir = New-Object System.IO.DirectoryInfo($vs2012ToolsPath)
 	$pathToVsixInstaller = [io.path]::Combine($vs2012Dir.Parent.FullName, "IDE")
 	$pathToVsixInstaller = [io.path]::Combine($pathToVsixInstaller, "VSIXInstaller.exe")
 	
 	Write-Host "Path to VsixInstaller: $pathToVsixInstaller"


	$arguments  ="`"$vsixOnLocalDisk`" /quiet"
	
	Write-Host "Invoking vsix installer with arguments: $arguments"
    
	$validExitCodes = @(0,1001)  #1001 means extention already installed

    Start-ChocolateyProcessAsAdmin "$arguments" "$pathToVsixInstaller" -validExitCodes $validExitCodes

    Write-ChocolateySuccess $packageName
    Remove-Item $vsixOnLocalDisk -ErrorAction SilentlyContinue 
} catch {
	Write-ChocolateyFailure $packageName $($_.Exception.Message)
	throw
}
