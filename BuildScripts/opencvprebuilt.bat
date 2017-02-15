IF "%~1"=="" (
 set opencvroot=%cd%
)ElSE (
set opencvroot=%~1
)

set mypath=%~dp0

chdir %opencvroot%
%mypath%\wget.exe https://github.com/opencv/opencv/releases/download/3.1.0/opencv-3.1.0.exe
%mypath%\7z.exe x %opencvroot%\opencv-3.1.0.exe -o%opencvroot% opencv\build\*.* -r
