set thirdparty=d:\ThirdParty\
set mypath=%cd%
set package=SuiteSparse-4.5.4
set metis=metis-5.1.0
set buildtype="Visual Studio 14 Win64"
set buildfolder=buildx64
chdir %thirdparty%
git clone https://github.com/jlblancoc/suitesparse-metis-for-windows.git

%mypath%\wget.exe -P "%thirdparty%suitesparse-metis-for-windows" "http://faculty.cse.tamu.edu/davis/SuiteSparse/%package%.tar.gz" 
%mypath%\wget.exe -P "%thirdparty%suitesparse-metis-for-windows" "http://glaros.dtc.umn.edu/gkhome/fetch/sw/metis/%metis%.tar.gz"

cd suitesparse-metis-for-windows
%mypath%\7z.exe x %thirdparty%suitesparse-metis-for-windows\%package%.tar.gz -so | %mypath%\7z.exe x -si -ttar
%mypath%\7z.exe x %thirdparty%suitesparse-metis-for-windows\%metis%.tar.gz -so | %mypath%\7z.exe x -si -ttar

xcopy %metis%\*.* metis /y /s 

::download suitespasrse and replace the suitesparse folder
::download metis 5.1.0 and replace the metis folder
:: in the gk_arch.h  comment the line //#define rint(x) ((int)((x)+0.5))
%mypath%\Replace.exe %thirdparty%suitesparse-metis-for-windows\metis\GKlib\gk_arch.h "#define rint(x) ((int)((x)+0.5))" "//#define rint(x) ((int)((x)+0.5))"

mkdir %buildfolder%
cd %buildfolder%

cmake .. -G %buildtype% -DCMAKE_CXX_FLAGS="/MP6"
cmake --build . --target ALL_BUILD --config Release
cmake --build . --target ALL_BUILD --config Debug

pause>nul
