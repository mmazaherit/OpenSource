::provides general config for the build scripts here
:: it should only contains "set" variable commands, as it may be called several times
set buildtype="Visual Studio 14 Win64" 
set buildfolder=buildx64
::boost settings
set boostver=1_61
set boost=D:\ThirdParty\boost_%boostver%_0
set boostlib=%boost%\lib64-msvc-14.0
set boostvcver=vc140


set mypath=%~dp0

set cmakebuildrelease=cmake --build . --target ALL_BUILD --config Release
set cmakebuildebug=cmake --build . --target ALL_BUILD --config Debug

set "cmakebuild=%cmakebuildrelease% & %cmakebuildebug%"