::REM 
:: fix  functions.cmake "{Boost_INCLUDE_DIRS}"-> "${Boost_INCLUDE_DIRS}"
:: comment gcc flags  in functins.cmake  # set(GOOG_CXX_FLAGS...

IF "%~1"=="" (
 set cartoroot0=%cd%
)ElSE (
set cartoroot0=%~1
)
set cartoroot=%cartoroot0:\=/%

CALL config.bat

chdir %cartoroot%
git clone https://github.com/googlecartographer/cartographer.git
cd cartographer
mkdir %buildfolder%
mkdir ThirdParty
set cartothirdparty=%cartoroot%/cartographer/ThirdParty

git apply ..\cartographer.patch

chdir %cartothirdparty%
git clone --branch "v0.6.0" https://github.com/webmproject/libwebp.git

::CALL %mypath%ceres.bat %cartothirdparty%
::CALL %mypath%protobuf.bat %cartothirdparty%

set builddir=%cartoroot%/cartographer/%buildfolder%
set protobuf=%cartothirdparty%/protobuf
set suitesparse=%cartothirdparty%/ceres-solver/ThirdParty/suitesparse-metis-for-windows/SuiteSparse


chdir %cartoroot%/cartographer/%buildfolder%

cmake .. -G %buildtype%  -DHAVE_PTHREAD=0 -Dgoogle_enable_testing=0 -DCMAKE_USE_PTHREADS_INIT=0 -DGMock_DIR=%protobuf%/gmock/%buildfolder%  -DGLOG_LIBRARY=%cartothirdparty%/ceres-solver/ThirdParty/glog/%buildfolder%/Release/glog.lib -DGLOG_INCLUDE_DIR=%cartothirdparty%/ceres-solver/ThirdParty/glog/src/windows  -DGFLAGS_DIR=%cartothirdparty%/ceres-solver/ThirdParty/gflags/%buildfolder% -DGFLAGS_INCLUDE_DIR=%cartothirdparty%/ceres-solver/ThirdParty/gflags/%buildfolder%/include -Dgflags_DIR=%cartothirdparty%/ceres-solver/ThirdParty/gflags/%buildfolder% -DGFLAGS_LIBRARY=%cartothirdparty%/ceres-solver/ThirdParty/gflags/%buildfolder%/lib/Release/gflags_static.lib -DGFLAGS_LIBRARY_DEBUG=%cartothirdparty%/ceres-solver/ThirdParty/gflags/%buildfolder%/lib/Debug/gflags_static.lib -DEigen3_DIR=%cartothirdparty%/ceres-solver/ThirdParty/eigen-eigen/%buildfolder% -DGMOCK_INCLUDE_DIRS=%cartothirdparty%/protobuf/gmock/include -DGMOCK_LIBRARIES=%cartothirdparty%/protobuf/%buildfolder%/Release/gmock.lib  -DGMOCK_SRC_DIR=%cartothirdparty%/protobuf/gmock/src  -DGTEST_INCLUDE_DIRS=%cartothirdparty%/protobuf/gmock/gtest/include -DBoost_INCLUDE_DIR=%boost% -DBoost_DIR=%boost%/boost  -DBoost_IOSTREAMS_LIBRARY_DEBUG=%boostlib%/boost_iostreams-%boostvcver%-mt-gd-%boostver%.lib -DBoost_IOSTREAMS_LIBRARY_RELEASE=%boostlib%/boost_iostreams-%boostvcver%-mt-%boostver%.lib  -DBoost_SYSTEM_LIBRARY_DEBUG=%boostlib%/boost_system-%boostvcver%-mt-gd-%boostver%.lib -DBoost_SYSTEM_LIBRARY_RELEASE=%boostlib%/boost_system-%boostvcver%-mt-%boostver%.lib -DCeres_DIR=%cartothirdparty%/ceres-solver/%buildfolder% -DCERES_INCLUDE_DIRS=%cartothirdparty%/ceres-solver/internal;%suitesparse%/CHOLMOD/Include;%suitesparse%/CXSparse/Include;%suitesparse%/SuiteSparse_config;%suitesparse%/SPQR/Include;%protobuf%/gmock/gtest/include  -DLUA_INCLUDE_DIR=%cartothirdparty%/lua52/include  -DLUA_LIBRARY=%cartothirdparty%/lua52/lua.lib -DProtobuf_INCLUDE_DIR=%protobuf%/src  -DProtobuf_SRC_ROOT_FOLDER=%protobuf%/src -DProtobuf_LIBRARY_DEBUG=%protobuf%/%buildfolder%/Debug/libprotobufd.lib  -DProtobuf_LIBRARY_RELEASE=%protobuf%/%buildfolder%/Release/libprotobuf.lib  -DProtobuf_LITE_LIBRARY_DEBUG=%protobuf%/%buildfolder%/Debug/libprotobuf_lited.lib -DProtobuf_LITE_LIBRARY_RELEASE=%protobuf%/%buildfolder%/Release/libprotobuf-lite.lib  -DProtobuf_PROTOC_LIBRARY_DEBUG=%protobuf%/%buildfolder%/Debug/libprotocd.lib  -DProtobuf_PROTOC_LIBRARY_RELEASE=%protobuf%/%buildfolder%/Release/libprotoc.lib  -DPROTOBUF_PROTOC_EXECUTABLE=%protobuf%/%buildfolder%/Release/protoc.exe -DSPHINX_EXECUTABLE="C:/Python27/Scripts/sphinx-quickstart.exe" -DPKG_CONFIG_EXECUTABLE=%cartoroot%/cartographer/package.xml  -DCAIRO_INCLUDE_DIRS=%cartothirdparty%/cairo/cairo-1.12.16/src;%cartothirdparty%/cairo/projects/cairo_1_12_16/src;%cartothirdparty%/libwebp/src  -DCAIRO_LIBRARIES=%cartothirdparty%/cairo/projects/x64/Release/cairo_1_12_16.lib

pause>nul