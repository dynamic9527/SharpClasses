using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TOOL
{
    class ThisInfo
    {
        public static string ProgPath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }
        public static string Version
        {
            get
            {
                return Application.ProductVersion;
            }
        }
    }
}
