using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.Bean
{
    public class ClassBean
    {
        public string Name { get; set; }
        public Dictionary<string,string> expDictionary { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}   
