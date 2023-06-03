#!/bin/zsh

cd `dirname $0`/..

declare _excutable_js=bin/csv/BUILD/csv.js

which node
if [ $? -ne 0 ]; then
    echo not found node.js
    exit 1
fi

if [ ! -f "$_excutable_js" ]; then
    echo not found executable
    exit 1
fi

node --enable-source-maps "$_excutable_js"
if [ $? -ne 0 ]; then
    echo the executable returns error
    exit 1
fi

exit 0
