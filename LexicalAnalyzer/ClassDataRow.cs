using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer
{
    class ClassDataRow
    {
        public string Name;
        public string Type;
        public string AM;
        public string TM;
        public ClassDataRow(string name, string type, string am, string tm)
        {
            Name = name;
            Type = type;
            AM = am;
            TM = tm;
        }
    }
}
