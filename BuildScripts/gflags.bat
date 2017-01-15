set srcdir=D:\ThirdParty\gflags\
set builddir=%srcdir%build

mkdir %builddir%
chdir %builddir%
cmake .. -G "Visual Studio 12 Win64" .. 