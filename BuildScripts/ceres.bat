set thirdparty=d:\ThirdParty\
chdir %thirdparty%

git clone https://github.com/ceres-solver/ceres-solver.git

set suitesparse=%thirdparty%suitesparse-metis-for-windows\
set suitesparselib=%thirdparty%suitesparse-metis-for-windows\build\lib\Release\

cd ceres-solver
mkdir build
cd build

cmake .. -G "Visual Studio 14 Win64" -DEXPORT_BUILD_DIR=1 -DBUILD_EXAMPLES=0 -DBUILD_TESTING=0 -DEIGENSPARSE=1 -DSUITESPARSE=1 -DEIGEN_INCLUDE_DIR=%thirdparty%eigen-eigen -DLAPACK_LIBRARIES=%suitesparse%lapack_windows\x64\liblapack.lib  -DBLAS_LIBRARIES=%suitesparse%lapack_windows\x64\libblas.lib -DAMD_INCLUDE_DIR=%suitesparse%SuiteSparse\AMD\include -DAMD_LIBRARY=%suitesparselib%libamd.lib -DCAMD_INCLUDE_DIR=%suitesparse%SuiteSparse\CAMD\include -DCAMD_LIBRARY=%suitesparselib%libcamd.lib -DCCOLAMD_INCLUDE_DIR=%suitesparse%SuiteSparse\CCOLAMD\include -DCCOLAMD_LIBRARY=%suitesparselib%libccolamd.lib -DCHOLMOD_INCLUDE_DIR=%suitesparse%\SuiteSparse\CHOLMOD\Include -DCHOLMOD_LIBRARY=%suitesparselib%libcholmod.lib -DCOLAMD_INCLUDE_DIR=%suitesparse%SuiteSparse\COLAMD\include -DCOLAMD_LIBRARY=%suitesparselib%libcolamd.lib -DCXSPARSE_INCLUDE_DIR=%suitesparse%SuiteSparse\CXSPARSE\include -DCXSPARSE_LIBRARY=%suitesparselib%libcxsparse.lib -DSUITESPARSEQR_INCLUDE_DIR=%suitesparse%SuiteSparse\SPQR\include -DSUITESPARSEQR_LIBRARY=%suitesparselib%libspqr.lib -DSUITESPARSE_CONFIG_INCLUDE_DIR=%suitesparse%SuiteSparse\SuiteSparse_config\ -DSUITESPARSE_CONFIG_LIBRARY=%suitesparselib%suitesparseconfig.lib -DMETIS_LIBRARY=%suitesparselib%metis.lib -DGFLAGS_INCLUDE_DIR=%thirdparty%gflags\build\include -Dgflags_DIR=%thirdparty%gflags\build -DGFLAGS_LIBRARY=%thirdparty%gflags\build\lib\Release\gflags_static.lib -DGLOG_INCLUDE_DIR=%thirdparty%glog\src\windows -DGLOG_LIBRARY=%thirdparty%glog\Release\glog.lib
cmake --build . --target ALL_BUILD --config Release
cmake --build . --target ALL_BUILD --config Debug
pause>nul