using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer
{
    class ClassData
    {
        List<ClassDataRow> classData;
        public ClassData()
        {
            classData = new List<ClassDataRow>();
        }
        public bool addRow(string name, string type, string am)
        {
            foreach(var temp in classData)
            {
                if(temp.Name == name)
                {
                    return false;
                }
            }
            classData.Add(new ClassDataRow(name, type, am));
            return true;
        }
        public ClassDataRow getRow(string name)
        {
            ClassDataRow result = null;
            foreach(var temp in classData)
            {
                if (temp.Name == name)
                     result = temp;
            }
            return result;
        }
    }
}
