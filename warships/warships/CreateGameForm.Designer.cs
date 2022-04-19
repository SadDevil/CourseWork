
namespace warships
{
    partial class CreateGameForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.CreateButton = new System.Windows.Forms.Button();
            this.JoinButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.IPBox = new System.Windows.Forms.TextBox();
            this.NicknameBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CreateButton
            // 
            this.CreateButton.BackColor = System.Drawing.Color.LightCoral;
            this.CreateButton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.CreateButton.Location = new System.Drawing.Point(71, 63);
            this.CreateButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CreateButton.Name = "CreateButton";
            this.CreateButton.Size = new System.Drawing.Size(148, 28);
            this.CreateButton.TabIndex = 0;
            this.CreateButton.Text = "Створити";
            this.CreateButton.UseVisualStyleBackColor = false;
            this.CreateButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // JoinButton
            // 
            this.JoinButton.BackColor = System.Drawing.Color.LightCoral;
            this.JoinButton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.JoinButton.Location = new System.Drawing.Point(71, 165);
            this.JoinButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.JoinButton.Name = "JoinButton";
            this.JoinButton.Size = new System.Drawing.Size(148, 28);
            this.JoinButton.TabIndex = 1;
            this.JoinButton.Text = "Приєднатись";
            this.JoinButton.UseVisualStyleBackColor = false;
            this.JoinButton.Click += new System.EventHandler(this.JoinButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 137);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "IP супротивника";
            // 
            // IPBox
            // 
            this.IPBox.BackColor = System.Drawing.Color.LightCoral;
            this.IPBox.Location = new System.Drawing.Point(145, 133);
            this.IPBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.IPBox.MaxLength = 15;
            this.IPBox.Name = "IPBox";
            this.IPBox.Size = new System.Drawing.Size(121, 22);
            this.IPBox.TabIndex = 3;
            this.IPBox.Text = "127.0.0.1";
            // 
            // NicknameBox
            // 
            this.NicknameBox.BackColor = System.Drawing.Color.LightCoral;
            this.NicknameBox.Location = new System.Drawing.Point(145, 18);
            this.NicknameBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.NicknameBox.MaxLength = 10;
            this.NicknameBox.Name = "NicknameBox";
            this.NicknameBox.Size = new System.Drawing.Size(121, 22);
            this.NicknameBox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 18);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Введіть ім\'я";
            // 
            // CreateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(284, 213);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.NicknameBox);
            this.Controls.Add(this.IPBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.JoinButton);
            this.Controls.Add(this.CreateButton);
            this.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "CreateForm";
            this.Text = "Create game";
            this.Load += new System.EventHandler(this.CreateForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CreateButton;
        private System.Windows.Forms.Button JoinButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox IPBox;
        private System.Windows.Forms.TextBox NicknameBox;
        private System.Windows.Forms.Label label2;
    }
}