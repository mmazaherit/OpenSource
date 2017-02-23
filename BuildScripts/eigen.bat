IF "%~1"=="" (
 set eigenroot=%cd%
 set eigentag=3.3.2
)ElSE (
set eigenroot=%~1
)
IF "%~2"=="" ( 
 set eigentag=3.3.2
)ElSE (
set eigentag=%~2
)

set mypath=%~dp0


chdir %eigenroot%
%mypath%\wget.exe "http://bitbucket.org/eigen/eigen/get/%eigentag%.tar.gz"
%mypath%\7z.exe x  %eigentag%.tar.gz -so | %mypath%\7z.exe x -si -ttar
::eigen add junks to the folder name, must be removed
set cmd="dir eigen-eigen* /b"
FOR /F "tokens=*" %%i IN (' %cmd% ') DO SET eigendir=%%i
rename "%eigendir%" "eigen-eigen"
