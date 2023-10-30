using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MainProject.Bean;
using MainProject.Utils;
using MainProject.Properties;
using NXOpen;

namespace MainProject.Forms
{
    public partial class LoginForm : Form
    {
        Dictionary<string, string> keyValuePairs = null;
        bool isLoginSuccessful = false;
        string userId = "";
        public LoginForm(List<ButtonBean> buttonBeans)
        {
            InitializeComponent();
            keyValuePairs = NPOIUtils.loadExcelLoginData(Constain.VENDORDIR + "\\data\\login.xlsx");
            NXOpenUI.FormUtilities.SetApplicationIcon(this);
            NXOpenUI.FormUtilities.ReparentForm(this);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            userId = loginUser_textBox.Text;
            if (verificationComponent1.Verification())
            {
                string userId = loginUser_textBox.Text;
                string userPassword = loginPassword_textBox.Text;
                if (keyValuePairs!=null)
                {
                    string keyValue = "";
                    if(keyValuePairs.TryGetValue(userId,out keyValue))
                    {
                        if(userPassword.Equals(keyValue))
                        {
                            isLoginSuccessful = true;
                            foreach (ButtonBean buttonBean in Project.bts)
                            {
                                NXOpen.MenuBar.MenuButton button = Project.theUI.MenuBarManager.GetButtonFromName(buttonBean.ID_VALUE);
                                button.ButtonSensitivity = NXOpen.MenuBar.MenuButton.SensitivityStatus.Sensitive;
                            }
                        }
                    }
                }

                if (isLoginSuccessful)
                {
                    NXOpen.MenuBar.MenuButton button = Project.theUI.MenuBarManager.GetButtonFromName("PD_IMPORT_TEMPLATE");
                    NXOpen.MenuBar.MenuButton button1 = Project.theUI.MenuBarManager.GetButtonFromName("PD_BATCH_MODIFY_NAME");
                    if(button!=null)button.ButtonSensitivity = NXOpen.MenuBar.MenuButton.SensitivityStatus.Sensitive;
                    if(button1 != null) button1.ButtonSensitivity = NXOpen.MenuBar.MenuButton.SensitivityStatus.Sensitive;
                    Project.theUI.NXMessageBox.Show("Success", NXMessageBox.DialogType.Information, "登陆成功！");
                    Dispose();
                }else
                {
                    Project.theUI.NXMessageBox.Show("Warning", NXMessageBox.DialogType.Warning, "登陆失败,请检查账号密码！");
                }
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            this.loginUser_textBox.Text = Settings.Default.yonghuming;
        }


        private void cancelButton_Click(object sender, EventArgs e)
        {
            userId = loginUser_textBox.Text;
            Dispose();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.Default.yonghuming = userId;
            Settings.Default.Save();
        }
    }
}
