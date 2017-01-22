IF "%~1"=="" (
 set eigenroot=%cd%
)ElSE (
set eigenroot=%~1
)
set mypath=%~dp0

set eigenpackage=3.3.2

chdir %eigenroot%
%mypath%\wget.exe "http://bitbucket.org/eigen/eigen/get/%eigenpackage%.tar.gz"
%mypath%\7z.exe x  %eigenpackage%.tar.gz -so | %mypath%\7z.exe x -si -ttar
::eigen add junks to the folder name, must be removed
set cmd="dir eigen-eigen* /b"
FOR /F "tokens=*" %%i IN (' %cmd% ') DO SET eigendir=%%i
rename "%eigendir%" "eigen-eigen"
