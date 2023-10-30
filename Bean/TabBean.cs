using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.Bean
{
    public class TabBean
    {
        //类型名称
        public string TabName { get; set; }
        public int index { get; set; }

        //按钮图片
        public List<ParameterBean> ParameterBeans { get; set; }
        public List<SkechButtonBean> SkecthButtonBeans { get; set; }
        public List<ReferenceButtonBean> referenceButtonBeans { get; set; }

        public string Bitmap { get; set; }



    }

}
