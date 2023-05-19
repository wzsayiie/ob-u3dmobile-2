@echo off

setlocal

cd /d %~dp0\..

::required environment variable:
echo _store_channel     : %_store_channel%
echo _package_identifier: %_package_identifier%
echo _bundle_identifier : %_bundle_identifier%

if "%_store_channel%"=="" (
    echo no input store channel
    exit /b 1
)
if "%_package_identifier%"=="" (
    echo no input package identifier
    exit /b 1
)
if "%_bundle_identifier%"=="" (
    echo no input bundle identifier
    exit /b 1
)

set _build_date=%date:~0,4%-%date:~5,2%-%date:~8,2%
set _build_time=%time:~0,2%-%time:~3,2%-%time:~6,2%
set _time_stamp=%_build_date%_%_build_time%

::NOTE: unity will use "_output_dir" as output directory.
set _output_dir=BUILD\%_time_stamp%_%_store_channel%_%_package_identifier%_%_bundle_identifier%
set  _unity_log=%_output_dir%\log.txt

"C:\Program Files\Unity\Hub\Editor\2021.3.22f1c1\Editor\Unity.exe" ^
    -executeMethod U3DMobileEditor.BuildProcess.Launch ^
    -logFile %_unity_log% ^
    -batchmode ^
    -quit

if not "%errorlevel%"=="0" (
    echo unity returns error %errorlevel%
    exit /b 1
)

exit /b 0

endlocal
