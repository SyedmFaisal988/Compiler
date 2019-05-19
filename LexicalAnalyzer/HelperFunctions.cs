using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer
{
    class HelperFunctions
    {
        List<ClassesTableRow> ClassesTable;
        List<FunctionTableRow> FunctionTable;
        Stack<int> ScopeHirarchy;
        int currentScope;
        int scopeCount;
        public HelperFunctions()
        {
            ClassesTable = new List<ClassesTableRow>();
            FunctionTable = new List<FunctionTableRow>();
            ScopeHirarchy = new Stack<int>();
            currentScope = -1;
            scopeCount = 0;

        }
        public string lookup(string name) 
        {
            string type = "";
            foreach(ClassesTableRow temp in ClassesTable)
            {
                if (temp.Name == name)
                    type = temp.Type;
            }
            return type;
        }
        public ClassDataRow lookupCT(string name, ClassData Ref )
        {
            return Ref.getRow(name);
        }
        public string lookupFT(string name, string className)
        {
            string type = "";
            var hirarCpy = new Stack<int>(ScopeHirarchy.Reverse());
            for(int i=0; i < ScopeHirarchy.Count; i++)
            {
                int currentScope = hirarCpy.Pop();
                foreach(var funcDataRow in FunctionTable)
                {
                    if (funcDataRow.Name == name && currentScope == funcDataRow.Scope)
                        type = funcDataRow.Type;
                }
            }
            if(type == "")
            {
                foreach(var classes in ClassesTable)
                {
                    if(classes.Name == className)
                    {
                        var result = lookupCT(name, classes.Link);
                        if (result != null)
                            type = result.Type;
                        break;
                    }
                }
            }
            return type;
        }
        public bool insert(string name, string type, string parent)
        {
            bool status = true;
            string result = lookup(name);
            if (result == "")
            {
                
            }
            else
                status = false;
            return status;
        }
        public bool insertCT(string name, string type, string am, ClassData Ref)
        {
            return Ref.addRow(name, type, am);
        }
        public bool insertFT(string name, string type)
        {
            foreach(var funcData in FunctionTable)
            {
                if (funcData.Name == name && funcData.Scope != currentScope)
                    return false;
            }
            FunctionTable.Add(new FunctionTableRow(name, type, currentScope));
            return true;
        }
        public int createScope()
        {
            currentScope = scopeCount;
            scopeCount++;
            ScopeHirarchy.Push(currentScope);
            return currentScope;
        }
        public int destroyScope()
        {
            currentScope = ScopeHirarchy.Pop();
            return currentScope;
        }

    }
}
