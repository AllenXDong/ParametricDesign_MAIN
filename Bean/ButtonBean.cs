using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.Bean
{
    public class ButtonBean
    {
        //按钮名称
        public string Name { get; set; }
        public string ID_VALUE { get; set; }

        public string ID_NAME { get; internal set; }
        public string ICON { get; internal set; }
        public string IMAGE { get; internal set; }

        public NXOpen.MenuBar.MenuButton button { get; set; }
        public bool Sensitive { get; set; }

        public List<TabBean> TabBeans { get; set; }

        public List<ClassBean> classBeans { get; set; }
        public List<ParameterBean> parameterBeans { get; set; }
        public string templateName { get; set; }
        public string excelPath { get; set; }
        public string excelSheet { get; set; }

        public override string ToString()
        {
            return "Name = "+Name + ",ID_VALUE = " + ID_VALUE + ",ID_NAME = " + ID_NAME + ",ICON = "+ ICON + ",IMAGE = "+ IMAGE;
        }
    }

}
