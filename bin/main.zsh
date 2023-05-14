#!/bin/zsh

cd `dirname %0`/..

#requied environment variables:
echo _target_branch: $_target_branch
echo _action_script: $_action_script

if [ ! $_target_branch ]; then
    echo no input branch
    exit 1
fi

if [ ! $_action_script ]; then
    echo no input script
    exit 1
fi

#checkout the branch:
which git
if [ $? -ne 0 ]; then
    echo no found git
    exit 1
fi

git checkout .
git clean -fd

git fetch
if [ $? -ne 0 ]; then
    #may not be able to access the repository.
    echo the repository fetch failed
    exit 1
fi

git checkout $_target_branch
if [ $? -ne 0 ]; then
    echo unknown branch "$_target_branch"
    exit 1
fi

#on specialized build machines,
#consider "git reset --hard origin/$_target_branch".
git pull

#call the script:
if [ ! -f "$_action_script" ]; then
    echo unknown script "$_action_script"
    exit 1
fi

sh "$_action_script"
if [ $? -ne 0 ]; then
    echo "$_action_script" return error
    exit 1
fi

exit 0
