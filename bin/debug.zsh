#!/bin/zsh

#this script records the environment variables needed by shell tools.
#if necessary, to "source" this script to get these parameters.

#set "android" or "ios".
export _target_platform="ios"

#if build for android, options are "apk", "aab" and "bundle",
#else for ios, options are "ipa" and "bundle".
#"bundle" means "asset bundles".
export _target_product="ipa"

#installation package signing key files.
export  _apk_keystore="master"
export _ipa_provision="master"

#application informations.
export  _app_package_id="com.enterprise.game"
export _app_version_str="1.0.0"
export _app_version_num=1

#GameSettings parameters.
export  _package_serial=1
export  _first_language=chinese
export   _store_channel="appstore"
export _channel_gateway="appstore"
export       _asset_url="local"
export       _patch_url="local"
export   _asset_flavors="normal"
export      _user_flags="UID:12;GM:true"

#BuildSettings parameters.
export   _bundle_serial=1
export   _force_rebuild=false
export _use_past_bundle=true
export   _current_carry="full"
