namespace HydroInfoReptile
{
    partial class Form_Main
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Main));
            this.label_Macrofuns = new System.Windows.Forms.Label();
            this.panel_error = new System.Windows.Forms.Panel();
            this.linkLabel_Data = new System.Windows.Forms.LinkLabel();
            this.linkLabel_log = new System.Windows.Forms.LinkLabel();
            this.linkLabel_html = new System.Windows.Forms.LinkLabel();
            this.textBox_log = new System.Windows.Forms.TextBox();
            this.panel_error.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_Macrofuns
            // 
            this.label_Macrofuns.AutoSize = true;
            this.label_Macrofuns.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Macrofuns.Location = new System.Drawing.Point(229, 189);
            this.label_Macrofuns.Name = "label_Macrofuns";
            this.label_Macrofuns.Size = new System.Drawing.Size(115, 21);
            this.label_Macrofuns.TabIndex = 1;
            this.label_Macrofuns.Text = "by Macrofuns";
            // 
            // panel_error
            // 
            this.panel_error.Controls.Add(this.linkLabel_Data);
            this.panel_error.Controls.Add(this.linkLabel_log);
            this.panel_error.Controls.Add(this.linkLabel_html);
            this.panel_error.Location = new System.Drawing.Point(12, 180);
            this.panel_error.Name = "panel_error";
            this.panel_error.Size = new System.Drawing.Size(211, 43);
            this.panel_error.TabIndex = 2;
            // 
            // linkLabel_Data
            // 
            this.linkLabel_Data.AutoSize = true;
            this.linkLabel_Data.Location = new System.Drawing.Point(109, 16);
            this.linkLabel_Data.Name = "linkLabel_Data";
            this.linkLabel_Data.Size = new System.Drawing.Size(53, 12);
            this.linkLabel_Data.TabIndex = 2;
            this.linkLabel_Data.TabStop = true;
            this.linkLabel_Data.Text = "查看输出";
            this.linkLabel_Data.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_Data_LinkClicked);
            // 
            // linkLabel_log
            // 
            this.linkLabel_log.AutoSize = true;
            this.linkLabel_log.Location = new System.Drawing.Point(50, 16);
            this.linkLabel_log.Name = "linkLabel_log";
            this.linkLabel_log.Size = new System.Drawing.Size(53, 12);
            this.linkLabel_log.TabIndex = 1;
            this.linkLabel_log.TabStop = true;
            this.linkLabel_log.Text = "查看日志";
            this.linkLabel_log.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_log_LinkClicked);
            // 
            // linkLabel_html
            // 
            this.linkLabel_html.AutoSize = true;
            this.linkLabel_html.Location = new System.Drawing.Point(3, 16);
            this.linkLabel_html.Name = "linkLabel_html";
            this.linkLabel_html.Size = new System.Drawing.Size(41, 12);
            this.linkLabel_html.TabIndex = 0;
            this.linkLabel_html.TabStop = true;
            this.linkLabel_html.Text = "查看源";
            this.linkLabel_html.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_html_LinkClicked);
            // 
            // textBox_log
            // 
            this.textBox_log.Location = new System.Drawing.Point(0, 0);
            this.textBox_log.Multiline = true;
            this.textBox_log.Name = "textBox_log";
            this.textBox_log.ReadOnly = true;
            this.textBox_log.Size = new System.Drawing.Size(356, 186);
            this.textBox_log.TabIndex = 3;
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(356, 220);
            this.Controls.Add(this.textBox_log);
            this.Controls.Add(this.panel_error);
            this.Controls.Add(this.label_Macrofuns);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "全国雨水情数据下载器";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Main_FormClosing);
            this.Load += new System.EventHandler(this.Form_Main_Load);
            this.panel_error.ResumeLayout(false);
            this.panel_error.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_Macrofuns;
        private System.Windows.Forms.Panel panel_error;
        private System.Windows.Forms.LinkLabel linkLabel_Data;
        private System.Windows.Forms.LinkLabel linkLabel_log;
        private System.Windows.Forms.LinkLabel linkLabel_html;
        private System.Windows.Forms.TextBox textBox_log;

    }
}

