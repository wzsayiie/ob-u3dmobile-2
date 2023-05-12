@echo off

setlocal

cd /d %~dp0\..

set _branch=%1
set _action=%2

::1 checkout the specified branch:
where git
if not "%errorlevel%"=="0" (
    echo no found git
    exit /b 1
)

git checkout .
git clean -fd

git fetch
if not "%errorlevel%"=="0" (
    ::may not be able to access the repository.
    echo the repository fetch failed
    exit /b 1
)

if "%_branch%"=="" (
    echo no input branch
    exit /b 1
)

git checkout %_branch%
if not "%errorlevel%"=="0" (
    echo unknown branch "%_branch%"
    exit /b 1
)

git pull

::2 call action:
if "%_action%"=="" (
    echo no input batch file
    exit /b 1
)

if not exist "%_action%" (
    echo unknown batch file "%_action%"
    exit /b 1
)

call "%_action%"
if not "%errorlevel%"=="0" (
    echo "%_action%" return error
    exit /b 1
)

::3 finish.
exit /b 0

endlocal
