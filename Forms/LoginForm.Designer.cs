using System.Collections.Generic;
using MainProject.Bean;

namespace MainProject.Forms
{
    partial class LoginForm
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.loginUser_textBox = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.loginPassword_textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.verificationComponent1 = new HZH_Controls.Controls.VerificationComponent(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(19, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 45);
            this.label1.TabIndex = 10;
            this.label1.Text = "用户名";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // loginUser_textBox
            // 
            this.loginUser_textBox.Location = new System.Drawing.Point(124, 78);
            this.loginUser_textBox.Name = "loginUser_textBox";
            this.loginUser_textBox.Size = new System.Drawing.Size(125, 21);
            this.loginUser_textBox.TabIndex = 0;
            this.verificationComponent1.SetVerificationCustomRegex(this.loginUser_textBox, "");
            this.verificationComponent1.SetVerificationErrorMsg(this.loginUser_textBox, "");
            this.verificationComponent1.SetVerificationModel(this.loginUser_textBox, HZH_Controls.Controls.VerificationModel.None);
            this.verificationComponent1.SetVerificationRequired(this.loginUser_textBox, true);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(251, 257);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(2);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(60, 34);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "取消";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.okButton.Location = new System.Drawing.Point(171, 257);
            this.okButton.Margin = new System.Windows.Forms.Padding(2);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(60, 34);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "确定";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // loginPassword_textBox
            // 
            this.loginPassword_textBox.Location = new System.Drawing.Point(124, 141);
            this.loginPassword_textBox.Name = "loginPassword_textBox";
            this.loginPassword_textBox.PasswordChar = '*';
            this.loginPassword_textBox.Size = new System.Drawing.Size(125, 21);
            this.loginPassword_textBox.TabIndex = 1;
            this.verificationComponent1.SetVerificationCustomRegex(this.loginPassword_textBox, "");
            this.verificationComponent1.SetVerificationErrorMsg(this.loginPassword_textBox, "");
            this.verificationComponent1.SetVerificationModel(this.loginPassword_textBox, HZH_Controls.Controls.VerificationModel.None);
            this.verificationComponent1.SetVerificationRequired(this.loginPassword_textBox, true);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(19, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 45);
            this.label2.TabIndex = 11;
            this.label2.Text = "密码";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // verificationComponent1
            // 
            this.verificationComponent1.AutoCloseErrorTipsTime = 3000;
            this.verificationComponent1.ErrorTipsBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(58)))));
            this.verificationComponent1.ErrorTipsForeColor = System.Drawing.Color.White;
            // 
            // LoginForm
            // 
            this.AcceptButton = this.okButton;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(322, 302);
            this.ControlBox = false;
            this.Controls.Add(this.loginPassword_textBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.loginUser_textBox);
            this.Controls.Add(this.label1);
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "登陆";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LoginForm_FormClosed);
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox loginUser_textBox;
        private HZH_Controls.Controls.VerificationComponent verificationComponent1;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.TextBox loginPassword_textBox;
        private System.Windows.Forms.Label label2;
    }
}