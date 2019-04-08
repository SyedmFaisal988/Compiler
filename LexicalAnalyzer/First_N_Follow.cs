﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer
{
    public static class First_N_Follow
    {
        public static string[] FirstClassStr = new string[] { "abstract", "sealed", "class" };
        public static string[] FollowClassStr = new string[] { "$" };
        public static string[] FirstAbstract = new string[] { "abstract" };
        public static string[] FollowAbstract = new string[] { "sealed", "class" };
        public static string[] FirstSealed = new string[] { "sealed", "null" };
        public static string[] FollowSealed = new string[] { "class" };
        public static string[] FirstExtend = new string[] { "extend", "null" };
        public static string[] FollowExtend = new string[] { "{" };
        public static string[] FirstClass_Body = new string[] { "{", "ter" };
        public static string[] FollowClass_body = new string[] { "abstract, extend, sealed, class" };
        public static string[] FirstClass_Body1 = new string[] { "public", "private", "DT", "ID", "abstract" };
        public static string[] FollowClass_body1 = new string[] {"{" };
        public static string[] FirstClass_Re = new string[] { "abstract", "sealed", "class", "null" };
        public static string[] FollowClass_Re = new string[] { "$" };
        public static string[] FirstDecl_Init = new string[] { "public", "private", "DT", "ID", "abstract" };
        public static string[] FollowDecl_Init = new string[] { "public", "private", "DT", "ID", "abstract" };
        public static string[] FirstDecl = new string[] { "DT", "ID", "abstract" };
        public static string[] FollowDecl = new string[] { "public", "private", "DT", "ID", "abstract" };
        public static string[] FirstAss = new string[] { "=" };
        public static string[] FollowAss = new string[] { "(", "ID", "new" };
        public static string[] FirstDec3 = new string[] { "ID" };
        public static string[] FollowDec3 = new string[] { "public", "private", "DT", "ID", "abstract" };
        public static string[] FirstDec4 = new string[] { "ID", "{" };
        public static string[] FollowDec4 = new string[] { "public", "private", "DT", "ID", "abstract" };
        public static string[] FirstDec2 = new string[] { "ID", "new" };
        public static string[] FollowDec2 = new string[] { "ter" };
        public static string[] FirstDec5 = new string[] { "[" };
        public static string[] FollowDec5 = new string[] { "public", "private", "DT", "ID", "abstract" };
        public static string[] FirstParams = new string[] { "||", "&&","PM","RO","MDM","ROP", "ID", "int_const", "float_const", "string_const", "char_const", "(", "!", "inc","dec", "null" };
        public static string[] FollowParams = new string[] { ")", "]" };
        public static string[] FirstCommas = new string[] { "," };
        public static string[] FollowCommas = new string[] { ")","]" };
        public static string[] FirstArrayInit = new string[] { "ID" };
        public static string[] FollowArrayInit = new string[] { "" };
        public static string[] FirstInit = new string[] { "int_const", "float_const", "string_const", "char_const", "null" };
        public static string[] FollowInit = new string[] { "ter", "," };
        public static string[] FirstList = new string[] { "ter", "," };
        public static string[] FollowList = new string[] { "" };
        public static string[] FirstFunction_body = new string[] { "ter" };
        public static string[] FollowFunction_body = new string[] { "" };
        public static string[] FirstR1 = new string[] { "||", "&&", "PM", "RO", "MDM", "ROP", "ID", "int_const", "float_const", "string_const", "char_const", "(", "!", "inc", "dec", "null" };
        public static string[] FollowR1 = new string[] { "}" };
        public static string[] FirstConst = new string[] { "int_const", "float_const", "string_const", "char_const", "null" };
        public static string[] FirstInt_const = new string[] { "int_const" };
        public static string[] Firstfloat_const = new string[] { "float_const" };
        public static string[] FirstString_const = new string[] { "string_const" };
        public static string[] FirstChar_const = new string[] { "char_const" };
        public static string[] FirstAM = new string[] { "public", "private", "null" };
        public static string[] FollowAM = new string[] { "DT","ID", "abstract" };
        public static string[] FirstBody = new string[] { "ter", "for", "while","do","if", "switch", "public", "private", "DT", "ID", "abstract", "return", "break", "continue", "ID", "{"  };
        public static string[] FirstReturn_stmt = new string[] { "return" };
        public static string[] FollowReturn_stmt = new string[] { "||", "&&", "PM", "RO", "MDM", "ROP", "ID", "int_const", "float_const", "string_const", "char_const", "(", "!", "inc", "dec" };
        public static string[] FirstAssign = new string[] { "ID" };
        public static string[] FirstINC_DEC = new string[] { "inc", "dec" };
        public static string[] FirstExp = new string[] { "||", "&&", "PM", "RO", "MDM", "ROP", "ID", "int_const", "float_const", "string_const", "char_const", "(", "!", "inc", "dec" };
        public static string[] FirstIf_else_stmt = new string[] { "if" };
        public static string[] FirstSwitch_stmt = new string[] { "switch" };
        public static string[] FirstSST = new string[] { "for", "while", "do", "if", "switch", "public", "private", "DT", "ID", "abstract", "return", "break", "continue", "ID" };
        public static string[] FirstMST = new string[] { "for", "while", "do", "if", "switch", "public", "private", "DT", "ID", "abstract", "return", "break", "continue", "ID" };
    }
}