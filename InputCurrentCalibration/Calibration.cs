using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace InputCurrentCalibration
{
    public partial class Calibration : Form
    {
        public int Vi, ErrorStatus=-1;
        public StringBuilder Feedback = new StringBuilder("", 3000);
        public StringBuilder instr = new StringBuilder("INSTR");
        public delegate void SendLog(string str);
        public delegate void ConfigObject(Control label, string text, Color color);
        public string[] s;
        public double InputCurrent;
        private string cmd;
        public string GPIB_Address;
        private static string CmdPath = @"C:\Windows\System32\cmd.exe";
        Process AudioTest= new Process();
        public bool End_Out=true;
        Thread runCa;
        public Calibration()
        {
            InitializeComponent();
            //Control.CheckForIllegalCrossThreadCalls = false;
            Output("GPIB address unavailable!");
            Result.Text = "未开始";
            Result.BackColor = Color.Red;
            
            
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {

            if(buttonStart.Text == "开始")
            { 
                LOG.Clear();
                Result.Text = "等待中";
                Result.BackColor = Color.Orange;
                Result.Update();
                buttonStart.Text = "结束";
                buttonStart.Update();
                runCa = new Thread(Runcal);
                runCa.Start();
            }
            else if(buttonStart.Text == "结束")
            {
                Result.Text = "停止";
                Result.BackColor = Color.Orange;
                Result.Update();
                buttonStart.Text = "开始";
                buttonStart.Update();
                runCa.Abort();
                runCa.Join();
                
            }
            c2000.Text = "待校准";
            c2000.BackColor = Color.Orange;
            c1040.Text = "待校准";
            c1040.BackColor = Color.Orange;
            c800.Text = "待校准";
            c800.BackColor = Color.Orange;
            c500.Text = "待校准";
            c500.BackColor = Color.Orange;
            return;
        }
        public void Runcal()
        {
            string sn_no = "";
            Output("Start...");
            
            if (!GetVi())
            {
                Config_Lable(buttonStart, "开始",Color.White);
                //buttonStart.Text = "Start";
                //buttonStart.Update();
                return;
            }
            Thread.Sleep(1000);
            
            while (true)
            {
               
                Checkdevice(out string Sn);
                if (Sn.Equals("") || Sn.Equals(sn_no))
                {
                    if (Sn.Equals(""))
                    {
                        Config_Lable(Result, "等待中", Color.Orange);
                       

                    }

                    //buttonStart.Text = "Start";
                    //buttonStart.Update();
                    Output("Wait For new device ....");
                    Thread.Sleep(3000);
                    
                    continue;
                }
                else
                {
                    Config_Lable(Result, "校准中",Color.Orange);
                    Config_Lable(c2000, "待校准", Color.Orange);
                    Config_Lable(c1040, "待校准", Color.Orange);
                    Config_Lable(c800, "待校准", Color.Orange);
                    Config_Lable(c500, "待校准", Color.Orange);
                    /*
                    Result.Text = "校准中";
                    Result.BackColor = Color.Orange;
                    c2000.Text = "Waiting";
                    c2000.BackColor = Color.Orange;
                    c1040.Text = "Waiting";
                    c1040.BackColor = Color.Orange;
                    c800.Text = "Waiting";
                    c800.BackColor = Color.Orange;
                    c500.Text = "Waiting";
                    c500.BackColor = Color.Orange;
                    */
                    sn_no = Sn;
                    Output("Find device " + Sn );
                }
                try
                {
                    
                    if (Config("limit.bat", "", 1))
                    {
                        
                        bool IsSuccess = CalibCurrent();
                        if (!Config("limit.bat", "", 0))
                        {
                            Config_Lable(Result, "失败", Color.Orange);
                           // Result.Text = "Fail";
                            //Result.BackColor = Color.Orange;
                        }
                        else
                        {
                            if (IsSuccess)
                            {
                                Config_lable(Result, true);
                              
                            }
                            else
                            {
                                Config_lable(Result, false);
                            }

                        }
                        
                       
                }
                 
                }
                catch (ThreadAbortException)
                {
                    Setdata(0, 0, rege: "0A 0B ");
                    Output("校准中止。");
                    Config("limit.bat", "", 0);
                    break;
                }
                catch (Exception er)
                {

                    Output(er.Message);
                    Config("limit.bat", "", 0);
                    break;
                }
                

            }
            Config_Lable(c2000, "待校准", Color.Orange);
            Config_Lable(c1040, "待校准", Color.Orange);
            Config_Lable(c800, "待校准", Color.Orange);
            Config_Lable(c500, "待校准", Color.Orange);
            Config_Lable(buttonStart, "开始", Color.White);
            //Visa32.viClose(Vi);
           // buttonStart.Text = "Start";
            //Visa32.viClose(Vi);
            //buttonStart.Update();
        }

        public void CloseVi(object sender,EventArgs e)
        {
            //Visa32.viClose(Vi);
        }

        public bool GetVi()
        {
            //CalibrationCurrent.Properties.Resources.cmd2
            //ErrorStatus = -1;
            short t1 = 1, t2 = 0;
            GPIB_Address = GPIB.Text;
            Visa32.viGetDefaultRM(out int defrm);
            Thread.Sleep(200);
            if(ErrorStatus != 0)
            {
                Visa32.viParseRsrcEx(defrm, "GPIB0::" + GPIB_Address + "::0::INSTR", ref t1, ref t2, instr, null, null);
                ErrorStatus = Visa32.viOpen(defrm, "GPIB0::" + GPIB_Address + "::0::INSTR", 1, 3000, out Vi);
                if (ErrorStatus != 0)
                {
                    Output(ErrorStatus.ToString());
                    Output("GPIB address unavailable!");
                    Config_Lable(Result, "失败", Color.Red);
                    //Result.Text = "FAIL";
                    //Result.BackColor = Color.OrangeRed;
                    return false;
                }
            }
            
            
            Feedback.Remove(0, Feedback.Length);
            Visa32.viPrintf(Vi, "*IDN?"+ System.Environment.NewLine);
            Thread.Sleep(500);
            Visa32.viScanf(Vi, "%t", Feedback);
            Output(Feedback.ToString());
            Config_Lable(Result, "已连接", Color.PaleGreen);
            //Result.Text = "Connecting";
            //Result.BackColor = Color.PaleGreen;
            //Result.Update();
            return true;
        }

        public bool CalibCurrent()
        {
            int Data00 = 0;
            int Data01 = 0;
            int step00 = 1;
            int step01 = 1;
            double resultcurrent;
            bool Is2000=false, Is1040=false, Is800=false, Is500=false,Isversion=false;
            double maxcurrent;
            Isversion = Setdata(35, 35, rege: "12 12 ");
            if (Isversion)
            {
                Output("成功写入版本号35！");
            }
            else
            {
                Output("未能写入版本号！");
                Setdata(255, 255, rege: "06 07 ");
                Setdata(255, 255, rege: "10 11 ");
                Setdata(255, 255, rege: "08 09 ");
                Setdata(255, 255, rege: "02 03 ");
                Config_Lable(Result, "失败", Color.Red);
                //Result.Text = "FAIL";
                //Result.BackColor = Color.OrangeRed;
                return false;
            }

            for (int tempI = 0; tempI < 12; tempI++)
            {
                maxcurrent = Steptest(0, 0);
                Config_Lable(Current, maxcurrent.ToString(), Color.White);
                //Current.Text = maxcurrent.ToString();
                //Current.Update();
                if (maxcurrent == 0)
                {
                    Config_Lable(Result, "失败", Color.Red);
                    //Result.Text = "FAIL";
                    //Result.BackColor = Color.OrangeRed;
                    Output("寄存器操作失败。。。。。。");
                    return false;
                }
                else if (0 < maxcurrent && maxcurrent <= 0.8)
                {
                    if (tempI == 11)
                    {
                        Output("电池电量太满，无法校准");
                        return false;
                    }
                    Thread.Sleep(1000);
                    continue;
                }
                else if (0.8 < maxcurrent && maxcurrent <= 1.90)
                {
                    if (tempI == 11)
                    {
                        Config_Lable(Result, "失败", Color.Red);
                        //Result.Text = "FAIL";
                        //Result.BackColor = Color.OrangeRed;
                        Output("最大电流异常，请检查电池电量！");
                        return false;
                    }
                    Thread.Sleep(1000);
                    continue;
                }
                else{
                    break;
                }
            }
            
            
            for(Data00=3;Data00<255;Data00=Data00+step00)
            {
                for (Data01 = Data00; Data01 <= Data00 + step01; Data01++)
                {
                    if (Data00 == 128)
                    {
                        break;
                    }
                    resultcurrent = Steptest(Data00, Data01);
                    Config_Lable(Current, resultcurrent.ToString(), Color.White);
                    //Current.Text = resultcurrent.ToString();
                    //Current.Update();
                    if(!Is2000 && resultcurrent<= 2)
                    {

                        if (resultcurrent < 1.92)
                        {
                            Data00 = Data00 - 2;
                            break;
                        }
                        //Is2000 = Config("USB.bat", "config_data_ac_resistor", Data00 * 256 + Data01);
                        Is2000 = Setdata(Data00, Data01, rege: "06 07 ");
                        if (Is2000)
                        {
                            Output("WALL adapter 2A 校准成功！");
                            Config_lable(c2000, true);
                            Data00 = Data01=Data00+15;
                        }
                        else
                        {
                            Output("WALL adapter 2A 校准失败！");
                            Setdata(255, 255, rege: "06 07 ");
                            Setdata(255, 255, rege: "10 11 ");
                            Setdata(255, 255, rege: "08 09 ");
                            Setdata(255, 255, rege: "02 03 ");
                            Config_lable(c2000, false);
                            Config_lable(Result, false);
                            Update();
                            return false;
                        }
                    }
                    if(!Is1040 && resultcurrent<=1.04)
                    {
                        if (resultcurrent < 1.01)
                        {
                            Data00 = Data00 - 2;
                            break;
                        }
                        Is1040 = Setdata(Data00, Data01, rege: "10 11 ");
                        if (Is1040)
                        {
                            Output("WLC 1A  校准成功！");
                            Config_lable(c1040, true);
                            Data00 = Data01=Data00+7;
                            step01 = 0;
                        }
                        else
                        {
                            Output("WLC 1A 校准失败！");
                            //Setdata(255, 255, rege: "06 07 ");
                            Setdata(255, 255, rege: "10 11 ");
                            Setdata(255, 255, rege: "08 09 ");
                            Setdata(255, 255, rege: "02 03 ");
                            Config_lable(c1040, false);
                            Config_lable(Result, false);
                            Update();
                            return false;
                        }

                    }
                    if (!Is800 && resultcurrent <= 0.8)
                    {
                        if (resultcurrent < 0.78)
                        {
                            Data00=Data01= Data00 - 2;
                            break;
                        }
                        //Is800 = Config("USB.bat", "config_data_wlc_5w_resistor", Data00 * 256 + Data01);
                        Is800 = Setdata(Data00, Data01, rege: "08 09 ");
                        if (Is800)
                        {
                            Output(" WLC 6V, 800mA  校准成功！");
                            Config_lable(c800, true);
                            Data00 = Data01=Data00+67;
                            step00 = 1;
                        }
                        else
                        {
                            Output("WLC 6V, 800mA 校准失败！");
                            //Setdata(255, 255, rege: "06 07 ");
                            //Setdata(255, 255, rege: "10 11 ");
                            Setdata(255, 255, rege: "08 09 ");
                            Setdata(255, 255, rege: "02 03 ");
                            Config_lable(c800, false);
                            Config_lable(Result, false);
                            Update();
                            return false;
                        }
                    }
                    
                    if (!Is500 && resultcurrent <= 0.5)
                    {
                        if (resultcurrent < 0.49)
                        {
                            Data00 = Data01 = Data00 - 2;
                            break;
                        }
                        
                        //Is500 = Config("USB.bat", "config_data_usb_resistor", Data00 * 256 + Data01);
                        Is500 = Setdata(Data00, Data01, rege: "02 03 ");
                        if (Is500)
                        {
                            Output("USB PC 500mA  校准成功！");
                            Config_lable(c500, true);
                            return true;
                            
                        }
                        else
                        {
                            Output("USB PC 500mA 校准失败！");
                            //Setdata(255, 255, rege: "06 07 ");
                            //Setdata(255, 255, rege: "10 11 ");
                            //Setdata(255, 255, rege: "08 09 ");
                            Setdata(255, 255, rege: "02 03 ");
                            Config_lable(c500, false);
                            Config_lable(Result, false);
                            Update();
                            return false;
                        }
                    }

                }
            }
            
            if (!Is2000)
            {
                Setdata(255, 255, rege: "06 07 ");
                Output("WALL adapter 2A 校准失败！");
                Config_lable(c2000, false);
            }
            if (!Is1040)
            {
                Setdata(255, 255, rege: "10 11 ");
                Output("WLC 1A 校准失败！");
                Config_lable(c1040, false);
            }
            if(!Is800)
            {
                Setdata(255, 255, rege: "08 09 ");
                Output("WLC 6V, 800mA 校准失败!");
                Config_lable(c800, false);
            }
            if (!Is500)
            {
                Setdata(255, 255, rege: "02 03 ");
                Output("USB PC 500mA 校准失败！");
                Config_lable(c500, false);
            }

            Config_lable(Result, false);
            Update();
            return false;
        }
        public void Config_lable(Label lable,bool result)
        {
            if (result)
            {
                Config_Lable(lable,"通过", Color.PaleGreen);
                //lable.Text = "Pass";
                //lable.BackColor = Color.PaleGreen;
                //Update();
            }
            else
            {
                Config_Lable(lable, "失败", Color.Red);
                //lable.Text = "Fail";
                //lable.BackColor = Color.Red;
                //Update();
            }
        }

        public double Steptest(int Data00,int Data01)
        {
            if (!Setdata(Data00,Data01))
            {
                Config_Lable(Result, "失败", Color.Red);
               // Result.Text = "FAIL";
                //Result.BackColor = Color.OrangeRed;
                Output("未能成功写入寄存器");
                return 0;
            }
            //Output00.Update();
            Feedback.Remove(0, Feedback.Length);
            Visa32.viPrintf(Vi, "MEAS:CURR?"+ System.Environment.NewLine);
            Thread.Sleep(500);
            Visa32.viScanf(Vi, "%t", Feedback);
            s = Feedback.ToString().Split(',');
            InputCurrent = Convert.ToDouble(s[0]);
            return InputCurrent;
        }
        
        public bool Setdata(int Data00,int Data01,string rege="00 01 ")
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
                        Config_Lable(Output00, tempM.Value.Remove(0, 2), Color.White);
                        //Output00.Text = tempM.Value.Remove(0, 2);
                        Ischange = tempM.Value.Remove(0, 2).ToString().Equals(Data00.ToString());
                        //Output(output.ToString());
                        //Update();
                        Output("Register"+rege.Split(' ')[0] +": "+ tempM.Value.Remove(0,2));
                    }
                    else
                    {
                        Config_Lable(Output00, tempM.Value.Remove(0, 2), Color.White);
                        //Output00.Text = tempM.Value.Remove(0, 2);
                        //this.Update();
                        //Output00.Update();
                        Output("Register"+rege.Split(' ')[1]+": "+ tempM.Value.Remove(0, 2));
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
                    Config_Lable(Output00, tempM.Value.Remove(0, 2), Color.White);

                    //Output00.Text = tempM.Value.Remove(0, 2);
                    Ischange = tempM.Value.Remove(0, 2).ToString().Equals(data.ToString());
                    //Update();
                    //Output00.Update();
                    Output("写入数据成功:" + tempM);
                }
                p.WaitForExit();//等待程序执行完退出进程
                p.Close();
                return Ischange;
            }
        }

        public void Checkdevice(out string Sn)
        {
            string cmd, output,error;
            cmd ="adb shell getprop gsm.facsn &exit";//说明：不管命令是否成功均执行exit命令，否则当调用ReadToEnd()方法时，会处于假死状态
            //string match = @"(.*?)\s+device";
            Regex match = new Regex(@"(\w{28})\r\n$");
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
                    Output(error.ToString());        
                }
                
                    //MatchCollection temp = Regex.Matches(output, match);
                MatchCollection temp = match.Matches(output);
                   
                    if (temp.Count>0)
                    {
                    Sn = temp[0].ToString(); 
                    }
                    else
                    {
                        Output("No device found, please check connect!");
                    Sn = "";
                    }
                   
            
                p.WaitForExit();//等待程序执行完退出进程
                p.Close();
            }
        }
        public void Output(string log)
        {
            if (this.LOG.InvokeRequired)
            {
                SendLog printlog = new SendLog(Output);
                this.LOG.Invoke(printlog, log);                                                                                                                                                                                                                                                                                                                     
            }
            else
            {
                this.LOG.AppendText(String.Format("{0}|{1}\r\n", DateTime.Now.ToString("hh:mm:ss"), log));
            }




           // LOG.AppendText(log + "\n"); //DateTime.Now.ToString("HH:mm:ss") + "  " +
            //LOG.Update();
        }

        public void Config_Lable(Control lable,string text,Color color)
        {
            if (lable.InvokeRequired)
            {
                ConfigObject configObject = new ConfigObject(Config_Lable);
                lable.Invoke(configObject,lable,text,color);
            }
            else
            {
                lable.Text = text;
                lable.BackColor = color;
            }
        }

        private void GPIB_TextChanged(object sender, EventArgs e)
        {
            ErrorStatus = -1;
        }

                                 

        private void Calibration_FormClosing(object sender, FormClosingEventArgs e)
        {
            End_Out = false;
            try
            {
                AudioTest.Kill();
            }
            catch
            {

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
        [DllImportAttribute("Visa32.dll", EntryPoint = "#141", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int viOpenDefaultRM(out int sesn);
        [DllImportAttribute("Visa32.dll", EntryPoint = "#128", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int viGetDefaultRM(out int sesn);
        [DllImportAttribute("Visa32.dll", EntryPoint = "#131", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int viOpen(int sesn, string viDesc, int mode, int timeout, out int vi);
        [DllImportAttribute("Visa32.dll", EntryPoint = "#132", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int viClose(int vi);
        [DllImportAttribute("Visa32.DLL", EntryPoint = "#146", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int viParseRsrcEx(int sesn, string desc, ref short intfType, ref short intfNum, StringBuilder rsrcClass, StringBuilder expandedUnaliasedName, StringBuilder aliasIfExists);
        [DllImportAttribute("Visa32.dll", EntryPoint = "#269", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern int viPrintf(int vi, string writeFmt);
        [DllImportAttribute("Visa32.dll", EntryPoint = "#271", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern int viScanf(int vi, string readFmt, StringBuilder arg);
        #endregion
#pragma warning restore IDE1006 // 命名样式
    }

}
