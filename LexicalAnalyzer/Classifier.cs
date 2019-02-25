using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LexicalAnalyzer
{
    class Classifier
    {
        string[,] keywords = { 
                { "dt", "int" },
                { "dt", "float" },
                { "dt", "string" },
                { "dt", "char" },
                { "constant", "constant" },
                { "loop", "loop" },
                { "if", "if" },
                { "else", "else" },
                { "switch", "switch"},
                { "case", "case" },
                { "break", "break" },
                { "continue", "continue" },
                { "return", "return" },
                { "void", "void" },
                { "main", "main" },
                { "acm", "private" },
                { "acm", "public" },
                { "new", "new" },
                { "this", "this" },
                { "extend", "extend" },
                { "sealed", "sealed" },
                { "base", "base" }
        };
        string[,] operators = {
            { "mdm", "*"},
            { "mdm", "/" },
            { "mdm", "%" },
            { "pm", "+" },
            { "pm", "-" },
            { "ro", "<" },
            { "ro", ">" },
            { "ro", "<=" },
            { "ro", ">=" },
            { "ro", "!=" },
            { "ro", "==" },
            { "and", "&&" },
            { "or", "||" },
            { "not", "!"},
            { "assign", "=" },
            { "compAss", "-="},
            { "compAss", "+="},
            { "compAss", "*="},
            { "compAss", "/="},
            { "compAss", "%="},
        };

        string[,] puntuators =
        {
            { "ter", ";" },
            { "ter", "\n" },
            {",", "," },
            {".", "." },
            {"(", "(" },
            {")", ")" },
            {"{", "{" },
            {"}", "}" },
            {"[", "[" },
            {"]", "]" },
        };

        string isKeyword(string word)
        {
            int keywordSize = keywords.Length;
            string classPart = "";
            for(int i=0; i< keywordSize; i++)
            {
                if (keywords[i, 1] == word)
                {
                    classPart = keywords[i, 0];
                    break;
                }
            }

            return classPart;
        }

        string isOpr(string word)
        {
            int operatorSize = operators.Length;
            string classPart = "";
            for(int i=0; i < operatorSize; i++)
            {
                if(operators[i,1] == word)
                {
                    classPart = operators[i, 0];
                    break;
                }
            }
            return classPart;
        }

        string isPunct(string word)
        {
            int punctSize = puntuators.Length;
            string classPart = "";
            for(int i=0; i<punctSize; i++)
            {
                if(operators[i,1] == word)
                {
                    classPart = operators[i, 0];
                    break;
                }
            }
            return classPart;
        }

        bool isIdentifier(string word)
        {
            Regex reg = new Regex("^[A-Za-z_]{1,1}[A-Za-z_0-9]{0,30}[A-Za-z0-9]{1,1}$");
            return reg.IsMatch(word);
        }

        bool isInt(string word)
        {
            Regex reg = new Regex("^[0-9]{1,7}$");
            return reg.IsMatch(word);
        }

        bool isFloat(string word)
        {
            Regex reg = new Regex("^[+-]{0,1}[0-9]{0,7}.[0-9]{1,7}$");
            return reg.IsMatch(word);
        }

        bool isChar(string word)
        {
            bool status = false;
            Regex reg = new Regex("^[\\\\][\\\\\'\"ntrn]$");
            Regex reg2 = new Regex("^[^\\\\\'\"]$");
            if (reg.IsMatch(word) || reg2.IsMatch(word))
            {
                status = !status;
            }
            return status;
        }

        void classifier()
        {
            foreach(Token token in StaticComponents.tokenSet)
            {
                char firstWord = token.value[0];
                
            }
        }
    }
}
