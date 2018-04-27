echo off
echo echo %1 ^> sys/devices/platform/mcp_6442-user/force_cal > temp.txt
echo sleep 0.8 >> temp.txt
echo cat sys/devices/platform/mcp_6442-user/force_cal >> temp.txt
echo exit >> temp.txt
adb shell < temp.txt
del temp.txt