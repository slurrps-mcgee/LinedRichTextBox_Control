using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LinedRichTextBox
{
    public partial class LinedRichTextBox : UserControl
    {
        #region Variables
        //Settings for the control----
        private Font _Font = UserControl.DefaultFont;
        //-----

        //Holds the current lineNumber
        private int intCurrLineNum;
        //Holds whether text has been changed
        private bool isChanged = false;
        //Holds wether softWrap is on or off
        private static bool softWrap;

        //Variables to calculate Softwrap
        //Holds the string line
        private string strLine;
        //Holds the width of the string
        private int stringWidth;
        //Holds the lineCount for rtbLinedBox
        private int intLineNum;

        #endregion

        public LinedRichTextBox()
        {
            InitializeComponent();
            //Default Values on initialization
            rtbLinedBox.SelectionIndent = 10;//Set the rtbLinedBox indentation
            LineNumForeColor = Color.Blue;//Set the color of the text in the LineNumForeColor setting
            rtbLineNums.SelectionAlignment = HorizontalAlignment.Center;//Set the alignment of the rtbLineNums rich text box
            softWrap = rtbLinedBox.WordWrap;//set the softWrap to the rtbLinedBox.WordWrap property
        }

        #region Events

        #region KeyDown
        //KeyDown Event for rtbLinedBox
        private void rtbLinedBox_KeyDown(object sender, KeyEventArgs e)
        {
            //Code may be useless----------------------------------------
            //Set lineNum variable to equal the rtbLinedBox line count + 1
            intCurrLineNum = rtbLinedBox.Lines.Count() + 1;

            //Check if keycode is Enter
            if (e.KeyCode == Keys.Enter)
            {
                isChanged = true;
            }//End KeyCode.Enter

            //Check if keycode is Back
            if (e.KeyCode == Keys.Back)
            {
                isChanged = true;
                rtbLinedBox.ScrollToCaret();
            }//End KeyCode.Back

            //Check if keycode is combo V + Control
            if (e.KeyCode == Keys.V && e.Control)
            {
                isChanged = true;
            }//End Ctrl + V

            //Check if keycode is Delete
            if (e.KeyCode == Keys.Delete || (e.KeyCode == Keys.X && e.Control))
            {
                isChanged = true;
            }//End Delete

            //Check if keycode is combo Z + Control
            if (e.KeyCode == Keys.Z && e.Control)
            {
                isChanged = true;
            }//End Ctrl + Z
        }
        #endregion

        #region KeyPress
        //Coding Options down below matching symbols generic library
        private void rtbLinedBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Auto complete brackets
            String s = e.KeyChar.ToString();
            int sel = rtbLinedBox.SelectionStart;

            switch (s)
            {
                case "(":
                    rtbLinedBox.Text = rtbLinedBox.Text.Insert(sel, "()");
                    e.Handled = true;
                    rtbLinedBox.SelectionStart = sel + 1;
                    break;

                case "{":
                    String t = "{}";
                    rtbLinedBox.Text = rtbLinedBox.Text.Insert(sel, t);
                    e.Handled = true;
                    rtbLinedBox.SelectionStart = sel + t.Length - 1;
                    break;

                case "[":
                    rtbLinedBox.Text = rtbLinedBox.Text.Insert(sel, "[]");
                    e.Handled = true;
                    rtbLinedBox.SelectionStart = sel + 1;
                    break;

                case "\"":
                    rtbLinedBox.Text = rtbLinedBox.Text.Insert(sel, "\"\"");
                    e.Handled = true;
                    rtbLinedBox.SelectionStart = sel + 1;
                    break;

                case "'":
                    rtbLinedBox.Text = rtbLinedBox.Text.Insert(sel, "''");
                    e.Handled = true;
                    rtbLinedBox.SelectionStart = sel + 1;
                    break;

                case "<":
                    rtbLinedBox.Text = rtbLinedBox.Text.Insert(sel, "<>");
                    e.Handled = true;
                    rtbLinedBox.SelectionStart = sel + 1;
                    break;
            }
        }
        #endregion

        #region Scrolling
        //VScroll Event for rtbLinedBox
        private void rtbLinedBox_VScroll(object sender, EventArgs e)
        {
            //Call SyncScroll()
            SyncScroll();
        }
        #endregion

        #region TextChanged
        //TextChanged Event for rtbLinedBox
        private void rtbLinedBox_TextChanged(object sender, EventArgs e)
        {
            //RedrawLines();
            //********************MAY NOT NEED******************************
            if (isChanged == true)
            {
                RedrawLines();
                SyncScroll();
                isChanged = false;
            }
            else
            {
                return;
            }
        }
        #endregion

        #region MouseEvents
        //MouseClick Event for rtbLineNums
        private void rtbLineNums_MouseClick(object sender, MouseEventArgs e)
        {
            //int firstcharindex = rtbLineNums.GetFirstCharIndexOfCurrentLine();

            //int currentline = rtbLineNums.GetLineFromCharIndex(firstcharindex);

            //string currentlinetext = rtbLinedBox.Lines[currentline];

            //rtbLinedBox.Select(currentline, currentlinetext.Length);

            //HighlightLine(rtbLinedBox, currentline, selection);
        }

        //MouseEnter Event for rtbLineNums
        private void rtbLineNums_MouseEnter(object sender, EventArgs e)
        {
            rtbLineNums.Cursor = Cursors.Arrow;
        }
        #endregion

        #endregion

        #region Settings
        //Font settings for both rich text boxes
        public override Font Font
        {
            get { return _Font; }
            set
            {
                _Font = rtbLinedBox.Font = value;
                _Font = rtbLineNums.Font = value;
            }
        }
        //Indentation on the Main rich text box rtbLinedBox
        public int Indentation
        {
            get { return rtbLinedBox.SelectionIndent; }
            set { rtbLinedBox.SelectionIndent = value; }
        }

        //rtbLineNums rich text box settings
        public Color LineNumForeColor
        {
            get { return rtbLineNums.ForeColor; }
            set { rtbLineNums.ForeColor = value; }
        }

        public Color LineNumBackColor
        {
            get { return rtbLineNums.BackColor; }
            set { rtbLineNums.BackColor = value; }
        }

        //rtbLinedBox rich text box settings
        public Color MainForeColor
        {
            get { return rtbLinedBox.ForeColor; }
            set { rtbLinedBox.ForeColor = value; }
        }

        public Color MainBackColor
        {
            get { return rtbLinedBox.BackColor; }
            set { rtbLinedBox.BackColor = value; }
        }

        public bool LineNumbers
        {
            get { return rtbLineNums.Visible; }
            set { rtbLineNums.Visible = value; }
        }

        public bool SoftWrap
        {
            get { return softWrap; }
            set { softWrap = rtbLinedBox.WordWrap = value; }
        }

        #endregion

        #region Methods

        #region Get String and GetStringWidth
        private void GetString(int num)
        {
            //Get the most recent line
            intLineNum = (num);

            //Try and get the most recent line in the rtbLinedBox
            try
            {
                //Set line to rtbLinedBox.Lines[lineCount]
                strLine = rtbLinedBox.Lines[intLineNum];
            }
            catch
            {
                //Do nothing
                return;
            }
        }

        private void GetStringWidth(int num)
        {
            //Call the GetString function and send it the num integer
            GetString(num);
            //Set the string width (TextRenderer.MeasureText(Send it string line, rtbLinedBox.Font property).Get the Width)
            stringWidth = (TextRenderer.MeasureText(strLine, rtbLinedBox.Font).Width);
        }
        #endregion

        #region Sync Scroll
        //Link the scrolling of the two rich text boxes
        //Sync
        //Create a constant in called WM_USER and set it to 0x400
        const int WM_USER = 0x400;
        //create a contant int called EM_GETSCROLLPOS and set it to WM_USER + 221
        const int EM_GETSCROLLPOS = WM_USER + 221;
        //create a contant int called EM_GETSCROLLPOS and set it to WM_USER + 222
        const int EM_SETSCROLLPOS = WM_USER + 222;
        //Import the dll user32.dll
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        //Create a static sendMessage reference from the dll
        static extern int SendMessage(IntPtr hWnd, int msg, int wParam, ref Point lParam);

        //Used to sync the scrolling of the two richtextbox's
        private void SyncScroll()
        {
            //Set a new point
            Point pt = new Point();
            //send message to the rtbLinedBox using the new point
            SendMessage(rtbLinedBox.Handle, EM_GETSCROLLPOS, 0, ref pt);
            //send message to the rtbLineNums using the new point
            SendMessage(rtbLineNums.Handle, EM_SETSCROLLPOS, 0, ref pt);
        }
        #endregion

        #region Add and Remove Lines
        
        //Method to add lines 
        private void Adding(int num)
        {
            //Pre: Needs the variable num to be initialized.
            //Purpose: To Add line numbers or spaces depending whether softWrap is on or not.
            //Pose: Add the line numbers or spaces in the rtbLineNums box.

            //grab string array from the rtbLineNums.lines
            string[] lineArr = rtbLineNums.Lines;
            //turn the array into a list
            var lineColl = new List<string>(lineArr);
            //Adds spaces if the SoftWrap is true to the line numbers-------------------------
            if(softWrap == true)
            {
                //Check that the string width is bigger or equal to the control width
                if (stringWidth >= rtbLinedBox.Width)
                {
                    //While integer StringWidth is greater or equal to the width of the richtextbox
                    while (stringWidth >= rtbLinedBox.Width || stringWidth > (rtbLinedBox.Width - Indentation))
                    {
                        //Change the integer stringWidth to equal itself minus width of the rtbLinedBox
                        stringWidth = (stringWidth + Indentation) - rtbLinedBox.Width;
                        //add a new empty line to the list
                        lineColl.Add(" ");
                    }
                }
            }
            //--------------------------------------------------------------------------------
            //add a new line to the list
            lineColl.Add((num).ToString());
            //convert back to an array
            lineArr = lineColl.ToArray();
            //set the array to the rtbLineNums.lines
            rtbLineNums.Lines = lineArr;
        }

        //Use this to redraw the rtbLineNum box using the rtbLinedBox line count
        public void RedrawLines()
        {
            //Pre: Does not need any incoming variables.
            //Purpose: To redraw the lines in the rtbLineNums box.
            //Post: Redraw the lineNumbers on the rtbLineNums box according to the
            //Amount of lines in the rtbLinedBox and whether the softwrap is on.

            if (softWrap == true)
            {
                rtbLineNums.Clear();
                rtbLineNums.Text += 1;

                for (int i = 0; i < (rtbLinedBox.Lines.Count() - 1); i++)
                {
                    GetStringWidth(i);

                    Adding(i + 2);
                }
            }
            else
            {
                rtbLineNums.Clear();
                rtbLineNums.Text += 1;

                for (int i = 0; i < (rtbLinedBox.Lines.Count() - 1); i++)
                {
                    Adding(i + 2);
                }
            }
            rtbLineNums.SelectAll();
            rtbLineNums.SelectionAlignment = HorizontalAlignment.Center;
        }//End RedrawLines

        #endregion

        #region Drag And Drop Public method


        //FIXME ADD File handling

        public int GetLineCount()
        {
            int count = 0;
            rtbLinedBox.Refresh();
            count = rtbLinedBox.Lines.Count();

            return count;
        }
        #endregion

        #endregion

        
    }
}
