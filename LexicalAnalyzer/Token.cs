using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer
{
    public class Token
    {
        public string classKeyword { get; set; }
        public string value { get; set; }
        public int index { get; set; }
        public int wordNumber { get; set; }
        public int lineNumber { get; set; }

        public Token(string _classKeyword, string _value, int _index, int _wordNumber, int _lineNumber)
        {
            classKeyword = _classKeyword;
            value = _value;
            index = _index;
            wordNumber = _wordNumber;
            lineNumber = _lineNumber;
        }

        public override string ToString()
        {
            return String.Format("Class Keyword: {0} Value: {1} Index: {2} Word Number: {3} Line Number: {4}", classKeyword, value, index, wordNumber, lineNumber);
        }
    }
}
