@echo off
echo Batch file name: %0
echo Number of arguments: %#
echo All arguments: %*
echo.

echo Listing arguments one by one:
setlocal enabledelayedexpansion
set counter=1

:loop
if "%~1"=="" goto end
echo Argument !counter!: %1
shift
set /a counter+=1
goto loop

:end
echo.
