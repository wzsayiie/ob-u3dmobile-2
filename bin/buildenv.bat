@echo off

::this file records the environment variables needed by build tools.
::if building manually, remember to execute this file to get these parameters.

::set "android" or "ios".
set _target_platform=android

::if build for android, options are "apk", "aab" and "abs",
::else for ios, options are "ipa" and "abs".
::"abs" means "asset bundles".
set _target_product=apk

::installation package signing key files.
set  _apk_keystore=master
set _ipa_provision=master

::application informations.
set  _app_package_id=me.wzsayiie.u3dmobile
set _app_version_str=1.0.0
set _app_version_num=1

::GameSettings parameters.
set   _package_serial=1
set    _store_channel=googleplay
set  _channel_gateway=googleplay
set _forced_asset_url=https://xx.com
set _forced_patch_url=https://xx.com
set     _asset_flavor=master
set       _user_flags=UID:1;GM:true

::BuildSettings parameters.
set _bundle_serial=1
set  _carry_option=none
