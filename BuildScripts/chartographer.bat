::REM 
:: fix  functions.cmake "{Boost_INCLUDE_DIRS}"-> "${Boost_INCLUDE_DIRS}"
:: comment gcc flags  in functins.cmake  # set(GOOG_CXX_FLAGS...

set srcdir=D:\ThirdParty\cartographer\
set builddir=%srcdir%build
set thirdparty=d:\ThirdParty\
set ceres=%thirdparty%ceres-solver-1.11.0\
set boost=%thirdparty%boost_1_60_0
set protobuf=%thirdparty%protobuf\

mkdir %builddir%
chdir %builddir%

:: copy ceres config file to the name recognized by cartographer
::copy %ceres%build\CeresConfig-install.cmake %ceres%build\CeresConfig.cmake

cmake .. -G "Visual Studio 14 Win64" -DEigen3_DIR=%thirdparty%eigen-eigen -DGMOCK_INCLUDE_DIRS=%thirdparty%googletest-master\googlemock\include -DGMOCK_LIBRARIES=%thirdparty%googletest-master\build\googlemock\Release\gmock.lib  -DGMOCK_SRC_DIR=%thirdparty%googletest-master\googlemock\src  -DGTEST_INCLUDE_DIRS=%thirdparty%googletest-master\googletest\include -DBoost_INCLUDE_DIR=%boost% -DBoost_DIR=%boost%\boost -DBoost_IOSTREAMS_LIBRARY_DEBUG=%boost%\lib64-msvc-12.0\boost_iostreams-vc120-mt-gd-1_60.lib -DBoost_IOSTREAMS_LIBRARY_RELEASE=%boost%\lib64-msvc-12.0\boost_iostreams-vc120-mt-1_60.lib -DBoost_SYSTEM_LIBRARY_DEBUG=%boost%\lib64-msvc-12.0\boost_system-vc120-mt-gd-1_60.lib -DBoost_SYSTEM_LIBRARY_RELEASE=%boost%\lib64-msvc-12.0\boost_system-vc120-mt-1_60.lib -DCeres_DIR=%ceres%build  -DLUA_INCLUDE_DIR=%thirdparty%lua52\include -DLUA_LIBRARY=%thirdparty%lua52\lua.lib -DProtobuf_INCLUDE_DIR=%protobuf%src -DProtobuf_SRC_ROOT_FOLDER=%protobuf% -DProtobuf_LIBRARY_DEBUG=%protobuf%cmake\build\debug\libprotobufd.lib -DProtobuf_LIBRARY_RELEASE=%protobuf%cmake\build\release\libprotobuf.lib -DProtobuf_LITE_LIBRARY_DEBUG=%protobuf%cmake\build\debug\libprotobuf_lited.lib -DProtobuf_LITE_LIBRARY_RELEASE=%protobuf%cmake\build\release\libprotobuf-lite.lib -DProtobuf_PROTOC_LIBRARY_DEBUG=%protobuf%cmake\build\debug\libprotocd.lib -DProtobuf_PROTOC_LIBRARY_RELEASE=%protobuf%cmake\build\release\libprotoc.lib -DProtobuf_PROTOC_EXECUTABLE=%protobuf%cmake\build\release\protoc.exe -DSPHINX_EXECUTABLE="C:\Python27\Scripts\sphinx-quickstart.exe" -DPKG_CONFIG_EXECUTABLE=%thirdparty%cartographer\package.xml -DCAIRO_LIBRARIES=%thirdparty%cairo\projects\x64\Release\cairo_1_12_16.lib

pause>nul