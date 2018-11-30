@echo off

set ans=Altiris.NS
set adb=Altiris.Database
set acm=Altiris.Common
set asi=Altiris.NS.StandardItems
set ars=Altiris.Resource

if "%1"=="8.5" goto build-8.5
if "%1"=="8.1" goto build-8.1
if "%1"=="8.0" goto build-8.0
if "%1"=="7.6" goto build-7.6
if "%1"=="7.1" goto build-7.1

:default build path
:build-8.5
set build=8.5

set gac=C:\Windows\Microsoft.NET\assembly\GAC_MSIL
set csc=@c:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe

set ver1=v4.0_8.5.3073.0__d516cb311cfb6e4f
set ver2=v4.0_8.5.3073.0__d516cb311cfb6e4f
set ver3=v4.0_8.5.3073.0__99b1e4cc0d03f223

goto build


:build-8.1
set build=8.1

set gac=C:\Windows\Microsoft.NET\assembly\GAC_MSIL
set csc=@c:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe

set ver1=v4.0_8.1.4528.0__d516cb311cfb6e4f
set ver2=v4.0_8.1.4528.0__d516cb311cfb6e4f
set ver3=v4.0_8.1.4528.0__99b1e4cc0d03f223

goto build


:build-8.0
set build=8.0

set gac=C:\Windows\Microsoft.NET\assembly\GAC_MSIL
set csc=@c:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe

set ver1=v4.0_8.0.2298.0__d516cb311cfb6e4f
set ver2=v4.0_8.0.2298.0__d516cb311cfb6e4f
set ver3=v4.0_8.0.2298.0__99b1e4cc0d03f223

goto build


:build-7.6
set build=7.6

set gac=C:\Windows\Microsoft.NET\assembly\GAC_MSIL
set csc=@c:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe

set ver1=v4.0_7.6.1383.0__d516cb311cfb6e4f
set ver2=v4.0_7.6.1395.0__d516cb311cfb6e4f
set ver3=v4.0_7.6.1383.0__99b1e4cc0d03f223

goto build


:build-7.5
set build=7.5
set gac=C:\Windows\Assembly\GAC_MSIL
set csc=@c:\Windows\Microsoft.NET\Framework\v2.0.50727\csc.exe

set ver1=7.5.3153.0__d516cb311cfb6e4f
set ver2=7.5.3219.0__d516cb311cfb6e4f
set ver3=7.5.3153.0__99b1e4cc0d03f223

goto build


:build-7.1
set build=7.1

set gac=C:\Windows\Assembly\GAC_MSIL
set csc=@c:\Windows\Microsoft.NET\Framework\v2.0.50727\csc.exe

set ver1=7.1.8400.0__d516cb311cfb6e4f
set ver2=7.5.3219.0__d516cb311cfb6e4f
set ver3=7.5.3153.0__99b1e4cc0d03f223

goto build


:build
cmd /c %csc% /reference:%gac%\%adb%\%ver1%\%adb%.dll /reference:%gac%\%ans%\%ver1%\%ans%.dll /reference:%gac%\%asi%\%ver1%\%asi%.dll /reference:%gac%\%acm%\%ver1%\%acm%.dll /reference:%gac%\%ars%\%ver1%\%ars%.dll /win32icon:altiris.ico /out:SWDExecTrending-%build%.exe *.cs

:end