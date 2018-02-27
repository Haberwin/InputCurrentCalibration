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
        Thread runCa;
        public Calibration()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            try { GetVi(); } catch
            {
                Output("GPIB address unavailable!");
                Result.Text = "FAIL";
                Result.BackColor = Color.OrangeRed;
            }
            
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            if(buttonStart.Text == "Start")
            { 
                LOG.Clear();
                Result.Text = "测试中";
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
                OandClosePort(0);
            }
            
            return;
        }
        public void Runcal()
        {
            Output("Start...");
            try
            {
                if (OandClosePort(1))
                {
                    Thread.Sleep(5000);
                    try { CalibCurrent(); } catch { }
                    if (!OandClosePort(0))
                    {
                        Result.Text = "Fail";
                        Result.BackColor = Color.Orange;
                    }
                }
            } catch
            {
                Output("缺少.bat文件");
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

            Result.Text = "电源已连接";
            Result.BackColor = Color.PaleGreen;
            Result.Update();

            return true;
        }

        public bool CalibCurrent()
        {
            int Data00 = 76;
            int Gap = 0;
            try
            {
                Data00 = Convert.ToInt32(Input00.Text);

            }
            catch { Output("输入的格式不正确"); }
            do
            {
                if(Input("00 01 ", Data00)==0)
                {
                    Result.Text = "FAIL";
                    Result.BackColor = Color.OrangeRed;
                    Output("未能成功写入寄存器");
                    return false;

                }
                Output00.Update();
                //Thread.Sleep(1500);
                Feedback.Remove(0, Feedback.Length);
                Visa32.viPrintf(Vi, "MEAS:CURR:DC?; *WAI\n");
                Thread.Sleep(500);
                Visa32.viScanf(Vi, "%t", Feedback);
                s = Feedback.ToString().Split(',');
                InputCurrent = Convert.ToDouble(s[0]);
                if(InputCurrent<1.039)
                {
                    Gap = -1;
                }
                else if(InputCurrent > 1.045)
                {
                    Gap = 1;
                }
                else
                {
                    Gap = 0;
                    //Input("02 03 ", Data00);
                    
                }
                Current.Text = InputCurrent.ToString();
                Current.Update();
                Output("Register00:"+Data00.ToString()+"\tRegister01:"+Data00.ToString()+"\tInput Current:"+ InputCurrent.ToString());
                Data00 += Gap;
                
            } while ((InputCurrent<1.039||InputCurrent>1.045)&&(0<=Data00 && Data00<=200));
            if(Data00 ==Input("02 03 ", Data00))
            {
                Output00.Update();
                Result.Text = "PASS";
                Result.BackColor = Color.PaleGreen;
                Result.Update();
                return true;
            }
            else
            {
                Result.Text = "FAIL";
                Result.BackColor = Color.OrangeRed;
                return false;
            }

        }
        public int Input(string str,int Register1)
        {
            string output, error;
            string lastData="";
            int i=0;
            string match = @"\r\n(\d+)";
            Input00.Text = Register1.ToString();
            Input00.Update();
            
            Process p = new Process();
             cmd = System.AppDomain.CurrentDomain.BaseDirectory;
            string path = cmd + "cmd.bat";
            ProcessStartInfo pi = new ProcessStartInfo(path, str + Register1.ToString())
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
            p.WaitForExit();
            //p.Close();
            if (error != null && error.Length>0)
            {
                Output(error);
                return 0;
                
            }
            foreach (Match tempM in Regex.Matches(output, match))
            {
                
                if (i == 0)
                {
                    Output00.Text = tempM.Value.Remove(0,2);
                    lastData = tempM.ToString();
                    Output00.Update();
                    Output("Register 00:" + tempM);
                }
                else
                {
                    Output00.Text = tempM.Value.Remove(0, 2);
                    this.Update();
                    Output00.Update();
                    Output("Register 01 :" + tempM);
                    if(lastData.Equals(tempM.ToString()))
                    {
                        try
                        {
                            return Convert.ToInt32(tempM.ToString());
                        }
                        catch { return 0; }

                    }
                    else
                    {
                        Output("未能成功写入寄存器");
                        return 0;
                    }
                    
                }
                i++;
               
                
            }
            Output("手机未连接好，请重试！");


            return 0;
        }
        public bool OandClosePort(int data)
        {
            string output,error;
            string match = @"\r\n(\d+)";
            Process p = new Process();
            cmd = System.AppDomain.CurrentDomain.BaseDirectory;
            string path = cmd + "cmd1.bat";
            ProcessStartInfo pi = new ProcessStartInfo(path, data.ToString())
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            CreateNoWindow = true
            };//第二个参数为传入的参数，string类型以空格分隔各个参数  
            p.StartInfo = pi;
            p.Start();
            //output = p.StandardOutput.ReadToEnd();
            output = p.StandardOutput.ReadToEnd();
            error= p.StandardError.ReadLine();
            p.WaitForExit();
            //p.Close();
            if(error != null)
            {
                Output("手机未连接好，请重试！");
                Output(error);
                return false;
            }
            foreach (Match tempM in Regex.Matches(output, match))
                try
                {
                    if (Convert.ToInt32(tempM.ToString()) == data)
                    {
                        Output("更新USB限流开关成功！");
                        return true;
                    }
                    else
                    {
                        Output("更新USB限流开关失败！");
                        return false;
                    }

                } catch
                {
                }

            Output("更新USB限流开关失败！");
            return false;
        }
        public void Output(string log)
        {
            LOG.AppendText(log + "\r\n"); //DateTime.Now.ToString("HH:mm:ss") + "  " +
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
                Result.BackColor = Color.OrangeRed;
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
