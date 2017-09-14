IF "%~1"=="" (
 set pclroot=%cd%
)ElSE (
set pclroot=%~1
)

::boost_1_63 vs12 OK
CALL config.bat

set pcl=%pclroot%/pcl



::"pcl-1.8.0" had issue with vtk7.1, so pcl-1.8.1rc1 branch is used for now
chdir %pclroot%
git clone --branch pcl-1.8.1rc1 https://github.com/PointCloudLibrary/pcl.git

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

::install 
CALL %mypath%eigen.bat %pclthirdparty% "3.3.3"

chdir %pcl% 
mkdir %buildfolder%
cd %buildfolder%

set builddir=%pcl%/%buildfolder%
set flann=%pclthirdparty%/flann
set flannbuild=%flann%/%buildfolder%
set zlib=%pclthirdparty%/zlib
set zlibbuild=%zlib%%buildfolder%
set qhull=%pclthirdparty%/qhull



cmake .. -G %buildtype% -DBUILD_visualization=off -DEIGEN_INCLUDE_DIR=%pclthirdparty%/eigen-eigen -DFLANN_INCLUDE_DIR=%flann%/src/cpp -DFLANN_LIBRARY=%flannbuild%/lib/Release/flann_s.lib -DFLANN_LIBRARY_DEBUG=%flannbuild%/lib/Debug/flann_s.lib -DZLIB_INCLUDE_DIR=%zlib% -DZLIB_LIBRARY_DEBUG=%zlibbuild%Debug/zlibd.lib -DZLIB_LIBRARY_RELEASE=%zlibbuild%/Debug/zlib.lib -DQHULL_INCLUDE_DIR=%qhull%/src -DQHULL_LIBRARY=%qhull%/%buildfolder%/Release/qhullstatic.lib -DQHULL_LIBRARY_DEBUG=%qhull%/%buildfolder%/Debug/qhullstatic.lib  -DBoost_INCLUDE_DIR=%boost%  -DBoost_LIBRARY_DIR=%boostlib%
%cmakebuild%
