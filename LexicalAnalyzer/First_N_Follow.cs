using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer
{
    static class First_N_Follow
    {
        static string[] FirstClassStr = new string[] { "abstract", "sealed", "class" };
        static string[] FollowClassStr = new string[] { "$" };
        static string[] FirstSealed = new string[] { "sealed", "null" };
        static string[] FollowSealed = new string[] { "class" };
        static string[] FirstExtend = new string[] { "extend", "null" };
        static string[] FollowExtend = new string[] { "{" };
        static string[] FirstClass_Body = new string[] { "{" };
        static string[] FollowClass_body = new string[] { "abstract, extend, sealed, class" };
        static string[] FirstClass_Body1 = new string[] { "public", "private", "DT", "ID", "abstract" };
        static string[] FollowClass_body1 = new string[] { "public", "private", "DT", "ID", "abstract" };
        static string[] FirstClass_Re = new string[] { "abstract", "sealed", "class", "null" };
        static string[] FollowClass_Re = new string[] { "$" };
        static string[] FirstDecl_Init = new string[] { "public", "private", "DT", "ID", "abstract" };
        static string[] FollowDecl_Init = new string[] { "public", "private", "DT", "ID", "abstract" };
        static string[] FirstDecl = new string[] { "DT", "ID", "abstract" };
        static string[] FollowDecl = new string[] { "public", "private", "DT", "ID", "abstract" };
        static string[] FirstAss = new string[] { "=" };
        static string[] FollowAss = new string[] { "(", "ID", "new" };
        static string[] FirstDec3 = new string[] { "ID" };
        static string[] FollowDec3 = new string[] { "public", "private", "DT", "ID", "abstract" };
        static string[] FirstDec4 = new string[] { "ID", "{" };
        static string[] FollowDec4 = new string[] { "public", "private", "DT", "ID", "abstract" };
        static string[] FirstDec2 = new string[] { "ID", "new" };
        static string[] FollowDec2 = new string[] { "ter" };
        static string[] FirstDec5 = new string[] { "[" };
        static string[] FollowDec5 = new string[] { "public", "private", "DT", "ID", "abstract" };
        static string[] FirstParams = new string[] { "||", "&&","PM","RO","MDM","ROP", "ID", "int_const", "float_const", "string_const", "char_const", "(", "!", "inc","dec", "null" };
        static string[] FollowParams = new string[] { ")", "]" };
        static string[] FirstCommas = new string[] { "," };
        static string[] FollowCommas = new string[] { ")","]" };
        static string[] FirstArrayInit = new string[] { "ID" };
        static string[] FollowArrayInit = new string[] { "" };
        static string[] FirstInit = new string[] { "int_const", "float_const", "string_const", "char_const", "null" };
        static string[] FollowInit = new string[] { "ter", "," };
        static string[] FirstList = new string[] { "ter", "," };
        static string[] FollowList = new string[] { "" };
        static string[] FirstFunction_body = new string[] { "ter" };
        static string[] FollowFunction_body = new string[] { "" };
        static string[] FirstR1 = new string[] { "||", "&&", "PM", "RO", "MDM", "ROP", "ID", "int_const", "float_const", "string_const", "char_const", "(", "!", "inc", "dec", "null" };
        static string[] FollowR1 = new string[] { "}" };
        static string[] FirstConst = new string[] { "int_const", "float_const", "string_const", "char_const", "null" };
        static string[] FirstInt_const = new string[] { "int_const" };
        static string[] Firstfloat_const = new string[] { "float_const" };
        static string[] FirstString_const = new string[] { "string_const" };
        static string[] FirstChar_const = new string[] { "char_const" };
        static string[] FirstAM = new string[] { "public", "private", "null" };
        static string[] FollowAM = new string[] { "DT","ID", "abstract" };
        static string[] FirstBody = new string[] { "ter", "for", "while","do","if", "switch", "public", "private", "DT", "ID", "abstract", "return", "break", "continue", "ID", "{"  };
        static string[] FirstReturn_stmt = new string[] { "return" };
        static string[] FollowReturn_stmt = new string[] { "||", "&&", "PM", "RO", "MDM", "ROP", "ID", "int_const", "float_const", "string_const", "char_const", "(", "!", "inc", "dec" };
        static string[] FirstAssign = new string[] { "ID" };
        static string[] FirstINC_DEC = new string[] { "inc", "dec" };
        static string[] FirstExp = new string[] { "||", "&&", "PM", "RO", "MDM", "ROP", "ID", "int_const", "float_const", "string_const", "char_const", "(", "!", "inc", "dec" };
        static string[] FirstIf_else_stmt = new string[] { "if" };
        static string[] FirstSwitch_stmt = new string[] { "switch" };
        static string[] FirstSST = new string[] { "for", "while", "do", "if", "switch", "public", "private", "DT", "ID", "abstract", "return", "break", "continue", "ID" };
        static string[] FirstMST = new string[] { "for", "while", "do", "if", "switch", "public", "private", "DT", "ID", "abstract", "return", "break", "continue", "ID" };
    }
}
