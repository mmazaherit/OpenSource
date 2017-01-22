:: for this package you need to build VTK in thirdparty folder as well as downloading the eigen in thirdparty


set thirdparty=D:\ThirdParty\
set pcl=D:\ThirdParty\pcl\

set buildfolder=buildx64
set buildtype="Visual Studio 14 Win64"
::boost settings
set boostver=1_61
set boost=D:\ThirdParty\boost_%boostver%_0
set boostlib=%boost%\lib64-msvc-14.0\
set vcver=vc140
set vtk=%thirdparty%VTK\%buildfolder%\

chdir %thirdparty%
git clone https://github.com/PointCloudLibrary/pcl.git

cd pcl
mkdir ThirdParty
cd ThirdParty
::download flann
git clone git://github.com/mariusmuja/flann.git

cd flann
mkdir %buildfolder%
cd %buildfolder%

cmake .. -G %buildtype%
cmake --build . --target ALL_BUILD --config Release
cmake --build . --target ALL_BUILD --config Debug

::download zlib
cd ..
cd ..
git clone https://github.com/madler/zlib.git
cd zlib
mkdir %buildfolder%
cd %buildfolder%
cmake .. -G %buildtype%
cmake --build . --target ALL_BUILD --config Release
cmake --build . --target ALL_BUILD --config Debug

::qhull
cd ..
cd ..
git clone https://github.com/qhull/qhull.git
cd qhull
mkdir %buildfolder%
cd %buildfolder%
cmake .. -G %buildtype%
cmake --build . --target ALL_BUILD --config Release
cmake --build . --target ALL_BUILD --config Debug


chdir %pcl% 
mkdir %buildfolder%
cd %buildfolder%

set builddir=%pcl%%buildfolder%\
set flann=%pcl%ThirdParty\flann\
set flannbuild=%flann%%buildfolder%\
set zlib=%pcl%ThirdParty\zlib\
set zlibbuild=%zlib%%buildfolder%\
set qhull=%pcl%ThirdParty\qhull\



cmake .. -G %buildtype% -DVTK_DIR=%vtk% -DEIGEN_INCLUDE_DIR=%thirdparty%eigen-eigen -DFLANN_INCLUDE_DIR=%flann%src\cpp -DFLANN_LIBRARY=%flannbuild%lib\Release\flann_s.lib -DFLANN_LIBRARY_DEBUG=%flannbuild%lib\Debug\flann_s.lib -DZLIB_INCLUDE_DIR=%zlib% -DZLIB_LIBRARY_DEBUG=%zlibbuild%Debug\zlibd.lib -DZLIB_LIBRARY_RELEASE=%zlibbuild%Debug\zlib.lib -DQHULL_INCLUDE_DIR=%qhull%src -DQHULL_LIBRARY=%qhull%%buildfolder%\Release\qhullstatic.lib -DQHULL_LIBRARY_DEBUG=%qhull%%buildfolder%\Debug\qhullstatic.lib  -DBoost_INCLUDE_DIR=%boost% -DBoost_IOSTREAMS_LIBRARY_DEBUG=%boostlib%boost_iostreams-%vcver%-mt-gd-%boostver%.lib -DBoost_IOSTREAMS_LIBRARY_RELEASE=%boostlib%boost_iostreams-%vcver%-mt-%boostver%.lib -DBoost_SYSTEM_LIBRARY_DEBUG=%boostlib%boost_system-%vcver%-mt-gd-%boostver%.lib -DBoost_SYSTEM_LIBRARY_RELEASE=%boostlib%boost_system-%vcver%-mt-%boostver%.lib -DBoost_ATOMIC_LIBRARY_DEBUG=%boostlib%boost_atomic-%vcver%-mt-gd-%boostver%.lib -DBoost_ATOMIC_LIBRARY_RELEASE=%boostlib%boost_atomic-%vcver%-mt-%boostver%.lib -DBoost_CHRONO_LIBRARY_DEBUG=%boostlib%boost_chrono-%vcver%-mt-gd-%boostver%.lib -DBoost_CHRONO_LIBRARY_RELEASE=%boostlib%boost_chrono-%vcver%-mt-%boostver%.lib -DBoost_FILESYSTEM_LIBRARY_DEBUG=%boostlib%boost_filesystem-%vcver%-mt-gd-%boostver%.lib -DBoost_FILESYSTEM_LIBRARY_RELEASE=%boostlib%boost_filesystem-%vcver%-mt-%boostver%.lib -DBoost_DATE_TIME_LIBRARY_DEBUG=%boostlib%\boost_date_time-%vcver%-mt-gd-%boostver%.lib -DBoost_DATE_TIME_LIBRARY_RELEASE=%boostlib%boost_date_time-%vcver%-mt-%boostver%.lib -DBoost_MPI_LIBRARY_DEBUG=%boostlib%boost_mpi-%vcver%-mt-gd-%boostver%.lib -DBoost_MPI_LIBRARY_RELEASE=%boostlib%boost_mpi-%vcver%-mt-%boostver%.lib -DBoost_REGEX_LIBRARY_DEBUG=%boostlib%boost_regex-%vcver%-mt-gd-%boostver%.lib -DBoost_REGEX_LIBRARY_RELEASE=%boostlib%boost_regex-%vcver%-mt-%boostver%.lib -DBoost_SERIALIZATION_LIBRARY_DEBUG=%boostlib%\boost_serialization-%vcver%-mt-gd-%boostver%.lib -DBoost_SERIALIZATION_LIBRARY_RELEASE=%boostlib%boost_serialization-%vcver%-mt-%boostver%.lib -DBoost_THREAD_LIBRARY_DEBUG=%boostlib%boost_thread-%vcver%-mt-gd-%boostver%.lib -DBoost_THREAD_LIBRARY_RELEASE=%boostlib%boost_thread-%vcver%-mt-%boostver%.lib
cmake --build . --target ALL_BUILD --config Release 
cmake --build . --target ALL_BUILD --config Debug 