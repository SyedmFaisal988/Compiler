using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LexicalAnalyzer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb.Text = "";
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = @"C:\Users\DevOps\Desktop\";
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Title = "Browse Source Files";
            openFileDialog1.DefaultExt = "txt";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.FileName = null;
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                fctb.Text = System.IO.File.ReadAllText(openFileDialog1.FileName);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fctb.Text == "")
            {
                fctb.Text = System.IO.File.ReadAllText(@"Compiler.txt");
            }
            index = 0;
            lineNumber = 1;
            wordNumber = 1;
            breakKeywords();
        }

        public int index = 0, wordNumber = 1, lineNumber = 1;
        string temp = "", testString;

        public void breakKeywords()
        {
            StaticComponents.tokenSet.Clear();
            temp = "";
            string source = fctb.Text;
            char c;
            bool stringFlag = false, commentFlag = false, charFlag = false;

            for (int i = 0; i < source.Length; i++)
            {
            start:;
                index = i;
                c = source.ElementAt(i);
                if (c == 10)
                {
                    lineNumber++;
                }
                if (c == 32 && !stringFlag)
                {
                    if (temp != "")
                    {
                        addTokenToList(temp);
                    }
                    continue;
                }
                if (c == 13 || c == 10)
                {
                    if (!commentFlag && !stringFlag)
                    {
                        if (temp != "")
                        {
                            addTokenToList(temp);
                        }
                        continue;
                    }
                }
                //comments
                if (!stringFlag)
                {
                    if (c == '/')
                    {
                        if (source.Length != (i + 1))
                        {
                            testString = c.ToString() + source.ElementAt(i + 1);
                            if (testString == "//")
                            {
                                if (temp != "")
                                {
                                    addTokenToList(temp);
                                }
                                commentFlag = true;
                                while (commentFlag)
                                {
                                    if (c == 10 || (i + 1) == source.Length)
                                    {
                                        commentFlag = false;
                                        i++;
                                        if (c == 10)
                                        {
                                            lineNumber++;
                                            goto start;
                                        }
                                        continue;
                                    }
                                    i++;
                                    c = source.ElementAt(i);
                                }
                            }
                            else if (testString == "/*")
                            {
                                if (temp != "")
                                {
                                    addTokenToList(temp);
                                }
                                commentFlag = true;
                                while (commentFlag)
                                {
                                    if ((i + 1) == source.Length)
                                    {
                                        commentFlag = false;
                                        continue;
                                    }
                                    testString = c.ToString() + source.ElementAt(i + 1);
                                    if (testString == "*/")
                                    {
                                        commentFlag = false;
                                        i = i + 2;
                                        if (i < source.Length)
                                        {
                                            c = source.ElementAt(i);
                                        }
                                        continue;
                                    }
                                    i++;
                                    c = source.ElementAt(i);
                                    if (c == 10)
                                    {
                                        lineNumber++;
                                    }
                                }
                            }
                        }
                    }
                }


                //temp is empty
                if (temp == "")
                {
                    //char is a limited punctuator ;,(){}
                    if (regexCheck(c, 3))
                    {
                        addTokenToList(c);
                    }
                    //compound check
                    else if (regexCheck(c, 6))
                    {
                        if (source.Length != (i + 1))
                        {
                            try
                            {
                                testString = c.ToString() + source.ElementAt(i + 1);
                                if (compoundCheck(testString))
                                {
                                    i++;
                                    addTokenToList(testString);
                                }
                                else
                                {
                                    //+- check
                                    if (regexCheck(c, 5))
                                    {
                                        temp += c;
                                    }
                                    else
                                    {
                                        addTokenToList(c);
                                    }
                                }
                            }
                            catch (Exception) { }

                        }
                        else if (c != 10 || c != 13)
                        {
                            temp += c;
                        }
                    }
                    //anything else
                    else
                    {
                        if (c == '"')
                        {
                            stringFlag = true;
                        }
                        else if (c == '\'')
                        {
                            charFlag = true;
                        }
                        temp += c;
                        if (charFlag)
                        {
                            int j = 2;
                            if ((i + 1) != source.Length)
                            {
                                if (source.ElementAt(i + 1) == '\\')
                                {
                                    j++;
                                }
                                while (j > 0)
                                {
                                    i++;
                                    temp += source.ElementAt(i);
                                    j--;
                                }
                            }
                            addTokenToList(temp);
                            charFlag = false;
                            continue;
                        }
                    }
                }
                //temp is not empty
                else
                {
                    //string
                    if (stringFlag)
                    {
                        if (c == 13)
                        {
                            stringFlag = false;
                            addTokenToList(temp);
                            continue;
                        }
                        temp += c;
                        if (c == '"')
                        {
                            if (source.ElementAt(i - 1) != '\\')
                            {
                                stringFlag = false;
                                addTokenToList(temp);
                            }
                        }
                    }
                    //char is an alphabet
                    else if (regexCheck(c, 1))
                    {
                        //if temp is a .
                        if (temp == "." || regexCheck(temp, 5))
                        {
                            addTokenToList(temp);
                        }
                        temp += c;
                    }
                    //char is a number
                    else if (regexCheck(c, 2))
                    {
                        temp += c;
                    }
                    //if char is a punctuator
                    else
                    {
                        if (c == '\'')
                        {
                            addTokenToList(temp);
                            goto start;
                        }
                        else
                            if (c == '.')
                        {
                            //if temp is only numbers
                            if (regexCheck(temp, 5))
                            {
                                temp += c;
                            }
                            //anything except numbers
                            else
                            {
                                addTokenToList(temp);
                                temp += c;
                            }
                        }
                        else if (c == '"')
                        {
                            //temp is a string
                            if (stringFlag)
                            {
                                temp += c;
                                if (source.ElementAt(i - 1) != '\\')
                                {
                                    stringFlag = false;
                                    addTokenToList(temp);
                                }
                            }
                            else
                            {
                                addTokenToList(temp);
                                temp += c;
                                stringFlag = true;
                            }
                        }
                        //limited punctuator
                        else if (regexCheck(c, 3))
                        {
                            addTokenToList(temp);
                            addTokenToList(c);
                        }
                        //compund op
                        else if (regexCheck(c, 6))
                        {
                            addTokenToList(temp);
                            if (source.Length != (i + 1))
                            {
                                testString = c.ToString() + source.ElementAt(i + 1);
                                if (compoundCheck(testString))
                                {
                                    i++;
                                    addTokenToList(testString);
                                }
                                else
                                {
                                    //+- check
                                    if (regexCheck(c, 5))
                                    {
                                        temp += c;
                                    }
                                    else
                                    {
                                        addTokenToList(c);
                                    }
                                }
                            }
                            else
                            {
                                temp += c;
                            }
                        }
                        else if (regexCheck(temp, 4))
                        {
                            addTokenToList(temp);
                            temp += c;
                        }
                        else
                        {
                            temp += c;
                        }
                    }
                }
            }
            if (temp != "")
            {
                addTokenToList(temp);
            }
            StartCompile();
            
        }
        private void StartCompile()
        {
            string hh = "Break Keywords\n\r";
            addTokenToList("$");
            foreach (Token t in StaticComponents.tokenSet)
                hh += t.ToString() + "\n";
           // MessageBox.Show(hh);
            Classifier classify = new Classifier();
            classify.classifier();
            //MessageBox.Show(classify.test());
            hh = "Classifier \n\r";
            foreach (Token t in StaticComponents.tokenSet)
                hh += t.ToString() + "\n";
            System.IO.StreamWriter sw = new System.IO.StreamWriter("Token.txt");
            foreach (Token t in StaticComponents.tokenSet)
                sw.WriteLine(t.ToString());
            sw.Close();
            MessageBox.Show(hh);
            ParseTree PT = new ParseTree();
            List<ParseError> ParseError = PT.Parse();
            //List<ParseError> ParseError = ParseErrorRaw.Distinct().ToList();
            string err = "";
            ParseError Previous = new ParseError(0, "", 0);
            foreach( ParseError error in ParseError)
            {
                if(Previous.ClassKeyword!=error.ClassKeyword && Previous.lineNumber != error.lineNumber)
                {
                    err += error.ToString() + "\n";
                    Previous = error;
                }
            }
            if(err == "")
            {
                err = "Code compiler with 0 error";
            }
            MessageBox.Show(err, "Errors");

        }
        public bool regexCheck(dynamic keyword, int type)
        {
            string regex = "";
            switch (type)
            {
                //char is alphabet
                case 1:
                    regex = @"^[a-zA-Z]$";
                    break;
                //string is numbers 0-9
                case 2:
                    regex = @"^[0-9]+$";
                    break;
                //limited punctuators
                case 3:
                    regex = @"^[;,(){}[\]]$";
                    break;
                //check string if float
                case 4:
                    regex = @"^[+-]?[0-9]*\.?[0-9]*$";
                    break;
                //+- at starting and then numbers
                case 5:
                    regex = @"^[+-]?[0-9]*$";
                    break;
                //compound ops <>!=+-*/%&|
                case 6:
                    regex = @"^[<>!=\-+*/%&|]$";
                    break;
                default:
                    break;
            }
            //MessageBox.Show(regexCheck(keyword.ToString(), regex).ToString() + "Character: " + keyword);
            //MessageBox.Show(regex);
            return regexCheck(keyword.ToString(), regex);
        }

        public bool regexCheck(dynamic keyword, string regex)
        {
            Match m = Regex.Match(keyword.ToString(), regex);
            if (m.Success)
            {
                return true;
            }
            else
                return false;
        }

        public void addTokenToList(dynamic value)
        {
            StaticComponents.tokenSet.Add(new Token("", value.ToString(), index, wordNumber, lineNumber));
            StaticComponents.tokenSet.Last().index -= StaticComponents.tokenSet.Last().value.Length;
            wordNumber++;
            temp = "";
        }

        public bool compoundCheck(string s)
        {
            return StaticComponents.compoundOperators.Any(x => x == s);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb.Copy();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb.Cut();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 116)
            {
                startToolStripMenuItem_Click(sender, e);
            }
}

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb.Paste();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb.Undo();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Lexical Analyzer - Compiler Construction\nMs. Maryam Feroze\n\nSyed M. Faisal\nMuhammad Asad\nMuhammad Danish");
        }
    }
}