namespace gobang
{
    partial class SelectionForm
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            radioButton1 = new RadioButton();
            radioButton2 = new RadioButton();
            label1 = new Label();
            confirm_button = new Button();
            SuspendLayout();
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.CausesValidation = false;
            radioButton1.Font = new Font("新宋体", 12F, FontStyle.Bold);
            radioButton1.Location = new Point(41, 173);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(195, 37);
            radioButton1.TabIndex = 0;
            radioButton1.TabStop = true;
            radioButton1.Text = "玩家-电脑";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Font = new Font("新宋体", 12F, FontStyle.Bold);
            radioButton2.Location = new Point(274, 173);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(195, 37);
            radioButton2.TabIndex = 1;
            radioButton2.TabStop = true;
            radioButton2.Text = "玩家-玩家";
            radioButton2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("宋体", 13.875F, FontStyle.Bold, GraphicsUnit.Point, 134);
            label1.Location = new Point(41, 79);
            label1.Name = "label1";
            label1.Size = new Size(321, 37);
            label1.TabIndex = 2;
            label1.Text = "请选择对战方式：";
            label1.TextAlign = ContentAlignment.TopCenter;
            // 
            // confirm_button
            // 
            confirm_button.BackColor = SystemColors.GradientActiveCaption;
            confirm_button.Font = new Font("小米兰亭", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 134);
            confirm_button.Location = new Point(294, 258);
            confirm_button.Name = "confirm_button";
            confirm_button.Size = new Size(175, 64);
            confirm_button.TabIndex = 3;
            confirm_button.Text = "确认";
            confirm_button.UseVisualStyleBackColor = false;
            confirm_button.Click += confirm_button_Click;
            // 
            // SelectionForm
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Info;
            ClientSize = new Size(504, 356);
            Controls.Add(confirm_button);
            Controls.Add(label1);
            Controls.Add(radioButton2);
            Controls.Add(radioButton1);
            Location = new Point(60, 628);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SelectionForm";
            StartPosition = FormStartPosition.CenterParent;
            Load += SelectionForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private Label label1;
        private Button confirm_button;
    }
}
