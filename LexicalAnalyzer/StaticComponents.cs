﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer
{
    public static class StaticComponents
    {
        public static List<Token> tokenSet = new List<Token>();
        public static List<string> compoundOperators = new List<string>(new string[] { "<=", ">=", "!=", "==", "&&", "||", "-=", "+=", "*=", "/=", "%=", "++", "--" });
    }
}
