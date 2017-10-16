::provides general config for the build scripts here
::it should only contains "set" variable commands, as it may be called several times

::camke build type for different visual studio
::::vs2013 set buildtype="Visual Studio 12 Win64" 
::::vs2015 set buildtype="Visual Studio 14 Win64" 

set buildtype="Visual Studio 12 Win64" 
::boost settings
::::boost version
set boostver=1_63
::::boost path,example set boost=D:/ThirdParty/boost_%boostver%_0
set boost=D:/ThirdParty/boost_%boostver%_0
::::boostlib folder which is inside the boost package 
set boostlib=%boost%/lib64-msvc-12.0
::::boost visual studio version 
set boostvcver=vc120
:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::



set buildfolder=buildx64
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