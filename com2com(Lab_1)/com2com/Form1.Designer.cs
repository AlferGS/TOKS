
namespace com2com
{
    partial class com2com
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.InputBox = new System.Windows.Forms.TextBox();
            this.ComboBox = new System.Windows.Forms.ComboBox();
            this.SendButton = new System.Windows.Forms.Button();
            this.OutputBox = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.updatePortsButton = new System.Windows.Forms.Button();
            this.clrOutputButton = new System.Windows.Forms.Button();
            this.Debug = new System.Windows.Forms.RichTextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // InputBox
            // 
            this.InputBox.Enabled = false;
            this.InputBox.Location = new System.Drawing.Point(0, 21);
            this.InputBox.Multiline = true;
            this.InputBox.Name = "InputBox";
            this.InputBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.InputBox.Size = new System.Drawing.Size(452, 104);
            this.InputBox.TabIndex = 0;
            this.InputBox.TextChanged += new System.EventHandler(this.InputBox_TextChanged);
            // 
            // ComboBox
            // 
            this.ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox.FormattingEnabled = true;
            this.ComboBox.Location = new System.Drawing.Point(198, 21);
            this.ComboBox.Name = "ComboBox";
            this.ComboBox.Size = new System.Drawing.Size(121, 24);
            this.ComboBox.TabIndex = 1;
            this.ComboBox.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged_1);
            // 
            // SendButton
            // 
            this.SendButton.Location = new System.Drawing.Point(325, 21);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(121, 40);
            this.SendButton.TabIndex = 3;
            this.SendButton.Text = "Send";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // OutputBox
            // 
            this.OutputBox.FormattingEnabled = true;
            this.OutputBox.HorizontalScrollbar = true;
            this.OutputBox.ItemHeight = 16;
            this.OutputBox.Location = new System.Drawing.Point(0, 21);
            this.OutputBox.Name = "OutputBox";
            this.OutputBox.ScrollAlwaysVisible = true;
            this.OutputBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.OutputBox.Size = new System.Drawing.Size(452, 116);
            this.OutputBox.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.InputBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(452, 125);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.OutputBox);
            this.groupBox2.Location = new System.Drawing.Point(12, 138);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(452, 146);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Output";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.updatePortsButton);
            this.groupBox3.Controls.Add(this.clrOutputButton);
            this.groupBox3.Controls.Add(this.Debug);
            this.groupBox3.Controls.Add(this.SendButton);
            this.groupBox3.Controls.Add(this.ComboBox);
            this.groupBox3.Location = new System.Drawing.Point(12, 290);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(452, 139);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Debug&&Control";
            // 
            // updatePortsButton
            // 
            this.updatePortsButton.Location = new System.Drawing.Point(325, 102);
            this.updatePortsButton.Name = "updatePortsButton";
            this.updatePortsButton.Size = new System.Drawing.Size(121, 29);
            this.updatePortsButton.TabIndex = 9;
            this.updatePortsButton.Text = "Update ports";
            this.updatePortsButton.UseVisualStyleBackColor = true;
            this.updatePortsButton.Click += new System.EventHandler(this.updatePortsButton_Click);
            // 
            // clrOutputButton
            // 
            this.clrOutputButton.Location = new System.Drawing.Point(325, 67);
            this.clrOutputButton.Name = "clrOutputButton";
            this.clrOutputButton.Size = new System.Drawing.Size(121, 29);
            this.clrOutputButton.TabIndex = 10;
            this.clrOutputButton.Text = "Clear Output";
            this.clrOutputButton.UseVisualStyleBackColor = true;
            this.clrOutputButton.Click += new System.EventHandler(this.clrOutputButton_Click);
            // 
            // Debug
            // 
            this.Debug.AutoSize = true;
            this.Debug.Location = new System.Drawing.Point(6, 21);
            this.Debug.Name = "Debug";
            this.Debug.ReadOnly = true;
            this.Debug.Size = new System.Drawing.Size(186, 110);
            this.Debug.TabIndex = 9;
            this.Debug.Text = "";
            // 
            // com2com
            // 
            this.ClientSize = new System.Drawing.Size(476, 434);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(494, 481);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(494, 481);
            this.Name = "com2com";
            this.Text = "com2com";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Com2com_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.ComboBox ComboBox;
        private System.Windows.Forms.ListBox OutputBox;
        private System.Windows.Forms.TextBox InputBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RichTextBox Debug;
        private System.Windows.Forms.Button clrOutputButton;
        private System.Windows.Forms.Button updatePortsButton;
    }
}

