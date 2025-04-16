namespace gobang
{
    partial class Form0
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button_start = new Button();
            button_restart = new Button();
            button_quit = new Button();
            mainpanel = new Panel();
            subpanel = new Panel();
            button_withdraw = new Button();
            hint = new Label();
            subpanel.SuspendLayout();
            SuspendLayout();
            // 
            // button_start
            // 
            button_start.BackColor = SystemColors.GradientActiveCaption;
            button_start.Cursor = Cursors.Hand;
            button_start.FlatAppearance.BorderColor = Color.FromArgb(0, 192, 192);
            button_start.FlatAppearance.CheckedBackColor = Color.FromArgb(255, 192, 255);
            button_start.FlatAppearance.MouseDownBackColor = Color.FromArgb(192, 192, 255);
            button_start.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 192, 255);
            button_start.FlatStyle = FlatStyle.Popup;
            button_start.Font = new Font("微软雅黑", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 134);
            button_start.Location = new Point(26, 62);
            button_start.Margin = new Padding(2);
            button_start.Name = "button_start";
            button_start.Size = new Size(134, 52);
            button_start.TabIndex = 0;
            button_start.Text = "开始游戏";
            button_start.UseVisualStyleBackColor = false;
            button_start.Click += button_start_Click;
            // 
            // button_restart
            // 
            button_restart.BackColor = SystemColors.GradientActiveCaption;
            button_restart.Cursor = Cursors.Hand;
            button_restart.FlatAppearance.BorderColor = Color.FromArgb(0, 192, 192);
            button_restart.FlatAppearance.CheckedBackColor = Color.FromArgb(255, 192, 255);
            button_restart.FlatAppearance.MouseDownBackColor = Color.FromArgb(192, 192, 255);
            button_restart.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 192, 255);
            button_restart.FlatStyle = FlatStyle.Popup;
            button_restart.Font = new Font("微软雅黑", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 134);
            button_restart.Location = new Point(26, 139);
            button_restart.Margin = new Padding(2);
            button_restart.Name = "button_restart";
            button_restart.Size = new Size(134, 52);
            button_restart.TabIndex = 1;
            button_restart.Text = "重新开始";
            button_restart.UseVisualStyleBackColor = false;
            button_restart.Click += button_restart_Click;
            // 
            // button_quit
            // 
            button_quit.BackColor = SystemColors.GradientActiveCaption;
            button_quit.Cursor = Cursors.Hand;
            button_quit.FlatAppearance.BorderColor = Color.FromArgb(0, 192, 192);
            button_quit.FlatAppearance.CheckedBackColor = Color.FromArgb(255, 192, 255);
            button_quit.FlatAppearance.MouseDownBackColor = Color.FromArgb(192, 192, 255);
            button_quit.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 192, 255);
            button_quit.FlatStyle = FlatStyle.Popup;
            button_quit.Font = new Font("微软雅黑", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 134);
            button_quit.Location = new Point(26, 218);
            button_quit.Margin = new Padding(2);
            button_quit.Name = "button_quit";
            button_quit.Size = new Size(134, 52);
            button_quit.TabIndex = 2;
            button_quit.Text = "退出游戏";
            button_quit.UseVisualStyleBackColor = false;
            button_quit.Click += button_quit_Click;
            // 
            // mainpanel
            // 
            mainpanel.BackColor = SystemColors.Info;
            mainpanel.BackgroundImageLayout = ImageLayout.Center;
            mainpanel.BorderStyle = BorderStyle.Fixed3D;
            mainpanel.Cursor = Cursors.Cross;
            mainpanel.Location = new Point(0, 0);
            mainpanel.Margin = new Padding(2);
            mainpanel.Name = "mainpanel";
            mainpanel.Size = new Size(509, 514);
            mainpanel.TabIndex = 3;
            mainpanel.Paint += mainpanel_Paint;
            mainpanel.MouseDown += mainpanel_MouseDown;
            // 
            // subpanel
            // 
            subpanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            subpanel.BackColor = SystemColors.ActiveCaption;
            subpanel.Controls.Add(button_withdraw);
            subpanel.Controls.Add(hint);
            subpanel.Controls.Add(button_quit);
            subpanel.Controls.Add(button_restart);
            subpanel.Controls.Add(button_start);
            subpanel.Location = new Point(510, 0);
            subpanel.Margin = new Padding(2);
            subpanel.Name = "subpanel";
            subpanel.Size = new Size(194, 512);
            subpanel.TabIndex = 4;
            subpanel.Paint += subpanel_Paint;
            // 
            // button_withdraw
            // 
            button_withdraw.BackColor = SystemColors.GradientActiveCaption;
            button_withdraw.Cursor = Cursors.Hand;
            button_withdraw.FlatAppearance.BorderColor = Color.FromArgb(0, 192, 192);
            button_withdraw.FlatAppearance.CheckedBackColor = Color.FromArgb(255, 192, 255);
            button_withdraw.FlatAppearance.MouseDownBackColor = Color.FromArgb(192, 192, 255);
            button_withdraw.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 192, 255);
            button_withdraw.FlatStyle = FlatStyle.Popup;
            button_withdraw.Font = new Font("微软雅黑", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 134);
            button_withdraw.Location = new Point(93, 434);
            button_withdraw.Margin = new Padding(2);
            button_withdraw.Name = "button_withdraw";
            button_withdraw.Size = new Size(67, 52);
            button_withdraw.TabIndex = 4;
            button_withdraw.Text = "悔棋";
            button_withdraw.UseVisualStyleBackColor = false;
            button_withdraw.Click += button_withdraw_Click;
            // 
            // hint
            // 
            hint.AutoSize = true;
            hint.BorderStyle = BorderStyle.Fixed3D;
            hint.Cursor = Cursors.IBeam;
            hint.FlatStyle = FlatStyle.Popup;
            hint.Font = new Font("仿宋", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            hint.ForeColor = Color.Crimson;
            hint.Location = new Point(14, 314);
            hint.Margin = new Padding(2, 0, 2, 0);
            hint.Name = "hint";
            hint.Size = new Size(104, 46);
            hint.TabIndex = 0;
            hint.Text = "\n游戏提示\n";
            hint.TextAlign = ContentAlignment.TopCenter;
            // 
            // Form0
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoSize = true;
            ClientSize = new Size(704, 512);
            Controls.Add(subpanel);
            Controls.Add(mainpanel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(2);
            MaximizeBox = false;
            Name = "Form0";
            Text = "五子棋";
            Load += Form1_Load;
            subpanel.ResumeLayout(false);
            subpanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button button_start;
        private Button button_restart;
        private Button button_quit;
        private Panel mainpanel;
        private Panel subpanel;
        private Label hint;
        private Button button_withdraw;
    }
}
