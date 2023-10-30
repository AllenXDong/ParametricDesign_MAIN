using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.Bean
{
    public class ParameterBean
    {
        public string DisplayName { get; set; }
        public string ExpressionName { get; set; }


        public string ValueType { get; set; }
        public string ValueScope { get; set; }

        public Dictionary<string, string> DisRealValueDic{ get; set; } //圆形=1,方形=2,椭圆形=3
    public Boolean isDisplay { get; set; }
    }
}
