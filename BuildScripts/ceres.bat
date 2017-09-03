IF "%~1"=="" (
 set ceresroot0=%cd%
)ElSE (
set ceresroot0=%~1
)
set ceresroot=%ceresroot0:\=/%

CALL config.bat

set cerestag="1.13.0"
set eigentag="3.3.4"

chdir %ceresroot%
git clone --branch %cerestag% https://github.com/ceres-solver/ceres-solver.git


cd ceres-solver
mkdir ThirdParty
set ceresthirdparty=%ceresroot%/ceres-solver/ThirdParty

::install suitesparse in thirdparty folder
chdir %ceresthirdparty%
CALL %mypath%SuiteSparse.bat %ceresthirdparty%
set suitesparse=%ceresthirdparty%/suitesparse-metis-for-windows/
set suitesparselib=%ceresthirdparty%/suitesparse-metis-for-windows/%buildfolder%/lib/Release/

::install eigen
CALL %mypath%eigen.bat %ceresthirdparty% %eigentag%


::GFLAGS
chdir %ceresthirdparty%
git clone --branch "v2.2.0" https://github.com/gflags/gflags.git
cd gflags
mkdir %buildfolder%
cd %buildfolder%
cmake .. -G %buildtype%
%cmakebuild%

::Glog
chdir %ceresthirdparty%
git clone --branch "master" https://github.com/google/glog.git
cd glog
mkdir %buildfolder%
cd %buildfolder% 
 :: add this to the project to avoid linker errors "shlwapi.lib" as well as GOOGLE_GLOG_DLL_DECL=;GFLAGS_IS_A_DLL=0;
cmake .. -G %buildtype% -Dgflags_DIR=%ceresthirdparty%/gflags/%buildfolder%
%cmakebuild%



chdir %ceresroot%/ceres-solver
mkdir %buildfolder%
cd %buildfolder%

cmake .. -G %buildtype% -DCMAKE_CXX_FLAGS="/MP6" -DEXPORT_BUILD_DIR=1 -DBUILD_EXAMPLES=0 -DBUILD_TESTING=0 -DEIGENSPARSE=1 -DSUITESPARSE=1 -DEigen3_DIR=%ceresthirdparty%/eigen-eigen/%buildfolder% -DEIGEN_INCLUDE_DIR=%ceresthirdparty%/eigen-eigen -DLAPACK_LIBRARIES=%suitesparse%/lapack_windows/x64/liblapack.lib  -DBLAS_LIBRARIES=%suitesparse%/lapack_windows/x64/libblas.lib -DAMD_INCLUDE_DIR=%suitesparse%/SuiteSparse/AMD/include -DAMD_LIBRARY=%suitesparselib%/libamd.lib -DCAMD_INCLUDE_DIR=%suitesparse%/SuiteSparse/CAMD/include -DCAMD_LIBRARY=%suitesparselib%/libcamd.lib -DCCOLAMD_INCLUDE_DIR=%suitesparse%/SuiteSparse/CCOLAMD/include -DCCOLAMD_LIBRARY=%suitesparselib%/libccolamd.lib -DCHOLMOD_INCLUDE_DIR=%suitesparse%/SuiteSparse/CHOLMOD/Include -DCHOLMOD_LIBRARY=%suitesparselib%/libcholmod.lib -DCOLAMD_INCLUDE_DIR=%suitesparse%/SuiteSparse/COLAMD/include -DCOLAMD_LIBRARY=%suitesparselib%/libcolamd.lib -DCXSPARSE_INCLUDE_DIR=%suitesparse%/SuiteSparse/CXSPARSE/include -DCXSPARSE_LIBRARY=%suitesparselib%/libcxsparse.lib -DSUITESPARSEQR_INCLUDE_DIR=%suitesparse%/SuiteSparse/SPQR/include -DSUITESPARSEQR_LIBRARY=%suitesparselib%/libspqr.lib -DSUITESPARSE_CONFIG_INCLUDE_DIR=%suitesparse%/SuiteSparse/SuiteSparse_config/ -DSUITESPARSE_CONFIG_LIBRARY=%suitesparselib%/suitesparseconfig.lib -DMETIS_LIBRARY=%suitesparselib%/metis.lib -DGFLAGS_INCLUDE_DIR=%ceresthirdparty%/gflags/%buildfolder%/include -Dgflags_DIR=%ceresthirdparty%/gflags/%buildfolder% -DGFLAGS_LIBRARY=%ceresthirdparty%/gflags/%buildfolder%/lib/Release/gflags_static.lib -DGLOG_INCLUDE_DIR=%ceresthirdparty%/glog/src/windows -DGLOG_LIBRARY=%ceresthirdparty%/glog/Release/glog.lib
%cmakebuildrelease% -- /m
%cmakebuilddebug% -- /m