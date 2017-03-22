IF "%~1"=="" (
 set cairoroot0=%cd%
)ElSE (
set cairoroot0=%~1
)
set cairoroot=%cairoroot0:\=/%

CALL config.bat
git clone https://github.com/CMakePorts/cairo.git

cd cairo
mkdir ThirdParty
cd ThirdParty
set cairothirdparty=%cairoroot%/cairo/ThirdParty

call %mypath%libpng.bat %cairothirdparty%

chdir %cairothirdparty%
git clone https://github.com/CMakePorts/pixman.git
cd pixman
mkdir %buildfolder%
cd %buildfolder%
cmake .. -G %buildtype%
%cmakebuild%


chdir %cairothirdparty%
git clone https://github.com/libexpat/libexpat.git
cd libexpat
cd expat
mkdir %buildfolder%
cd %buildfolder%
cmake .. -G %buildtype%
%cmakebuild%

chdir %cairothirdparty%
git clone git://git.sv.nongnu.org/freetype/freetype2.git
cd freetype2
mkdir %buildfolder%
cd %buildfolder%
cmake .. -G %buildtype%
%cmakebuild%

chdir %cairothirdparty%
git clone https://github.com/CMakePorts/fontconfig.git
cd fontconfig
mkdir %buildfolder%
cd %buildfolder%
cmake .. -G %buildtype% -DEXPAT_LIBRARY=%cairothirdparty%/libexpat/expat/%buildfolder%/Release/expat.lib -DEXPAT_INCLUDE_DIR=%cairothirdparty%/libexpat/expat/lib -DFREETYPE_LIBRARY=%cairothirdparty%/freetype2/%buildfolder%/Release/freetype2.lib  -DFREETYPE_INCLUDE_DIRS=%cairothirdparty%/freetype2/src
%cmakebuild%

chdir %cairoroot%/cairo
mkdir %buildfolder%
cd %buildfolder%
cmake .. -G %buildtype%  -DPIXMAN_INCLUDE_DIRS=%cairothirdparty%/pixman -DPIXMAN_LIBRARIES=%cairothirdparty%/pixman/%buildfolder%/pixman/Release/pixman-1_static.lib -DZLIB_INCLUDE_DIR=%cairothirdparty%/zlib;%cairothirdparty%/zlib/%buildfolder% -DZLIB_LIBRARY=%cairothirdparty%/zlib/%buildfolder%/Release/zlibstatic.lib -DPNG_PNG_INCLUDE_DIR=%cairothirdparty%/libpng -DPNG_LIBRARY=%cairothirdparty%/libpng/%buildfolder%/Release/libpng16_static.lib -DFONTCONFIG_INCLUDE_DIRS=%cairothirdparty%/fontconfig/
%cmakebuild%