@echo off

::this file records the environment variables needed by command line tools.
::if necessary, to execute this file to get these parameters.

::set "android" or "ios".
set _target_platform=android

::if build for android, options are "apk", "aab" and "bundle",
::else for ios, options are "ipa" and "bundle".
::"bundle" means "asset bundles".
set _target_product=apk

::installation package signing key files.
set  _apk_keystore=master
set _ipa_provision=master

::application informations.
set  _app_package_id=com.enterprise.game
set _app_version_str=1.0.0
set _app_version_num=1

::GameSettings parameters.
set  _package_serial=1
set  _first_language=chinese
set   _store_channel=googleplay
set _channel_gateway=googleplay
set       _asset_url=local
set       _patch_url=local
set   _asset_flavors=normal
set      _user_flags=UID:12;GM:true

::BuildSettings parameters.
set   _bundle_serial=1
set   _force_rebuild=false
set _use_past_bundle=true
set   _current_carry=full
