#for ubuntu 14.04.5
#add rep
sudo add-apt-repository ppa:rabbitvcs/ppa -y
sudo add-apt-repository ppa:nathan-renniewaldock/flux -y
sudo sh -c 'echo "deb http://packages.ros.org/ros/ubuntu $(lsb_release -sc) main" > /etc/apt/sources.list.d/ros-latest.list'
sudo apt-key adv --keyserver hkp://ha.pool.sks-keyservers.net:80 --recv-key 421C365BD9FF1F717815A3895523BAEEB01FA116
sudo add-apt-repository ppa:xorg-edgers/ppa -y
sudo add-apt-repository ppa:plushuang-tw/uget-stable -y
sudo apt-get update

sudo apt-get install g++ -y
mkdir ~/thirdparty

#install cmake-gui from source, needs qt
#sudo apt-get purge cmake -y
sudo apt-get install qt4-default -y
wget https://cmake.org/files/v3.9/cmake-3.9.0.tar.gz -P ~/thirdparty
cd ~/thirdparty
tar -xzvf ~/thirdparty/cmake-3.9.0.tar.gz
cd ~/thirdparty/cmake-3.9.0
./configure --qt-gui 
make -j4
sudo make install

#install python3 from source
sudo apt-get install -y libbz2-dev zlib1g-dev
wget https://www.python.org/ftp/python/3.6.2/Python-3.6.2.tgz -P ~/thirdparty
cd ~/thirdparty
tar xzf Python-3.6.2.tgz
cd Python-3.6.2
./configure --enable-optimizations --with-ensurepip=install
sudo make install


#install qt, cmake gui needs it
wget http://download.qt.io/official_releases/online_installers/qt-unified-linux-x64-online.run -P ~/thirdparty
chmod +x ~/thirdparty/qt-unified-linux-x64-online.run
~/thirdparty/qt-unified-linux-x64-online.run


#install eigen from source
wget http://bitbucket.org/eigen/eigen/get/3.3.4.tar.bz2 -P ~/thirdparty
tar -xvjf ~/thirdparty/3.3.4.tar.bz2 -C ~/thirdparty
cd ~/thirdparty/eigen-eigen-5a0156e40feb
mkdir build
cd build 
cmake ..
make 
sudo make install

#install tools
sudo apt-get install rpm synaptic fluxgui vlc uget -y
sudo apt-get install git subversion yum -y
sudo apt-get install rar unrar -y

#install rabbitvcs
sudo apt-get install rabbitvcs-nautilus3 rabbitvcs-nautilus rabbitvcs-thunar rabbitvcs-gedit rabbitvcs-cli -y
chown -R $USER:$USER ~/.config/rabbitvcs

#install ROS indigo
sudo apt-get install ros-indigo-desktop-full -y
echo "source /opt/ros/indigo/setup.bash" >> ~/.bashrc
source /opt/ros/indigo/setup.bash
sudo rosdep init
rosdep update
sudo apt-get install -y python-rosinstall python-wstool python-rosdep ninja-build


#install standard ros packages, build with catkin_make
mkdir -p ~/catkin_ws/src
cd ~/catkin_ws/src
git clone https://github.com/swri-robotics/gps_umd.git
git clone https://github.com/ros-geographic-info/geographic_info.git 
cd ~/catkin_ws/
rosdep update
rosdep install --from-paths src --ignore-src --rosdistro=indigo -y 
catkin_make install

#non-standard packages install
mkdir -p ~/catkin_ns/src
cd ~/catkin_ns
wstool init src
wstool merge -t src https://raw.githubusercontent.com/googlecartographer/cartographer_ros/master/cartographer_ros.rosinstall
wstool update -t src
rosdep update
rosdep install --from-paths src --ignore-src --rosdistro=indigo -y
catkin_make_isolated --install --use-ninja

#install virtualbox, secure boot must be disabled from bios
sudo sh -c 'echo "deb http://download.virtualbox.org/virtualbox/debian $(lsb_release -cs) contrib" >> /etc/apt/sources.list.d/virtualbox.list'
wget -q https://www.virtualbox.org/download/oracle_vbox_2016.asc -O- | sudo apt-key add -
wget -q https://www.virtualbox.org/download/oracle_vbox.asc -O- | sudo apt-key add -
#sudo apt-get remove virtualbox virtualbox-4.* virtualbox-5.0
sudo apt-get update
sudo apt-get install virtualbox-5.1 -y






