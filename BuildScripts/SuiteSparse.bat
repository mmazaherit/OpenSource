set thirdparty=d:\ThirdParty\

cd %thirdparty%

::git clone https://github.com/jlblancoc/suitesparse-metis-for-windows.git

::download suitespasrse and replace the suitesparse folder
::download metis 5.1.0 and replace the metis folder
:: in the gk_arch.h  comment the line //#define rint(x) ((int)((x)+0.5))
cd suitesparse-metis-for-windows
mkdir build
cd build


cmake .. -G "Visual Studio 14 Win64"

pause>nul
