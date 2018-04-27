echo off
echo echo %1 ^> sys/devices/platform/mcp_6442-user/reg > temp.txt
echo echo %3 ^> sys/devices/platform/mcp_6442-user/data >> temp.txt
echo sleep 0.8 >>temp.txt
echo echo %2 ^> sys/devices/platform/mcp_6442-user/reg >> temp.txt
echo echo %4 ^> sys/devices/platform/mcp_6442-user/data >> temp.txt
echo echo %1 ^> sys/devices/platform/mcp_6442-user/reg >> temp.txt
echo cat sys/devices/platform/mcp_6442-user/data >> temp.txt
echo echo %2 ^> sys/devices/platform/mcp_6442-user/reg >> temp.txt
echo cat sys/devices/platform/mcp_6442-user/data >> temp.txt
echo exit >> temp.txt
adb shell < temp.txt
del temp.txt



