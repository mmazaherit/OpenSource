::provides general config for the build scripts here
:: it should only contains "set" variable commands, as it may be called several times
set buildtype="Visual Studio 15 Win64" 
set buildfolder=buildx64
::boost settings
set boostver=1_61
set boost=D:/ThirdParty/boost_%boostver%_0
set boostlib=%boost%/lib64-msvc-14.0
set boostvcver=vc140
set Boost_INCLUDE_DIR=%boost%  
set BOOST_ROOT=%boost%
set Boost_LIBRARY_DIR=%boostlib%


set mypath0=%~dp0
set mypath=%mypath0:\=/%

set cmakebuildrelease=cmake --build . --target ALL_BUILD --config Release
set cmakebuildreleaseinstall=cmake --build . --target INSTALL --config Release

set cmakebuilddebug=cmake --build . --target INSTALL --config Debug
set cmakebuilddebuginstall=cmake --build . --target INSTALL --config Debug

set "cmakebuild=%cmakebuildrelease% & %cmakebuildreleaseinstall% & %cmakebuilddebug% & %cmakebuilddebuginstall%"