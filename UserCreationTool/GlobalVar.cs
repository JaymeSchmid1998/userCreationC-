using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserCreationTool
{
   static class GlobalVar
    {
        private static string _PlacenameVar = "";

        public static string GlobalVar1
        {
            get { return _PlacenameVar; }
            set { _PlacenameVar = value; }
        }

        private static long _generatednum = 0;

        public static long GlobalVar2
        {
            get { return _generatednum; }
            set { _generatednum = value; }
        }

        private static string _path = @"C:\Users\zbook j\Desktop\test";

        public static string GlobalVar3
        {
            get { return _path; }
            set { _path = value; }
        }

        private static string _path2 = @"C:\Users\zbook j\Desktop\test";

        public static string GlobalVar4
        {
            get { return _path2; }
            set { _path2 = value; }
        }


    }
}
