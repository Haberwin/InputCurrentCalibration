echo off
echo echo %1 ^> /sys/devices/platform/config_data/%2 > temp.txt
echo sleep 0.8 >> temp.txt
echo cat /sys/devices/platform/config_data/%2 >> temp.txt
echo exit >> temp.txt
adb shell < temp.txt
del temp.txt