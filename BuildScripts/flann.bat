set thirdparty=D:\ThirdParty\
set srcdir=%thirdparty%flann-1.8.4-src\
set builddir=%srcdir%build


mkdir %builddir%
chdir %builddir%
cmake .. -G "Visual Studio 12"

pause>nul