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
                { "for", "for" },
                { "while", "while" },
                { "do", "while" },
                { "class", "class" },
                { "if", "if" },
                { "else", "else" },
                { "switch", "switch"},
                { "case", "case" },
                { "break", "break" },
                { "continue", "continue" },
                { "return", "return" },
                { "void", "void" },
                { "main", "main" },
                { "AM", "private" },
                { "AM", "public" },
                { "new", "new" },
                { "this", "this" },
                { "extend", "extend" },
                { "sealed", "sealed" },
                { "base", "base" },
                { "abstract", "abstract"},
                {"$", "$" }
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
            { "inc", "++"},
            {"dec", "--" },
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
            int keywordSize = keywords.Length / 2;
            string classPart = "";
            for (int i = 0; i < keywordSize; i++)
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
            int operatorSize = operators.Length / 2;
            string classPart = "";
            for (int i = 0; i < operatorSize; i++)
            {
                if (operators[i, 1] == word)
                {
                    classPart = operators[i, 0];
                    break;
                }
            }
            return classPart;
        }

        string isPunct(string word)
        {
            int punctSize = puntuators.Length / 2;
            string classPart = "";
            for (int i = 0; i < punctSize; i++)
            {
                if (puntuators[i, 1] == word)
                {
                    classPart = puntuators[i, 0];
                    break;
                }
            }
            return classPart;
        }

        bool isIdentifier(string word)
        {
            bool status = false;
            Regex reg = new Regex("^[A-Za-z_][A-Za-z_0-9]{0,30}$");
            Regex reg1 = new Regex("[A-Za-z0-9]$");
            if (reg1.IsMatch(word))
            {
                status = reg.IsMatch(word);
            }
            return status;
        }

        bool isInt(string word)
        {
            Regex reg = new Regex("^[-+]?[0-9]{1,7}$");
            return reg.IsMatch(word);
        }

        bool isFloat(string word)
        {
            Regex reg = new Regex("^[+-]?[0-9]{0,7}.[0-9]{1,7}$");
            return reg.IsMatch(word);
        }

        bool isString(string word)
        {
            Regex reg2 = new Regex("^[\\\\][\\\\\"ntra]$");
            Regex reg = new Regex("^[^\\\\\"]$");
            bool status = true;
            string temp = "";
            if (word[0] != '\"' || word.Last() != '\"')
            {
                return false;
            }
            Console.WriteLine(word.Last());
            word = word.Substring(1, word.Length - 2);
            Console.WriteLine(word);
            for (int i = 0; i < word.Length; i++)
            {
                if (reg.IsMatch(word[i].ToString()))
                {
                    status = true;
                }
                else
                {
                    try
                    {
                        temp = word[i].ToString() + word[++i].ToString();
                    }
                    catch (IndexOutOfRangeException)
                    {
                        Console.WriteLine("exp say");
                        return false;
                    }
                    if (reg2.IsMatch(temp))
                    {
                        status = true;
                        temp = "";
                    }
                    else
                    {
                        status = false;
                        break;
                    }
                }
            }
            return status;
        }
        bool isChar(string word)
        {
            bool status = false;
            Regex reg = new Regex("^\'[\\\\][\\\\\'\"ntrn]\'$");
            Regex reg2 = new Regex("^\'[^\\\\\'\"]\'$");
            if (reg.IsMatch(word) || reg2.IsMatch(word))
            {
                status = !status;
            }
            return status;
        }

        string getClass(string tokenValue)
        {
            string classPart = "";
            if (isIdentifier(tokenValue))
            {
                classPart = isKeyword(tokenValue);
                if (classPart == "")
                    classPart = "ID";
            }
            else
            {
                classPart = isPunct(tokenValue);
                if (classPart == "")
                {
                    classPart = isOpr(tokenValue);
                    if (classPart == "")
                    {
                        classPart = "invalid_token";
                    }
                }
            }
            return classPart;
        }

        public string test()
        {
            string test = "";
            string temp;
            foreach (var token in StaticComponents.tokenSet)
            {
                if (token.value != "")
                    temp = token.value[0].ToString();
                test += token.value[0].ToString() + " ";
            }
            return test;
        }

        public void classifier()
        {
            string firstWord;
            int index = 0;
            Classifier classify = new Classifier();
            List<int> error = new List<int>();
            foreach (Token token in StaticComponents.tokenSet)
            {
                index++;
                if (token.value == "")
                {
                    error.Add(index - 1);
                    continue;
                }
                if (char.IsDigit(token.value[0]))
                {
                    firstWord = "0";
                }
                else
                {
                    firstWord = token.value[0].ToString();
                }
                switch (firstWord)
                {
                    case "+":
                    case "-":
                    case ".":
                    case "0":
                        if (classify.isInt(token.value))
                            token.classKeyword = "int_const";
                        else if (classify.isFloat(token.value))
                            token.classKeyword = "float_const";
                        else
                            token.classKeyword = getClass(token.value);
                        break;
                    case "\"":
                        if (classify.isString(token.value))
                        {
                            token.value = token.value.Substring(1, token.value.Length - 2);
                            token.classKeyword = "string_const";
                        }
                        else
                        {
                            token.classKeyword = "invalid_token";
                        }
                        break;
                    case "\'":
                        if (classify.isChar(token.value))
                        {
                            token.value = token.value.Trim('\'');
                            token.classKeyword = "char_const";
                        }
                        else
                        {
                            token.classKeyword = "invalid_token";
                        }
                        break;
                    case "_":
                        if (classify.isIdentifier(token.value))
                        {
                            token.classKeyword = "ID";
                        }
                        else
                        {
                            token.classKeyword = "invalid_token";
                        }
                        break;
                    default:
                        token.classKeyword = getClass(token.value);
                        break;
                }
            }
            index = 0;
            foreach (int i in error)
            {
                StaticComponents.tokenSet.RemoveAt(i - index);
                index++;
            }
        }
    }
}
