$packageName = "ServiceMatrix.VS2013"

try {

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


	$arguments  ="/uninstall:EE4496BE-1C92-42DB-B5FA-FB1B3AA306D0 /quiet"
	
	Write-Host "Invoking vsix installer with arguments: $arguments";
    
    Start-ChocolateyProcessAsAdmin "$arguments" "$pathToVsixInstaller" -validExitCodes 0

    Write-ChocolateySuccess $packageName
} catch {
	Write-ChocolateyFailure $packageName $($_.Exception.Message)
	throw
}
