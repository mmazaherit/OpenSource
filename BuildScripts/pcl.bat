:: for this package you need to build VTK in thirdparty folder as well as downloading the eigen in thirdparty
IF "%~1"=="" (
 set pclroot=%cd%
)ElSE (
set pclroot=%~1
)

CALL config.bat

set pcl=%pclroot%/pcl

set vtk=D:/ThirdParty/VTK/%buildfolder%

::"pcl-1.8.0" had issue with vtk7.1
chdir %pclroot%
git clone --branch master https://github.com/PointCloudLibrary/pcl.git

cd pcl
mkdir ThirdParty
cd ThirdParty
set pclthirdparty=%pclroot%/pcl/ThirdParty
::download flann
git clone --branch  "1.9.1"  http://github.com/mariusmuja/flann.git

cd flann
mkdir %buildfolder%
cd %buildfolder%

cmake .. -G %buildtype%
%cmakebuild%

::download and build zlib
call %mypath%zlib.bat %pclthirdparty% "v1.2.11"

::qhull
chdir %pclthirdparty%
git clone https://github.com/qhull/qhull.git
cd qhull
mkdir %buildfolder%
cd %buildfolder%
cmake .. -G %buildtype%
%cmakebuild%

::install eigen
CALL %mypath%eigen.bat %pclthirdparty%

chdir %pcl% 
mkdir %buildfolder%
cd %buildfolder%

set builddir=%pcl%/%buildfolder%
set flann=%pclthirdparty%/flann
set flannbuild=%flann%/%buildfolder%
set zlib=%pclthirdparty%/zlib
set zlibbuild=%zlib%%buildfolder%
set qhull=%pclthirdparty%/qhull



cmake .. -G %buildtype% -DVTK_DIR=%vtk% -DEIGEN_INCLUDE_DIR=%pclthirdparty%/eigen-eigen -DFLANN_INCLUDE_DIR=%flann%/src/cpp -DFLANN_LIBRARY=%flannbuild%/lib/Release/flann_s.lib -DFLANN_LIBRARY_DEBUG=%flannbuild%/lib/Debug/flann_s.lib -DZLIB_INCLUDE_DIR=%zlib% -DZLIB_LIBRARY_DEBUG=%zlibbuild%Debug/zlibd.lib -DZLIB_LIBRARY_RELEASE=%zlibbuild%/Debug/zlib.lib -DQHULL_INCLUDE_DIR=%qhull%/src -DQHULL_LIBRARY=%qhull%/%buildfolder%/Release/qhullstatic.lib -DQHULL_LIBRARY_DEBUG=%qhull%/%buildfolder%/Debug/qhullstatic.lib  -DBoost_INCLUDE_DIR=%boost% -DBoost_IOSTREAMS_LIBRARY_DEBUG=%boostlib%/boost_iostreams-%boostvcver%-mt-gd-%boostver%.lib -DBoost_IOSTREAMS_LIBRARY_RELEASE=%boostlib%/boost_iostreams-%boostvcver%-mt-%boostver%.lib -DBoost_SYSTEM_LIBRARY_DEBUG=%boostlib%/boost_system-%boostvcver%-mt-gd-%boostver%.lib -DBoost_SYSTEM_LIBRARY_RELEASE=%boostlib%/boost_system-%boostvcver%-mt-%boostver%.lib -DBoost_ATOMIC_LIBRARY_DEBUG=%boostlib%/boost_atomic-%boostvcver%-mt-gd-%boostver%.lib -DBoost_ATOMIC_LIBRARY_RELEASE=%boostlib%/boost_atomic-%boostvcver%-mt-%boostver%.lib -DBoost_CHRONO_LIBRARY_DEBUG=%boostlib%/boost_chrono-%boostvcver%-mt-gd-%boostver%.lib -DBoost_CHRONO_LIBRARY_RELEASE=%boostlib%/boost_chrono-%boostvcver%-mt-%boostver%.lib -DBoost_FILESYSTEM_LIBRARY_DEBUG=%boostlib%/boost_filesystem-%boostvcver%-mt-gd-%boostver%.lib -DBoost_FILESYSTEM_LIBRARY_RELEASE=%boostlib%/boost_filesystem-%boostvcver%-mt-%boostver%.lib -DBoost_DATE_TIME_LIBRARY_DEBUG=%boostlib%/boost_date_time-%boostvcver%-mt-gd-%boostver%.lib -DBoost_DATE_TIME_LIBRARY_RELEASE=%boostlib%/boost_date_time-%boostvcver%-mt-%boostver%.lib -DBoost_MPI_LIBRARY_DEBUG=%boostlib%/boost_mpi-%boostvcver%-mt-gd-%boostver%.lib -DBoost_MPI_LIBRARY_RELEASE=%boostlib%/boost_mpi-%boostvcver%-mt-%boostver%.lib -DBoost_REGEX_LIBRARY_DEBUG=%boostlib%/boost_regex-%boostvcver%-mt-gd-%boostver%.lib -DBoost_REGEX_LIBRARY_RELEASE=%boostlib%/boost_regex-%boostvcver%-mt-%boostver%.lib -DBoost_SERIALIZATION_LIBRARY_DEBUG=%boostlib%/boost_serialization-%boostvcver%-mt-gd-%boostver%.lib -DBoost_SERIALIZATION_LIBRARY_RELEASE=%boostlib%/boost_serialization-%boostvcver%-mt-%boostver%.lib -DBoost_THREAD_LIBRARY_DEBUG=%boostlib%/boost_thread-%boostvcver%-mt-gd-%boostver%.lib -DBoost_THREAD_LIBRARY_RELEASE=%boostlib%/boost_thread-%boostvcver%-mt-%boostver%.lib
%cmakebuild%
