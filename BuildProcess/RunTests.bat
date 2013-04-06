set msBuildPath=C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727

"%msBuildPath%\MSBuild.exe" RunTests.msbuild /p:basedir=%cd%\.. /l:FileLogger,Microsoft.Build.Engine;logfile=RunTests.log

Pause
