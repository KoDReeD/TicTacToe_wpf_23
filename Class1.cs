using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrestikiNoliki
{
    public static class Manager
    {
        public static string name1 { get; set; }
        public static string name2 { get; set; }
        public static string selecteg { get; set; }

        public static void SetSettings(string n1, string n2, string select)
        {
            name1 = n1;
            name2 = n2;
            selecteg = select;
        }
    }
}
