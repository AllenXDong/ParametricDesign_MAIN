using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.Utils
{
    public class Constain
    {

        public static string VENDORDIR = getVendorDir();// dllPath.Substring(0, dllPath.LastIndexOf("\\"));


        static string getVendorDir()
        {
            string dllPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                dllPath = dllPath.Substring(0, dllPath.LastIndexOf("\\"));
                dllPath = dllPath.Substring(0, dllPath.LastIndexOf("\\"));

            return dllPath;
        }

    }
}
