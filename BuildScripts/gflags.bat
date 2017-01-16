set thirdparty=D:\ThirdParty\
chdir %thirdparty%

git clone https://github.com/gflags/gflags.git

cd gflags
rename BUILD BUILD_RENAMED
mkdir build
cd build

cmake .. -G "Visual Studio 14 Win64" 
cmake --build . --target ALL_BUILD --config Release
cmake --build . --target ALL_BUILD --config Debug

pause>null