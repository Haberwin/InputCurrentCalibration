using System;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace InputCurrentCalibration
{
    public partial class Calibration : Form
    {
        public int Vi, ErrorStatus;
        public StringBuilder Feedback = new StringBuilder("", 3000);
        public string[] s;
        public double InputCurrent;
        private string cmd;
        public string GPIB_Address;
        private static string CmdPath = @"C:\Windows\System32\cmd.exe";
        Thread runCa;
        public Calibration()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            try { GetVi(); } catch
            {
                Output("GPIB address unavailable!");
                Result.Text = "FAIL";
                Result.BackColor = Color.Red;
            }
            
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {

            if(buttonStart.Text == "Start")
            { 
                LOG.Clear();
                Result.Text = "校准中";
                Result.BackColor = Color.Orange;
                Result.Update();
                buttonStart.Text = "Stop";
                buttonStart.Update();
                runCa = new Thread(Runcal);
                runCa.Start();
            }
            else if(buttonStart.Text == "Stop")
            {
                Result.Text = "停止";
                Result.BackColor = Color.Orange;
                Result.Update();
                buttonStart.Text = "Start";
                buttonStart.Update();
                runCa.Abort();
                Config("limit.bat","",0);
            }
            c2000.Text = "Waiting";
            c2000.BackColor = Color.Orange;
            c1040.Text = "Waiting";
            c1040.BackColor = Color.Orange;
            c800.Text = "Waiting";
            c800.BackColor = Color.Orange;
            c500.Text = "Waiting";
            c500.BackColor = Color.Orange;
            return;
        }
        public void Runcal()
        {
            Output("Start...");
            if (!GetVi())
            {
                return;
            }
            checkdevice(out bool Isconnect);
            if(!Isconnect)
            { return; }
            try
            {
                if (Config("limit.bat","",1))
                {
                    CalibCurrent();
                    if (!Config("limit.bat", "",0))
                    {
                        Result.Text = "Fail";
                        Result.BackColor = Color.Orange;
                    }
                }
            }
            catch (ThreadAbortException)
            {
                Output("校准中止。");
            }
            catch(Exception er)
            {
                Output(er.Message);
            }
            //Visa32.viClose(Vi);
            buttonStart.Text = "Start";
            buttonStart.Update();
        }

        public void closeVi(object sender,EventArgs e)
        {
            Visa32.viClose(Vi);
        }

        public bool GetVi()
        {
            //CalibrationCurrent.Properties.Resources.cmd2
            ErrorStatus = -1;
            GPIB_Address = GPIB.Text;
            Visa32.viOpenDefaultRM(out int defrm);
            ErrorStatus = Visa32.viOpen(defrm, "GPIB0::" + GPIB_Address + "::INSTR", 0, 1000, out Vi);
            if (ErrorStatus != 0)
            {
                Output("GPIB address unavailable!");
                Result.Text = "FAIL";
                Result.BackColor = Color.OrangeRed;
                return false;
            }
            Visa32.viPrintf(Vi, "*IDN?\n");
            Visa32.viScanf(Vi, "%t", Feedback);
            Output(Feedback.ToString());
            Result.Text = "已连接电源";
            Result.BackColor = Color.PaleGreen;
            Result.Update();
            return true;
        }

        public bool CalibCurrent()
        {
            int Data00 = 0;
            int Data01 = 0;
            int step = 1;
            double resultcurrent;
            bool Is2000=false, Is1040=false, Is800=false, Is500=false;
            double maxcurrent;
            
            for(int tempI = 0; tempI < 10; tempI++)
            {
                maxcurrent = steptest(0, 0);
                if (maxcurrent == 0)
                {
                    Result.Text = "FAIL";
                    Result.BackColor = Color.OrangeRed;
                    Output("寄存器操作失败。。。。。。");
                    return false;
                }
                else if(0<maxcurrent && maxcurrent < 0.5)
                {
                    Thread.Sleep(1000);
                    continue;
                }
                else if (0.5<maxcurrent && maxcurrent <= 1.95)
                {
                    Result.Text = "FAIL";
                    Result.BackColor = Color.OrangeRed;
                    Output("最大电流异常，请检查电池电量！");
                    return false;
                }
                else
                {
                    break;
                }
            }
            
            for(Data00=6;Data00<256;Data00=Data00+2-step)
            {
                for (Data01 = Data00; Data01 <= Data00 + step; Data01++)
                {
                    resultcurrent = steptest(Data00, Data01);
                    Current.Text = resultcurrent.ToString();
                    Current.Update();
                    if(!Is2000 && resultcurrent<= 2)
                    {
                        //Is2000 = Config("USB.bat", "config_data_ac_resistor", Data00 * 256 + Data01);
                        Is2000= setdata(Data00, Data01, rege: "06 07 ");
                        if (Is2000)
                        {
                            Output("WALL adapter 2A 校准成功！");
                            config_lable(c2000, true);
                            Data00 = Data01=Data00+14;
                        }
                        else
                        {
                            Output("WALL adapter 2A 校准失败！");
                            config_lable(c2000, false);
                            config_lable(Result, false);
                            Update();
                            return false;
                        }
                    }
                    if(!Is1040 && resultcurrent<=1.04)
                    {
                        Is1040 = setdata(Data00, Data01, rege: "02 03 ");
                        if (Is1040)
                        {
                            Output("WLC 1A  校准成功！");
                            config_lable(c1040, true);
                            Data00 = Data01=Data00+15;
                        }
                        else
                        {
                            Output("WLC 1A 校准失败！");
                            config_lable(c1040, false);
                            config_lable(Result, false);
                            Update();
                            return false;
                        }

                    }
                    if (!Is800 && resultcurrent <= 0.8)
                    {
                        //Is800 = Config("USB.bat", "config_data_wlc_5w_resistor", Data00 * 256 + Data01);
                        Is800 = setdata(Data00, Data01, rege: "08 09 ");
                        if (Is800)
                        {
                            Output(" WLC 6V, 800mA  校准成功！");
                            config_lable(c800, true);
                            Data00 = Data01=Data00+140;
                            step = 0;
                        }
                        else
                        {
                            Output(" WLC 6V, 800mA 校准失败！");
                            config_lable(c800, false);
                            config_lable(Result, false);
                            Update();
                            return false;
                        }

                    }
                    if (!Is500 && resultcurrent <= 0.5)
                    {
                        //Is500 = Config("USB.bat", "config_data_usb_resistor", Data00 * 256 + Data01);
                        Is500 = setdata(Data00, Data01, rege: "0A 0B ");
                        if (Is500)
                        {
                            Output("USB PC 500mA  校准成功！");
                            config_lable(c500, true);
                            config_lable(Result, true);
                            return true;
                        }
                        else
                        {
                            Output("USB PC 500mA 校准失败！");
                            config_lable(c500, false);
                            config_lable(Result, false);
                            Update();
                            return false;
                        }

                    }
                }
            }
            return false;
        }
        public void config_lable(Label lable,bool result)
        {
            if (result)
            {
                lable.Text = "Pass";
                lable.BackColor = Color.PaleGreen;
                Update();
            }
            else
            {
                lable.Text = "Fail";
                lable.BackColor = Color.Red;
                Update();
            }
        }

        public double steptest(int Data00,int Data01)
        {
            if (!setdata(Data00,Data01))
            {
                Result.Text = "FAIL";
                Result.BackColor = Color.OrangeRed;
                Output("未能成功写入寄存器");
                return 0;
            }
            Output00.Update();
            Feedback.Remove(0, Feedback.Length);
            Visa32.viPrintf(Vi, "MEAS:CURR:DC?; *WAI\n");
            Thread.Sleep(500);
            Visa32.viScanf(Vi, "%t", Feedback);
            s = Feedback.ToString().Split(',');
            InputCurrent = Convert.ToDouble(s[0]);
            return InputCurrent;
        }
        
        public bool setdata(int Data00,int Data01,string rege="00 01 ")
        {
            string output, error;
            string command=rege+Data00.ToString()+" "+Data01.ToString();
            bool Ischange = false;
            int i=0;
            string match = @"\r\n(\d+)";
            cmd = System.AppDomain.CurrentDomain.BaseDirectory;
            string path = cmd + "cmd.bat";
            using (Process p = new Process())
            {
                ProcessStartInfo pi = new ProcessStartInfo(path, command + " &exit")
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };//第二个参数为传入的参数，string类型以空格分隔各个参数  
                p.StartInfo = pi;
                p.Start();
                output = p.StandardOutput.ReadToEnd();
                error = p.StandardError.ReadToEnd();
                if (error != null && error.Length > 0)
                {
                    Output(error);
                    return false;
                }
                foreach (Match tempM in Regex.Matches(output, match))
                {
                    if (i == 0)
                    {
                        Output00.Text = tempM.Value.Remove(0, 2);
                        Ischange = tempM.Value.Remove(0, 2).ToString().Equals(Data00.ToString());
                        //Output(output.ToString());
                        Update();
                        Output("Register 00:" + tempM);
                    }
                    else
                    {
                        Output00.Text = tempM.Value.Remove(0, 2);
                        this.Update();
                        Output00.Update();
                        Output("Register 01 :" + tempM);
                        Ischange = tempM.Value.Remove(0, 2).ToString().Equals(Data01.ToString());

                        if (Ischange)
                        {

                            return true;
                        }
                        else
                        {
                            Output("未能成功写入寄存器");
                            return false;
                        }
                    }
                    i++;
                }
                p.WaitForExit();//等待程序执行完退出进程
                p.Close();
                return false;
            }    
        }
        public bool Config(string cmdname,string configpath,int data)
        {
            string output,error;
            bool Ischange = false;
            int i = 0;
            string match = @"\r\n(\d+)";
            cmd = System.AppDomain.CurrentDomain.BaseDirectory;
            string path = cmd + cmdname;
            using (Process p = new Process())
            {
                ProcessStartInfo pi = new ProcessStartInfo(path, data.ToString()+" "+ configpath  + " &exit")
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };//第二个参数为传入的参数，string类型以空格分隔各个参数  
                p.StartInfo = pi;
                p.Start();
                output = p.StandardOutput.ReadToEnd();
                error = p.StandardError.ReadToEnd();
                if (error != null && error.Length > 0)
                {
                    Output(error);
                    return false;
                }
                foreach (Match tempM in Regex.Matches(output, match))
                {
                    Output00.Text = tempM.Value.Remove(0, 2);
                    Ischange = tempM.Value.Remove(0, 2).ToString().Equals(data.ToString());
                    Update();
                    Output00.Update();
                    Output("写入数据成功:" + tempM);
                }
                p.WaitForExit();//等待程序执行完退出进程
                p.Close();
                return Ischange;
            }
        }

        public void checkdevice( out bool Isconnect)
        {
            string cmd, output, error;
            cmd ="adb devices &exit";//说明：不管命令是否成功均执行exit命令，否则当调用ReadToEnd()方法时，会处于假死状态
            string match = @"(.*?)\s+device";
            using (Process p = new Process())
            {
                p.StartInfo.FileName = CmdPath;
                p.StartInfo.UseShellExecute = false;        //是否使用操作系统shell启动
                p.StartInfo.RedirectStandardInput = true;   //接受来自调用程序的输入信息
                p.StartInfo.RedirectStandardOutput = true;  //由调用程序获取输出信息
                p.StartInfo.RedirectStandardError = true;   //重定向标准错误输出
                p.StartInfo.CreateNoWindow = true;          //不显示程序窗口
                p.Start();//启动程序
                //向cmd窗口写入命令
                p.StandardInput.WriteLine(cmd);
                p.StandardInput.AutoFlush = true;
                //获取cmd窗口的输出信息
                output = p.StandardOutput.ReadToEnd();
                error = p.StandardError.ReadToEnd();
                if (error != null && error.Length > 0)
                {
                    Output("请配置好adb 环境:" + error.ToString());
                    Isconnect = false;
                    
                }
                else
                {
                    MatchCollection temp = Regex.Matches(output, match);
                    if (temp.Count > 2)
                    {
                        Output("Device is connecting!");
                        Isconnect = true;
                    }
                    else
                    {
                        Output("No device found, please check connect!");
                        Isconnect = false;
                    }
                    
                }
                p.WaitForExit();//等待程序执行完退出进程
                p.Close();
            }
        }


        public void Output(string log)
        {
            LOG.AppendText(log + "\r\n"); //DateTime.Now.ToString("HH:mm:ss") + "  " +
            LOG.Update();
        }
        public void TextLimit(object sender, EventArgs e)
        {
            try
            {
                GetVi();             
            }
            catch
            {
                Output("GPIB address unavailable!");
                Result.Text = "FAIL";
                Result.BackColor = Color.Red;
            }
        }

        //科学计数转数字
        private Decimal ChangeDataToD(string strData)
        {
            Decimal dData = 0.0M;
            if (strData.Contains("E"))
            {
                dData = Convert.ToDecimal(Decimal.Parse(strData.ToString(), System.Globalization.NumberStyles.Float));
            }
            return dData;
        }
    }

    internal sealed class Visa32
    {
        // --------------------------------------------------------------------------------
        //  Adapted from visa32.bas which was distributed by VXIplug&play Systems Alliance
        //  Distributed By Agilent Technologies, Inc.
        // --------------------------------------------------------------------------------
        // -------------------------------------------------------------------------
        public const int VI_SPEC_VERSION = 4194304;
#pragma warning disable IDE1006 // 命名样式
        #region - Resource Template Functions and Operations ----------------------------
        [DllImportAttribute("VISA32.DLL", EntryPoint = "#141", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int viOpenDefaultRM(out int sesn);
        [DllImportAttribute("VISA32.DLL", EntryPoint = "#128", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int viGetDefaultRM(out int sesn);
        [DllImportAttribute("VISA32.DLL", EntryPoint = "#131", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int viOpen(int sesn, string viDesc, int mode, int timeout, out int vi);
        [DllImportAttribute("VISA32.DLL", EntryPoint = "#132", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int viClose(int vi);
        [DllImportAttribute("VISA32.DLL", EntryPoint = "#269", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern int viPrintf(int vi, string writeFmt);
        [DllImportAttribute("VISA32.DLL", EntryPoint = "#271", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern int viScanf(int vi, string readFmt, StringBuilder arg);
        #endregion
#pragma warning restore IDE1006 // 命名样式
    }

}
