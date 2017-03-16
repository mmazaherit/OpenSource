IF "%~1"=="" (
 set protobufroot=%cd%
)ElSE (
set protobufroot=%~1
)

set protobuftag=v3.2.0

CALL config.bat

chdir %protobufroot%
git clone --branch %protobuftag% https://github.com/google/protobuf.git
cd protobuf

git clone -b release-1.7.0 https://github.com/google/googlemock.git gmock
cd gmock
git clone -b release-1.7.0 https://github.com/google/googletest.git gtest

cd ..


mkdir %buildfolder%
cd %buildfolder% 
 :: add this to the project to avoid linker errors "shlwapi.lib" as well as GOOGLE_GLOG_DLL_DECL=;GFLAGS_IS_A_DLL=0;
cmake ../cmake -G %buildtype%
%cmakebuild%
