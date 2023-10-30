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
using NXOpen;

namespace MainProject.Forms
{
    public partial class ImportTempalteForm : Form
    {

        public ImportTempalteForm()
        {
            string dllPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            dllPath = dllPath.Substring(0, dllPath.LastIndexOf("\\"));
            dllPath = dllPath.Substring(0, dllPath.LastIndexOf("\\"));
            List<TemplateItem> LoadAllTemplateFromXML = XmlDocumentUtil.LoadAllTemplateFromXML(dllPath + "\\config\\ParametricDesignConfig.xml");
            InitializeComponent();
            initCombox(LoadAllTemplateFromXML);
            NXOpenUI.FormUtilities.SetApplicationIcon(this);
            NXOpenUI.FormUtilities.ReparentForm(this);
        }
        private void initCombox(List<TemplateItem> LoadAllTemplateFromXML)
        {
            List<KeyValuePair<string, string>> temoList = new List<KeyValuePair<string, string>>();
            foreach (TemplateItem item in LoadAllTemplateFromXML)
            {
                temoList.Add(new KeyValuePair<string, string>(item.TEMPLATEPATH, item.DISPLAYNAME));
            }
            ucCombox1.Source = temoList;
        }
        private void button_Click(object sender, EventArgs e)
        {
            string tempalteFolder = ucCombox1.SelectedValue;

            string target_folder = NXUtils.select_folder();
            if (!"".Equals(target_folder) && !"".Equals(tempalteFolder))
            {
                //string template_folder = Constain.VENDORDIR + "\\template\\";
                //string target_folder = Path.GetDirectoryName(file_path);
                //string replace_name = Path.GetFileNameWithoutExtension(file_path);
                FileUtil.CopyDirectory(tempalteFolder, target_folder);
                //file_path
                //NXUtils.AssemClone(target_folder, template_path, template_name, replace_name);
                //NXUtils.addcomponent(target_folder + "\\" + template_name + ".prt");
            }
            Dispose();
        }


        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
