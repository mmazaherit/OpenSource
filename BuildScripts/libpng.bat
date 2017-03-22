IF "%~1"=="" (
 set libpngroot0=%cd%
 set libpngtag=v1.6.29
)ElSE (
set libpngroot0=%~1
)
IF "%~2"=="" ( 
 set libpngtag=v1.6.29
)ElSE (
set libpngtag=%~2
)
set libpngroot=%libpngroot0:\=/%

CALL config.bat

chdir %libpngroot%
CALL %mypath%zlib.bat %libpngroot% "v1.2.11"

chdir %libpngroot%
git clone https://github.com/glennrp/libpng.git
cd libpng
mkdir %buildfolder%
cd %buildfolder%
cmake .. -G %buildtype% -DZLIB_INCLUDE_DIR=%libpngroot%/zlib;%libpngroot%/zlib/%buildfolder% -DZLIB_LIBRARY=%libpngroot%/zlib/%buildfolder%/Release/zlibstatic.lib
%cmakebuild%