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
    public partial class Form1 : Form
    {

        List<Control> parameterFieldList = null;
        List<Control> updateFieldList = null;
        int lastWidth = 1000;
        ButtonBean buttonBean = null;
        private static Form1 _instance = null;
        public Form1(ButtonBean inputBean)
        {
            this.buttonBean = inputBean;
            parameterFieldList = new List<Control>();
            updateFieldList = new List<Control>();
            InitializeComponent();
            NXOpenUI.FormUtilities.SetApplicationIcon(this);
            NXOpenUI.FormUtilities.ReparentForm(this);
        }
        public static Form1 InstanceObject(ButtonBean inputBean)
        {
            if (_instance == null)
                _instance = new Form1(inputBean);
            return _instance;
        }
        private void loadForm(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void Form1_LoadTst(object sender, EventArgs e)
        {
            this.ClientSize = new System.Drawing.Size(Settings.Default.width, Settings.Default.height);
            // 
            // groupBox4
            // 
            List<ClassBean> classBeans = buttonBean.classBeans;
            for (int i = 0; i < classBeans.Count; i++)
            {
                ClassBean bean = classBeans[i];
                classBeanCombox.Items.Add(bean);
            }

            List<TabBean> tabBeans = buttonBean.TabBeans;
            for (int i = 0; i < tabBeans.Count; i++)
            {
                TabBean tabBean = tabBeans[i];
                TabPage tabPage = new TabPage();
                tabPage.Location = new System.Drawing.Point(4, 28);
                tabPage.Name = "tabPage1";
                tabPage.Padding = new System.Windows.Forms.Padding(3);
                tabPage.Size = new System.Drawing.Size(957, 423);
                tabPage.TabIndex = 0;
                tabPage.Text = tabBean.TabName;
                tabPage.UseVisualStyleBackColor = true;
                tabPage.Tag = tabBean;
                this.tabControl1.Controls.Add(tabPage);

                SplitContainer splitContainer = new SplitContainer();
                splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
                splitContainer.Location = new System.Drawing.Point(3, 3);
                splitContainer.Name = "splitContainer1";
                splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
                splitContainer.Size = new System.Drawing.Size(957, 348);
                splitContainer.SplitterDistance = 200;
                splitContainer.SplitterIncrement = 10;
                //splitContainer.SplitterWidth = 5;
                splitContainer.TabIndex = 0;
                splitContainer.BackColor = Color.Gray;
                splitContainer.Panel1.BackColor = SystemColors.ControlLight;
                splitContainer.Panel2.BackColor = SystemColors.ControlLight;
                string bitmapName = tabBean.Bitmap; //buttonBean.IMAGE;// 
                ;
                int rowindex = 0;
                if (i == 0)
                {
                    if (File.Exists(bitmapName))
                        this.pictureBox1.Image = Image.FromFile(bitmapName);
                }
                    ;// (Constain.VENDORDIR + "\\bmp\\" + bitmapName);
                TableLayoutPanel tableLayoutPanel = new TableLayoutPanel()
                {
                    RowCount = 1,
                    ColumnCount = 1,
                    //AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    Location = new System.Drawing.Point(12, 12),
                    Dock = DockStyle.Fill,
                    AutoScroll = true,
                };

                //tableLayoutPanel.RowStyles.Clear();
                //tableLayoutPanel.ColumnStyles.Clear();
                TableLayoutColumnStyleCollection styles = tableLayoutPanel.ColumnStyles;

                //foreach (ColumnStyle style in styles)
                //{
                //     style.SizeType = SizeType.Percent;
                //     style.Width = 25;
                // }


                List<ReferenceButtonBean> referenceButtonBeans = tabBean.referenceButtonBeans;

                List<ParameterBean> parameterBeans = tabBean.ParameterBeans;

                List<SkechButtonBean> buttonBeans = tabBean.SkecthButtonBeans;
                

                //for (int j = 0; j < 2; j++)
                {
                //    tableLayoutPanel.ColumnCount++;
                //    tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                }

                //for (int j = 0; j < (referenceButtonBeans.Count / 2 + 1) + parameterBeans.Count + (buttonBeans.Count / 2 + 1); j++)
                //{
                //    tableLayoutPanel.RowCount++;
                //    tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
                //}


                if (referenceButtonBeans != null)
                {
                    for (int ii = 0; ii < referenceButtonBeans.Count; ii++)
                    {
                        ReferenceButtonBean referenceButtonBean = referenceButtonBeans[ii];
                        Button button = new Button();
                        button.Location = new System.Drawing.Point(398, 1225);
                        button.Name = "okButton";
                        button.Dock = System.Windows.Forms.DockStyle.Fill;
                        //button.Size = new System.Drawing.Size(240, 30);
                        button.TabIndex = 6;
                        button.Text = referenceButtonBean.ButtonName;
                        button.UseVisualStyleBackColor = true;
                        button.Tag = referenceButtonBean;
                        button.Click += new System.EventHandler(referenceButton_Click);
                        //if(ii%2 == 1)
                        { 
                        //    tableLayoutPanel.Controls.Add(button,1, rowindex);
                        //    rowindex++;
                        }
                        //else
                        {
                            tableLayoutPanel.RowCount++;
                            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
                            tableLayoutPanel.Controls.Add(button, 0, rowindex++);

                        }
                    }
                    //splitContainer.Panel2.Controls.Add(buttonflowLayoutPane);
                }


                for (int ii = 0; ii < parameterBeans.Count; ii++)
                {

                    TableLayoutPanel parameterTableLayoutPanel = new TableLayoutPanel()
                    {
                        RowCount = 1,
                        ColumnCount = 2,
                        //AutoSize = true,
                        AutoSizeMode = AutoSizeMode.GrowAndShrink,
                        Location = new System.Drawing.Point(12, 12),
                        Dock = DockStyle.Fill,
                        AutoScroll = false,
                    };
                    for (int j = 0; j < 2; j++)
                    {
                        parameterTableLayoutPanel.ColumnCount++;
                        parameterTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
                    }

                    ParameterBean parameterBean = parameterBeans[ii];
                    Label label = new Label();
                    Control control = null;
                    label.AutoSize = true;
                    label.Location = new System.Drawing.Point(276, 26);
                    label.Dock = System.Windows.Forms.DockStyle.Fill;
                    label.Name = parameterBean.ExpressionName + "_label";
                    label.Size = new System.Drawing.Size(130, 30);
                    label.TabIndex = 2;
                    label.TextAlign = ContentAlignment.MiddleLeft;
                    label.Text = parameterBean.DisplayName;
                    //label.Font = new System.Drawing.Font("宋体", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

                    string expValue = "0";
                    NXUtils.getexpvalue(parameterBean.ExpressionName, out expValue);

                    switch (parameterBean.ValueType)
                    {
                        case "1": //仅可以输入的参数
                            {
                                TextBox textBox = new System.Windows.Forms.TextBox();
                                textBox.Dock = System.Windows.Forms.DockStyle.Fill;
                                textBox.Location = new System.Drawing.Point(85, 162);
                                textBox.Name = parameterBean.ExpressionName + "_textBox";
                                textBox.Size = new System.Drawing.Size(95, 20);
                                textBox.Text = expValue;
                                textBox.Tag = parameterBean;
                                textBox.TextChanged += new System.EventHandler(valueChange_Handler);
                                control = textBox;
                                break;
                            };
                        case "2": //可以选择也可以输入的参数
                            {
                                ComboBox comboBox = new ComboBox();

                                comboBox.Dock = System.Windows.Forms.DockStyle.Fill;
                                comboBox.FormattingEnabled = true;
                                comboBox.Location = new System.Drawing.Point(3, 24);
                                comboBox.Name = parameterBean.ExpressionName + "_comboBox";
                                comboBox.Size = new System.Drawing.Size(95, 20);
                                comboBox.Text = expValue;
                                comboBox.Tag = parameterBean;
                                comboBox.TextChanged += new System.EventHandler(valueChange_Handler);
                                if (parameterBean.DisRealValueDic != null)
                                {
                                    comboBox.Items.AddRange(parameterBean.DisRealValueDic.Keys.ToArray());
                                };
                                control = comboBox;
                                break;
                            };
                        case "3": //仅可以选择的参数
                            {
                                ComboBox comboBox = new ComboBox();

                                comboBox.Dock = System.Windows.Forms.DockStyle.Fill;
                                comboBox.FormattingEnabled = true;
                                comboBox.Location = new System.Drawing.Point(3, 24);
                                comboBox.Name = parameterBean.ExpressionName + "_comboBox";
                                comboBox.Size = new System.Drawing.Size(95, 20);
                                comboBox.Text = expValue;
                                comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                                comboBox.Tag = parameterBean;
                                comboBox.TextChanged += new System.EventHandler(valueChange_Handler);
                                if (parameterBean.DisRealValueDic != null)
                                {
                                    comboBox.Items.AddRange(parameterBean.DisRealValueDic.Keys.ToArray());

                                    //Linq
                                    var keys = parameterBean.DisRealValueDic.Where(q => q.Value == expValue).Select(q => q.Key);  //get all keys

                                    List<string> keyList = (from q in parameterBean.DisRealValueDic
                                                            where q.Value == expValue
                                                            select q.Key).ToList<string>(); //get all keys

                                    string firstKey = parameterBean.DisRealValueDic.FirstOrDefault(q => q.Value == expValue).Key;  //get first key
                                    comboBox.Text = firstKey;
                                }
                                control = comboBox;
                                break;
                            };
                        case "4"://仅可以查看的
                            {
                                double realValue = 0;
                                NXUtils.getexpReadvalue(parameterBean.ExpressionName, out realValue); 
                                TextBox textBox = new System.Windows.Forms.TextBox();
                                textBox.Dock = System.Windows.Forms.DockStyle.Fill;
                                textBox.Location = new System.Drawing.Point(85, 162);
                                textBox.Name = parameterBean.ExpressionName + "_textBox";
                                textBox.Size = new System.Drawing.Size(95, 20);
                                textBox.Enabled = false;
                                textBox.Text = realValue.ToString();
                                textBox.Tag = parameterBean;
                                textBox.TextChanged += new System.EventHandler(valueChange_Handler);
                                control = textBox;
                                //textBox.TabIndex = 1;
                                break;
                            };
                    }

                    control.Dock = System.Windows.Forms.DockStyle.Fill;
                    if (parameterBean.isDisplay)
                    {

                        parameterTableLayoutPanel.Controls.Add(label, 0, ii);
                        parameterTableLayoutPanel.Controls.Add(control, 1, ii);
                        tableLayoutPanel.RowCount++;
                        tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
                        tableLayoutPanel.Controls.Add(parameterTableLayoutPanel, 0, rowindex++);
                        //if (index % 2 != 0)
                        //    label.Margin = new System.Windows.Forms.Padding(60, 0, 0, 0);
                        //parameterflowLayoutPane.Controls.Add(control);
                        //rowindex++;
                    }
                    parameterFieldList.Add(control);
                    
                }
                //splitContainer.Panel1.Controls.Add(parameterflowLayoutPane);
                if (buttonBeans != null)
                {
                    for (int ii = 0; ii < buttonBeans.Count; ii++)
                    {
                        tableLayoutPanel.RowCount++;
                        SkechButtonBean sketchButtonBean = buttonBeans[ii];
                        Button button = new Button();
                        button.Location = new System.Drawing.Point(398, 1225);
                        button.Name = "okButton";
                        //button.Size = new System.Drawing.Size(240, 30);
                        button.Dock = System.Windows.Forms.DockStyle.Fill;
                        button.TabIndex = 6;
                        button.Text = sketchButtonBean.ButtonName;
                        button.UseVisualStyleBackColor = true;
                        button.Tag = sketchButtonBean;
                        button.Click += new System.EventHandler(sketchButton_Click);
                        //if (ii % 2 == 1)
                        //{
                        //    tableLayoutPanel.Controls.Add(button, 1, rowindex);
                        //    rowindex++;
                        //}
                        //else
                        {
                            tableLayoutPanel.RowCount++;
                            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
                            tableLayoutPanel.Controls.Add(button, 0, rowindex++);
                        }
                    }
                    //splitContainer.Panel2.Controls.Add(buttonflowLayoutPane);
                }
                tabPage.Controls.Add(tableLayoutPanel);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 
            // groupBox4
            // 
            List<ClassBean> classBeans = buttonBean.classBeans;
            for (int i = 0; i < classBeans.Count; i++)
            {
                ClassBean bean = classBeans[i];
                classBeanCombox.Items.Add(bean);
            }

            List<TabBean> tabBeans = buttonBean.TabBeans;
            for (int i = 0; i < tabBeans.Count; i++)
            {
                TabBean tabBean = tabBeans[i];
                TabPage tabPage = new TabPage();
                tabPage.Location = new System.Drawing.Point(4, 28);
                tabPage.Name = "tabPage1";
                tabPage.Padding = new System.Windows.Forms.Padding(3);
                tabPage.Size = new System.Drawing.Size(957, 423);
                tabPage.TabIndex = 0;
                tabPage.Text = tabBean.TabName;
                tabPage.UseVisualStyleBackColor = true;
                tabPage.Tag = tabBean;
                this.tabControl1.Controls.Add(tabPage);

                SplitContainer splitContainer = new SplitContainer();
                splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
                splitContainer.Location = new System.Drawing.Point(3, 3);
                splitContainer.Name = "splitContainer1";
                splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
                splitContainer.Size = new System.Drawing.Size(957, 348);
                splitContainer.SplitterDistance = 200;
                splitContainer.SplitterIncrement = 10;
                //splitContainer.SplitterWidth = 5;
                splitContainer.TabIndex = 0;
                splitContainer.BackColor = Color.Gray;
                splitContainer.Panel1.BackColor = SystemColors.ControlLight;
                splitContainer.Panel2.BackColor = SystemColors.ControlLight;
                string bitmapName = tabBean.Bitmap; //buttonBean.IMAGE;// 
                FlowLayoutPanel parameterflowLayoutPane = new FlowLayoutPanel();
                ;
                int index = 0;
                if (i == 0)
                {
                    if (File.Exists(bitmapName))
                        this.pictureBox1.Image = Image.FromFile(bitmapName);
                }
                    ;// (Constain.VENDORDIR + "\\bmp\\" + bitmapName);
                parameterflowLayoutPane.Dock = System.Windows.Forms.DockStyle.Fill;
                parameterflowLayoutPane.Location = new System.Drawing.Point(3, 3);
                parameterflowLayoutPane.Name = "flowLayoutPanel1";
                ///parameterflowLayoutPane.Size = new System.Drawing.Size(954, 201);
                parameterflowLayoutPane.AutoScroll = true;
                //parameterflowLayoutPane.Padding = new Padding(50);
                //parameterflowLayoutPane.Margin = new Padding(50);
                parameterflowLayoutPane.TabIndex = 0;
                parameterflowLayoutPane.AutoSize = true;
                List<ReferenceButtonBean> referenceButtonBeans = tabBean.referenceButtonBeans;
                if (referenceButtonBeans != null)
                {
                    for (int ii = 0; ii < referenceButtonBeans.Count; ii++)
                    {
                        ReferenceButtonBean referenceButtonBean = referenceButtonBeans[ii];
                        Button button = new Button();
                        button.Location = new System.Drawing.Point(398, 1225);
                        button.Name = "okButton";
                        button.Size = new System.Drawing.Size(240, 30);
                        button.TabIndex = 6;
                        button.Text = referenceButtonBean.ButtonName;
                        button.UseVisualStyleBackColor = true;
                        if (index % 2 != 0)
                            button.Margin = new System.Windows.Forms.Padding(50, 5, 0, 0);
                        button.Tag = referenceButtonBean;
                        button.Click += new System.EventHandler(referenceButton_Click);
                        parameterflowLayoutPane.Controls.Add(button);
                        index++;
                    }
                    //splitContainer.Panel2.Controls.Add(buttonflowLayoutPane);
                }


                List<ParameterBean> parameterBeans = tabBean.ParameterBeans; //buttonBean.parameterBeans;//
                for (int ii = 0; ii < parameterBeans.Count; ii++)
                {
                    
                    ParameterBean parameterBean = parameterBeans[ii];
                    Label label = new Label();
                    Control control = null;
                    label.AutoSize = true;
                    label.Location = new System.Drawing.Point(276, 26);
                    label.Name = parameterBean.ExpressionName + "_label";
                    label.Size = new System.Drawing.Size(130, 30);
                    label.TabIndex = 2;
                    label.TextAlign = ContentAlignment.MiddleLeft;
                    label.Text = parameterBean.DisplayName;
                    //label.Font = new System.Drawing.Font("宋体", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

                    string expValue = "0";
                    NXUtils.getexpvalue(parameterBean.ExpressionName, out expValue);

                    switch (parameterBean.ValueType)
                    {
                        case "1": //仅可以输入的参数
                            {
                                TextBox textBox = new System.Windows.Forms.TextBox();
                                textBox.Dock = System.Windows.Forms.DockStyle.Fill;
                                textBox.Location = new System.Drawing.Point(85, 162);
                                textBox.Name = parameterBean.ExpressionName + "_textBox";
                                textBox.Size = new System.Drawing.Size(95, 20);
                                textBox.Text = expValue;
                                textBox.Tag = parameterBean;
                                textBox.TextChanged += new System.EventHandler(valueChange_Handler);
                                control = textBox;
                                break;
                            };
                        case "2": //可以选择也可以输入的参数
                            {
                                ComboBox comboBox = new ComboBox();

                                comboBox.Dock = System.Windows.Forms.DockStyle.Fill;
                                comboBox.FormattingEnabled = true;
                                comboBox.Location = new System.Drawing.Point(3, 24);
                                comboBox.Name = parameterBean.ExpressionName+"_comboBox";
                                comboBox.Size = new System.Drawing.Size(95, 20);
                                comboBox.Text = expValue;
                                comboBox.Tag = parameterBean;
                                comboBox.TextChanged += new System.EventHandler(valueChange_Handler);
                                if (parameterBean.DisRealValueDic != null)
                                {
                                    comboBox.Items.AddRange(parameterBean.DisRealValueDic.Keys.ToArray());
                                };
                                control = comboBox;
                                break;
                            };
                        case "3": //仅可以选择的参数
                            {
                                ComboBox comboBox = new ComboBox();

                                comboBox.Dock = System.Windows.Forms.DockStyle.Fill;
                                comboBox.FormattingEnabled = true;
                                comboBox.Location = new System.Drawing.Point(3, 24);
                                comboBox.Name = parameterBean.ExpressionName + "_comboBox";
                                comboBox.Size = new System.Drawing.Size(95, 20);
                                comboBox.Text = expValue;
                                comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                                comboBox.Tag = parameterBean;
                                comboBox.TextChanged += new System.EventHandler(valueChange_Handler);
                                if (parameterBean.DisRealValueDic != null)
                                {
                                    comboBox.Items.AddRange(parameterBean.DisRealValueDic.Keys.ToArray());

                                    //Linq
                                    var keys = parameterBean.DisRealValueDic.Where(q => q.Value == expValue).Select(q => q.Key);  //get all keys

                                    List<string> keyList = (from q in parameterBean.DisRealValueDic
                                                            where q.Value == expValue
                                                            select q.Key).ToList<string>(); //get all keys

                                    string firstKey = parameterBean.DisRealValueDic.FirstOrDefault(q => q.Value == expValue).Key;  //get first key
                                    comboBox.Text = firstKey;
                                }
                                control = comboBox;
                                break;
                            };
                        case "4"://仅可以查看的
                            {
                                TextBox textBox = new System.Windows.Forms.TextBox();
                                textBox.Dock = System.Windows.Forms.DockStyle.Fill;
                                textBox.Location = new System.Drawing.Point(85, 162);
                                textBox.Name = parameterBean.ExpressionName + "_textBox";
                                textBox.Size = new System.Drawing.Size(95, 20);
                                textBox.Enabled = false;
                                textBox.Text = expValue;
                                textBox.Tag = parameterBean;
                                textBox.TextChanged += new System.EventHandler(valueChange_Handler);
                                control = textBox;
                                //textBox.TabIndex = 1;
                                break;
                            };
                    }
                    if (parameterBean.isDisplay)
                    {

                        parameterflowLayoutPane.Controls.Add(label);
                        if (index % 2 != 0)
                            label.Margin = new System.Windows.Forms.Padding(60, 0, 0, 0);
                        parameterflowLayoutPane.Controls.Add(control);
                        index++;


                        parameterFieldList.Add(control);
                    }
                }
                //splitContainer.Panel1.Controls.Add(parameterflowLayoutPane);
                List<SkechButtonBean> buttonBeans = tabBean.SkecthButtonBeans;
                if(buttonBeans!=null)
                { 
                    for (int ii = 0; ii < buttonBeans.Count; ii++)
                    {
                        SkechButtonBean sketchButtonBean = buttonBeans[ii];
                        Button button = new Button();
                        button.Location = new System.Drawing.Point(398, 1225);
                        button.Name = "okButton";
                        button.Size = new System.Drawing.Size(240, 30);
                        button.TabIndex = 6;
                        button.Text = sketchButtonBean.ButtonName;
                        button.UseVisualStyleBackColor = true;
                        if (index % 2 != 0)
                            button.Margin = new System.Windows.Forms.Padding(50, 5, 0, 0);
                        button.Tag = sketchButtonBean;
                        button.Click += new System.EventHandler(sketchButton_Click);
                        parameterflowLayoutPane.Controls.Add(button);
                        index++;
                    }
                    //splitContainer.Panel2.Controls.Add(buttonflowLayoutPane);
                }
                tabPage.Controls.Add(parameterflowLayoutPane);
            }
            
        }

        
        private void valueChange_Handler(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            updateFieldList.Add(control);
        }

        private void sketchButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            SkechButtonBean bean = (SkechButtonBean)button.Tag;
            NXUtils.activesketch(bean.SketchName);
        }

        private void referenceButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            ReferenceButtonBean bean = (ReferenceButtonBean)button.Tag;
            try
            {
                switch (bean.ReferenceType)
                {
                    case "6": NXUtils.editPointFeature(bean); break;
                    case "7": NXUtils.editFaceFeature(bean); break;
                    case "8": NXUtils.editEdgeFeature(bean); break;
                    case "9": NXUtils.editBodyFeature(bean); break;
                    case "10": NXUtils.editSketchFeature(bean); break;
                    case "11": NXUtils.editCsysFeature(bean); break;
                    case "12": NXUtils.editDatumPlaneFeature(bean); break;
                }
            }
            catch (NXException ex)
            {
                Project.theUI.NXMessageBox.Show("Error", NXMessageBox.DialogType.Error, ex.Message);
                return;
            }
            
            //NXUtils.editEdgeFeature(bean); ;
            //NXUtils.activesketch(bean.SketchName);
        }
        
        private void classBeanCombox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClassBean classBean = (ClassBean)classBeanCombox.SelectedItem;
            Dictionary<string,string>expDic = classBean.expDictionary;
            foreach (string item in expDic.Keys)
            {
                string value = expDic[item];
                Control control = parameterFieldList.Find(x => x.Name.StartsWith(item+"_"));
                if(control!=null)
                {
                    control.Text = value;
                }
            }


        }

        private void updatePartExpressions()
        {
            for (int i = 0; i < updateFieldList.Count; i++)
            {
                string expValue = "";
                Control control = updateFieldList[i];
                ParameterBean parameterBean = (ParameterBean)control.Tag;
                if (parameterBean.ValueType == "3")
                { 
                    if (parameterBean.DisRealValueDic!=null)
                    {
                        string value = "";
                        if (parameterBean.DisRealValueDic.TryGetValue(control.Text, out value))
                        {
                            expValue = value;
                        }else
                        {
                            string errorMessage = "未找到" + parameterBean.DisplayName + "参数[" + control.Text + "]对应的真实值,请检查Excel配置文件";
                            Project.theUI.NXMessageBox.Show("Error", NXMessageBox.DialogType.Error, errorMessage);
                        }
                    }
                }
                else 
                {
                    expValue = control.Text;
                }
                NXUtils.updateExpValue(parameterBean, expValue);
            }
            Project.theSession.UpdateManager.DoUpdate(Project.theSession.NewestVisibleUndoMark);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            updatePartExpressions();
            this.Close();
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            updatePartExpressions();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            TabBean tabBean = (TabBean)e.TabPage.Tag;
            if (File.Exists(tabBean.Bitmap))
                this.pictureBox1.Image = Image.FromFile(tabBean.Bitmap);
            //MessageBox.Show(tabBean.Bitmap);
        }


        private void expandButton_Click(object sender, EventArgs e)
        {
            int currentHeight = this.ClientSize.Height;

            string buttonText = button1.Text;
            if ("<".Equals(buttonText))
            {
                this.pictureBox1.Visible = true;//.Panel2Collapsed = !checkBox1.Checked;
                this.ClientSize = new System.Drawing.Size(lastWidth, currentHeight);
                this.button1.Text = ">";
            }
            else
            {
                lastWidth = this.ClientSize.Width;
                this.pictureBox1.Visible = false;//.Panel2Collapsed = !checkBox1.Checked;
                this.ClientSize = new System.Drawing.Size(290, currentHeight);
                this.button1.Text = "<";
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.Default.width = this.ClientSize.Width;
            Settings.Default.height = this.ClientSize.Height;
            Settings.Default.Save();
            _instance = null;
        }
    }
}
