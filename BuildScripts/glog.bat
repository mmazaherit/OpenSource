set thirdparty=D:\ThirdParty\
chdir %thirdparty%

git clone https://github.com/google/glog.git

cd glog
mkdir build
cd build
 
 :: add this to the project to avoid linker errors "shlwapi.lib" as well as GOOGLE_GLOG_DLL_DECL=;GFLAGS_IS_A_DLL=0;

cmake .. -G "Visual Studio 14 Win64" -Dgflags_DIR=%thirdparty%gflags\build

cmake --build . --target ALL_BUILD --config Release
cmake --build . --target ALL_BUILD --config Debug

pause>null