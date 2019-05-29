using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer
{
    class ParseTree
    {
        string t2, opr = "";
        string ClassName;
        private List<Token> tokenSet;
        private List<ParseError> errorLine;
        public HelperFunctions helpers;
        public List<string> SemanticErrors;
        ClassData Ref;
        public ParseTree()
        {
            tokenSet = new List<Token>(StaticComponents.tokenSet.Count);
            errorLine = new List<ParseError>();
            helpers = new HelperFunctions();
            SemanticErrors = new List<string>();
            Ref = null;
            StaticComponents.tokenSet.ForEach((item) =>
            {
                tokenSet.Add(item);
            });
        }
        public List<ParseError> Parse()
        {
            S();
            return errorLine;
        }
        bool S()
        {
            bool status = true;
            if (First_N_Follow.FirstClassStr.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                status = Class_str();
                if (status)
                {
                    if (tokenSet.ElementAt(0).classKeyword == "$")
                    {
                        tokenSet.RemoveAt(0);
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        bool Class_str()
        {
            string category = "general";
            string parent = "";
            string type = "";
            string name = "";

            bool status = true;
            if (First_N_Follow.FirstAbstract.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowAbstract.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                status = Abstract_Class(ref category);
                if (First_N_Follow.FirstSealed.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowSealed.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Sealed(ref category);
                    if (tokenSet.ElementAt(0).classKeyword == "class")
                    {
                        type = tokenSet.ElementAt(0).classKeyword;
                        tokenSet.RemoveAt(0);
                        if (tokenSet.ElementAt(0).classKeyword == "ID")
                        {
                            name = tokenSet.ElementAt(0).value;
                            ClassName = name;
                            tokenSet.RemoveAt(0);
                            status = Extend(ref parent);
                            Ref = helpers.insert(name, type, category, parent);   //ye add kiya ha
                            if(Ref == null)
                            {
                                SemanticErrors.Add("Redeclaration Error At " + tokenSet.ElementAt(0).lineNumber);
                            }
                            else if (First_N_Follow.FirstClass_Body.Contains(tokenSet.ElementAt(0).classKeyword))
                            {
                                status = Class_Body();
                                if (status)
                                {
                                    if (First_N_Follow.FirstClass_Re.Contains(tokenSet.ElementAt(0).classKeyword))
                                    {
                                        status = Class_Re();
                                        if (!status)
                                            errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                    }
                                }
                                else
                                {
                                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                }
                            }
                            else
                            {
                                status = false;
                                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                            }
                        }
                        else
                        {
                            errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                            status = false;
                        }
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        bool Class_Re()
        {
            bool status = true;
            if (First_N_Follow.FirstClass_Re.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                status = Class_str();
                if (!status)
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
            }
            return status;
        }
        bool AM(ref string am)
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "AM")
            {
                am = tokenSet.ElementAt(0).value;
                tokenSet.RemoveAt(0);
            }
            return status;
        }
        bool Init2(string t1)
        {
            bool status = true;
            if (First_N_Follow.FirstConst.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                string temp = helpers.Compatible(t1, tokenSet.ElementAt(0).classKeyword, "=");
                if (temp == "")
                {
                    SemanticErrors.Add("Type Mismatch in assigning At " + tokenSet.ElementAt(0).lineNumber);
                    status = false;
                }
                tokenSet.RemoveAt(0);
            }
            else if (tokenSet.ElementAt(0).classKeyword == "ID")
            {
                string t2 = helpers.lookup(tokenSet.ElementAt(0).value);
                if (t2 == "")
                {
                    SemanticErrors.Add("Use of undeclared variable" + tokenSet.ElementAt(0).lineNumber);
                    status = false;
                }
                t1 = helpers.Compatible(t1, t2, "=");
                if (t1 == "")
                {
                    SemanticErrors.Add("Type Mismatch in assigning At " + tokenSet.ElementAt(0).lineNumber);
                    status = false;
                }
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstInit.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowInit.Contains(tokenSet.ElementAt(0).classKeyword) && status)
                {
                    status = Init(t1);
                    if (!status)
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            return status;
        }
        bool Init(string t1)
        {
            bool status = true;
            //if (First_N_Follow.FirstInit2.Contains(tokenSet.ElementAt(0).classKeyword))  init main kharabi tu isay dekho
            //{
            //    status = Init2();
            //}
            //else 
            if (tokenSet.ElementAt(0).classKeyword=="assign")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstInit2.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Init2(t1);
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            return status;
        }
        bool List(string am, string type, string name)
        {
            bool status = true;
            string tm = "";
            if (tokenSet.ElementAt(0).classKeyword == "ter")
            {
                if (name != "" && !helpers.insertCT(name, type, am, tm, Ref))
                {
                    SemanticErrors.Add("Redeclaration Error at " + tokenSet.ElementAt(0).lineNumber);
                    status = false;
                }
                tokenSet.RemoveAt(0);
            }
            else if (tokenSet.ElementAt(0).classKeyword == ",")
            {
                tokenSet.RemoveAt(0);
                if(tokenSet.ElementAt(0).classKeyword == "ID")
                {
                    name = tokenSet.ElementAt(0).value;
                    tokenSet.RemoveAt(0);
                    if (!helpers.insertCT(name, type, am, tm, Ref))
                    {
                        SemanticErrors.Add("Redeclaration Error at " + tokenSet.ElementAt(0).lineNumber);
                        status = false;
                    }
                    if (First_N_Follow.FirstInit.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowInit.Contains(tokenSet.ElementAt(0).classKeyword))
                    {
                        status = Init(type);
                        if (First_N_Follow.FirstList.Contains(tokenSet.ElementAt(0).classKeyword) && status)
                        {
                            status = List(am, type, "");
                        }
                        else
                        {
                            errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                            status = false;
                        }
                    }
                }
                //}
                //else
                //{
                //    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                //    status = false;
                //}
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        bool Const()
        {
            bool status = true;
            if (First_N_Follow.FirstConst.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                tokenSet.RemoveAt(0);
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        bool Loop_Str()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "for")
            {
                status = ForLoop();
            }
            else if (tokenSet.ElementAt(0).classKeyword == "while")
            {
                status = WhileLoop();
            }
            else if (tokenSet.ElementAt(0).classKeyword == "do")
            {
                status = DoWhileLoop();
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            if (!status)
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        bool Cond3_1()
        {
            string t2 = ""; //solve karna ha
            bool status = true;
            if (First_N_Follow.FirstCond3_1.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Exp(ref t2);
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            return status;
        }
        bool Cond3()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "ID")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstCond3_1.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowCond3_1.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Cond3_1();
                }
                else if (tokenSet.ElementAt(0).classKeyword == "inc" || tokenSet.ElementAt(0).classKeyword == "dec")
                {
                    tokenSet.RemoveAt(0);
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else if (tokenSet.ElementAt(0).classKeyword == "inc" || tokenSet.ElementAt(0).classKeyword == "dec")
            {
                tokenSet.RemoveAt(0);
                if (tokenSet.ElementAt(0).classKeyword == "ID")
                {
                    tokenSet.RemoveAt(0);
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        bool Switch2()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "case")
            {
                status = Case();
                if ((tokenSet.ElementAt(0).classKeyword == "case" || tokenSet.ElementAt(0).classKeyword == "}") && status)
                {
                    status = Switch2();
                }
            }
            return status;
        }
        bool Default()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "default")
            {
                tokenSet.RemoveAt(0);
                if (tokenSet.ElementAt(0).classKeyword == ":")
                {
                    tokenSet.RemoveAt(0);
                    if (First_N_Follow.FirstMST.Contains(tokenSet.ElementAt(0).classKeyword))
                    {
                        status = MST();
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        bool Case()
        {
            bool status = true;
            string t2 = ""; //solve karna ha
            if (tokenSet.ElementAt(0).classKeyword == "case")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Exp(ref t2);
                    if (tokenSet.ElementAt(0).classKeyword == ":")
                    {
                        tokenSet.RemoveAt(0);
                        if (First_N_Follow.FirstMST.Contains(tokenSet.ElementAt(0).classKeyword) && status)
                        {
                            status = MST();
                        }
                        else
                        {
                            errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                            status = false;
                        }
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        bool SwitchBody()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "case")
            {
                status = Case();
                if ((First_N_Follow.FirstSwitchBody.Contains(tokenSet.ElementAt(0).classKeyword) || tokenSet.ElementAt(0).classKeyword == "}") && status)
                {
                    status = SwitchBody();
                }
            }
            else if (tokenSet.ElementAt(0).classKeyword == "default")
            {
                status = Default();
                if (tokenSet.ElementAt(0).classKeyword == "case" || tokenSet.ElementAt(0).classKeyword == "}")
                {
                    status = Switch2();
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            return status;
        }
        bool Switch()
        {
            bool status = true;
            string t2 = ""; //solve karna ha
            if (tokenSet.ElementAt(0).classKeyword == "switch")
            {
                tokenSet.RemoveAt(0);
                if (tokenSet.ElementAt(0).classKeyword == "(")
                {
                    tokenSet.RemoveAt(0);
                    if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
                    {
                        status = Exp(ref t2);
                        if (tokenSet.ElementAt(0).classKeyword == ")" && status)
                        {
                            tokenSet.RemoveAt(0);
                            if (tokenSet.ElementAt(0).classKeyword == "{")
                            {
                                tokenSet.RemoveAt(0);
                                if (First_N_Follow.FirstSwitchBody.Contains(tokenSet.ElementAt(0).classKeyword) || tokenSet.ElementAt(0).classKeyword == "}")
                                {
                                    status = SwitchBody();
                                    if (tokenSet.ElementAt(0).classKeyword == "}" && status)
                                    {
                                        tokenSet.RemoveAt(0);
                                    }
                                    else
                                    {
                                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                        status = false;
                                    }
                                }
                                else
                                {
                                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                    status = false;
                                }
                            }
                            else
                            {
                                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                status = false;
                            }
                        }
                        else
                        {
                            errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                            status = false;
                        }
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        bool AssignList(string name, ref string type)
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == ".")
            {
                tokenSet.RemoveAt(0);
                if (tokenSet.ElementAt(0).classKeyword == "ID")
                {
                    string name2 = tokenSet.ElementAt(0).value;
                    type = helpers.lookupFT(name2, name);
                    if(type == "")
                    {
                        SemanticErrors.Add("Use of Undeclared Var At " + tokenSet.ElementAt(0).lineNumber);
                        return false;
                    }
                    tokenSet.RemoveAt(0);
                    if (tokenSet.ElementAt(0).classKeyword == "." || First_N_Follow.FollowAssignList.Contains(tokenSet.ElementAt(0).classKeyword))
                    {
                        status = AssignList(name2, ref type);
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            return status;
        }
        bool AssignCall2(ref string type, string name)
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "(")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstParams.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowParams.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    string paralist = "";
                    status = Params(ref paralist);
                    string[] splitString = type.Split('-');
                    if(splitString[1] != paralist)
                    {
                        SemanticErrors.Add("Use of undeclared Function At " + tokenSet.ElementAt(0).lineNumber);
                        return false;
                    }
                    if (tokenSet.ElementAt(0).classKeyword == ")" && status)
                    {
                        tokenSet.RemoveAt(0);
                        if (tokenSet.ElementAt(0).classKeyword == "ter")
                        {
                            tokenSet.RemoveAt(0);
                        }
                        else
                        {
                            errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                            status = false;
                        }
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else if (tokenSet.ElementAt(0).classKeyword == "assign" || tokenSet.ElementAt(0).classKeyword == "compAss")
            {
                string opr = tokenSet.ElementAt(0).value;
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstCall.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Call(type,name, opr);
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        //bool Call1()
        //{
        //    bool status = true;
        //    if (tokenSet.ElementAt(0).classKeyword == "(")
        //    {
        //        tokenSet.RemoveAt(0);
        //        if (First_N_Follow.FirstParams.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowParams.Contains(tokenSet.ElementAt(0).classKeyword))
        //        {
        //            status = Params();
        //            if (tokenSet.ElementAt(0).classKeyword == ")" && status)
        //            {
        //                tokenSet.RemoveAt(0);
        //                if (tokenSet.ElementAt(0).classKeyword == "ter")
        //                {
        //                    tokenSet.RemoveAt(0);
        //                }
        //                else
        //                {
        //                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
        //                    status = false;
        //                }
        //            }
        //            else
        //            {
        //                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
        //                status = false;
        //            }
        //        }
        //        else if (First_N_Follow.FirstInitList.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
        //        {
        //            status = InitList();
        //            if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword) && status)
        //            {
        //                status = Exp();
        //                if (tokenSet.ElementAt(0).classKeyword == "ter" && status)
        //                {
        //                    tokenSet.RemoveAt(0);
        //                }
        //                else
        //                {
        //                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
        //                    status = false;
        //                }
        //            }
        //            else
        //            {
        //                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
        //                status = false;
        //            }
        //        }
        //        else
        //        {
        //            errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
        //            status = false;
        //        }
        //    }
        //    return status;
        //}
        bool InitList()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "assign")
            {
                tokenSet.RemoveAt(0);
                if (tokenSet.ElementAt(0).classKeyword == "ID")
                {
                    tokenSet.RemoveAt(0);
                    if (First_N_Follow.FirstInitList.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
                    {
                        status = InitList();
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            return status;
        }
        bool Call(string t1, string name, string opr)
        {
            bool status = true;
            string t2 = "";
            if (tokenSet.ElementAt(0).classKeyword == "ID")
            {
                if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Exp(ref t2);
                    if (helpers.Compatible(t1, t2, opr)=="")
                    {
                        SemanticErrors.Add("Type Mismatch At " + tokenSet.ElementAt(0).lineNumber);
                        return false;
                    }
                    if (tokenSet.ElementAt(0).classKeyword == "ter")
                    {
                        tokenSet.RemoveAt(0);
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else if (First_N_Follow.FirstConst.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                if(helpers.Compatible(t1, tokenSet.ElementAt(0).classKeyword ,opr) == "")
                {
                    SemanticErrors.Add("Type Mismatch At " + tokenSet.ElementAt(0).lineNumber);
                    return false;
                }
                tokenSet.RemoveAt(0);
                if (tokenSet.ElementAt(0).classKeyword == "ter")
                {
                    tokenSet.RemoveAt(0);
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return true;
        }
        bool AssignCall()
        {
            bool status = true;
            string name = "";
            string type = "";
            if (tokenSet.ElementAt(0).classKeyword == "ID")
            {
                name = tokenSet.ElementAt(0).value;
                type = helpers.lookupFT(name, ClassName);
                tokenSet.RemoveAt(0);
                if (tokenSet.ElementAt(0).classKeyword == "." || First_N_Follow.FollowAssignList.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    if (type == "")
                    {
                        SemanticErrors.Add("Use of Undeclared Var At " + tokenSet.ElementAt(0).lineNumber);
                        return false;
                    }
                    status = AssignList(name, ref type);
                    if (First_N_Follow.FollowAssignList.Contains(tokenSet.ElementAt(0).classKeyword))
                    {
                        status = AssignCall2(ref type, name);
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else if (First_N_Follow.FollowArrayOpt.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FirstArrayOpt.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    type = name;
                    status = Array_opt(ref type);
                    if (tokenSet.ElementAt(0).classKeyword == "ID")
                    {
                        name = tokenSet.ElementAt(0).value;
                        if(!helpers.insertFT(name, type))
                        {
                            SemanticErrors.Add("Redeclaration Error At " + tokenSet.ElementAt(0).lineNumber);
                            return false;
                        }
                        tokenSet.RemoveAt(0);
                        if (tokenSet.ElementAt(0).classKeyword == "assign")
                        {
                            opr = tokenSet.ElementAt(0).value;
                            tokenSet.RemoveAt(0);
                            if (tokenSet.ElementAt(0).classKeyword == "new")
                            {
                                tokenSet.RemoveAt(0);
                                if (tokenSet.ElementAt(0).classKeyword == "ID")
                                {
                                    string t2 = tokenSet.ElementAt(0).value; 
                                    if(type != t2)
                                    {
                                        SemanticErrors.Add("Type Mismatch Error At " + tokenSet.ElementAt(0).lineNumber);
                                        return false;
                                    }
                                    tokenSet.RemoveAt(0);
                                    if (tokenSet.ElementAt(0).classKeyword == "(")
                                    {
                                        tokenSet.RemoveAt(0);
                                        if (First_N_Follow.FirstParams.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowParams.Contains(tokenSet.ElementAt(0).classKeyword))
                                        {
                                            string paralist = "";
                                            status = Params(ref paralist);
                                            type += "-" + paralist;
                                            if (tokenSet.ElementAt(0).classKeyword == ")")
                                            {
                                               if (helpers.lookupCT(t2, Ref) == null)
                                                {
                                                    SemanticErrors.Add("Use of Undeclared Variable At " + tokenSet.ElementAt(0).lineNumber);
                                                    return false;
                                                }
                                                tokenSet.RemoveAt(0);
                                                if (tokenSet.ElementAt(0).classKeyword == "ter")
                                                {
                                                    tokenSet.RemoveAt(0);
                                                }
                                                else
                                                {
                                                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                                    status = false;
                                                }
                                            }
                                            else
                                            {
                                                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                                status = false;
                                            }
                                        }
                                        else
                                        {
                                            errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                            status = false;
                                        }
                                    }
                                }
                                else
                                {
                                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                    status = false;
                                }
                            }
                            else if (tokenSet.ElementAt(0).classKeyword == "(")
                            {
                                if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
                                {
                                    string t2 = "";
                                    status = Exp(ref t2);
                                    if(type != t2)
                                    {
                                        SemanticErrors.Add("Type Mismatch Error At " + tokenSet.ElementAt(0).lineNumber);
                                        return false;
                                    }
                                    if (tokenSet.ElementAt(0).classKeyword == "ter")
                                    {
                                        tokenSet.RemoveAt(0);
                                    }
                                    else
                                    {
                                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                        status = false;
                                    }
                                }
                                else
                                {
                                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                    status = false;
                                }
                            }
                            else if (tokenSet.ElementAt(0).classKeyword == "ID")  //ye add kiya ha function call
                            {
                                if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
                                {
                                    string t2 = "";
                                    status = Exp(ref t2);
                                    if(type != t2)
                                    {
                                        SemanticErrors.Add("Type Mismatch Error At " + tokenSet.ElementAt(0).lineNumber);
                                        return false;
                                    }
                                    if (tokenSet.ElementAt(0).classKeyword == "ter" || status)
                                    {
                                        tokenSet.RemoveAt(0);
                                    }
                                    else
                                    {
                                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                        status = false;
                                    }
                                }
                            }
                            else
                            {
                                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                status = false;
                            }
                        }
                        else if (tokenSet.ElementAt(0).classKeyword == "ter")
                        {
                            tokenSet.RemoveAt(0);
                        }
                        else
                        {
                            errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                            status = false;
                        }
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else if (tokenSet.ElementAt(0).classKeyword == "DT")
            {
                type = tokenSet.ElementAt(0).value;
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstArrayOpt.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowArrayOpt.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Array_opt(ref type);
                    if (tokenSet.ElementAt(0).classKeyword == "ID")
                    {
                        name = tokenSet.ElementAt(0).value;
                        tokenSet.RemoveAt(0);
                        if(!helpers.insertFT(name, type))
                        {
                            SemanticErrors.Add("Redeclaration Error At " + tokenSet.ElementAt(0).lineNumber);
                            return false;
                        }
                        if (tokenSet.ElementAt(0).classKeyword == "assign")
                        {
                            opr = tokenSet.ElementAt(0).value;
                            tokenSet.RemoveAt(0);
                            if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
                            {
                                string t2 = "";
                                status = Exp(ref t2);
                                type += "_const";
                                if(type != t2)
                                {
                                    SemanticErrors.Add("Type Mismatch Error At " + tokenSet.ElementAt(0).lineNumber);
                                    return false;
                                }
                                if (tokenSet.ElementAt(0).classKeyword == "ter" && status)
                                {
                                    tokenSet.RemoveAt(0);
                                }
                                else
                                {
                                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                    status = false;
                                }
                            }
                            else if (tokenSet.ElementAt(0).classKeyword == "new")
                            {
                                tokenSet.RemoveAt(0);
                                if (tokenSet.ElementAt(0).classKeyword == "DT")
                                {
                                    string t2 = tokenSet.ElementAt(0).value;
                                    if(type != t2)
                                    {
                                        SemanticErrors.Add("Mismatch Error At " + tokenSet.ElementAt(0).lineNumber);
                                        return false;
                                    }
                                    string paralist = "";
                                    tokenSet.RemoveAt(0);
                                    if (tokenSet.ElementAt(0).classKeyword == "(")
                                    {
                                        tokenSet.RemoveAt(0);
                                        if (First_N_Follow.FirstParams.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowParams.Contains(tokenSet.ElementAt(0).classKeyword))
                                        {
                                            status = Params(ref paralist);
                                            paralist = "ctor-" + paralist;
                                            var temp = helpers.lookupCT(t2, Ref);
                                            if (temp == null)
                                            {
                                                SemanticErrors.Add("Undeclared Constructor Error At " + tokenSet.ElementAt(0).lineNumber);
                                                return false;
                                            }
                                            else if(temp.Type != paralist)
                                            {
                                                SemanticErrors.Add("Type Mismatch Error At " + tokenSet.ElementAt(0).lineNumber);
                                                return false;
                                            }
                                            if (tokenSet.ElementAt(0).classKeyword == ")")
                                            {
                                                tokenSet.RemoveAt(0);
                                                if (tokenSet.ElementAt(0).classKeyword == "ter")
                                                {
                                                    tokenSet.RemoveAt(0);
                                                }
                                                else
                                                {
                                                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                                    status = false;
                                                }
                                            }
                                            else
                                            {
                                                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                                status = false;
                                            }
                                        }
                                        else
                                        {
                                            errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                            status = false;
                                        }
                                    }
                                }
                                else
                                {
                                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                    status = false;
                                }
                            }
                            else
                            {
                                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                status = false;
                            }
                        }
                        else if (tokenSet.ElementAt(0).classKeyword == "ter")
                        {
                            tokenSet.RemoveAt(0);
                        }
                        else
                        {
                            errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                            status = false;
                        }
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        bool List3()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "else")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstBody.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Body();
                }
                else if (tokenSet.ElementAt(0).classKeyword == "if")
                {
                    status = If_Else();
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            return status;
        }
        bool If_Else()
        {
            bool status = true;
            string t2 = "";
            if (tokenSet.ElementAt(0).classKeyword == "if")
            {
                tokenSet.RemoveAt(0);
                if (tokenSet.ElementAt(0).classKeyword == "(")
                {
                    tokenSet.RemoveAt(0);
                    if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
                    {
                        status = Exp(ref t2);
                        if (t2 != "int_const")
                        {
                            SemanticErrors.Add("Type Mismatch Error At " + tokenSet.ElementAt(0).lineNumber);
                            return false;
                        }
                        if (tokenSet.ElementAt(0).classKeyword == ")" && status)
                        {
                            tokenSet.RemoveAt(0);
                            if (First_N_Follow.FirstBody.Contains(tokenSet.ElementAt(0).classKeyword))
                            {
                                status = Body();
                                if ((tokenSet.ElementAt(0).classKeyword == "else" || First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword) && status))
                                {
                                    status = List3();
                                }
                                else
                                {
                                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                    status = false;
                                }
                            }
                            else
                            {
                                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                status = false;
                            }
                        }
                        else
                        {
                            errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                            status = false;
                        }
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        bool Body()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "ter")
            {
                tokenSet.RemoveAt(0);
            }
            else if (First_N_Follow.FirstSST.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                status = SST();
            }
            else if (tokenSet.ElementAt(0).classKeyword == "{")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstMST.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = MST();
                    if (tokenSet.ElementAt(0).classKeyword == "}" && status)
                    {
                        tokenSet.RemoveAt(0);
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        bool DoWhileLoop()
        {
            bool status = true;
            string t2 = "";
            if (tokenSet.ElementAt(0).classKeyword == "do")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstBody.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Body();
                    if (tokenSet.ElementAt(0).classKeyword == "while" && status)
                    {
                        tokenSet.RemoveAt(0);
                        if (tokenSet.ElementAt(0).classKeyword == "(")
                        {
                            tokenSet.RemoveAt(0);
                            if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
                            {
                                status = Exp(ref t2);
                                if(t2 != "int_const")
                                {
                                    SemanticErrors.Add("Type Mismatch Error At " + tokenSet.ElementAt(0).lineNumber);
                                    return false;
                                }
                                if (tokenSet.ElementAt(0).classKeyword == ")" && status)
                                {
                                    tokenSet.RemoveAt(0);
                                    if (tokenSet.ElementAt(0).classKeyword == "ter")
                                    {
                                        tokenSet.RemoveAt(0);
                                    }
                                    else
                                    {
                                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                        status = false;
                                    }
                                }
                                else
                                {
                                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                    status = false;
                                }
                            }
                            else
                            {
                                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                status = false;
                            }
                        }
                        else
                        {
                            errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                            status = false;
                        }
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        bool WhileLoop()
        {
            bool status = true;
            string t2 = "";
            if (tokenSet.ElementAt(0).classKeyword == "while")
            {
                tokenSet.RemoveAt(0);
                if (tokenSet.ElementAt(0).classKeyword == "(")
                {
                    tokenSet.RemoveAt(0);
                    if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
                    {
                        status = Exp(ref t2);
                        if (tokenSet.ElementAt(0).classKeyword == ")" && status)
                        {
                            tokenSet.RemoveAt(0);
                            if (First_N_Follow.FirstBody.Contains(tokenSet.ElementAt(0).classKeyword))
                            {
                                status = Body();
                            }
                        }
                        else
                        {
                            errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                            status = false;
                        }
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        bool ForLoop()
        {
            bool status = true;
            string t2 = "";
            if (tokenSet.ElementAt(0).classKeyword == "for")
            {
                tokenSet.RemoveAt(0);
                if (tokenSet.ElementAt(0).classKeyword == "(")
                {
                    tokenSet.RemoveAt(0);
                    if (First_N_Follow.FirstAssignCall.Contains(tokenSet.ElementAt(0).classKeyword))
                    {
                        status = AssignCall();
                        //if(tokenSet.ElementAt(0).classKeyword=="ter" && status)
                        //{
                        //    tokenSet.RemoveAt(0);
                        if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
                        {
                            status = Exp(ref t2);
                            if (t2 != "int_const")
                            {
                                SemanticErrors.Add("Type Mismatch Error At " + tokenSet.ElementAt(0).lineNumber);
                                return false;
                            }
                            if (tokenSet.ElementAt(0).classKeyword == "ter" && status)
                            {
                                tokenSet.RemoveAt(0);
                                if (First_N_Follow.FirstCond3.Contains(tokenSet.ElementAt(0).classKeyword))
                                {
                                    status = Cond3();
                                    if (tokenSet.ElementAt(0).classKeyword == ")" && status)
                                    {
                                        tokenSet.RemoveAt(0);
                                        if (First_N_Follow.FirstBody.Contains(tokenSet.ElementAt(0).classKeyword))
                                        {
                                            status = Body();
                                        }
                                        else
                                        {
                                            errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                            status = false;
                                        }
                                    }
                                    else
                                    {
                                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                        status = false;
                                    }
                                }
                                else
                                {
                                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                    status = false;
                                }
                            }
                            else
                            {
                                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                status = false;
                            }
                        }
                        else
                        {
                            errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                            status = false;
                        }
                        //}
                        //else
                        //{
                        //    errorLine.Add(new ParseError( tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        //    status = false;
                        //}
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        bool SST()
        {
            bool status = true;
            if (First_N_Follow.FirstLoop_Str.Contains(tokenSet.ElementAt(0).classKeyword))
                status = Loop_Str();
            else if (tokenSet.ElementAt(0).classKeyword == "if")
                status = If_Else();
            else if (tokenSet.ElementAt(0).classKeyword == "switch")
                status = Switch();
            else if (First_N_Follow.FirstAssignCall.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                status = AssignCall();
            }
            else if (tokenSet.ElementAt(0).classKeyword == "return")
                status = Return();
            else if (tokenSet.ElementAt(0).classKeyword == "break")
            {
                tokenSet.RemoveAt(0);
                if (tokenSet.ElementAt(0).classKeyword == "ter")
                {
                    tokenSet.RemoveAt(0);
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else if (tokenSet.ElementAt(0).classKeyword == "continue")
            {
                tokenSet.RemoveAt(0);
                if (tokenSet.ElementAt(0).classKeyword == "ter")
                {
                    tokenSet.RemoveAt(0);
                }
            }
            else if (tokenSet.ElementAt(0).classKeyword == "ID")
            {
                status = AssignCall();
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        bool MST()
        {
            bool status = true;
            if (First_N_Follow.FirstSST.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                status = SST();
                if (First_N_Follow.FirstMST.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = MST();
                }
            }
            return status;
        }
        bool Return()
        {
            bool status = true;
            string type = "";
            if (tokenSet.ElementAt(0).classKeyword == "return")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Exp(ref type);
                    string funcType = helpers.lookup(ClassName);
                    string[] funcRetType = funcType.Split('-');
                    if (funcRetType[0] != type)
                    {
                        SemanticErrors.Add("Invalid Return Type At " + tokenSet.ElementAt(0).lineNumber);
                        return false;
                    }
                    if (tokenSet.ElementAt(0).classKeyword == "ter" && status)
                    {
                        tokenSet.RemoveAt(0);
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else if (tokenSet.ElementAt(0).classKeyword == "ter")
                {
                    tokenSet.RemoveAt(0);
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        bool Decl2(string t1)
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "ID")
            {
                string t2 = helpers.lookup(tokenSet.ElementAt(0).value);
                if (t2 == "")
                {
                    SemanticErrors.Add("Use of undeclared variable At " + tokenSet.ElementAt(0).lineNumber);
                    status = false;
                }
                t1 = helpers.Compatible(t1, t2, "=");
                if(t1 == "")
                {
                    SemanticErrors.Add("Type Mismatch in assigning At " + tokenSet.ElementAt(0).lineNumber);
                    status = false;
                }
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstInit.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowInit.Contains(tokenSet.ElementAt(0).classKeyword) && status)
                {
                    status = Init(t1);
                    if (First_N_Follow.FirstList.Contains(tokenSet.ElementAt(0).classKeyword) && status)
                    {
                        status = List("private", t1, "");
                        if (!status)
                        {
                            errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                            status = false;
                        }
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else if (tokenSet.ElementAt(0).classKeyword == "new")
            {
                tokenSet.RemoveAt(0);
                if (tokenSet.ElementAt(0).classKeyword == "DT")
                {
                    t2 = tokenSet.ElementAt(0).value;
                    if (t1 != t2)
                    {
                        SemanticErrors.Add("Type mismatch Error At " + tokenSet.ElementAt(0).lineNumber);
                    }
                    tokenSet.RemoveAt(0);
                    if (tokenSet.ElementAt(0).classKeyword == "(")
                    {
                        string type = "ctor-";
                        tokenSet.RemoveAt(0);
                        if (First_N_Follow.FirstParams.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowParams.Contains(tokenSet.ElementAt(0).classKeyword))
                        {
                            status = Params(ref type);
                            if (tokenSet.ElementAt(0).classKeyword == ")" && status)
                            {
                                tokenSet.RemoveAt(0);
                                if (tokenSet.ElementAt(0).classKeyword == "ter")
                                {
                                    tokenSet.RemoveAt(0);
                                }
                                else
                                {
                                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                    status = false;
                                }
                            }
                            else
                            {
                                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                status = false;
                            }
                        }
                        else
                        {
                            errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                            status = false;
                        }
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        //bool Commas(ref string paralist)
        //{
        //    bool status = true;
        //    if (tokenSet.ElementAt(0).classKeyword == ",")
        //    {
        //        paralist += ",";
        //        tokenSet.RemoveAt(0);
        //        if (First_N_Follow.FirstParams.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowParams.Contains(tokenSet.ElementAt(0).classKeyword))
        //        {
        //            status = Params(ref paralist);
        //            if (!status)
        //            {
        //                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
        //                status = false;
        //            }
        //        }
        //        else
        //        {
        //            errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
        //            status = false;
        //        }
        //    }
        //    return status;
        //}
        bool Params(ref string type)
        {
            bool status = true;
            if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                string t2 = "";
                status = Exp(ref t2);
                type += t2;
                t2 = "";              
                if(tokenSet.ElementAt(0).classKeyword == ",")
                {
                    type += ",";
                    tokenSet.RemoveAt(0);
                    status = Params(ref type);
                    if (!status)
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
            }
            return status;
        }
        bool Exp(ref string t1)
        {
            opr = "";
            bool status = true;
            if (First_N_Follow.FirstAnd.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                status = AND(ref t1);
                if (tokenSet.ElementAt(0).classKeyword == "||" || First_N_Follow.FollowExp.Contains(tokenSet.ElementAt(0).classKeyword) && status)
                {
                    status = RO1(ref t1);
                }
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        bool RO1(ref string t1)
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "||")
            {
                opr = "||";
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstAnd.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = AND(ref t1);
                    if (tokenSet.ElementAt(0).classKeyword == "||" || First_N_Follow.FollowExp.Contains(tokenSet.ElementAt(0).classKeyword))
                    {
                        status = RO1(ref t1);
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else if (First_N_Follow.FollowExp.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                status = true;
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        bool AND(ref string t1)
        {
            bool status = true;
            if (First_N_Follow.FirstAnd.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                status = ROP(ref t1);
                if (First_N_Follow.FollowAND1.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowExp.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = AND1(ref t1);
                }
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        bool AND1(ref string t1)
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "&&")
            {
                opr = "&&";
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstAnd.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = ROP(ref t1);
                    if (First_N_Follow.FollowAND1.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowExp.Contains(tokenSet.ElementAt(0).classKeyword) && status)
                    {
                        status = AND1(ref t1);
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else if (First_N_Follow.FollowAND1.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowExp.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                status = true;
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        bool ROP1(ref string t1)
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "RO")
            {
                opr = tokenSet.ElementAt(0).value;
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstAnd.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = PM(ref t1);
                    if (First_N_Follow.FollowROP1.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowExp.Contains(tokenSet.ElementAt(0).classKeyword) && status)
                    {
                        status = ROP1(ref t1);
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }

            return status;
        }
        bool ROP(ref string t1)
        {
            bool status = true;
            if (First_N_Follow.FirstAnd.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                status = PM(ref t1);
                if (First_N_Follow.FollowROP1.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowExp.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = ROP1(ref t1);
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        bool PM(ref string t1)
        {
            bool status = true;
            if (First_N_Follow.FirstAnd.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                status = MDM(ref t1);
                if (First_N_Follow.FollowPM1.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowExp.Contains(tokenSet.ElementAt(0).classKeyword) && status)
                {
                    status = PM1(ref t1);
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        bool PM1(ref string t1)
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "PM")
            {
                opr = tokenSet.ElementAt(0).value;
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstAnd.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = MDM(ref t1);
                    if (First_N_Follow.FollowPM1.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowExp.Contains(tokenSet.ElementAt(0).classKeyword) && status)
                    {
                        status = PM1(ref t1);
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            return status;
        }
        bool MDM(ref string t1)
        {
            bool status = true;
            if (First_N_Follow.FirstAnd.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                status = F(ref t1);
                if (First_N_Follow.FollowMDM1.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowExp.Contains(tokenSet.ElementAt(0).classKeyword) && status)
                {
                    status = MDM1(ref t1);
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            return status;
        }
        bool MDM1(ref string t1)
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "MDM")
            {
                opr = tokenSet.ElementAt(0).value;
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstAnd.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = F(ref t1);
                    if (First_N_Follow.FollowMDM1.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowExp.Contains(tokenSet.ElementAt(0).classKeyword) && status)
                    {
                        status = MDM1(ref t1);
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            return status;
        }
        bool F(ref string t1)
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "ID")
            {
                if(opr != "")
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, "sementatic error", tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
                else
                {
                    t1 = tokenSet.ElementAt(0).value;
                    tokenSet.RemoveAt(0);
                    if (First_N_Follow.FirstDec_inc.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowDec_inc.Contains(tokenSet.ElementAt(0).classKeyword))
                    {
                        status = DEC_INC(ref t1);
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
            }
            else if (First_N_Follow.FirstConst.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                if (opr == "")
                {
                    t1 = tokenSet.ElementAt(0).classKeyword;
                }
                else
                {
                    t2 = tokenSet.ElementAt(0).classKeyword;
                    t1 = helpers.Compatible(t1, t2, opr);
                    if (t1 == "")
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, "sementatic error", tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                    else
                    {
                        t2 = opr = "";
                    }
                }
                tokenSet.RemoveAt(0);
            }
            else if (tokenSet.ElementAt(0).classKeyword == "(")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Exp(ref t1);
                    if (tokenSet.ElementAt(0).classKeyword == ")")
                    {
                        tokenSet.RemoveAt(0);
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else if (tokenSet.ElementAt(0).classKeyword == "!")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstAnd.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = F(ref t1);
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else if (tokenSet.ElementAt(0).classKeyword == "inc" || tokenSet.ElementAt(0).classKeyword == "dec")
            {
                tokenSet.RemoveAt(0);
                if (tokenSet.ElementAt(0).classKeyword == "ID")
                {
                    tokenSet.RemoveAt(0);
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        bool DEC_INC_RE(ref string t1)
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == ".")
            {
                tokenSet.RemoveAt(0);
                if (tokenSet.ElementAt(0).classKeyword == "ID")
                {
                    string name = tokenSet.ElementAt(0).value;
                    tokenSet.RemoveAt(0);
                    status = DEC_INC_RE_1(ref t1, name);
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            return status;
        }
        bool DEC_INC_RE_1(ref string t1, string name)
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "(")
            {
                t1 = helpers.lookupFT(name, t1);
                if (t1 == "")
                {
                    SemanticErrors.Add("Use of Undeclared variable At " + tokenSet.ElementAt(0).lineNumber);
                    return false;
                }
                tokenSet.RemoveAt(0);
                string paralist = "";
                status = Params(ref paralist);
                string[] para = t1.Split('-');
                if (t1 != para[1])
                {
                    SemanticErrors.Add("Constructor not defined At " + tokenSet.ElementAt(0).lineNumber);
                    return false;
                }
                if (tokenSet.ElementAt(0).classKeyword == ")" && status)
                {
                    tokenSet.RemoveAt(0);
                    status = DEC_INC_RE(ref t1);
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else if (tokenSet.ElementAt(0).classKeyword == "[")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    string temp = "";
                    status = Exp(ref temp); //ye dekho agar array masla karay
                    if (tokenSet.ElementAt(0).classKeyword == "]")
                    {
                        tokenSet.RemoveAt(0);
                        status = DEC_INC_RE(ref t1);
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else
            {
                t1 = helpers.lookupFT(t1, ClassName);
                if (t1 == "")
                {
                    SemanticErrors.Add("Use of Undeclared Variable At " + tokenSet.ElementAt(0).lineNumber);
                    return false;
                }
                status = DEC_INC_RE(ref t1);
            }
            return status;
        }
        bool DEC_INC(ref string t1)
        {
            bool status = true;
            if (First_N_Follow.FirstINC_DEC.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                if(helpers.lookupFT(t1, ClassName) == "")
                {
                    SemanticErrors.Add("Use of Undeclared variable At " + tokenSet.ElementAt(0).lineNumber);
                    return false;
                }
                tokenSet.RemoveAt(0);
            }
            else if (tokenSet.ElementAt(0).classKeyword == "[")
            {
                tokenSet.RemoveAt(0);
                string name = t1;
                string argument = "";
                t1 = helpers.lookupFT(t1, ClassName);
                if(t1 == "")
                {
                    SemanticErrors.Add("Use of Undeclared variable At " + tokenSet.ElementAt(0).lineNumber);
                    return false;
                }
                if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Exp(ref argument);
                    string arrayType = helpers.lookupFT(name, ClassName);
                    if(arrayType == "")
                    {
                        SemanticErrors.Add("Use of Undeclared variable At " + tokenSet.ElementAt(0).lineNumber);
                        return false;
                    }
                    else
                    {
                        arrayType = arrayType.Substring(arrayType.Length - 2);
                        if (arrayType != t1)
                        {
                            SemanticErrors.Add("Type Mismatch Error At " + tokenSet.ElementAt(0).lineNumber);
                            return false;
                        }
                        t1 = arrayType;
                    }
                    if (tokenSet.ElementAt(0).classKeyword == "]")
                    {
                        tokenSet.RemoveAt(0);
                        if (tokenSet.ElementAt(0).classKeyword == ".")
                        {
                            status = DEC_INC_RE(ref t1);
                        }

                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else if (tokenSet.ElementAt(0).classKeyword == "(")
            {
                tokenSet.RemoveAt(0);
                string type = helpers.lookupCT(t1, Ref).Type;
                if (First_N_Follow.FirstParams.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowParams.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    string paralist = "ctor-";
                    status = Params(ref paralist);
                    if(type != paralist)
                    {
                         SemanticErrors.Add("Use of Undeclared variable At " + tokenSet.ElementAt(0).lineNumber);
                         return false;
                    }
                    t1 = type;
                    if (tokenSet.ElementAt(0).classKeyword == ")")
                    {
                        tokenSet.RemoveAt(0);
                        if (tokenSet.ElementAt(0).classKeyword == ".")
                        {
                            status = DEC_INC_RE(ref t1);
                        }
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else if (tokenSet.ElementAt(0).classKeyword == ".")
            {
                tokenSet.RemoveAt(0);
                if (tokenSet.ElementAt(0).classKeyword == "ID")
                {
                    t1 = helpers.lookupFT(tokenSet.ElementAt(0).value, t1);
                    if (t1 == "")
                    {
                        SemanticErrors.Add("Use of Undeclared Variable At " + tokenSet.ElementAt(0).lineNumber);
                        return false;
                    }
                    tokenSet.RemoveAt(0);
                    if (First_N_Follow.FirstDec_inc.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowMDM1.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowExp.Contains(tokenSet.ElementAt(0).classKeyword))
                    {
                        status = DEC_INC(ref t1);
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            return status;
        }
        //bool R1()
        //{
        //    bool status = true;
        //    if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
        //    {
        //        status = Exp();
        //        if (tokenSet.ElementAt(0).classKeyword == "ter" && status)
        //        {
        //            tokenSet.RemoveAt(0);
        //        }
        //        else
        //        {
        //            errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
        //            status = false;
        //        }
        //    }
        //    else if (tokenSet.ElementAt(0).classKeyword == "ter")
        //    {
        //        tokenSet.RemoveAt(0);
        //    }
        //    else
        //    {
        //        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
        //        status = false;
        //    }
        //    return status;
        //}
        bool Array_opt(ref string paralist)
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "[")
            {
                tokenSet.RemoveAt(0);
                    if (tokenSet.ElementAt(0).classKeyword == "]")
                    {
                        paralist += "[]";
                        tokenSet.RemoveAt(0);
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
            }
            return status;
        }
        bool List4(ref string paralist)
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == ",")
            {
                paralist += ",";
                tokenSet.RemoveAt(0);
                if (tokenSet.ElementAt(0).classKeyword == "DT" || tokenSet.ElementAt(0).classKeyword == "ID")
                {
                    status = funct_params(ref paralist);
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        bool funct_params(ref string paralist)
        {
            bool status = true;
            string type = "";
            string name = "";
            if (tokenSet.ElementAt(0).classKeyword == "ID")
            {
                type += tokenSet.ElementAt(0).value;
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstArrayOpt.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowArrayOpt.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Array_opt(ref type);
                    if (tokenSet.ElementAt(0).classKeyword == "ID" && status)
                    {
                        helpers.insertFT(tokenSet.ElementAt(0).value, type);
                        tokenSet.RemoveAt(0);
                        paralist += type;
                        type = "";
                        if (tokenSet.ElementAt(0).classKeyword == ",")
                        {
                            status = List4(ref paralist);
                        }
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else if (tokenSet.ElementAt(0).classKeyword == "DT")
            {
                type = tokenSet.ElementAt(0).value;
                paralist += tokenSet.ElementAt(0).value;
                string arrVal = "";
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstArrayOpt.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowArrayOpt.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Array_opt(ref arrVal);
                    if (arrVal == "")
                    {
                        type += "_const";
                        paralist += "_const";
                    }
                    else
                    {
                        type += arrVal;
                        paralist += arrVal;
                    }
                    if (tokenSet.ElementAt(0).classKeyword == "ID" && status)
                    {
                        name = tokenSet.ElementAt(0).value;
                        if(!helpers.insertFT(name, type))
                        {
                            SemanticErrors.Add("Redeclaration Error At" + tokenSet.ElementAt(0).lineNumber);
                            return false;
                        }
                        tokenSet.RemoveAt(0);
                        if (tokenSet.ElementAt(0).classKeyword == ",")
                        {
                            status = List4(ref paralist);
                        }
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            return status;
        }
        bool FunctionBody()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "ter")
            {
                tokenSet.RemoveAt(0);
            }
            else if (tokenSet.ElementAt(0).classKeyword == "{")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstMST.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = MST();
                    //if(tokenSet.ElementAt(0).classKeyword=="return" && status)
                    //{
                    //    tokenSet.RemoveAt(0);
                    //    //if (First_N_Follow.FirstR1.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowR1.Contains(tokenSet.ElementAt(0).classKeyword))
                    //    //{
                    //    //    status = R1();
                    //    //    if(tokenSet.ElementAt(0).classKeyword=="}" && status)
                    //    //    {
                    //    //        tokenSet.RemoveAt(0);
                    //    //    }
                    //    //    else
                    //    //    {
                    //    //        errorLine.Add(new ParseError( tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    //    //        status = false;
                    //    //    }
                    //    //}
                    //    //else
                    //    //{
                    //    //    errorLine.Add(new ParseError( tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    //    //    status = false;
                    //    //}
                    //    
                    //}
                    //else
                    //{
                    //    errorLine.Add(new ParseError( tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    //    status = false;
                    //}
                    if (tokenSet.ElementAt(0).classKeyword == "}" && status)
                    {
                        tokenSet.RemoveAt(0);
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    } //ye extra ha
                }
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        bool Ass(string t1)
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "assign")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstAss1.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Ass1(t1);
                    if (!status)
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        bool Ass1(string t1)
        {
            bool status = true;
            if (First_N_Follow.FirstDec2.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                status = Decl2(t1);
                if (tokenSet.ElementAt(0).classKeyword == "ter" && status)
                {
                    tokenSet.RemoveAt(0);
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else if (First_N_Follow.FirstConst.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                if(helpers.Compatible(t1, tokenSet.ElementAt(0).classKeyword, "=") == "")
                {
                    SemanticErrors.Add("type Mismatch");
                }
                else
                {
                    tokenSet.RemoveAt(0);
                    if (tokenSet.ElementAt(0).classKeyword == "ter")
                    {
                        tokenSet.RemoveAt(0);
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        bool ArrayInit(string t1)
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "ID")
            {

                tokenSet.RemoveAt(0);
            }
            else if (tokenSet.ElementAt(0).classKeyword == "[")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstParams.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowParams.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    string paralist = "";
                    status = Params(ref paralist);
                    string[] para = paralist.Split(',');
                    string temp = t1.Substring(0,t1.Length - 2);
                    temp += "_const";
                    foreach(var param in para)
                    {
                        if (param != temp)
                        {
                            SemanticErrors.Add("Type Mismatch Error At " + tokenSet.ElementAt(0).lineNumber);
                            return false;
                        }
                    }
                    if (tokenSet.ElementAt(0).classKeyword == "]" && status)
                    {
                        tokenSet.RemoveAt(0);
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        bool Decl5(string am, string type, string name)
        {
            bool status = true;
            string tm = "";
            if (tokenSet.ElementAt(0).classKeyword == "assign")
            {
                if( !helpers.insertCT(name, type, am, tm ,Ref))
                {
                    SemanticErrors.Add("Redeclaration Error At " + tokenSet.ElementAt(0).lineNumber);
                    status = false;
                }
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstArrayInit.Contains(tokenSet.ElementAt(0).classKeyword) && status)
                {
                    status = ArrayInit(type);
                    if (tokenSet.ElementAt(0).classKeyword == "ter" && status)
                    {
                        tokenSet.RemoveAt(0);
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }

            }
            else if (tokenSet.ElementAt(0).classKeyword == "ter")
            {
                if (!helpers.insertCT(name, type, am, tm,Ref))
                {
                    SemanticErrors.Add("Redeclaration Error At " + tokenSet.ElementAt(0).lineNumber);
                    status = false;
                }
                tokenSet.RemoveAt(0);
            }
            else if (tokenSet.ElementAt(0).classKeyword == "(")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstFunctParams.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowParams.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    string paralist = "";
                    helpers.createScope();
                    status = funct_params(ref paralist);
                    type += "-" + paralist;
                    if (tokenSet.ElementAt(0).classKeyword == ")")
                    {
                        if (!helpers.insertCT(name, type, am, tm, Ref))
                        {
                            SemanticErrors.Add("Redeclaration Error At " + tokenSet.ElementAt(0).lineNumber);
                            status = false;
                        }
                        tokenSet.RemoveAt(0);
                        if (First_N_Follow.FirstFunction_body.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowFunction_body.Contains(tokenSet.ElementAt(0).classKeyword))
                        {
                            status = FunctionBody();
                        }
                        else
                        {
                            errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                            status = false;
                        }
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }

            return status;
        }
        bool Decl6(ref string type, ref string am)
        {
            bool status = true;
            string name = "";
            string tm = "";
            if (tokenSet.ElementAt(0).classKeyword == "ID")
            {
                name = tokenSet.ElementAt(0).value;
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstAss.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    if (!helpers.insertCT(name, type, am, tm ,Ref))
                    {
                        SemanticErrors.Add("Redeclaration Error at " + tokenSet.ElementAt(0).lineNumber);
                        status = false;
                    }
                    status = Ass(type);
                    if (!status)
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else if (tokenSet.ElementAt(0).classKeyword == "[")
                {
                    tokenSet.RemoveAt(0);
                        if (tokenSet.ElementAt(0).classKeyword == "]")
                        {
                            tokenSet.RemoveAt(0);
                            type += "[]";
                            if (tokenSet.ElementAt(0).classKeyword == "ID")
                            {
                                name = tokenSet.ElementAt(0).value;
                                tokenSet.RemoveAt(0);
                                status = Decl5(am, type, name);
                            }
                            else
                            {
                                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                status = false;
                            }
                        }
                        else
                        {
                            errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                            status = false;
                        }
                }
                else if (First_N_Follow.FirstList.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = List(am, type, name);
                }
                else if (First_N_Follow.FirstDec5.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Decl5(am, type, name);
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else if (tokenSet.ElementAt(0).classKeyword == "[")
            {
                tokenSet.RemoveAt(0);
                if (tokenSet.ElementAt(0).classKeyword == "]")
                {
                    tokenSet.RemoveAt(0);
                    type += "[]";
                    if (tokenSet.ElementAt(0).classKeyword == "ID")
                    {
                        name = tokenSet.ElementAt(0).value;
                        tokenSet.RemoveAt(0);
                        if (First_N_Follow.FirstDec5.Contains(tokenSet.ElementAt(0).classKeyword))
                        {
                            status = Decl5(am, type, name);
                        }
                        else
                        {
                            errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                            status = false;
                        }
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        bool Decl(ref string am)
        {
            bool status = true;
            string type = "";
            string name = "";
            string tm = "";
            if (tokenSet.ElementAt(0).classKeyword == "DT") 
            {
                type = tokenSet.ElementAt(0).value;
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstDecl6.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Decl6(ref type, ref am);
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else if (tokenSet.ElementAt(0).classKeyword == "abstract")
            {
                tm = "abstract";
                tokenSet.RemoveAt(0);
                if (tokenSet.ElementAt(0).classKeyword == "DT")
                {
                    type = tokenSet.ElementAt(0).value;
                    tokenSet.RemoveAt(0);
                    if (tokenSet.ElementAt(0).classKeyword == "ID")
                    {
                        name = tokenSet.ElementAt(0).value;
                        tokenSet.RemoveAt(0);
                        if (tokenSet.ElementAt(0).classKeyword == "(")
                        {
                            tokenSet.RemoveAt(0);
                            if (First_N_Follow.FirstFunctParams.Contains(tokenSet.ElementAt(0).classKeyword))
                            {
                                string paralist = "";
                                helpers.createScope();
                                status = funct_params(ref paralist);
                                type += "-" + paralist;
                                if (tokenSet.ElementAt(0).classKeyword == ")" && status)
                                {
                                    tokenSet.RemoveAt(0);
                                    if(!helpers.insertCT(name, type, am, tm, Ref))
                                    {
                                        SemanticErrors.Add("Redeclaration Error At " + tokenSet.ElementAt(0).lineNumber);
                                    }
                                    if (First_N_Follow.FirstFunction_body.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowFunction_body.Contains(tokenSet.ElementAt(0).classKeyword))
                                    {
                                        status = FunctionBody();
                                        if (!status)
                                        {
                                            errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                            status = false;
                                        }
                                    }
                                    else
                                    {
                                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                        status = false;
                                    }
                                }
                                else
                                {
                                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                    status = false;
                                }
                            }
                            else
                            {
                                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                status = false;
                            }
                        }
                        else
                        {
                            errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                            status = false;
                        }
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else if (tokenSet.ElementAt(0).classKeyword == "ID")
            {
                tm = "";
                type = tokenSet.ElementAt(0).value;
                tokenSet.RemoveAt(0);
                if (tokenSet.ElementAt(0).classKeyword == "ID")
                {
                    name = tokenSet.ElementAt(0).value;
                    tokenSet.RemoveAt(0);
                    if (First_N_Follow.FirstAss.Contains(tokenSet.ElementAt(0).classKeyword))
                    {
                        if(!helpers.insertCT(name, type, am, tm, Ref))
                        {
                            SemanticErrors.Add("Redeclaration Error At " + tokenSet.ElementAt(0).lineNumber);
                        }
                        status = Ass(type);
                        if (!status)
                        {
                            errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                            status = false;
                        }
                    }
                    else if (tokenSet.ElementAt(0).classKeyword == "(")
                    {
                        tokenSet.RemoveAt(0);
                        if (First_N_Follow.FirstFunctParams.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowParams.Contains(tokenSet.ElementAt(0).classKeyword))
                        {
                            string paralist = "";
                            helpers.createScope();
                            status = funct_params(ref paralist);
                            type += "-" + paralist;
                            if (tokenSet.ElementAt(0).classKeyword == ")" && status)
                            {
                                if(!helpers.insertCT(name, type, am, tm, Ref))
                                {
                                    SemanticErrors.Add("Redeclaration Error At " + tokenSet.ElementAt(0).lineNumber);
                                    status = false;
                                }
                                tokenSet.RemoveAt(0);
                                if (First_N_Follow.FirstFunction_body.Contains(tokenSet.ElementAt(0).classKeyword) && status)
                                {
                                    status = FunctionBody();
                                }
                                else
                                {
                                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                    status = false;
                                }
                            }
                            else
                            {
                                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                status = false;
                            }
                        }
                        else
                        {
                            errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                            status = false;
                        }
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else if (tokenSet.ElementAt(0).classKeyword == "(")
                {
                    name = type;
                    type = "ctor";
                    tokenSet.RemoveAt(0);
                    if (First_N_Follow.FirstFunctParams.Contains(tokenSet.ElementAt(0).classKeyword) || tokenSet.ElementAt(0).classKeyword==")")
                    {
                        string paralist = "";
                        helpers.createScope();
                        status = funct_params(ref paralist);
                        type += "-" + paralist;
                        if (tokenSet.ElementAt(0).classKeyword == ")")
                        {
                            if (!helpers.insertCT(name, type, am, tm, Ref))
                            {
                                SemanticErrors.Add("Redeclaration Error At " + tokenSet.ElementAt(0).lineNumber);
                                status = false;
                            }
                            tokenSet.RemoveAt(0);
                            if (First_N_Follow.FirstFunction_body.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowFunction_body.Contains(tokenSet.ElementAt(0).classKeyword))
                            {
                                status = FunctionBody();
                            }
                            else
                            {
                                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                status = false;
                            }
                        }
                        else
                        {
                            errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                            status = false;
                        }
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else if (tokenSet.ElementAt(0).classKeyword == "[")
                {
                    tokenSet.RemoveAt(0);
                    if (tokenSet.ElementAt(0).classKeyword == "]")
                    {
                        tokenSet.RemoveAt(0);
                        type += "[]";
                        if (tokenSet.ElementAt(0).classKeyword == "ID")
                        {
                            name = tokenSet.ElementAt(0).value;
                            tokenSet.RemoveAt(0);
                            if (First_N_Follow.FirstDec5.Contains(tokenSet.ElementAt(0).classKeyword))
                            {
                                status = Decl5(am,type,name);
                            }
                            else
                            {
                                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                status = false;
                            }
                        }
                        else
                        {
                            errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                            status = false;
                        }
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        bool Decl_Init()
        {
            bool status = true;
            string am = "private";
            if (First_N_Follow.FirstAM.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowAM.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                status = AM(ref am);
                if (First_N_Follow.FirstDecl.Contains(tokenSet.ElementAt(0).classKeyword) && status)
                {
                    status = Decl(ref am);
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        bool Class_Body1()
        {
            bool status = true;
            if (First_N_Follow.FirstDecl_Init.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                status = Decl_Init();
                if (status)
                {
                    status = Class_Body1();
                    if (!status)
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                }
            }
            return status;
        }
        bool Class_Body()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "{")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstDecl_Init.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Decl_Init();
                    if (status)
                    {
                        if (First_N_Follow.FirstClass_Body1.Contains(tokenSet.ElementAt(0).classKeyword) || tokenSet.ElementAt(0).classKeyword == "}")
                        {
                            status = Class_Body1();
                            if (status)
                            {
                                if (tokenSet.ElementAt(0).classKeyword == "}")
                                    tokenSet.RemoveAt(0);
                                else
                                    status = false;
                            }
                        }
                        else if (tokenSet.ElementAt(0).classKeyword == "ter")
                            tokenSet.RemoveAt(0);
                        else
                            status = false;
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    }
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }


        bool Extend(ref string parent)
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "extend")
            {
                tokenSet.RemoveAt(0);
                if (tokenSet.ElementAt(0).classKeyword == "ID")
                {
                    parent = tokenSet.ElementAt(0).value;
                    tokenSet.RemoveAt(0);
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            return status;
        }
        bool Abstract_Class(ref string category)
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "abstract")
            {
                category = "abstract";
                tokenSet.RemoveAt(0);
            }
            return status;
        }
        bool Sealed(ref string category)
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "sealed")
            {
                category = "sealed";
                tokenSet.RemoveAt(0);
            }                
            return status;
        }
    }
}
