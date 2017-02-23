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

CALL config.bat

chdir %gflagsroot%
git clone --branch %gflagstag% https://github.com/gflags/gflags.git
cd gflags
mkdir %buildfolder%
cd %buildfolder%
cmake .. -G %buildtype%
%cmakebuild%