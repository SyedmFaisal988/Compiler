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
        private List<int> errorLine = new List<int>();
        public ParseTree()
        {
            StaticComponents.tokenSet.ForEach((item) => {
                tokenSet.Add(item);
            });
        }
        public bool Parse()
        {
            return S();
        }
        bool S()
        {
            bool status = true;
            if (First_N_Follow.FirstClassStr.Contains(tokenSet.ElementAt(0).classKeyword) )
            {
                status = Class_str();
                if(status)
                {
                    if(tokenSet.ElementAt(0).classKeyword == "$")
                    {
                        tokenSet.RemoveAt(0);
                    }
                    else
                    {
                        errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                        status = false;
                    }
                }
                else
                {
                    status = false;
                }
            }
            else
            {
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
            }
            else if (First_N_Follow.FirstSealed.Contains(tokenSet.ElementAt(0).classKeyword))
                status = Sealed();
            else if(tokenSet.ElementAt(0).classKeyword == "class")
            {
                tokenSet.RemoveAt(0);
                if(tokenSet.ElementAt(0).classKeyword == "ID")
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
                                    errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                            }
                        }
                        else
                        {
                            errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                        }
                    }
                    else
                    {
                        status = false;
                        errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                    }
                }
                else
                {
                    errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                    status = false;
                }
            }
            else
            {
                errorLine.Add(tokenSet.ElementAt(0).lineNumber);
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
                    errorLine.Add(tokenSet.ElementAt(0).lineNumber);
            }
            return status;
        }
        bool AM()
        {
            bool status = true;
            if(tokenSet.ElementAt(0).classKeyword=="public" || tokenSet.ElementAt(0).classKeyword == "private")
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
                if(First_N_Follow.FirstInit.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowInit.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Init();
                    if (!status)
                    {
                        errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(tokenSet.ElementAt(0).lineNumber);
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
                if (tokenSet.ElementAt(0).classKeyword == "ID")
                {
                    tokenSet.RemoveAt(0);
                    if(First_N_Follow.FirstInit.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowInit.Contains(tokenSet.ElementAt(0).classKeyword))
                    {
                        status = Init();
                        if(First_N_Follow.FirstList.Contains(tokenSet.ElementAt(0).classKeyword) && status)
                        {
                            status = List();
                        }
                        else
                        {
                            errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                            status = false;
                        }
                    }
                }
                else
                {
                    errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                    status = false;
                }
            }
            else
            {
                errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                status = false;
            }
            return status;
        }
        bool Const()
        {
            bool status = true;
            if(First_N_Follow.FirstConst.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                tokenSet.RemoveAt(0);
            }
            else
            {
                errorLine.Add(tokenSet.ElementAt(0).lineNumber);
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
                errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                status = false;
            }
            if (!status)
            {
                errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                status = false;
            }
            return status;
        }
        bool Cond3()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "ID")
            {
                tokenSet.RemoveAt(0);
                if(First_N_Follow.FirstCond3_1.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowCond3_1.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Cond3_1;
                }
                else
                {
                    errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                    status = false;
                }
            }
            else if (tokenSet.ElementAt(0).classKeyword == "inc")
            {
                tokenSet.RemoveAt(0);
            }
            else if (tokenSet.ElementAt(0).classKeyword == "dec")
            {
                tokenSet.RemoveAt(0);
            }
            else
            {
                errorLine.Add(tokenSet.ElementAt(0).lineNumber);
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
            else if(tokenSet.ElementAt(0).classKeyword == "{")
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
                        errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                    status = false;
                }
            }
            else
            {
                errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                status = false;
            }
            return status;
        }
        bool DoWhileLoop()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "do")
            {
                if (First_N_Follow.FirstBody.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Body();
                    if(tokenSet.ElementAt(0).classKeyword=="while" && status)
                    {
                        tokenSet.RemoveAt(0);
                        if (tokenSet.ElementAt(0).classKeyword=="(")
                        {
                            tokenSet.RemoveAt(0);
                            if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
                            {
                                status = Exp();
                                if(tokenSet.ElementAt(0).classKeyword==")" && status)
                                {
                                    tokenSet.RemoveAt(0);
                                    if (tokenSet.ElementAt(0).classKeyword == "ter")
                                    {
                                        tokenSet.RemoveAt(0);
                                    }
                                }
                                else
                                {
                                    errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                                    status = false;
                                }
                            }
                            else
                            {
                                errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                                status = false;
                            }
                        }
                        else
                        {
                            errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                            status = false;
                        }
                    }
                    else
                    {
                        errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                    status = false;
                }
            }
            else
            {
                errorLine.Add(tokenSet.ElementAt(0).lineNumber);
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
                        if(tokenSet.ElementAt(0).classKeyword==")" && status)
                        {
                            tokenSet.RemoveAt(0);
                            if (First_N_Follow.FirstBody.Contains(tokenSet.ElementAt(0).classKeyword))
                            {
                                status = Body();
                            }
                        }
                        else
                        {
                            errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                            status = false;
                        }
                    }
                    else
                    {
                        errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                    status = false;
                }
            }
            else
            {
                errorLine.Add(tokenSet.ElementAt(0).lineNumber);
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
                    if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
                    {
                        status = Exp();
                        if(tokenSet.ElementAt(0).classKeyword=="ter" && status)
                        {
                            tokenSet.RemoveAt(0);
                            if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
                            {
                                status = Exp();
                                if(tokenSet.ElementAt(0).classKeyword=="ter" && status)
                                {
                                    tokenSet.RemoveAt(0);
                                    if (First_N_Follow.FirstCond3.Contains(tokenSet.ElementAt(0).classKeyword))
                                    {
                                        status = Cond3();
                                        if(tokenSet.ElementAt(0).classKeyword==")" && status)
                                        {
                                            tokenSet.RemoveAt(0);
                                            if (First_N_Follow.FirstBody.Contains(tokenSet.ElementAt(0).classKeyword))
                                            {
                                                status = Body();
                                            }
                                            else
                                            {
                                                errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                                                status = false;
                                            }
                                        }
                                        else
                                        {
                                            errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                                            status = false;
                                        }
                                    }
                                    else
                                    {
                                        errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                                        status = false;
                                    }
                                }
                                else
                                {
                                    errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                                    status = false;
                                }
                            }
                            else
                            {
                                errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                                status = false;
                            }
                        }
                        else
                        {
                            errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                            status = false;
                        }
                    }
                    else
                    {
                        errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                    status = false;
                }
            }
            else
            {
                errorLine.Add(tokenSet.ElementAt(0).lineNumber);
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
                            errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                            status = false;
                        }
                    }
                    else
                    {
                        errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                    status = false;
                }
            }
            else if (tokenSet.ElementAt(0).classKeyword == "new")
            {
                tokenSet.RemoveAt(0);
                if (tokenSet.ElementAt(0).classKeyword == "DT")
                {
                    tokenSet.RemoveAt(0);
                    if(tokenSet.ElementAt(0).classKeyword == "(")
                    {
                        tokenSet.RemoveAt(0);
                        if (First_N_Follow.FirstParams.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowParams.Contains(tokenSet.ElementAt(0).classKeyword))
                        {
                            status = Params();
                            if(tokenSet.ElementAt(0).classKeyword==")" && status)
                            {
                                tokenSet.RemoveAt(0);
                                if (tokenSet.ElementAt(0).classKeyword == "ter")
                                {
                                    tokenSet.RemoveAt(0);
                                }
                                else
                                {
                                    errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                                    status = false;
                                }
                            }
                            else
                            {
                                errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                                status = false;
                            }
                        }
                        else
                        {
                            errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                            status = false;
                        }
                    }
                    else {
                        errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                    status = false;
                }
            }
            else
            {
                errorLine.Add(tokenSet.ElementAt(0).lineNumber);
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
                if(First_N_Follow.FirstParams.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowParams.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Params();
                    if (!status)
                    {
                       errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                       status = false;
                    }
                }
                else
                {
                    errorLine.Add(tokenSet.ElementAt(0).lineNumber);
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
                    errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                    status = false;
                }
            }
            return status;
        }
        bool Exp()
        {
            bool status = true;
            // yahan
            return status;
        }
        bool R1()
        {
            bool status = true;
            if (First_N_Follow.FirstExp.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                status = Exp();
                if(tokenSet.ElementAt(0).classKeyword=="ter" && status)
                {
                    tokenSet.RemoveAt(0);
                }
                else
                {
                    errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                    status = false;
                }
            }
            else if (tokenSet.ElementAt(0).classKeyword == "ter")
            {
                tokenSet.RemoveAt(0);
            }
            else
            {
                errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                status = false;
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
                    if(tokenSet.ElementAt(0).classKeyword=="return" && status)
                    {
                        tokenSet.RemoveAt(0);
                        if (First_N_Follow.FirstR1.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowR1.Contains(tokenSet.ElementAt(0).classKeyword))
                        {
                            status = R1();
                            if(tokenSet.ElementAt(0).classKeyword=="}" && status)
                            {
                                tokenSet.RemoveAt(0);
                            }
                            else
                            {
                                errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                                status = false;
                            }
                        }
                        else
                        {
                            errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                            status = false;
                        }
                    }
                    else
                    {
                        errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                        status = false;
                    }
                }
            }
            return status;
        }
        bool Ass()
        {
            bool status = true;
            if(tokenSet.ElementAt(0).classKeyword == "=")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstAss1.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Ass1();
                    if (!status)
                    {
                        errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                        status = false;
                    }
                }
            }
            else
            {
                errorLine.Add(tokenSet.ElementAt(0).lineNumber);
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
                    errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                    status = false;
                }
            }
            else if (tokenSet.ElementAt(0).classKeyword == "(")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstParams.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Params();
                    if(tokenSet.ElementAt(0).classKeyword==")" && status)
                    {
                        tokenSet.RemoveAt(0);
                        if (First_N_Follow.FirstFunction_body.Contains(tokenSet.ElementAt(0).classKeyword))
                        {
                            status = FunctionBody();
                            if (!status)
                            {
                                errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                                status = false;
                            }
                        }
                        else
                        {
                            errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                            status = false;
                        }
                    }
                    else
                    {
                        errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                        status = false;
                    }
                }
            }
            else
            {
                errorLine.Add(tokenSet.ElementAt(0).lineNumber);
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
                if(First_N_Follow.FirstParams.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowParams.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Params();
                    if(tokenSet.ElementAt(0).classKeyword=="]" && status)
                    {
                        tokenSet.RemoveAt(0);
                    }
                    else
                    {
                        errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                    status = false;
                }
            }
            else
            {
                errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                status = false;
            }
            return status;
        }
        bool Decl5()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "[")
            {
                tokenSet.RemoveAt(0);
                if(tokenSet.ElementAt(0).classKeyword == "int_const")
                {
                    tokenSet.RemoveAt(0);
                    if (tokenSet.ElementAt(0).classKeyword == "]")
                    {
                        tokenSet.RemoveAt(0);
                        if (tokenSet.ElementAt(0).classKeyword == "ID")
                        {
                            tokenSet.RemoveAt(0);
                            if(tokenSet.ElementAt(0).classKeyword == "=")
                            {
                                tokenSet.RemoveAt(0);
                                if (First_N_Follow.FirstArrayInit.Contains(tokenSet.ElementAt(0).classKeyword))
                                {
                                    status = ArrayInit();
                                    if(tokenSet.ElementAt(0).classKeyword=="ter" && status)
                                    {
                                        tokenSet.RemoveAt(0);
                                    }
                                    else
                                    {
                                        errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                                        status = false;
                                    }
                                }
                                else
                                {
                                    errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                                    status = false;
                                }
                            }
                            else
                            {
                                errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                                status = false;
                            }
                        }
                        else
                        {
                            errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                            status = false;
                        }
                    }
                    else
                    {
                        errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                    status = false;
                }
            }
            else
            {
                errorLine.Add(tokenSet.ElementAt(0).lineNumber);
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
                            errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                            status = false;
                        }
                    }
                    else
                    {
                        errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                        status = false;
                    }
                }
                else if (tokenSet.ElementAt(0).classKeyword == "[")
                {
                    status = Decl5();
                    if (!status)
                    {
                        errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                    status = false;
                }
            }
            else if (tokenSet.ElementAt(0).classKeyword == "abstract")
            {
                tokenSet.RemoveAt(0);
                if (tokenSet.ElementAt(0).classKeyword == "DT")
                {
                    tokenSet.RemoveAt(0);
                    if(tokenSet.ElementAt(0).classKeyword == "ID")
                    {
                        tokenSet.RemoveAt(0);
                        if (tokenSet.ElementAt(0).classKeyword == "(")
                        {
                            tokenSet.RemoveAt(0);
                            if (First_N_Follow.FirstParams.Contains(tokenSet.ElementAt(0).classKeyword))
                            {
                                status = Params();
                                if(tokenSet.ElementAt(0).classKeyword==")" && status)
                                {
                                    tokenSet.RemoveAt(0);
                                    if(First_N_Follow.FirstFunction_body.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowFunction_body.Contains(tokenSet.ElementAt(0).classKeyword))
                                    {
                                        status = FunctionBody();
                                        if (!status)
                                        {
                                            errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                                            status = false;
                                        }
                                    }
                                    else
                                    {
                                        errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                                        status = false;
                                    }
                                }
                                else
                                {
                                    errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                                    status = false;
                                }
                            }
                            else
                            {
                                errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                                status = false;
                            }
                        }
                        else
                        {
                            errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                            status = false;
                        }
                    }
                    else
                    {
                        errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                    status = false;
                }
            }
            else if (tokenSet.ElementAt(0).classKeyword == "ID")
            {
                tokenSet.RemoveAt(0);
                if (tokenSet.ElementAt(0).classKeyword == "ID")
                {
                    tokenSet.RemoveAt(0);
                    if (First_N_Follow.FirstAss.Contains(tokenSet.ElementAt(0).classKeyword)){
                        status = Ass();
                        if (!status)
                        {
                            errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                            status = false;
                        }
                    }
                    else
                    {
                        errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                        status = false;
                    }
                }
                else
                {
                    errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                    status = false;
                }
            }
            else
            {
                errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                status = false;
            }
            return status;
        }
        bool Decl_Init()
        {
            bool status = true;
            if(First_N_Follow.FirstAM.Contains(tokenSet.ElementAt(0).classKeyword) || First_N_Follow.FollowAM.Contains(tokenSet.ElementAt(0).classKeyword))
            {
                status = AM();
                if(First_N_Follow.FirstDecl.Contains(tokenSet.ElementAt(0).classKeyword) && status)
                {
                    status = Decl();
                }
                else
                {
                    errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                    status = false;
                }
            }
            else
            {
                errorLine.Add(tokenSet.ElementAt(0).lineNumber);
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
                        errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                }
            }
            return status;
        }
        bool Class_Body()
        {
            bool status = true;
            if(tokenSet.ElementAt(0).classKeyword == "{")
            {
                tokenSet.RemoveAt(0);
                if (First_N_Follow.FirstDecl_Init.Contains(tokenSet.ElementAt(0).classKeyword))
                {
                    status = Decl_Init();
                    if (status)
                    {
                        if (First_N_Follow.FirstClass_Body1.Contains(tokenSet.ElementAt(0).classKeyword))
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
                        errorLine.Add(tokenSet.ElementAt(0).lineNumber);
                    }
                }
                else
                {
                    status = false;
                }
            }
            else
            {
                status = false;
            }
            return status;
        }

  
        bool Extend()
        {
            bool status = true;
            if (tokenSet.ElementAt(0).classKeyword == "extend")
                tokenSet.RemoveAt(0);
            return status;
        }
        bool Abstract_Class()
        {
            bool status = true;
            if(tokenSet.ElementAt(0).classKeyword == "abstract")
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
