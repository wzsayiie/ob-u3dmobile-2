@echo off

setlocal

cd /d %~dp0\..\..

set _build_date=%date:~0,4%-%date:~5,2%-%date:~8,2%
set _build_time=%time:~3,2%-%time:~6,2%-%time:~9,2%

endlocal
