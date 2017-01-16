set srcdir=D:\ThirdParty\vtk-master\
set builddir=%srcdir%build

set qdir=D:/ThirdParty/Qt/5.7/msvc2013/
set qmake=%qdir%bin/qmake.exe
set qcmakedir=%qdir%lib/cmake/Qt5

mkdir %builddir%
chdir %builddir%
cmake .. -G "Visual Studio 12" -DVTK_QT_VERSION=5 -DQt5_DIR=%qcmakedir%  -DQT_QMAKE_EXECUTABLE=%qmake% -DModule_vtkIOExportOpenGL2=1 -DModule_vtkRenderingLICOpenGL2=1 -DVTK_BUILD_QT_DESIGNER_PLUGIN=1 -DVTK_Group_Qt=1 -DModule_vtkGUISupportQtOpenGL=1

pause>nul