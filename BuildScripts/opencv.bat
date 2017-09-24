IF "%~1"=="" (
 set opencvroot=%cd%
)ElSE (
set opencvroot=%~1
)
CALL config.bat

set mypath=%~dp0
set ver=3.3.0
chdir %opencvroot%
::%mypath%\wget.exe https://github.com/opencv/opencv/archive/%ver%.zip
%mypath%\7z.exe x %opencvroot%\%ver%.zip -o%opencvroot% *.* -r

cd opencv-%ver%
mkdir %buildfolder%
cd %buildfolder%
cmake .. -G %buildtype%
%cmakebuild%
