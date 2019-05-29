using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer
{
    class HelperFunctions
    {
        public List<ClassesTableRow> ClassesTable;
        public List<FunctionTableRow> FunctionTable;
        Stack<int> ScopeHirarchy;
        ClassData currentRef;
        int currentScope;
        int scopeCount;
        string[,] compatible = {
            { "int_const", "int_const", "=", "int_const" },
            { "int", "int_const", "=", "int_const"},
            { "float_const", "int_const", "=", "float_const" },
            { "float_const", "float_const", "=", "float_const" },
            { "char_const", "char_const", "=", "char_const" },
            { "string_const", "string_const", "=", "string_const"},
            { "int_const", "int_const", "+", "int_const" },
            { "float_const", "int_const", "+", "float_const" },
            { "int_const", "float_const", "+", "float_const" },
            { "float_const", "float_const", "+", "float_const" },
            { "string_const", "string_const", "+", "string_const" },
            { "string_const", "float_const", "+", "string_const" },
            { "string_const", "int_const", "+", "string_const" },
            { "string_const", "char_const", "+", "string_const" },
            { "int_const", "string_const", "+", "string_const" },
            { "float_const", "string_const", "+", "string_const" },
            { "char_const", "string_const", "+", "string_const" },
            { "string_const", "string_const", "-", "string_const" },
            { "string_const", "float_const", "-", "string_const" },
            { "string_const", "int_const", "-", "string_const" },
            { "string_const", "char_const", "-", "string_const" },
            { "int_const", "string_const", "-", "string_const" },
            { "float_const", "string_const", "-", "string_const" },
            { "char_const", "string_const", "-", "string_const" },
            { "int_const", "int_const", "-", "int_const" },
            { "float_const", "int_const", "-","float_const" },
            { "int_const", "float_const", "-","float_const" },
            { "float_const", "float_const", "-","float_const" },
            { "int_const", "int_const", "*", "int_const" },
            { "float_const", "int_const", "*", "float_const" },
            { "int_const", "float_const", "*","float_const" },
            { "float_const", "float_const", "*","float_const" },
            { "int_const", "int_const", "/", "float_const" },
            { "float_const", "int_const", "/", "float_const" },
            { "int_const", "float_const", "/","float_const" },
            { "float_const", "float_const", "/","float_const" },

            { "int_const", "int_const", "<", "int_const" },
            { "int_const", "float_const", "<", "int_const" },
            { "float_const", "int_const", "<", "int_const" },
            { "float_const", "float_const", "<", "int_const" },
            { "int_const", "int_const", ">", "int_const" },
            { "int_const", "float_const", ">", "int_const" },
            { "float_const", "int_const", ">", "int_const" },
            { "float_const", "float_const", ">", "int_const" },
            { "int_const", "int_const", "<=", "int_const" },
            { "int_const", "float_const", "<=", "int_const" },
            { "float_const", "int_const", "<=", "int_const" },
            { "float_const", "float_const", "<=", "int_const" },
            { "int_const", "int_const", ">=", "int_const" },
            { "int_const", "float_const", ">=", "int_const" },
            { "float_const", "int_const", ">=", "int_const" },
            { "float_const", "float_const", ">=", "int_const" },
            { "int_const", "int_const", "!=", "int_const" },
            { "int_const", "float_const", "!=", "int_const" },
            { "float_const", "int_const", "!=", "int_const" },
            { "float_const", "float_const", "!=", "int_const" },
            { "int_const", "int_const", "==", "int_const" },
            { "int_const", "float_const", "==", "int_const" },
            { "float_const", "int_const", "==", "int_const" },
            { "float_const", "float_const", "==", "int_const" },
            { "int_const", "int_const", "&&", "int_const" },
            { "int_const", "float_const", "&&", "int_const" },
            { "float_const", "int_const", "&&", "int_const" },
            { "float_const", "float_const", "&&", "int_const" },
            { "int_const", "int_const", "||", "int_const" },
            { "int_const", "float_const", "||", "int_const" },
            { "float_const", "int_const", "||", "int_const" },
            { "float_const", "float_const", "||", "int_const" },

            { "int_const", "int_const", "+=", "int_const" },
            { "float_const", "int_const", "+=", "float_const" },
            { "int_const", "float_const", "+=", "float_const" },
            { "float_const", "float_const", "+=", "float_const" },
            { "string_const", "string_const", "+=", "string_const" },
            { "string_const", "float_const", "+=", "string_const" },
            { "string_const", "int_const", "+=", "string_const" },
            { "string_const", "char_const", "+=", "string_const" },
            { "int_const", "string_const", "+=", "string_const" },
            { "float_const", "string_const", "+=", "string_const" },
            { "char_const", "string_const", "+=", "string_const" },
            { "string_const", "string_const", "-=", "string_const" },
            { "string_const", "float_const", "-=", "string_const" },
            { "string_const", "int_const", "-=", "string_const" },
            { "string_const", "char_const", "-=", "string_const" },
            { "int_const", "string_const", "-=", "string_const" },
            { "float_const", "string_const", "-=", "string_const" },
            { "char_const", "string_const", "-=", "string_const" },
            { "int_const", "int_const", "-=", "int_const" },
            { "float_const", "int_const", "-=","float_const" },
            { "int_const", "float_const", "-=","float_const" },
            { "float_const", "float_const", "-=","float_const" },
            { "int_const", "int_const", "*=", "int_const" },
            { "float_const", "int_const", "*=", "float_const" },
            { "int_const", "float_const", "*=","float_const" },
            { "float_const", "float_const", "*=","float_const" },
            { "int_const", "int_const", "/=", "float_const" },
            { "float_const", "int_const", "/=", "float_const" },
            { "int_const", "float_const", "/=","float_const" },
            { "float_const", "float_const", "/=","float_const" },

        };
        public HelperFunctions()
        {
            ClassesTable = new List<ClassesTableRow>();
            FunctionTable = new List<FunctionTableRow>();
            ScopeHirarchy = new Stack<int>();
            currentScope = -1;
            scopeCount = 0;
            currentRef = new ClassData();
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
        public ClassData insert(string name, string type,  string category, string parent)
        {
            ClassesTableRow status = null;
            string result = lookup(name);
            if (result == "")
            {
                status = new ClassesTableRow(name, type, category, parent);
                ClassesTable.Add(status);
                return status.Link;
            }
            return status.Link;
        }
        public bool insertCT(string name, string type, string am, string tm,ClassData Ref)
        {
            return Ref.addRow(name, type, am, tm);
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
        public string Compatible(string type1, string type2, string opr)
        {
            string type = "";
            int length = compatible.Length / 4;
            for(int i = 0; i < length; i++)
            {
                if (compatible[i, 0] == type1 && compatible[i, 1] == type2 && compatible[i, 2] == opr)
                    type = compatible[i, 3];
            }
            return type;
        }
    }
}
