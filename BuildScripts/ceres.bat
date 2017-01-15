set srcdir=D:\ThirdParty\ceres-solver-1.11.0\
set builddir=%srcdir%build
set thirdparty=d:\ThirdParty\
set suitesparse=D:\ThirdParty\suitesparse-metis-for-windows-1.3.1\
set suitesparselib=D:\ThirdParty\suitesparse-metis-for-windows-1.3.1\build\lib\Release\

mkdir %builddir%
chdir %builddir%
cmake .. -G "Visual Studio 12 Win64" -DEXPORT_BUILD_DIR=1 -DBUILD_EXAMPLES=0 -DBUILD_TESTING=0 -DEIGEN_INCLUDE_DIR=%thirdparty%eigen-eigen -DLAPACK_LIBRARIES=%suitesparse%lapack_windows\x64\liblapack.lib  -DBLAS_LIBRARIES=%suitesparse%\lapack_windows\x64\libblas.lib -DAMD_INCLUDE_DIR=%suitesparse%\SuiteSparse\AMD\include -DAMD_LIBRARY=%suitesparselib%\libamd.lib -DCAMD_INCLUDE_DIR=%suitesparse%\SuiteSparse\CAMD\include -DCAMD_LIBRARY=%suitesparselib%\libcamd.lib -DCCOLAMD_INCLUDE_DIR=%suitesparse%\SuiteSparse\CCOLAMD\include -DCCOLAMD_LIBRARY=%suitesparselib%\libccolamd.lib -DCHOLMOD_INCLUDE_DIR=%suitesparse%\SuiteSparse\CHOLMOD\Include -DCHOLMOD_LIBRARY=%suitesparselib%\libcholmod.lib -DCOLAMD_INCLUDE_DIR=%suitesparse%\SuiteSparse\COLAMD\include -DCOLAMD_LIBRARY=%suitesparselib%\libcolamd.lib -DCXSPARSE_INCLUDE_DIR=%suitesparse%\SuiteSparse\CXSPARSE\include -DCXSPARSE_LIBRARY=%suitesparselib%\libcxsparse.lib -DSUITESPARSEQR_INCLUDE_DIR=%suitesparse%\SuiteSparse\SPQR\include -DSUITESPARSEQR_LIBRARY=%suitesparselib%\libspqr.lib -DSUITESPARSE_CONFIG_INCLUDE_DIR=%suitesparse%\SuiteSparse\SuiteSparse_config\ -DSUITESPARSE_CONFIG_LIBRARY=%suitesparselib%\suitesparseconfig.lib -DMETIS_LIBRARY=%suitesparselib%metis.lib -DGFLAGS_INCLUDE_DIR=%thirdparty%gflags\build\include -Dgflags_DIR=%thirdparty%gflags\build -DGFLAGS_LIBRARY=%thirdparty%gflags\build\lib\Release\gflags.lib -DGLOG_INCLUDE_DIR=%thirdparty%glog\src\windows -DGLOG_LIBRARY=%thirdparty%glog\x64\Release\libglog.lib

pause>nul