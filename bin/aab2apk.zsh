#!/bin/zsh

declare _apk_file=$1
if [ ! $_apk_file ]; then
    echo no input
    exit 1
fi
if [ ! -f "$_apk_file" ]; then
    echo not found "$_apk_file"
    exit 1
fi

#check required tools:
which java
if [ $? -ne 0 ]; then
    echo not found java
    exit 1
fi

which unzip
if [ $? -ne 0 ]; then
    echo not found unzip
    exit 1
fi

which touch
if [ $? -ne 0 ]; then
    echo not found touch
    exit 1
fi

#IMPORTANT: users need to setup "bundletool" and the key.
declare _jar_name=bundletool-all.jar
declare _jks_name=master

declare _bin_directory=$(dirname `realpath "$0"`)
declare _exec_jar_file=$_bin_directory/$_jar_name
declare _jks_save_file=$_bin_directory/../keystores/android/$_jks_name.jks
declare _jks_pass_file=$_bin_directory/../keystores/android/$_jks_name.jkspass.txt
declare _key_name_file=$_bin_directory/../keystores/android/$_jks_name.key.txt
declare _key_pass_file=$_bin_directory/../keystores/android/$_jks_name.keypass.txt

if [ ! -f "$_exec_jar_file" ]; then echo not found "$_exec_jar_file"; exit 1; fi
if [ ! -f "$_jks_save_file" ]; then echo not found "$_jks_save_file"; exit 1; fi
if [ ! -f "$_jks_pass_file" ]; then echo not found "$_jks_pass_file"; exit 1; fi
if [ ! -f "$_key_name_file" ]; then echo not found "$_key_name_file"; exit 1; fi
if [ ! -f "$_key_pass_file" ]; then echo not found "$_key_pass_file"; exit 1; fi

declare _jks_pass=$(cat "$_jks_pass_file")
declare _key_name=$(cat "$_key_name_file")
declare _key_pass=$(cat "$_key_pass_file")

echo "jks store   : $_jks_save_file"
echo "jks password: $_jks_pass"
echo "key name    : $_key_name"
echo "key password: $_key_pass"

#goto destination directory and clean:
declare _out_dir=$(dirname  "$_apk_file")
declare _in_base=$(basename "$_apk_file")
declare _out_nam=${_in_base%.*}

cd "$_out_dir"

rm -f "$_out_nam.apks"
rm -f "universal.apk"
rm -f "$_out_nam.apk"

#export apks:
java -jar "$_exec_jar_file" build-apks                      \
    --mode=universal                                        \
    --overwrite                                             \
    --ks="$_jks_save_file"      --ks-pass=pass:$_jks_pass   \
    --ks-key-alias=$_key_name  --key-pass=pass:$_key_pass   \
    --bundle="$_out_nam.aab"                                \
    --output="$_out_nam.apks"

if [ $? -ne 0 ]; then
    echo export apks failed
    exit 1
fi

#extract the needed apk.
unzip "$_out_nam.apks" "universal.apk"
mv    "universal.apk"  "$_out_nam.apk"
touch "$_out_nam.apk"
rm -f "$_out_nam.apks"

exit 0
