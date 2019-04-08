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
        bool Decl_Init()
        {
            bool status = true;
            //yahan say shuro karna ha
            return false;
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
