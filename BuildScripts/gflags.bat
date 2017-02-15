IF "%~1"=="" (
 set gflagsroot=%cd%
 set gflagstag="master"
)ElSE (
set gflagsroot=%~1
)
IF "%~2"=="" ( 
 set gflagstag="master"
)ElSE (
set gflagsroot=%~2
)
set mypath=%~dp0
set buildtype="Visual Studio 14 Win64" 
set buildfolder=buildx64

chdir %gflagsroot%
git clone --branch %gflagstag% https://github.com/gflags/gflags.git
cd gflags
mkdir %buildfolder%
cd %buildfolder%
cmake .. -G %buildtype%
cmake --build . --target ALL_BUILD --config Release
cmake --build . --target ALL_BUILD --config Debug