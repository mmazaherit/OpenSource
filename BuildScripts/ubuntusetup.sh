#for ubuntu 14.04.5
#add rep
sudo add-apt-repository ppa:rabbitvcs/ppa -y
sudo add-apt-repository ppa:nathan-renniewaldock/flux -y
sudo sh -c 'echo "deb http://packages.ros.org/ros/ubuntu $(lsb_release -sc) main" > /etc/apt/sources.list.d/ros-latest.list'
sudo apt-key adv --keyserver hkp://ha.pool.sks-keyservers.net:80 --recv-key 421C365BD9FF1F717815A3895523BAEEB01FA116
sudo add-apt-repository ppa:xorg-edgers/ppa -y
sudo add-apt-repository ppa:plushuang-tw/uget-stable -y
sudo apt-get update

#install tools
sudo apt-get install g++ -y
sudo apt-get install aptitude -y
sudo apt-get install rpm synaptic fluxgui uget -y
sudo apt-get install git subversion yum -y
sudo apt-get install rar unrar -y
sudo apt-get install vlc -y
export VDPAU_DRIVER=vdpau
mkdir ~/thirdparty


#install cmake-gui from source, needs qt
#sudo apt-get purge cmake -y
sudo apt-get install qt4-default -y
wget https://cmake.org/files/v3.9/cmake-3.9.0.tar.gz -P ~/Downloads
cd ~/Downloads
tar -xzvf ~/Downloads/cmake-3.9.0.tar.gz
cd ~/Downloads/cmake-3.9.0
./configure --qt-gui 
make -j4
sudo make install

#install python from source
#version=2.7.13
#sudo apt-get install -y libbz2-dev zlib1g-dev
#sudo apt-get install -y libexpat1-dev libssl-dev libncurses5-dev  liblzma-dev  libsqlite3-dev libffi-dev tcl-dev libgdbm-dev libreadline-dev tk tk-dev libncurses5-dev libssl-dev
#sudo apt-get install libd-dev
#wget https://www.python.org/ftp/python/$version/Python-$version.tgz -P ~/Downloads
#cd ~/Downloads & tar xzf Python-$version.tgz
#cd ~/Downloads/Python-$version
#./configure --enable-optimizations --with-ensurepip=install 
#make 
#make install

#install anaconda, pyton + scientific packages
#wget https://repo.continuum.io/archive/Anaconda3-4.4.0-Linux-x86_64.sh -P ~/Downloads
#bash ~/Downloads/Anaconda3-4.4.0-Linux-x86_64.sh


#install qt, cmake gui needs it
wget http://download.qt.io/official_releases/online_installers/qt-unified-linux-x64-online.run -P ~/Downloads
chmod +x ~/Downloads/qt-unified-linux-x64-online.run
~/Downloads/qt-unified-linux-x64-online.run


#install eigen from source
wget http://bitbucket.org/eigen/eigen/get/3.3.4.tar.bz2 -P ~/Downloads
tar -xvjf ~/Downloads/3.3.4.tar.bz2 -C ~/Downloads
cd ~/Downloads/eigen-eigen-5a0156e40feb
mkdir build
cd build 
cmake ..
make 
sudo make install



#install rabbitvcs, need log off
sudo apt-get install rabbitvcs-nautilus -y
sudo apt-get install rabbitvcs-gedit rabbitvcs-cli -y
chown -R $USER:$USER ~/.config/rabbitvcs

#install ROS 
sudo apt-get install ros-kinetic-desktop-full -y
echo "source /opt/ros/kinetic/setup.bash" >> ~/.bashrc
source /opt/ros/kinetic/setup.bash
sudo rosdep init && rosdep update
sudo apt-get install python-rosinstall python-rosinstall-generator python-wstool build-essential ninja-build -y


#install standard ros packages, build with catkin_make
mkdir -p ~/catkin_ws/src
cd ~/catkin_ws/src
git clone https://github.com/swri-robotics/gps_umd.git
git clone https://github.com/ros-geographic-info/geographic_info.git 
cd ~/catkin_ws/
rosdep update
rosdep install --from-paths src --ignore-src --rosdistro=kinetic -y 
pip install catkin_pkg
catkin_make install

#non-standard packages install
mkdir -p ~/catkin_ns/src
cd ~/catkin_ns
wstool init src
wstool merge -t src https://raw.githubusercontent.com/googlecartographer/cartographer_ros/master/cartographer_ros.rosinstall
wstool update -t src
rosdep update
rosdep install --from-paths src --ignore-src --rosdistro=kinetic -y
catkin_make_isolated --install --use-ninja

#install virtualbox, secure boot must be disabled from bios
sudo sh -c 'echo "deb http://download.virtualbox.org/virtualbox/debian xenial contrib" >> /etc/apt/sources.list.d/virtualbox.list'
wget -q https://www.virtualbox.org/download/oracle_vbox_2016.asc -O- | sudo apt-key add -
wget -q https://www.virtualbox.org/download/oracle_vbox.asc -O- | sudo apt-key add -
#sudo apt-get remove virtualbox virtualbox-4.* virtualbox-5.0
sudo apt-get update
sudo apt-get install virtualbox-5.1 -y

#install samba
#sudo aptitude install samba samba-common system-config-samba python-glade2 gksu
#sudo cp /usr/share/samba/smb.conf /etc/samba/smb.conf
#sudo dpkg --configure -a
#sudo smbpasswd -a $USER
#sudo cp /etc/samba/smb.conf ~

#install chrome
wget https://dl.google.com/linux/direct/google-chrome-stable_current_amd64.deb -P ~/Downloads
sudo dpkg -i --force-all ~/Downloads/google-chrome*.deb
#install pycharm
rm -r ~/.PyCharm*/
wget https://download.jetbrains.com/python/pycharm-community-2017.2.1.tar.gz -P ~/Downloads
cd ~/Downloads && tar xzf pycharm-community-2017.2.1.tar.gz
cd  ~/Downloads/pycharm-community-2017.2.1/bin
#install onedrive
cd ~/thirdparty
git clone https://github.com/xybu92/onedrive-d.git
cd onedrive-d
./install-sh -y
onedrive-pref

#add the following to startup
#onedrive-d start

