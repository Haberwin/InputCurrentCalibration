﻿namespace InputCurrentCalibration
{
    public partial class Calibration
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.GPIB = new System.Windows.Forms.TextBox();
            this.LOG = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.Result = new System.Windows.Forms.Label();
            this.Current = new System.Windows.Forms.Label();
            this.Output00 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.c800 = new System.Windows.Forms.Label();
            this.c1040 = new System.Windows.Forms.Label();
            this.c2000 = new System.Windows.Forms.Label();
            this.c500 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.BackColor = System.Drawing.Color.MistyRose;
            this.buttonStart.Font = new System.Drawing.Font("Consolas", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStart.Location = new System.Drawing.Point(24, 23);
            this.buttonStart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(291, 306);
            this.buttonStart.TabIndex = 6;
            this.buttonStart.Text = "开始";
            this.buttonStart.UseVisualStyleBackColor = false;
            this.buttonStart.Click += new System.EventHandler(this.ButtonStart_Click);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Consolas", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(678, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(266, 49);
            this.label1.TabIndex = 1;
            this.label1.Text = "Input Current";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Font = new System.Drawing.Font("Consolas", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(678, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(266, 47);
            this.label3.TabIndex = 5;
            this.label3.Text = "GPIB Address";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // GPIB
            // 
            this.GPIB.BackColor = System.Drawing.Color.Chartreuse;
            this.GPIB.Font = new System.Drawing.Font("Consolas", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GPIB.Location = new System.Drawing.Point(974, 23);
            this.GPIB.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GPIB.Name = "GPIB";
            this.GPIB.Size = new System.Drawing.Size(207, 46);
            this.GPIB.TabIndex = 0;
            this.GPIB.Text = "5";
            this.GPIB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.GPIB.TextChanged += new System.EventHandler(this.GPIB_TextChanged);
            // 
            // LOG
            // 
            this.LOG.BackColor = System.Drawing.SystemColors.InfoText;
            this.LOG.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LOG.ForeColor = System.Drawing.SystemColors.Info;
            this.LOG.Location = new System.Drawing.Point(678, 241);
            this.LOG.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LOG.Multiline = true;
            this.LOG.Name = "LOG";
            this.LOG.ReadOnly = true;
            this.LOG.Size = new System.Drawing.Size(503, 519);
            this.LOG.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Font = new System.Drawing.Font("Consolas", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(678, 173);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(266, 49);
            this.label6.TabIndex = 11;
            this.label6.Text = "Current Data";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Result
            // 
            this.Result.BackColor = System.Drawing.Color.PaleGreen;
            this.Result.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Result.Font = new System.Drawing.Font("Consolas", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Result.Location = new System.Drawing.Point(325, 23);
            this.Result.Name = "Result";
            this.Result.Size = new System.Drawing.Size(318, 306);
            this.Result.TabIndex = 18;
            this.Result.Text = "未开始";
            this.Result.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Current
            // 
            this.Current.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Current.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Current.Font = new System.Drawing.Font("Consolas", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Current.Location = new System.Drawing.Point(974, 96);
            this.Current.Name = "Current";
            this.Current.Size = new System.Drawing.Size(207, 49);
            this.Current.TabIndex = 19;
            this.Current.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Output00
            // 
            this.Output00.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Output00.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Output00.Font = new System.Drawing.Font("Consolas", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Output00.Location = new System.Drawing.Point(974, 173);
            this.Output00.Name = "Output00";
            this.Output00.Size = new System.Drawing.Size(207, 49);
            this.Output00.TabIndex = 27;
            this.Output00.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Font = new System.Drawing.Font("Consolas", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 324);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(228, 50);
            this.label4.TabIndex = 29;
            this.label4.Text = "USB 500mA";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("Consolas", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 245);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(228, 50);
            this.label2.TabIndex = 30;
            this.label2.Text = "WLC 800mA";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Font = new System.Drawing.Font("Consolas", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(12, 160);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(228, 50);
            this.label5.TabIndex = 31;
            this.label5.Text = "WLC 1A";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Font = new System.Drawing.Font("Consolas", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(12, 76);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(228, 50);
            this.label7.TabIndex = 32;
            this.label7.Text = "Adapter 2A";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.c800);
            this.groupBox1.Controls.Add(this.c1040);
            this.groupBox1.Controls.Add(this.c2000);
            this.groupBox1.Controls.Add(this.c500);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Font = new System.Drawing.Font("Consolas", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 347);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(631, 413);
            this.groupBox1.TabIndex = 33;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Result";
            // 
            // c800
            // 
            this.c800.BackColor = System.Drawing.Color.Orange;
            this.c800.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.c800.Location = new System.Drawing.Point(385, 245);
            this.c800.Name = "c800";
            this.c800.Size = new System.Drawing.Size(221, 50);
            this.c800.TabIndex = 36;
            this.c800.Text = "Waiting";
            this.c800.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // c1040
            // 
            this.c1040.BackColor = System.Drawing.Color.Orange;
            this.c1040.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.c1040.Location = new System.Drawing.Point(385, 159);
            this.c1040.Name = "c1040";
            this.c1040.Size = new System.Drawing.Size(221, 50);
            this.c1040.TabIndex = 35;
            this.c1040.Text = "Waiting";
            this.c1040.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // c2000
            // 
            this.c2000.BackColor = System.Drawing.Color.Orange;
            this.c2000.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.c2000.Location = new System.Drawing.Point(385, 76);
            this.c2000.Name = "c2000";
            this.c2000.Size = new System.Drawing.Size(221, 50);
            this.c2000.TabIndex = 34;
            this.c2000.Text = "Waiting";
            this.c2000.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // c500
            // 
            this.c500.BackColor = System.Drawing.Color.Orange;
            this.c500.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.c500.Location = new System.Drawing.Point(385, 323);
            this.c500.Name = "c500";
            this.c500.Size = new System.Drawing.Size(221, 50);
            this.c500.TabIndex = 33;
            this.c500.Text = "Waiting";
            this.c500.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Calibration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1229, 828);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Output00);
            this.Controls.Add(this.Current);
            this.Controls.Add(this.Result);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.LOG);
            this.Controls.Add(this.GPIB);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonStart);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Calibration";
            this.Text = "Calibration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Calibration_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox GPIB;
        private System.Windows.Forms.TextBox LOG;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label Result;
        private System.Windows.Forms.Label Current;
        private System.Windows.Forms.Label Output00;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label c800;
        private System.Windows.Forms.Label c1040;
        private System.Windows.Forms.Label c2000;
        private System.Windows.Forms.Label c500;
    }
}

