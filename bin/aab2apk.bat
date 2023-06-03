@echo off

setlocal

set _apk_file=%1
if "%_apk_file%"=="" (
    echo no input
    exit /b 1
)
if not exist "%_apk_file%" (
    echo not found "%_apk_file%"
    exit /b 1
)

::check required tools:
where java
if not "%errorlevel%"=="0" (
    echo not found java
    exit /b 1
)

where unzip
if not "%errorlevel%"=="0" (
    echo not found unzip
    exit /b 1
)

where touch
if not "%errorlevel%"=="0" (
    echo not found touch
    exit /b 1
)

::IMPORTANT: users need to setup "bundletool" and the key.
set _jar_name=bundletool-all.jar
set _jks_name=master

set _exec_jar_file="%~dp0%_jar_name%"
set _jks_save_file="%~dp0..\keystores\android\%_jks_name%.jks"
set _jks_pass_file="%~dp0..\keystores\android\%_jks_name%.jkspass.txt"
set _key_name_file="%~dp0..\keystores\android\%_jks_name%.key.txt"
set _key_pass_file="%~dp0..\keystores\android\%_jks_name%.keypass.txt"

if not exist %_exec_jar_file% ( echo not found %_exec_jar_file% & exit /b 1 )
if not exist %_jks_save_file% ( echo not found %_jks_save_file% & exit /b 1 )
if not exist %_jks_pass_file% ( echo not found %_jks_pass_file% & exit /b 1 )
if not exist %_key_name_file% ( echo not found %_key_name_file% & exit /b 1 )
if not exist %_key_pass_file% ( echo not found %_key_pass_file% & exit /b 1 )

for /f %%i in ('type %_jks_pass_file%') do set _jks_pass=%%i
for /f %%i in ('type %_key_name_file%') do set _key_name=%%i
for /f %%i in ('type %_key_pass_file%') do set _key_pass=%%i

echo jks store   : %_jks_save_file%
echo jks password: %_jks_pass%
echo key name    : %_key_name%
echo key password: %_key_pass%

::goto destination directory and clean:
for %%i in ("%_apk_file%") do (
    set _wk_dir=%%~di%%~pi
    set _o_name=%%~ni
)

cd /d "%_wk_dir%"

if exist "%_o_name%.apks" ( del /q "%_o_name%.apks" )
if exist "universal.apk"  ( del /q "universal.apk"  )
if exist "%_o_name%.apk"  ( del /q "%_o_name%.apk"  )

::export apks:
java -jar %_exec_jar_file% build-apks                       ^
    --mode=universal                                        ^
    --overwrite                                             ^
    --ks=%_jks_save_file%       --ks-pass=pass:%_jks_pass%  ^
    --ks-key-alias=%_key_name% --key-pass=pass:%_key_pass%  ^
    --bundle="%_o_name%.aab"                                ^
    --output="%_o_name%.apks"

if not "%errorlevel%"=="0" (
    echo export apks failed
    exit /b 1
)

::extract the needed apk.
unzip  "%_o_name%.apks" "universal.apk"
rename "universal.apk"  "%_o_name%.apk"
touch  "%_o_name%.apk"
del /q "%_o_name%.apks"

exit /b 0

endlocal
