using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using MainProject.Bean;

namespace MainProject.Utils
{
    class XmlDocumentUtil
    {
        public static List<ButtonBean> LoadXML(string filePath)
        {
            List<ButtonBean> buttonBeans = new List<ButtonBean>(); 
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlElement roteNode = (XmlElement)doc.SelectSingleNode("ParametricDesignConfig");//選擇節點
            if (roteNode == null)
                return null;
            XmlNodeList buttonNodeList = roteNode.GetElementsByTagName("BUTTON");

            for (int i = 0; i < buttonNodeList.Count; i++)
            {
                XmlElement buttonNode = (XmlElement)buttonNodeList.Item(i);
                ButtonBean buttonBean = new ButtonBean();

                XmlNodeList nls = buttonNode.ChildNodes;//继续获取xe子节点的所有子节点
                foreach (XmlNode xn1 in nls)//遍历
                {
                    if (xn1.Name == "LABEL")//如果找到
                    {
                        buttonBean.Name = xn1.InnerText;
                    }
                    if (xn1.Name == "ID_NAME")//如果找到
                    {
                        buttonBean.ID_NAME = xn1.InnerText;
                    }
                    if (xn1.Name == "ID_VALUE")//如果找到
                    {
                        buttonBean.ID_VALUE = xn1.InnerText;
                    }
                    if (xn1.Name == "ICON")//如果找到
                    {
                        buttonBean.ICON = xn1.InnerText;
                    }
                    if (xn1.Name == "IMAGE")//如果找到
                    {
                        buttonBean.IMAGE = xn1.InnerText;
                    }
                    if (xn1.Name == "TEMPLATE_MODEL")//如果找到
                    {
                        buttonBean.templateName = xn1.InnerText;
                    }
                    if (xn1.Name == "EXCEL")//如果找到
                    {
                        buttonBean.excelPath = xn1.InnerText;
                    }
                    if (xn1.Name == "EXCEL_SHEET")//如果找到
                    {
                        buttonBean.excelSheet = xn1.InnerText;
                    }
                    
                }
                /*buttonBean.Name = buttonNode.GetElementsByTagName("LABEL")[0].Value;
                buttonBean.ID_NAME = buttonNode.GetAttribute("ID_NAME");
                buttonBean.ID_VALUE = buttonNode.GetAttribute("ID_VALUE");
                buttonBean.ICON = buttonNode.GetAttribute("ICON");
                buttonBean.IMAGE = buttonNode.GetAttribute("IMAGE");
                buttonBean.templateName = buttonNode.GetAttribute("TEMPLATE_MODEL");
                buttonBean.excelPath = buttonNode.GetAttribute("EXCEL");*/
                XmlNodeList tabNodeList = buttonNode.GetElementsByTagName("TAB");
                XmlNodeList moduleNodeList = buttonNode.GetElementsByTagName("MODULE");

                List<ClassBean> classBeans = new List<ClassBean>();
                if (moduleNodeList.Count!=0)
                {
                    XmlElement moduleNode = (XmlElement)moduleNodeList.Item(0);
                    XmlNodeList classNodeList = moduleNode.GetElementsByTagName("COMBOXVALUE");
                    for (int ii = 0; ii < classNodeList.Count; ii++)
                    {
                        ClassBean classBean = new ClassBean();
                        XmlElement classNode = (XmlElement)classNodeList.Item(ii);
                        string className = classNode.GetAttribute("NAME");
                        XmlNodeList expressionNodeList = classNode.GetElementsByTagName("EXPRESION");
                        classBean.Name = className;
                        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                        for (int iii = 0; iii < expressionNodeList.Count; iii++)
                        {
                            XmlElement expressionNode = (XmlElement)expressionNodeList.Item(iii);
                            string expName = expressionNode.GetAttribute("NAME");
                            string expValue = expressionNode.GetAttribute("VALUE");
                            keyValuePairs.Add(expName, expValue);
                        }
                        classBean.expDictionary = keyValuePairs;
                        classBeans.Add(classBean);
                    }
                }
                buttonBean.classBeans = classBeans;

                List<TabBean> tabBeans = new List<TabBean>();
                for (int ii = 0; ii < tabNodeList.Count; ii++)
                {
                    XmlElement tabNode = (XmlElement)tabNodeList.Item(ii);
                    TabBean tabBean = new TabBean();
                    tabBean.TabName = tabNode.GetAttribute("NAME");
                    tabBean.Bitmap = tabNode.GetAttribute("BMP");
                    XmlNodeList sketchButtonList = tabNode.GetElementsByTagName("SKETCHBUTTONS");
                    XmlNodeList parameterListList = tabNode.GetElementsByTagName("PARAMETERS");//ChildNodes;
                    if (parameterListList.Count != 0)
                    {
                        List<ParameterBean> parameterBeans = new List<ParameterBean>();
                        XmlElement tabChildNode = (XmlElement)parameterListList.Item(0);
                        XmlNodeList parameterListNode = tabChildNode.GetElementsByTagName("PARAMETER");
                        for (int iii = 0; iii < parameterListNode.Count; iii++)
                        {
                            ParameterBean parameterBean = new ParameterBean();
                            XmlElement parameterNode = (XmlElement)parameterListNode.Item(iii);
                            parameterBean.DisplayName = parameterNode.GetAttribute("DISPLAYNAME");
                            parameterBean.ExpressionName = parameterNode.GetAttribute("EXPRESSIONNAME");
                            parameterBean.ValueType = parameterNode.GetAttribute("TYPE");
                            parameterBean.isDisplay = "是".Equals(parameterNode.GetAttribute("ISDISPLAY")) ? true : false;
                            string values = parameterNode.GetAttribute("VALUES");
                            if(!"".Equals(values))
                            {
                                //parameterBean.Values = values.Split(';');
                            }
                            parameterBeans.Add(parameterBean);
                        }
                        tabBean.ParameterBeans = parameterBeans;
                    }
                    
                    if(sketchButtonList.Count!=0)
                    {
                        List<SkechButtonBean> sketchButtonBeans = new List<SkechButtonBean>();
                        XmlElement sketchButtonListNode = (XmlElement)sketchButtonList.Item(0);
                        XmlNodeList sketchButtonNodeList = sketchButtonListNode.GetElementsByTagName("SKETCHBUTTON");
                        for (int iii = 0; iii < sketchButtonNodeList.Count; iii++)
                        {
                            SkechButtonBean skechButtonBean= new SkechButtonBean();
                            XmlElement skechButtonNode = (XmlElement)sketchButtonNodeList.Item(iii);
                            skechButtonBean.SketchName = skechButtonNode.GetAttribute("SKECHNAME");
                            skechButtonBean.ButtonName = skechButtonNode.GetAttribute("DISPLAYNAME");
                            sketchButtonBeans.Add(skechButtonBean);
                        }
                        tabBean.SkecthButtonBeans = sketchButtonBeans;
                    }
                    tabBeans.Add(tabBean);
                }
                buttonBean.TabBeans = tabBeans;
                buttonBeans.Add(buttonBean);
            }
            return buttonBeans;
        }

        public static List<TemplateItem> LoadAllTemplateFromXML(string filePath)
        {
            List<TemplateItem> templateItemList = new List<TemplateItem>();
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlElement roteNode = (XmlElement)doc.SelectSingleNode("ParametricDesignConfig");//選擇節點
            if (roteNode == null)
                return null;
            XmlNodeList configItemNodeList = roteNode.GetElementsByTagName("TEMPLATE");

            for (int j = 0; j < configItemNodeList.Count; j++)
            {
                TemplateItem buttonBean = new TemplateItem();
                XmlElement buttonNode = (XmlElement)configItemNodeList.Item(j);
                buttonBean.DISPLAYNAME = buttonNode.GetAttribute("DISPLAYNAME");
                buttonBean.TEMPLATEPATH = buttonNode.GetAttribute("PATH");

                templateItemList.Add(buttonBean);
            }
            return templateItemList;
        }

    }
}
