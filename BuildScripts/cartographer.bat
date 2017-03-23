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
::git apply %mypath%cartographer.patch
git am --3way --ignore-space-change --keep-cr  %mypath%cartographer.patch

mkdir ThirdParty
set cartothirdparty=%cartoroot%/cartographer/ThirdParty
mkdir %buildfolder%
set builddir=%cartoroot%/cartographer/%buildfolder%


chdir %cartothirdparty%
git clone --branch "v0.6.0" https://github.com/webmproject/libwebp.git
cd libwebp
mkdir %buildfolder%
cd %buildfolder%
cmake .. -G %buildtype%
%cmakebuild%

chdir %cartothirdparty%
::becareful if there is any space , change %20 to space
%mypath%wget.exe "https://downloads.sourceforge.net/project/luabinaries/5.2.4/Windows Libraries/Static/lua-5.2.4_Win64_vc14_lib.zip"
%mypath%7z.exe x lua-5.2.4_Win64_vc14_lib.zip -olua52


CALL %mypath%ceres.bat %cartothirdparty%
CALL %mypath%protobuf.bat %cartothirdparty%
CALL %mypath%cairo.bat %cartothirdparty%

set suitesparse=%cartothirdparty%/ceres-solver/ThirdParty/suitesparse-metis-for-windows/SuiteSparse
set protobuf=%cartothirdparty%/protobuf

chdir %cartoroot%/cartographer/%buildfolder%

cmake .. -G %buildtype%  -DCMAKE_USE_RELATIVE_PATHS=0 -DHAVE_PTHREAD=0 -Dgoogle_enable_testing=0 -DCMAKE_USE_PTHREADS_INIT=0 -DGMock_DIR=%protobuf%/gmock/%buildfolder% -DGMOCK_SRC_DIR=%cartothirdparty%/protobuf/gmock/src  -DGLOG_LIBRARY=%cartothirdparty%/ceres-solver/ThirdParty/glog/%buildfolder%/Release/glog.lib -DGLOG_INCLUDE_DIR=%cartothirdparty%/ceres-solver/ThirdParty/glog/src/windows -DGFLAGS_DIR=%cartothirdparty%/ceres-solver/ThirdParty/gflags/%buildfolder% -Dgflags_DIR=%cartothirdparty%/ceres-solver/ThirdParty/gflags/%buildfolder% -DGFLAGS_INCLUDE_DIR=%cartothirdparty%/ceres-solver/ThirdParty/gflags/%buildfolder%/include  -DGFLAGS_LIBRARY=%cartothirdparty%/ceres-solver/ThirdParty/gflags/%buildfolder%/lib/$(Configuration)/gflags.lib  -DEigen3_DIR=%cartothirdparty%/ceres-solver/ThirdParty/eigen-eigen/%buildfolder% -DGMOCK_INCLUDE_DIRS=%cartothirdparty%/protobuf/gmock/include;%cartothirdparty%/protobuf/gmock/gtest/include -DGMOCK_LIBRARIES=%cartothirdparty%/protobuf/%buildfolder%/$(Configuration)/gmock.lib   -DBoost_INCLUDE_DIR=%boost% -DBoost_DIR=%boost%/boost  -DBoost_IOSTREAMS_LIBRARY_DEBUG=%boostlib%/libboost_iostreams-%boostvcver%-mt-gd-%boostver%.lib -DBoost_IOSTREAMS_LIBRARY_RELEASE=%boostlib%/libboost_iostreams-%boostvcver%-mt-%boostver%.lib  -DBoost_SYSTEM_LIBRARY_DEBUG=%boostlib%/libboost_system-%boostvcver%-mt-gd-%boostver%.lib -DBoost_SYSTEM_LIBRARY_RELEASE=%boostlib%/libboost_system-%boostvcver%-mt-%boostver%.lib -DBoost_ZLIB_LIBRARY_RELEASE=%boostlib%/libboost_zlib-%boostvcver%-mt-%boostver%.lib  -DBoost_ZLIB_LIBRARY_DEBUG=%boostlib%/libboost_zlib-%boostvcver%-mt-gd-%boostver%.lib -DCeres_DIR=%cartothirdparty%/ceres-solver/%buildfolder%  -DCERES_INCLUDE_DIRS=%cartothirdparty%/ceres-solver/internal    -DProtobuf_INCLUDE_DIR=%protobuf%/src  -DProtobuf_SRC_ROOT_FOLDER=%protobuf%/src -DProtobuf_LIBRARY_DEBUG=%protobuf%/%buildfolder%/Debug/libprotobufd.lib  -DProtobuf_LIBRARY_RELEASE=%protobuf%/%buildfolder%/Release/libprotobuf.lib  -DProtobuf_LITE_LIBRARY_DEBUG=%protobuf%/%buildfolder%/Debug/libprotobuf_lited.lib -DProtobuf_LITE_LIBRARY_RELEASE=%protobuf%/%buildfolder%/Release/libprotobuf-lite.lib  -DProtobuf_PROTOC_LIBRARY_DEBUG=%protobuf%/%buildfolder%/Debug/libprotocd.lib  -DProtobuf_PROTOC_LIBRARY_RELEASE=%protobuf%/%buildfolder%/Release/libprotoc.lib  -DPROTOBUF_PROTOC_EXECUTABLE=%protobuf%/%buildfolder%/$(Configuration)/protoc.exe -DSPHINX_EXECUTABLE="C:/Python27/Scripts/sphinx-quickstart.exe" -DPKG_CONFIG_EXECUTABLE=%cartoroot%/cartographer/package.xml -DCAIRO_INCLUDE_DIRS=%cartothirdparty%/cairo/src;%cartothirdparty%/cairo/%buildfolder%   -DCAIRO_LIBRARIES=%cartothirdparty%/cairo/%buildfolder%/src/$(Configuration)/cairo.lib -DLUA_INCLUDE_DIR=%cartothirdparty%/lua52/include  -DLUA_LIBRARY=%cartothirdparty%/lua52/lua52.lib -Dwebp_INCLUDE_DIR=%cartothirdparty%/libwebp/src -Dwebp_LIBRARY=%cartothirdparty%/libwebp/%buildfolder%/$(Configuration)/webp.lib

%cmakebuild%