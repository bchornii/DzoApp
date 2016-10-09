echo Start building installer package
@echo off
cls
cd %pathMSBuild%
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild.exe /t:Harvest;WIX setup.build
echo Installer package built
pause