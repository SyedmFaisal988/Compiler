using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer
{
    class ParseTree
    {
        private List<Token> tokenSet = new List<Token>(StaticComponents.tokenSet.Count);
        private List<ParseError> errorLine = new List<ParseError>();
        public ParseTree()
        {
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
            bool status = true;
            if (First_N_Follow.FirstAbstract.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowAbstract.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                status = Abstract_Class();
                if (First_N_Follow.FirstSealed.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowSealed.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Sealed();
                    if (tokenSet.ElementAt(0).classKeyword == "class")
                    {
                        tokenSet.RemoveAt(0);
                        if (tokenSet.ElementAt(0).classKeyword == "ID")
                        {
                            tokenSet.RemoveAt(0);
                            status = Extend();
                            if (First_N_Follow.FirstClass_Body.Contains(tokenSet.ElementAt(0).classKeyword))
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
        bool AM()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "AM")
            {
                tokenSet.RemoveAt(0);
            }
            return status;
        }
        bool Init2()
        {
            bool status = true;
            if (First_N_Follow.FirstConst.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                tokenSet.RemoveAt(0);
            }
            else if (tokenSet.ElementAt(0).classKeyword == "ID")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstInit.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowInit.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Init();
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
        bool Init()
        {
            bool status = true;
            if (First_N_Follow.FirstInit2.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                status = Init2();
            }
            else if (tokenSet.ElementAt(0).classKeyword=="assign")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstInit2.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Init2();
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            return status;
        }
        bool List()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "ter")
            {
                tokenSet.RemoveAt(0);
            }
            else if (tokenSet.ElementAt(0).classKeyword == ",")
            {
                tokenSet.RemoveAt(0);
                //if (tokenSet.ElementAt(0).classKeyword == "ID")
                //{
                //    tokenSet.RemoveAt(0);
                    if (First_N_Follow.FirstInit.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowInit.Contains(tokenSet.ElementAt(0).classKeyword))
                    {
                        status = Init();
                        if (First_N_Follow.FirstList.Contains(tokenSet.ElementAt(0).classKeyword) && status)
                        {
                            status = List();
                        }
                        else
                        {
                            errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                            status = false;
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
            bool status = true;
            if (First_N_Follow.FirstCond3_1.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Exp();
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
            if (tokenSet.ElementAt(0).classKeyword == "case")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Exp();
                    if (tokenSet.ElementAt(0).classKeyword == ":")
                    {
                        tokenSet.RemoveAt(0);
                        if (First_N_Follow.FirstMST.Contains(tokenSet.ElementAt(0).classKeyword) && status)
                        {
                            status = MST();
                            /* if (tokenSet.ElementAt(0).classKeyword == "break")
                             {
                                 tokenSet.RemoveAt(0);
                                 if (tokenSet.ElementAt(0).classKeyword == "ter"){
                                     tokenSet.RemoveAt(0);
                                 }
                             }
                             else
                             {
                                 errorLine.Add(new ParseError( tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                                 status = false;
                             }*/
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
            if (tokenSet.ElementAt(0).classKeyword == "switch")
            {
                tokenSet.RemoveAt(0);
                if (tokenSet.ElementAt(0).classKeyword == "(")
                {
                    tokenSet.RemoveAt(0);
                    if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
                    {
                        status = Exp();
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
        bool AssignList()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == ".")
            {
                tokenSet.RemoveAt(0);
                if (tokenSet.ElementAt(0).classKeyword == "ID")
                {
                    tokenSet.RemoveAt(0);
                    if (tokenSet.ElementAt(0).classKeyword == "." || First_N_Follow.FollowAssignList.Contains(tokenSet.ElementAt(0).classKeyword))
                    {
                        status = AssignList();
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
        bool AssignCall2()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "(")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstParams.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowParams.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Params();
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
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstCall.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Call();
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
        bool Call1()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "(")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstParams.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowParams.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Params();
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
                else if (First_N_Follow.FirstInitList.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = InitList();
                    if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword) && status)
                    {
                        status = Exp();
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
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            return status;
        }
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
        bool Call()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "ID")
            {
                if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Exp();
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

                //tokenSet.RemoveAt(0);
                //if(First_N_Follow.FollowAssignList.Contains(tokenSet.ElementAt(0).classKeyword) || tokenSet.ElementAt(0).classKeyword == ".")
                //{
                //    status = AssignList();
                //    if(First_N_Follow.FirstCall1.Contains(tokenSet.ElementAt(0).classKeyword) && status)
                //    {
                //        status = Call1();
                //    }
                //    else
                //    {
                //        errorLine.Add(new ParseError( tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                //        status = false;
                //    }
                //}
                //else
                //{
                //    errorLine.Add(new ParseError( tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                //    status = false;
                //}
            }
            else if (First_N_Follow.FirstConst.Contains(tokenSet.ElementAt(0).classKeyword))
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
            return true;
        }
        bool AssignCall()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "ID")
            {
                tokenSet.RemoveAt(0);
                if (tokenSet.ElementAt(0).classKeyword == "." || First_N_Follow.FollowAssignList.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = AssignList();
                    if (First_N_Follow.FollowAssignList.Contains(tokenSet.ElementAt(0).classKeyword))
                    {
                        status = AssignCall2();
                    }
                    else
                    {
                        errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                        status = false;
                    }
                }
                else if (First_N_Follow.FollowArrayOpt.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FirstArrayOpt.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Array_opt();
                    if (tokenSet.ElementAt(0).classKeyword == "ID")
                    {
                        tokenSet.RemoveAt(0);
                        if (tokenSet.ElementAt(0).classKeyword == "assign")
                        {
                            tokenSet.RemoveAt(0);
                            if (tokenSet.ElementAt(0).classKeyword == "new")
                            {
                                tokenSet.RemoveAt(0);
                                if (tokenSet.ElementAt(0).classKeyword == "ID")
                                {
                                    tokenSet.RemoveAt(0);
                                    if (tokenSet.ElementAt(0).classKeyword == "(")
                                    {
                                        tokenSet.RemoveAt(0);
                                        if (First_N_Follow.FirstParams.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowParams.Contains(tokenSet.ElementAt(0).classKeyword))
                                        {
                                            status = Params();
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
                            else if (tokenSet.ElementAt(0).classKeyword == "(")
                            {
                                if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
                                {
                                    status = Exp();
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
                                    status = Exp();
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
                                /*   tokenSet.RemoveAt(0);
                                   if (First_N_Follow.FollowAssignList.Contains(tokenSet.ElementAt(0).classKeyword) || tokenSet.ElementAt(0).classKeyword == "." || tokenSet.ElementAt(0).classKeyword == "(")
                                   {
                                       status = AssignList();
                                       if (tokenSet.ElementAt(0).classKeyword == "(")
                                       {
                                           tokenSet.RemoveAt(0);
                                           if (First_N_Follow.FirstParams.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowParams.Contains(tokenSet.ElementAt(0).classKeyword))
                                           {
                                               status = Params();
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
                               */
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
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstArrayOpt.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowArrayOpt.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Array_opt();
                    if (tokenSet.ElementAt(0).classKeyword == "ID")
                    {
                        tokenSet.RemoveAt(0);
                        if (tokenSet.ElementAt(0).classKeyword == "assign")
                        {
                            tokenSet.RemoveAt(0);
                            if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
                            {
                                status = Exp();
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
                                    tokenSet.RemoveAt(0);
                                    if (tokenSet.ElementAt(0).classKeyword == "(")
                                    {
                                        tokenSet.RemoveAt(0);
                                        if (First_N_Follow.FirstParams.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowParams.Contains(tokenSet.ElementAt(0).classKeyword))
                                        {
                                            status = Params();
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
            if (tokenSet.ElementAt(0).classKeyword == "if")
            {
                tokenSet.RemoveAt(0);
                if (tokenSet.ElementAt(0).classKeyword == "(")
                {
                    tokenSet.RemoveAt(0);
                    if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
                    {
                        status = Exp();
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
                                status = Exp();
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
            if (tokenSet.ElementAt(0).classKeyword == "while")
            {
                tokenSet.RemoveAt(0);
                if (tokenSet.ElementAt(0).classKeyword == "(")
                {
                    tokenSet.RemoveAt(0);
                    if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
                    {
                        status = Exp();
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
                            status = Exp();
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
            if (tokenSet.ElementAt(0).classKeyword == "return")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Exp();
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
        bool Decl2()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "ID")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstInit.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowInit.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Init();
                    if (First_N_Follow.FirstList.Contains(tokenSet.ElementAt(0).classKeyword) && status)
                    {
                        status = List();
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
                    tokenSet.RemoveAt(0);
                    if (tokenSet.ElementAt(0).classKeyword == "(")
                    {
                        tokenSet.RemoveAt(0);
                        if (First_N_Follow.FirstParams.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowParams.Contains(tokenSet.ElementAt(0).classKeyword))
                        {
                            status = Params();
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
        bool Commas()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == ",")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstParams.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowParams.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Params();
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
        bool Params()
        {
            bool status = true;
            if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                status = Exp();
                if (First_N_Follow.FirstCommas.Contains(tokenSet.ElementAt(0).classKeyword) && status)
                {
                    status = Commas();
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            return status;
        }
        bool Exp()
        {
            bool status = true;
            if (First_N_Follow.FirstAnd.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                status = AND();
                if (tokenSet.ElementAt(0).classKeyword == "||" || First_N_Follow.FollowExp.Contains(tokenSet.ElementAt(0).classKeyword) && status)
                {
                    status = RO1();
                }
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        bool RO1()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "||")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstAnd.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = AND();
                    if (tokenSet.ElementAt(0).classKeyword == "||" || First_N_Follow.FollowExp.Contains(tokenSet.ElementAt(0).classKeyword))
                    {
                        status = RO1();
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
        bool AND()
        {
            bool status = true;
            if (First_N_Follow.FirstAnd.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                status = ROP();
                if (First_N_Follow.FollowAND1.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowExp.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = AND1();
                }
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        bool AND1()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "&&")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstAnd.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = ROP();
                    if (First_N_Follow.FollowAND1.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowExp.Contains(tokenSet.ElementAt(0).classKeyword) && status)
                    {
                        status = AND1();
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
        bool ROP1()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "RO")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstAnd.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = PM();
                    if (First_N_Follow.FollowROP1.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowExp.Contains(tokenSet.ElementAt(0).classKeyword) && status)
                    {
                        status = ROP1();
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
        bool ROP()
        {
            bool status = true;
            if (First_N_Follow.FirstAnd.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                status = PM();
                if (First_N_Follow.FollowROP1.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowExp.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = ROP1();
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
        bool PM()
        {
            bool status = true;
            if (First_N_Follow.FirstAnd.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                status = MDM();
                if (First_N_Follow.FollowPM1.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowExp.Contains(tokenSet.ElementAt(0).classKeyword) && status)
                {
                    status = PM1();
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
        bool PM1()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "PM")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstAnd.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = MDM();
                    if (First_N_Follow.FollowPM1.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowExp.Contains(tokenSet.ElementAt(0).classKeyword) && status)
                    {
                        status = PM1();
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
        bool MDM()
        {
            bool status = true;
            if (First_N_Follow.FirstAnd.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                status = F();
                if (First_N_Follow.FollowMDM1.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowExp.Contains(tokenSet.ElementAt(0).classKeyword) && status)
                {
                    status = MDM1();
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            return status;
        }
        bool MDM1()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "MDM")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstAnd.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = F();
                    if (First_N_Follow.FollowMDM1.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowExp.Contains(tokenSet.ElementAt(0).classKeyword) && status)
                    {
                        status = MDM1();
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
        bool F()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "ID")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstDec_inc.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowMDM1.Contains(tokenSet.ElementAt(0).classKeyword) || tokenSet.ElementAt(0).classKeyword == "ter" || tokenSet.ElementAt(0).classKeyword == "]")
                {
                    status = DEC_INC();
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            else if (First_N_Follow.FirstConst.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                tokenSet.RemoveAt(0);
            }
            else if (tokenSet.ElementAt(0).classKeyword == "(")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Exp();
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
                    status = F();
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
        bool DEC_INC_RE()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == ".")
            {
                tokenSet.RemoveAt(0);
                if (tokenSet.ElementAt(0).classKeyword == "ID")
                {
                    tokenSet.RemoveAt(0);
                    status = DEC_INC_RE_1();
                }
                else
                {
                    errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                    status = false;
                }
            }
            return status;
        }
        bool DEC_INC_RE_1()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "(")
            {
                tokenSet.RemoveAt(0);
                status = Params();
                if (tokenSet.ElementAt(0).classKeyword == ")" && status)
                {
                    tokenSet.RemoveAt(0);
                    status = DEC_INC_RE();
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
                    status = Exp();
                    if (tokenSet.ElementAt(0).classKeyword == "]")
                    {
                        tokenSet.RemoveAt(0);
                        status = DEC_INC_RE();
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
                status = DEC_INC_RE();
            }
            return status;
        }
        bool DEC_INC()
        {
            bool status = true;
            if (First_N_Follow.FirstINC_DEC.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                tokenSet.RemoveAt(0);
            }
            else if (tokenSet.ElementAt(0).classKeyword == "[")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Exp();
                    if (tokenSet.ElementAt(0).classKeyword == "]")
                    {
                        tokenSet.RemoveAt(0);
                        if (tokenSet.ElementAt(0).classKeyword == ".")
                        {
                            status = DEC_INC_RE();
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
                if (First_N_Follow.FirstParams.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowParams.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Params();
                    if (tokenSet.ElementAt(0).classKeyword == ")")
                    {
                        tokenSet.RemoveAt(0);
                        if (tokenSet.ElementAt(0).classKeyword == ".")
                        {
                            status = DEC_INC_RE();
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
                    tokenSet.RemoveAt(0);
                    if (First_N_Follow.FirstDec_inc.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowMDM1.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowExp.Contains(tokenSet.ElementAt(0).classKeyword))
                    {
                        status = DEC_INC();
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
        bool R1()
        {
            bool status = true;
            if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                status = Exp();
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
            return status;
        }
        bool Array_opt()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "[")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstInt_const.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    tokenSet.RemoveAt(0);
                    if (tokenSet.ElementAt(0).classKeyword == "]")
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
            return status;
        }
        bool List4()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == ",")
            {
                tokenSet.RemoveAt(0);
                if (tokenSet.ElementAt(0).classKeyword == "DT" || tokenSet.ElementAt(0).classKeyword == "ID")
                {
                    status = funct_params();
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
        bool funct_params()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "ID")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstArrayOpt.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowArrayOpt.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Array_opt();
                    if (tokenSet.ElementAt(0).classKeyword == "ID" && status)
                    {
                        tokenSet.RemoveAt(0);
                        if (tokenSet.ElementAt(0).classKeyword == ",")
                        {
                            status = List4();
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
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstArrayOpt.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Array_opt();
                    if (tokenSet.ElementAt(0).classKeyword == "ID" && status)
                    {
                        tokenSet.RemoveAt(0);
                        if (tokenSet.ElementAt(0).classKeyword == ",")
                        {
                            status = List4();
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
        bool Ass()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "assign")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstAss1.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Ass1();
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
        bool Ass1()
        {
            bool status = true;
            if (First_N_Follow.FirstDec2.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                status = Decl2();
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
            else if (tokenSet.ElementAt(0).classKeyword == "(")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstFunctParams.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = funct_params();
                    if (tokenSet.ElementAt(0).classKeyword == ")" && status)
                    {
                        tokenSet.RemoveAt(0);
                        if (First_N_Follow.FirstFunction_body.Contains(tokenSet.ElementAt(0).classKeyword))
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
            }
            else if (First_N_Follow.FirstConst.Contains(tokenSet.ElementAt(0).classKeyword))
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
            else if (tokenSet.ElementAt(0).classKeyword == "[")
            {
                status = Decl5();
            }
            else
            {
                errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                status = false;
            }
            return status;
        }
        bool ArrayInit()
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
                    status = Params();
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
        bool Decl5()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "assign")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstArrayInit.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = ArrayInit();
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
                tokenSet.RemoveAt(0);
            }
            else if (tokenSet.ElementAt(0).classKeyword == "(")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstFunctParams.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowParams.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = funct_params();
                    if (tokenSet.ElementAt(0).classKeyword == ")")
                    {
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
        bool Decl()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "DT") //ye complete case nhe hain
            {
                tokenSet.RemoveAt(0);
                if (tokenSet.ElementAt(0).classKeyword == "ID")
                {
                    tokenSet.RemoveAt(0);
                    if (First_N_Follow.FirstAss.Contains(tokenSet.ElementAt(0).classKeyword))
                    {
                        status = Ass();
                        if (!status)
                        {
                            errorLine.Add(new ParseError(tokenSet.ElementAt(0).lineNumber, tokenSet.ElementAt(0).classKeyword, tokenSet.ElementAt(0).wordNumber));
                            status = false;
                        }
                    }
                    else if (tokenSet.ElementAt(0).classKeyword == "[")
                    {
                        tokenSet.RemoveAt(0);
                        if (First_N_Follow.FirstInt_const.Contains(tokenSet.ElementAt(0).classKeyword))
                        {
                            tokenSet.RemoveAt(0);
                            if (tokenSet.ElementAt(0).classKeyword == "]")
                            {
                                tokenSet.RemoveAt(0);
                                if (tokenSet.ElementAt(0).classKeyword == "ID")
                                {
                                    tokenSet.RemoveAt(0);
                                    status = Decl5();
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
                    else if (First_N_Follow.FirstList.Contains(tokenSet.ElementAt(0).classKeyword))
                    {
                        status = List();
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
                    if (First_N_Follow.FirstInt_const.Contains(tokenSet.ElementAt(0).classKeyword))
                    {
                        tokenSet.RemoveAt(0);
                        if (tokenSet.ElementAt(0).classKeyword == "]")
                        {
                            tokenSet.RemoveAt(0);
                            if (tokenSet.ElementAt(0).classKeyword == "ID")
                            {
                                tokenSet.RemoveAt(0);
                                if (First_N_Follow.FirstDec5.Contains(tokenSet.ElementAt(0).classKeyword))
                                {
                                    status = Decl5();
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
            else if (tokenSet.ElementAt(0).classKeyword == "abstract")
            {
                tokenSet.RemoveAt(0);
                if (tokenSet.ElementAt(0).classKeyword == "DT")
                {
                    tokenSet.RemoveAt(0);
                    if (tokenSet.ElementAt(0).classKeyword == "ID")
                    {
                        tokenSet.RemoveAt(0);
                        if (tokenSet.ElementAt(0).classKeyword == "(")
                        {
                            tokenSet.RemoveAt(0);
                            if (First_N_Follow.FirstFunctParams.Contains(tokenSet.ElementAt(0).classKeyword))
                            {
                                status = funct_params();
                                if (tokenSet.ElementAt(0).classKeyword == ")" && status)
                                {
                                    tokenSet.RemoveAt(0);
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
                tokenSet.RemoveAt(0);
              if (tokenSet.ElementAt(0).classKeyword == "ID")
                {
                    tokenSet.RemoveAt(0);
                    if (First_N_Follow.FirstAss.Contains(tokenSet.ElementAt(0).classKeyword))
                    {
                        status = Ass();
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
                            status = funct_params();
                            if (tokenSet.ElementAt(0).classKeyword == ")" && status)
                            {
                                tokenSet.RemoveAt(0);
                                if (First_N_Follow.FirstFunction_body.Contains(tokenSet.ElementAt(0).classKeyword))
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
                    tokenSet.RemoveAt(0);
                    if (First_N_Follow.FirstFunctParams.Contains(tokenSet.ElementAt(0).classKeyword) || tokenSet.ElementAt(0).classKeyword==")")
                    {
                        status = funct_params();
                        if (tokenSet.ElementAt(0).classKeyword == ")")
                        {
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
                    if (First_N_Follow.FirstInt_const.Contains(tokenSet.ElementAt(0).classKeyword))
                    {
                        tokenSet.RemoveAt(0);
                        if (tokenSet.ElementAt(0).classKeyword == "]")
                        {
                            tokenSet.RemoveAt(0);
                            if (tokenSet.ElementAt(0).classKeyword == "ID")
                            {
                                tokenSet.RemoveAt(0);
                                if (First_N_Follow.FirstDec5.Contains(tokenSet.ElementAt(0).classKeyword))
                                {
                                    status = Decl5();
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
        bool Decl_Init()
        {
            bool status = true;
            if (First_N_Follow.FirstAM.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowAM.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                status = AM();
                if (First_N_Follow.FirstDecl.Contains(tokenSet.ElementAt(0).classKeyword) && status)
                {
                    status = Decl();
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


        bool Extend()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "extend")
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
            return status;
        }
        bool Abstract_Class()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "abstract")
                tokenSet.RemoveAt(0);

            return status;
        }
        bool Sealed()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "sealed")
                tokenSet.RemoveAt(0);
            return status;
        }
    }
}
