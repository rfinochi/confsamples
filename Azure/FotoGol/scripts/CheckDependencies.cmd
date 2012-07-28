@echo off
setlocal
%~d0
cd "%~dp0"
Start .\..\..\..\Assets\DependencyChecker\ConfigurationWizard.exe Dependencies.xml
