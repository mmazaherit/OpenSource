IF "%~1"=="" (
 set suitesparseroot=%cd%
)ElSE (
set suitesparseroot=%~1
)

CALL config.bat

set package=SuiteSparse-4.5.4
set metis=metis-5.1.0

chdir %suitesparseroot%
git clone https://github.com/jlblancoc/suitesparse-metis-for-windows.git

cd suitesparse-metis-for-windows
%mypath%\wget.exe "http://faculty.cse.tamu.edu/davis/SuiteSparse/%package%.tar.gz" 
%mypath%\wget.exe "http://glaros.dtc.umn.edu/gkhome/fetch/sw/metis/%metis%.tar.gz"
%mypath%\7z.exe x %package%.tar.gz -so | %mypath%\7z.exe x -si -ttar
%mypath%\7z.exe x %metis%.tar.gz -so | %mypath%\7z.exe x -si -ttar

xcopy %metis%\*.* metis /y /s 

::download suitespasrse and replace the suitesparse folder
::download metis 5.1.0 and replace the metis folder
:: in the gk_arch.h  comment the line //#define rint(x) ((int)((x)+0.5))
%mypath%\Replace.exe %suitesparseroot%\suitesparse-metis-for-windows\metis\GKlib\gk_arch.h "#define rint(x) ((int)((x)+0.5))" "//#define rint(x) ((int)((x)+0.5))"

mkdir %buildfolder%
cd %buildfolder%

cmake .. -G %buildtype% -DCMAKE_CXX_FLAGS="/MP6"
%cmakebuild%