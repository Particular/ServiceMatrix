@echo off
set zipcl="c:/Program Files/7-zip/7z.exe"
set sourcePath=%1
streams -s -d %sourcePath%
copy *.vsix %sourcePath%
pushd %sourcePath%
md SourceCode\Studio\Binaries
move /Y *.vsix SourceCode\Studio\Binaries
cd SourceCode\Studio
msbuild /p:configuration=Debug
cd "Extensions\NServiceBusHost"
msbuild /p:configuration=Debug
cd "..\WebEndpoint"
msbuild /p:configuration=Debug
cd "..\WebMVCEndpoint"
msbuild /p:configuration=Debug
cd "..\.."
msbuild /p:configuration=Debug
cd binaries
%zipcl% x -otmpvsix nservicebusstudio.vsix *.vsix
rename nservicebusstudio.vsix nservicebusstudio.vsix.zip
%zipcl% d nservicebusstudio.vsix.zip *.vsix
cd tmpvsix
ren *.vsix *.zip
%zipcl% d NServiceBusHost.zip PatternToolkitManager.vsix
%zipcl% d WebEndpoint.zip PatternToolkitManager.vsix
%zipcl% d WebMVCEndpoint.zip PatternToolkitManager.vsix
%zipcl% d PatternToolkitManager.zip "GeneratedCode\Guidance\Content\*.*"
ren *.zip *.vsix
%zipcl% a "..\nservicebusstudio.vsix.zip" "*.vsix"
del *.* /Q
cd ..
rd tmpvsix
rename nservicebusstudio.vsix.zip NServiceBusStudio.vsix
popd