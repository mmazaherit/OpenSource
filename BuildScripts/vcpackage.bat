cd D:\ThirdParty
git clone https://github.com/Microsoft/vcpkg
cd vcpkg
CALL .\bootstrap-vcpkg.bat
.\vcpkg integrate install

set conf=:x64-windows 
.\vcpkg install eigen3%conf%
.\vcpkg install opencv%conf%
.\vcpkg install boost%conf%
.\vcpkg install vtk%conf%
.\vcpkg install qt5%conf%
.\vcpkg install glog%conf%
.\vcpkg install gflags%conf%
.\vcpkg install gdal%conf%
