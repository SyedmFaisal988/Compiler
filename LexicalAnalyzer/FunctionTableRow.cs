using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer
{
    class FunctionTableRow
    {
        public string Name;
        public string Type;
        public int Scope;
        public FunctionTableRow(string name, string type, int scope)
        {
            Name = name;
            Type = type;
            Scope = scope;
        }
    }
}
