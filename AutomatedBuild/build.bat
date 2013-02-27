@echo off
set zipcl="c:/Program Files/7-zip/7z.exe"
set sourcePath=%1
streams -s -d %sourcePath%
copy *.vsix %sourcePath%
pushd %sourcePath%
md SourceCode\Studio\Binaries
move /Y *.vsix SourceCode\Studio\Binaries
cd SourceCode\Studio

msbuild Studio.VS2010.sln /t:rebuild /p:configuration=Debug-VS2010
cd binaries
%zipcl% x -otmpvsix nservicebusstudio.10.0.vsix *.vsix
rename nservicebusstudio.10.0.vsix nservicebusstudio.10.0.vsix.zip
%zipcl% d nservicebusstudio.10.0.vsix.zip *.vsix
cd tmpvsix
ren *.vsix *.zip
%zipcl% d NuPatternToolkitManager.10.0.zip "GeneratedCode\Guidance\Content\*.*"
ren *.zip *.vsix
%zipcl% a "..\nservicebusstudio.10.0.vsix.zip" "*.vsix"
del *.* /Q
cd ..
rd tmpvsix
rename nservicebusstudio.10.0.vsix.zip NServiceBusStudio.10.0.vsix

cd ..
msbuild Studio.VS2012.sln /t:rebuild /p:configuration=Debug-VS2012
cd binaries
%zipcl% x -otmpvsix nservicebusstudio.11.0.vsix *.vsix
rename nservicebusstudio.11.0.vsix nservicebusstudio.11.0.vsix.zip
%zipcl% d nservicebusstudio.11.0.vsix.zip *.vsix
cd tmpvsix
ren *.vsix *.zip
%zipcl% d NuPatternToolkitManager.11.0.zip "GeneratedCode\Guidance\Content\*.*"
ren *.zip *.vsix
%zipcl% a "..\nservicebusstudio.11.0.vsix.zip" "*.vsix"
del *.* /Q
cd ..
rd tmpvsix
rename nservicebusstudio.11.0.vsix.zip NServiceBusStudio.11.0.vsix

popd