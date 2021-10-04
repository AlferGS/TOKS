
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
            this.Debug = new System.Windows.Forms.Label();
            this.SendButton = new System.Windows.Forms.Button();
            this.OutputBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // InputBox
            // 
            this.InputBox.Location = new System.Drawing.Point(12, 12);
            this.InputBox.Multiline = true;
            this.InputBox.Name = "InputBox";
            this.InputBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.InputBox.Size = new System.Drawing.Size(288, 106);
            this.InputBox.TabIndex = 0;
            // 
            // ComboBox
            // 
            this.ComboBox.FormattingEnabled = true;
            this.ComboBox.Location = new System.Drawing.Point(179, 287);
            this.ComboBox.Name = "ComboBox";
            this.ComboBox.Size = new System.Drawing.Size(121, 24);
            this.ComboBox.TabIndex = 1;
            this.ComboBox.SelectedIndexChanged += new System.EventHandler(this.ComboBox_SelectedIndexChanged_1);
            // 
            // Debug
            // 
            this.Debug.AutoSize = true;
            this.Debug.Location = new System.Drawing.Point(12, 258);
            this.Debug.Name = "Debug";
            this.Debug.Size = new System.Drawing.Size(20, 17);
            this.Debug.TabIndex = 2;
            this.Debug.Text = "...";
            // 
            // SendButton
            // 
            this.SendButton.Location = new System.Drawing.Point(179, 258);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(121, 23);
            this.SendButton.TabIndex = 3;
            this.SendButton.Text = "Send";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // OutputBox
            // 
            this.OutputBox.Enabled = false;
            this.OutputBox.FormattingEnabled = true;
            this.OutputBox.ItemHeight = 16;
            this.OutputBox.Location = new System.Drawing.Point(12, 124);
            this.OutputBox.Name = "OutputBox";
            this.OutputBox.Size = new System.Drawing.Size(288, 116);
            this.OutputBox.TabIndex = 4;
            // 
            // com2com
            // 
            this.ClientSize = new System.Drawing.Size(312, 376);
            this.Controls.Add(this.OutputBox);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.Debug);
            this.Controls.Add(this.ComboBox);
            this.Controls.Add(this.InputBox);
            this.Name = "com2com";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Com2com_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Debug;
        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.ComboBox ComboBox;
        private System.Windows.Forms.ListBox OutputBox;
        private System.Windows.Forms.TextBox InputBox;
    }
}

