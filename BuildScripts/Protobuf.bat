IF "%~1"=="" (
 set protobufroot0=%cd%
)ElSE (
set protobufroot0=%~1
)

set protobufroot=%protobufroot0:\=/%

set protobuftag=v3.2.0
::set protobuftag=master

CALL config.bat

chdir %protobufroot%
git clone --branch %protobuftag% https://github.com/google/protobuf.git
cd protobuf
git clone -b release-1.7.0 https://github.com/google/googlemock.git gmock
cd gmock
git clone -b release-1.7.0 https://github.com/google/googletest.git gtest

mkdir %buildfolder%
cd %buildfolder%
cmake .. -G %buildtype%
%cmakebuild%



chdir %protobufroot%\protobuf
mkdir %buildfolder%
cd %buildfolder% 
 :: add this to the project to avoid linker errors "shlwapi.lib" as well as GOOGLE_GLOG_DLL_DECL=;GFLAGS_IS_A_DLL=0; -Dprotobuf_BUILD_TESTS=0
cmake ../cmake -G %buildtype% -Dgtest_disable_pthreads=1 -DCMAKE_USE_PTHREADS_INIT=0 
%cmakebuild%
