#!/bin/zsh

cd `dirname $0`/..

#required environment variable:
echo "_store_channel : $_store_channel"
echo "_package_serial: $_package_serial"
echo "_bundle_serial : $_bundle_serial"

if [ ! $_store_channel ]; then
    echo no input store channel
    exit 1
fi
if [ ! $_package_serial ]; then
    echo no input package serial
    exit 1
fi
if [ ! $_bundle_serial ]; then
    echo no input bundle serial
    exit 1
fi

declare _time_stamp=`date "+%Y-%m-%d_%H-%M-%S"`

#NOTE: unity will use "_output_dir" as output directory.
export  _output_dir=BUILD/${_time_stamp}_${_store_channel}_${_package_serial}_${_bundle_serial}
declare _unity_logs=$_output_dir/log.txt

"/Applications/Unity/Hub/Editor/2021.3.22f1c1/Unity.app/Contents/MacOS/Unity"   \
    -projectPath   .                                                            \
    -executeMethod U3DMobileEditor.BuildProcess.Launch                          \
    -logFile       $_unity_logs                                                 \
    -batchmode                                                                  \
    -quit

cat $_unity_logs

if [ $? -ne 0 ]; then
    echo unity returns error
    exit 1
fi

exit 0
