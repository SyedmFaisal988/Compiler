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
            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Title = "Browse Source Files";
            openFileDialog1.DefaultExt = "txt";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.FileName = null;
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) {
                fctb.Text = System.IO.File.ReadAllText(openFileDialog1.FileName);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            breakKeywords();
        }

        public int index = 0, wordNumber = 1, lineNumber = 1;
        public void breakKeywords()
        {
            StaticComponents.tokenSet.Clear();
            string source = fctb.Text, temp = "";
            foreach (char c in source)
            {
                if (c == 32){
                    if (temp != ""){
                        //temp not starting with "
                        if (!regexCheck(temp, 6))
                        {
                            addTokenToList(temp);
                            temp = "";
                        }
                        else
                        {
                            temp += c;
                        }
                    }
                }
                else if (c == 13)
                {
                    if (temp != "")
                    {
                        addTokenToList(temp);
                        temp = "";
                    }
                    wordNumber = 1;
                }
                else if (c==10)
                {
                    lineNumber++;
                    index = 0;
                    wordNumber = 1;
                }
                // c alphabet
                else if (regexCheck(c,8))
                {
                    if (temp == "")
                        temp += c;
                    // temp .
                    else if (regexCheck(temp, 5))
                    {
                        addTokenToList(temp);
                        temp = c.ToString();
                    }
                    //temp alphanumeric or .numalpha
                    else if (regexCheck(temp, 1) || regexCheck(temp, 7))
                    {
                        if (temp.Last() == '.')
                        {
                            addTokenToList(temp);
                            temp = c.ToString();
                        }
                        else
                            temp += c;
                    }
                    //temp starts with "
                    else if (regexCheck(temp, 6))
                    {
                        temp += c;
                    }
                }
                //c numeric
                else if (regexCheck(c, 2))
                {
                    //temp alphanumeric or .numalpha
                    if (regexCheck(temp, 1) || regexCheck(temp, 7))
                    {
                        temp += c;
                    }
                    //temp starts with "
                    else if (regexCheck(temp, 6))
                    {
                        temp += c;
                    }
                }
                //c punctuator
                else if (regexCheck(c,3))
                {
                    //temp not starting with "
                    if (!regexCheck(temp, 6))
                    {
                        //c is a .
                        if (c == '.')
                        {
                            //temp is numeric
                            if (regexCheck(temp, 2))
                            {
                                temp += c;
                            }
                            //temp is alphanumeric or .numalpha
                            else if (regexCheck(temp, 1) || regexCheck(temp, 7))
                            {
                                addTokenToList(temp);
                                temp = c.ToString();
                            }
                        }
                        else if (c == '"')
                        {
                            if(temp!="")
                                addTokenToList(temp);
                            temp = "";
                            temp += c;
                        }
                        else if (temp == "")
                        {
                            addTokenToList(c.ToString());
                        }
                        else
                        {
                            addTokenToList(temp);
                            addTokenToList(c.ToString());
                            temp = "";
                        }
                    }
                    //c is a "
                    else if (c == '"')
                    {
                        //check if temp is not empty
                        if (temp != "")
                        {
                            if (temp.Last() != '\\')
                            {
                                temp += c;
                                addTokenToList(temp);
                                temp = "";
                            }
                            else
                            {
                                temp += c;
                            }
                        }
                    }
                    else
                    {
                        temp += c;
                    }
                }
                index++;
            }
            addTokenToList(temp);
            string hh = "";
            foreach(Token t in StaticComponents.tokenSet)
                hh+=t.ToString()+"\n";
            MessageBox.Show(hh);
            Classifier classify = new Classifier();
            classify.classifier();
            //MessageBox.Show(classify.test());
            hh = "";
            foreach (Token t in StaticComponents.tokenSet)
                hh += t.ToString() + "\n";
            MessageBox.Show(hh);
        }
        public bool regexCheck(dynamic keyword, int type)
        {
            string regex = "";
            switch (type)
            {
                //check if alphanumeric or _
                case 1:
                    regex = @"^[a-zA-Z0-9_]*$";
                    break;
                //string check if only numric
                case 2:
                    regex = @"^[0-9]*$";
                    break;
                //char check if punctuator
                case 3:
                    regex = @"[^a-zA-Z0-9_]";
                    break;
                //check string if numeric decimal
                case 4:
                    regex = @"^[0-9]*[.]{1}[0-9]*$";
                    break;
                //check if string = '.'
                case 5:
                    regex = @"^[.]{1}$";
                    break;
                //check string start with " can contain multiple \"
                case 6:
                    regex = "^\"[^\"]*(\\\\\")*[^\"]*$";
                    break;
                //check string is .numalpha
                case 7:
                    regex = @"^[0-9]*[.]?[a-zA-Z0-9_]*$";
                    break;
                // check string if only alphabets
                case 8:
                    regex = @"^[a-zA-Z]*$";
                    break;
                default:
                    break;
            }
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
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb.Copy();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb.Cut();
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