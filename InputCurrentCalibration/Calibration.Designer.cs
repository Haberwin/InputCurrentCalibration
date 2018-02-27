namespace InputCurrentCalibration
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
            this.components = new System.ComponentModel.Container();
            this.buttonStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Input00 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.GPIB = new System.Windows.Forms.TextBox();
            this.LOG = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.Result = new System.Windows.Forms.Label();
            this.Current = new System.Windows.Forms.Label();
            this.Output00 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.BackColor = System.Drawing.Color.MistyRose;
            this.buttonStart.Font = new System.Drawing.Font("Consolas", 50F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStart.Location = new System.Drawing.Point(582, 16);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(237, 295);
            this.buttonStart.TabIndex = 6;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = false;
            this.buttonStart.Click += new System.EventHandler(this.ButtonStart_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Consolas", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(18, 156);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(264, 40);
            this.label1.TabIndex = 1;
            this.label1.Text = "Input Current";
            // 
            // Input00
            // 
            this.Input00.Location = new System.Drawing.Point(25, 16);
            this.Input00.Name = "Input00";
            this.Input00.Size = new System.Drawing.Size(74, 22);
            this.Input00.TabIndex = 1;
            this.Input00.Text = "76";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Consolas", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(18, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(245, 40);
            this.label3.TabIndex = 5;
            this.label3.Text = "GPIB Address";
            // 
            // GPIB
            // 
            this.GPIB.BackColor = System.Drawing.Color.Chartreuse;
            this.GPIB.Font = new System.Drawing.Font("Consolas", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GPIB.Location = new System.Drawing.Point(376, 50);
            this.GPIB.Name = "GPIB";
            this.GPIB.Size = new System.Drawing.Size(145, 47);
            this.GPIB.TabIndex = 0;
            this.GPIB.Text = "22";
            this.GPIB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.GPIB.TextChanged += new System.EventHandler(this.TextLimit);
            // 
            // LOG
            // 
            this.LOG.BackColor = System.Drawing.SystemColors.InfoText;
            this.LOG.Font = new System.Drawing.Font("Consolas", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LOG.ForeColor = System.Drawing.SystemColors.Info;
            this.LOG.Location = new System.Drawing.Point(22, 332);
            this.LOG.Multiline = true;
            this.LOG.Name = "LOG";
            this.LOG.Size = new System.Drawing.Size(1191, 393);
            this.LOG.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Consolas", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(18, 272);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(245, 40);
            this.label6.TabIndex = 11;
            this.label6.Text = "Current Data";
            // 
            // Result
            // 
            this.Result.BackColor = System.Drawing.Color.PaleGreen;
            this.Result.Font = new System.Drawing.Font("Consolas", 80F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Result.Location = new System.Drawing.Point(825, 16);
            this.Result.Name = "Result";
            this.Result.Size = new System.Drawing.Size(377, 295);
            this.Result.TabIndex = 18;
            this.Result.Text = "PASS";
            this.Result.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Current
            // 
            this.Current.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Current.Font = new System.Drawing.Font("Consolas", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Current.Location = new System.Drawing.Point(376, 156);
            this.Current.Name = "Current";
            this.Current.Size = new System.Drawing.Size(145, 40);
            this.Current.TabIndex = 19;
            this.Current.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Output00
            // 
            this.Output00.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Output00.Font = new System.Drawing.Font("Consolas", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Output00.Location = new System.Drawing.Point(376, 272);
            this.Output00.Name = "Output00";
            this.Output00.Size = new System.Drawing.Size(145, 40);
            this.Output00.TabIndex = 27;
            this.Output00.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Calibration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1240, 737);
            this.Controls.Add(this.Output00);
            this.Controls.Add(this.Current);
            this.Controls.Add(this.Result);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.LOG);
            this.Controls.Add(this.GPIB);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Input00);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonStart);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Calibration";
            this.Text = "Calibration";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Input00;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox GPIB;
        private System.Windows.Forms.TextBox LOG;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label Result;
        private System.Windows.Forms.Label Current;
        private System.Windows.Forms.Label Output00;
    }
}

