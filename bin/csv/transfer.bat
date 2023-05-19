@echo off

setlocal

cd %~dp0\..\..

set _excutable_js="bin\csv\BUILD\transfer.js"

where node
if not "%errorlevel%"=="0" (
    echo not found node.js
    exit /b 1
)

if not exist %_excutable_js% (
    echo not found executable
    exit /b 1
)

node --enable-source-maps %_excutable_js%
if not "%errorlevel%"=="0" (
    echo the executable return error %errorlevel%
    exit /b 1
)

exit /b 0

endlocal
