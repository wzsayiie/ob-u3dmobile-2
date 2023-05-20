#!/bin/zsh

#this script records the environment variables needed by build tools.
#if building manually, remember to "source" this script to get these parameters.

#set "android" or "ios".
export _target_platform="android"

#if build for android, options are "apk", "aab" and "abs",
#else for ios, options are "ipa" and "abs".
#"abs" means "asset bundles".
export _target_product="apk"

#installation package signing key files.
export  _apk_keystore="master"
export _ipa_provision="master"

#application informations.
export  _app_package_id="me.wzsayiie.u3dmobile"
export _app_version_str="1.0.0"
export _app_version_num=1

#GameSettings parameters.
export   _package_serial=1
export    _store_channel="googleplay"
export  _channel_gateway="googleplay"
export _forced_asset_url="https://xx.com"
export _forced_patch_url="https://xx.com"
export     _asset_flavor="master"
export       _user_flags="UID:1;GM:true"

#BuildSettings parameters.
export _bundle_serial=1
export  _carry_option="none"
