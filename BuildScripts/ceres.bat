IF "%~1"=="" (
 set ceresroot=%cd%
)ElSE (
set ceresroot=%~1
)
set mypath=%~dp0


set buildtype="Visual Studio 14 Win64" 
set buildfolder=buildx64
set eigenpackage=3.3.2

chdir %ceresroot%
git clone https://github.com/ceres-solver/ceres-solver.git


cd ceres-solver
mkdir ThirdParty
set ceresthirdparty=%ceresroot%\ceres-solver\ThirdParty

::install suitesparse in thirdparty folder
chdir %ceresthirdparty%
CALL %mypath%\SuiteSparse.bat %ceresthirdparty%
set suitesparse=%ceresthirdparty%\suitesparse-metis-for-windows\
set suitesparselib=%ceresthirdparty%\suitesparse-metis-for-windows\%buildfolder%\lib\Release\



chdir %ceresthirdparty%
%mypath%\wget.exe "http://bitbucket.org/eigen/eigen/get/%eigenpackage%.tar.gz"
%mypath%\7z.exe x  %eigenpackage%.tar.gz -so | %mypath%\7z.exe x -si -ttar
::eigen add junks to the folder name, must be removed
set cmd="dir eigen-eigen* /b"
FOR /F "tokens=*" %%i IN (' %cmd% ') DO SET eigendir=%%i
rename "%eigendir%" "eigen-eigen"



::GFLAGS
chdir %ceresthirdparty%
git clone https://github.com/gflags/gflags.git
cd gflags
mkdir %buildfolder%
cd %buildfolder%
cmake .. -G %buildtype%
cmake --build . --target ALL_BUILD --config Release
cmake --build . --target ALL_BUILD --config Debug

::Glog
chdir %ceresthirdparty%
git clone https://github.com/google/glog.git
cd glog
mkdir %buildfolder%
cd %buildfolder% 
 :: add this to the project to avoid linker errors "shlwapi.lib" as well as GOOGLE_GLOG_DLL_DECL=;GFLAGS_IS_A_DLL=0;
cmake .. -G "Visual Studio 14 Win64" -Dgflags_DIR=%ceresthirdparty%\gflags\%buildfolder%
cmake --build . --target ALL_BUILD --config Release
cmake --build . --target ALL_BUILD --config Debug



chdir %ceresroot%\ceres-solver
mkdir %buildfolder%
cd %buildfolder%

cmake .. -G %buildtype% -DCMAKE_CXX_FLAGS="/MP6" -DEXPORT_BUILD_DIR=1 -DBUILD_EXAMPLES=0 -DBUILD_TESTING=0 -DEIGENSPARSE=1 -DSUITESPARSE=1 -DEIGEN_INCLUDE_DIR=%ceresthirdparty%\eigen-eigen -DLAPACK_LIBRARIES=%suitesparse%\lapack_windows\x64\liblapack.lib  -DBLAS_LIBRARIES=%suitesparse%\lapack_windows\x64\libblas.lib -DAMD_INCLUDE_DIR=%suitesparse%\SuiteSparse\AMD\include -DAMD_LIBRARY=%suitesparselib%\libamd.lib -DCAMD_INCLUDE_DIR=%suitesparse%\SuiteSparse\CAMD\include -DCAMD_LIBRARY=%suitesparselib%\libcamd.lib -DCCOLAMD_INCLUDE_DIR=%suitesparse%\SuiteSparse\CCOLAMD\include -DCCOLAMD_LIBRARY=%suitesparselib%\libccolamd.lib -DCHOLMOD_INCLUDE_DIR=%suitesparse%\SuiteSparse\CHOLMOD\Include -DCHOLMOD_LIBRARY=%suitesparselib%\libcholmod.lib -DCOLAMD_INCLUDE_DIR=%suitesparse%\SuiteSparse\COLAMD\include -DCOLAMD_LIBRARY=%suitesparselib%\libcolamd.lib -DCXSPARSE_INCLUDE_DIR=%suitesparse%\SuiteSparse\CXSPARSE\include -DCXSPARSE_LIBRARY=%suitesparselib%\libcxsparse.lib -DSUITESPARSEQR_INCLUDE_DIR=%suitesparse%\SuiteSparse\SPQR\include -DSUITESPARSEQR_LIBRARY=%suitesparselib%\libspqr.lib -DSUITESPARSE_CONFIG_INCLUDE_DIR=%suitesparse%\SuiteSparse\SuiteSparse_config\ -DSUITESPARSE_CONFIG_LIBRARY=%suitesparselib%\suitesparseconfig.lib -DMETIS_LIBRARY=%suitesparselib%\metis.lib -DGFLAGS_INCLUDE_DIR=%ceresthirdparty%\gflags\%buildfolder%\include -Dgflags_DIR=%ceresthirdparty%\gflags\%buildfolder% -DGFLAGS_LIBRARY=%ceresthirdparty%\gflags\%buildfolder%\lib\Release\gflags_static.lib -DGLOG_INCLUDE_DIR=%ceresthirdparty%\glog\src\windows -DGLOG_LIBRARY=%ceresthirdparty%\glog\Release\glog.lib
cmake --build .  --target ALL_BUILD --config Release -- /m
cmake --build . --target ALL_BUILD --config Debug -- /m