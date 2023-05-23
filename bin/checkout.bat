@echo off

setlocal

cd /d %~dp0\..

::required environment variables:
echo _target_branch: %_target_branch%
echo _action_script: %_action_script%

if "%_target_branch%"=="" (
    echo no input branch
    exit /b 1
)

if "%_action_script%"=="" (
    echo no input batch file
    exit /b 1
)

::checkout the branch:
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

git checkout %_target_branch%
if not "%errorlevel%"=="0" (
    echo unknown branch "%_target_branch%"
    exit /b 1
)

::on specialized build machines,
::consider "git reset --hard origin/$_target_branch".
git pull

::call the script:
if not exist "%_action_script%" (
    echo unknown batch file "%_action_script%"
    exit /b 1
)

call "%_action_script%"
if not "%errorlevel%"=="0" (
    echo "%_action_script%" returns error
    exit /b 1
)

exit /b 0

endlocal
