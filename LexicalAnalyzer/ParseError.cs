using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer
{
    class ParseError
    {
        public int lineNumber;
        public string ClassKeyword;
        public int word;
        public ParseError(int ln, string ck, int w)
        {
            lineNumber = ln;
            ClassKeyword = ck;
            word = w;                
        }
        public override string ToString()
        {
            return string.Format("line: {0}, class: {1}, word: {2}", lineNumber, ClassKeyword, word);
        }
    }
}
