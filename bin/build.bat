@echo off

setlocal

cd /d %~dp0\..

::required environment variable:
echo _store_channel : %_store_channel%
echo _package_serial: %_package_serial%
echo _bundle_serial : %_bundle_serial%

if "%_store_channel%"=="" (
    echo no input store channel
    exit /b 1
)
if "%_package_serial%"=="" (
    echo no input package serial
    exit /b 1
)
if "%_bundle_serial%"=="" (
    echo no input bundle serial
    exit /b 1
)

set _build_date=%date:~0,4%-%date:~5,2%-%date:~8,2%
set _build_time=%time:~0,2%-%time:~3,2%-%time:~6,2%
set _time_stamp=%_build_date%_%_build_time%

::NOTE: unity will use "_output_dir" as output directory.
set _output_dir=BUILD\%_time_stamp%_%_store_channel%_%_package_serial%_%_bundle_serial%
set _unity_logs=%_output_dir%\log.txt

"C:\Program Files\Unity\Hub\Editor\2021.3.22f1c1\Editor\Unity.exe"  ^
    -projectPath   .                                                ^
    -executeMethod U3DMobileEditor.BuildProcess.Launch              ^
    -logFile       %_unity_logs%                                    ^
    -batchmode                                                      ^
    -quit

type %_unity_logs%

if not "%errorlevel%"=="0" (
    echo unity returns error
    exit /b 1
)

exit /b 0

endlocal
