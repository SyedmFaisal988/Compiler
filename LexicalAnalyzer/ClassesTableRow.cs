using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer
{
    class ClassesTableRow
    {
        public string Name;
        public string Type;
        public string Parent;
        public string Category;
        public ClassData Link;
        public ClassesTableRow(string name, string type, string category, string parent)
        {
            Name = name;
            Type = type;
            Parent = parent;
            category = Category;
            Link = new ClassData();
        }
    }
}
