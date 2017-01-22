IF "%~1"=="" (
 set vtkroot=%cd%
)ElSE (
set vtkroot=%~1
)

set buildfolder=buildx64
set qdir=D:/ThirdParty/Qt/5.7/msvc2015_64/
set qmake=%qdir%bin/qmake.exe
set qcmakedir=%qdir%lib/cmake/Qt5

chdir %vtkroot%
git clone https://github.com/Kitware/VTK.git

cd VTK
mkdir %buildfolder%
cd %buildfolder%


cmake .. -G "Visual Studio 14 Win64" -DCMAKE_CXX_FLAGS="/MP6" -DVTK_QT_VERSION=5 -DQt5_DIR=%qcmakedir%  -DQT_QMAKE_EXECUTABLE=%qmake% -DModule_vtkIOExportOpenGL2=1 -DModule_vtkRenderingLICOpenGL2=1 -DVTK_BUILD_QT_DESIGNER_PLUGIN=1 -DVTK_Group_Qt=1 -DModule_vtkGUISupportQtOpenGL=1

cmake --build . --target ALL_BUILD --config Release -- /m
cmake --build . --target ALL_BUILD --config Debug -- /m