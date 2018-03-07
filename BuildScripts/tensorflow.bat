IF "%~1"=="" (
 set tfroot=%cd%
)ElSE (
set tfroot=%~1
)
::cudnn must be installed by downloading from nvidia and extract the files in appropriate folders
::run this batchfile in visual studo cmd "C:\Program Files (x86)\Microsoft Visual Studio 14.0\VC\bin\amd64\vcvarsall.bat"
::wheel and numpy must be installed
pip3 install numpy
pip3 install wheel

set SWIG_EXECUTABLE="C:/tools/swigwin-3.0.10/swig.exe"
set PYTHON_PATH="C:/Program Files/Python35"
set CUDA_VER=9.0
set CUDNN_VER=7.0

CALL config.bat
chdir %tfroot%
git clone https://github.com/tensorflow/tensorflow.git

set /p tmp=Goto tensorflow\third_party\gpus\cuda\ and open “cuda_config.h” and only select your gpu cuda capabilities, then press any key ...

cd tensorflow
cd tensorflow
cd contrib
cd cmake 
mkdir build
cd build

cmake .. -G %buildtype%  make .. -A x64 -DCMAKE_BUILD_TYPE=Release -DSWIG_EXECUTABLE=%SWIG_EXECUTABLE% -DPYTHON_EXECUTABLE=%PYTHON_PATH%/python.exe -DPYTHON_LIBRARIES=%PYTHON_PATH%/libs/python35.lib  -Dtensorflow_ENABLE_GPU=ON -Dtensorflow_WIN_CPU_SIMD_OPTIONS=/arch:AVX2 -Dtensorflow_CUDA_VERSION=%CUDA_VER% -Dtensorflow_CUDNN_VERSION=%CUDNN_VER%

::to avoid memory overflow only 1 thread is used
msbuild.exe tf_python_build_pip_package.vcxproj /m:1 /t:Build /verbosity:detailed /p:Configuration=Release;Platform=x64 


::pip install "C:\tensorflow\tensorflow\contrib\cmake\build\tf_python\dist\tensorflow_gpu-1.5.0-cp35-cp35m-win_amd64.whl"