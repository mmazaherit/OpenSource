set thirdparty=D:\ThirdParty\
set srcdir=%thirdparty%pcl-master\
set builddir=%srcdir%buildWin32
set flann=%thirdparty%flann-1.8.4-src\
set flannbuild=%flann%buildWin32\
set zlib=%thirdparty%zlib-1.2.8\
set zlibbuild=%zlib%buildWin32\
set qhull=%thirdparty%qhull-master\
set vtk=%thirdparty%vtk-master\build\

::boost settings
set boostver=1_60
set boost=%thirdparty%boost_%boostver%_0
set boostlib=%boost%\libWin32-msvc-12.0\
set vcver=vc120



mkdir %builddir%
chdir %builddir%



cmake .. -G "Visual Studio 12" -DVTK_DIR=%vtk% -DEIGEN_INCLUDE_DIR=%thirdparty%eigen-eigen -DFLANN_INCLUDE_DIR=%flann%src\cpp -DFLANN_LIBRARY=%flannbuild%lib\Release\flann.lib -DFLANN_LIBRARY_DEBUG=%flannbuild%lib\Debug\flann.lib -DZLIB_INCLUDE_DIR=%zlib% -DZLIB_LIBRARY_DEBUG=%zlibbuild%Debug\zlibd.lib -DZLIB_LIBRARY_RELEASE=%zlibbuild%Debug\zlib.lib -DQHULL_INCLUDE_DIR=%qhull%src -DQHULL_LIBRARY=%qhull%lib\qhull.lib -DQHULL_LIBRARY_DEBUG=%qhull%lib\qhullstatic_d.lib  -DBoost_INCLUDE_DIR=%boost% -DBoost_IOSTREAMS_LIBRARY_DEBUG=%boostlib%boost_iostreams-%vcver%-mt-gd-%boostver%.lib -DBoost_IOSTREAMS_LIBRARY_RELEASE=%boostlib%boost_iostreams-%vcver%-mt-%boostver%.lib -DBoost_SYSTEM_LIBRARY_DEBUG=%boostlib%boost_system-%vcver%-mt-gd-%boostver%.lib -DBoost_SYSTEM_LIBRARY_RELEASE=%boostlib%boost_system-%vcver%-mt-%boostver%.lib -DBoost_ATOMIC_LIBRARY_DEBUG=%boostlib%boost_atomic-%vcver%-mt-gd-%boostver%.lib -DBoost_ATOMIC_LIBRARY_RELEASE=%boostlib%boost_atomic-%vcver%-mt-%boostver%.lib -DBoost_CHRONO_LIBRARY_DEBUG=%boostlib%boost_chrono-%vcver%-mt-gd-%boostver%.lib -DBoost_CHRONO_LIBRARY_RELEASE=%boostlib%boost_chrono-%vcver%-mt-%boostver%.lib -DBoost_FILESYSTEM_LIBRARY_DEBUG=%boostlib%boost_filesystem-%vcver%-mt-gd-%boostver%.lib -DBoost_FILESYSTEM_LIBRARY_RELEASE=%boostlib%boost_filesystem-%vcver%-mt-%boostver%.lib -DBoost_DATE_TIME_LIBRARY_DEBUG=%boostlib%\boost_date_time-%vcver%-mt-gd-%boostver%.lib -DBoost_DATE_TIME_LIBRARY_RELEASE=%boostlib%boost_date_time-%vcver%-mt-%boostver%.lib -DBoost_MPI_LIBRARY_DEBUG=%boostlib%boost_mpi-%vcver%-mt-gd-%boostver%.lib -DBoost_MPI_LIBRARY_RELEASE=%boostlib%boost_mpi-%vcver%-mt-%boostver%.lib -DBoost_REGEX_LIBRARY_DEBUG=%boostlib%boost_regex-%vcver%-mt-gd-%boostver%.lib -DBoost_REGEX_LIBRARY_RELEASE=%boostlib%boost_regex-%vcver%-mt-%boostver%.lib -DBoost_SERIALIZATION_LIBRARY_DEBUG=%boostlib%\boost_serialization-%vcver%-mt-gd-%boostver%.lib -DBoost_SERIALIZATION_LIBRARY_RELEASE=%boostlib%boost_serialization-%vcver%-mt-%boostver%.lib -DBoost_THREAD_LIBRARY_DEBUG=%boostlib%boost_thread-%vcver%-mt-gd-%boostver%.lib -DBoost_THREAD_LIBRARY_RELEASE=%boostlib%boost_thread-%vcver%-mt-%boostver%.lib
pause>nul