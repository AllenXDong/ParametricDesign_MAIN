using System.Collections.Generic;
using MainProject.Bean;

namespace MainProject.Forms
{
    partial class ImportTempalteForm
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

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.verificationComponent1 = new HZH_Controls.Controls.VerificationComponent(this.components);
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.ucCombox1 = new HZH_Controls.Controls.UCCombox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // verificationComponent1
            // 
            this.verificationComponent1.AutoCloseErrorTipsTime = 3000;
            this.verificationComponent1.ErrorTipsBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(58)))));
            this.verificationComponent1.ErrorTipsForeColor = System.Drawing.Color.White;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(0, 0);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 21);
            this.textBox2.TabIndex = 0;
            this.verificationComponent1.SetVerificationCustomRegex(this.textBox2, "");
            this.verificationComponent1.SetVerificationErrorMsg(this.textBox2, "");
            this.verificationComponent1.SetVerificationModel(this.textBox2, HZH_Controls.Controls.VerificationModel.None);
            this.verificationComponent1.SetVerificationRequired(this.textBox2, false);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 21);
            this.textBox1.TabIndex = 0;
            this.verificationComponent1.SetVerificationCustomRegex(this.textBox1, "");
            this.verificationComponent1.SetVerificationErrorMsg(this.textBox1, "");
            this.verificationComponent1.SetVerificationModel(this.textBox1, HZH_Controls.Controls.VerificationModel.None);
            this.verificationComponent1.SetVerificationRequired(this.textBox1, false);
            // 
            // ucCombox1
            // 
            this.ucCombox1.BackColor = System.Drawing.Color.Transparent;
            this.ucCombox1.BackColorExt = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ucCombox1.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.ucCombox1.ConerRadius = 5;
            this.ucCombox1.DropPanelHeight = -1;
            this.ucCombox1.FillColor = System.Drawing.Color.White;
            this.ucCombox1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.ucCombox1.IsRadius = true;
            this.ucCombox1.IsShowRect = true;
            this.ucCombox1.ItemWidth = 70;
            this.ucCombox1.Location = new System.Drawing.Point(136, 40);
            this.ucCombox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ucCombox1.Name = "ucCombox1";
            this.ucCombox1.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ucCombox1.RectWidth = 1;
            this.ucCombox1.SelectedIndex = -1;
            this.ucCombox1.SelectedValue = "";
            this.ucCombox1.Size = new System.Drawing.Size(185, 32);
            this.ucCombox1.Source = null;
            this.ucCombox1.TabIndex = 8;
            this.ucCombox1.TextValue = null;
            this.ucCombox1.TriangleColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.verificationComponent1.SetVerificationCustomRegex(this.ucCombox1, "");
            this.verificationComponent1.SetVerificationErrorMsg(this.ucCombox1, "");
            this.verificationComponent1.SetVerificationModel(this.ucCombox1, HZH_Controls.Controls.VerificationModel.None);
            this.verificationComponent1.SetVerificationRequired(this.ucCombox1, false);
            // 
            // groupBox3
            // 
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 100);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 100);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 0;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(200, 100);
            this.flowLayoutPanel2.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.okButton.Location = new System.Drawing.Point(168, 96);
            this.okButton.Margin = new System.Windows.Forms.Padding(2);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(60, 34);
            this.okButton.TabIndex = 6;
            this.okButton.Text = "导入";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.button_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(245, 96);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(2);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(60, 34);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "取消";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(12, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 51);
            this.label3.TabIndex = 9;
            this.label3.Text = "模板名称";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ImportTempalteForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(334, 141);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ucCombox1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(350, 180);
            this.Name = "ImportTempalteForm";
            this.Text = "模板导入";
            this.ResumeLayout(false);

        }
        private HZH_Controls.Controls.VerificationComponent verificationComponent1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private HZH_Controls.Controls.UCCombox ucCombox1;
        private System.Windows.Forms.Label label3;
    }
}