IF "%~1"=="" (
 set zlibroot0=%cd%
 set zlibtag=v1.2.11
)ElSE (
set zlibroot0=%~1
)
IF "%~2"=="" ( 
 set zlibtag=v1.2.11
)ElSE (
set zlibtag=%~2
)
set zlibroot=%zlibroot0:\=/%

CALL config.bat

chdir %zlibroot%
git clone --branch %zlibtag% https://github.com/madler/zlib.git
cd zlib
mkdir %buildfolder%
cd %buildfolder%
cmake .. -G %buildtype%
%cmakebuild%