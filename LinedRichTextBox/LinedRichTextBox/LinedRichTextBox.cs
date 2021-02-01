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
        //Settings for the control
        private Font _Font = UserControl.DefaultFont;

        //Holds whether text has been changed
        private bool isChanged = false;
        //Holds wether softWrap is on or off set to rtbLinedBox.WordWrap value
        private static bool softWrap;
        //Holds wether autoBrackets is on or off default off
        private static bool autoBrackets = true;
        //Holds the rtbLinedBox indentation
        private static int indentation = 10;

        //Variables to calculate Softwrap
        //Holds the string line
        private string strLine;
        //Holds the width of the string
        private int stringWidth;
        //Holds the current LineNum for rtbLinedBox
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
            //Set lineNum variable to equal the rtbLinedBox line count
            intLineNum = rtbLinedBox.Lines.Count();

            //Check if keycode is Enter
            if (e.KeyCode == Keys.Enter)
            {
                IncreaseLineNum(intLineNum);
            }//End KeyCode.Enter

            //Check if keycode is Back
            if (e.KeyCode == Keys.Back)
            {
                DecreaseLineNum(intLineNum);
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
            try
            {
                //Auto complete brackets
                String s = e.KeyChar.ToString();
                int sel = rtbLinedBox.SelectionStart;
                //Check if the autoBrackets feature is enabled
                if (autoBrackets == true)
                {
                    //Switch statement to check brackets that are entered by user
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
                    }//End Switch statement
                }//End If statement
            }
            catch
            {

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
            //Check if isChanged is true
            if (isChanged)
            {
                //Call RedrawLines
                RedrawLines();
                //Call SynchScroll
                SyncScroll();
                //Set isChanged to false
                isChanged = false;
            }
            else
            {
                //Return
                return;
            }
        }
        #endregion

        #region MouseEvents
        //MouseClick Event for rtbLineNums
        //In progress to highlight row when line num is clicked
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
            get { return indentation; }
            set { rtbLinedBox.SelectionIndent = indentation = value; }
        }

        //rtbLineNums rich text box settings
        //rtbLineNums ForeColor
        public Color LineNumForeColor
        {
            get { return rtbLineNums.ForeColor; }
            set { rtbLineNums.ForeColor = value; }
        }
        //rtbLineNums BackColor
        public Color LineNumBackColor
        {
            get { return rtbLineNums.BackColor; }
            set { rtbLineNums.BackColor = value; }
        }
        //rtbLinedBox rich text box settings
        //ForeColor
        public Color MainForeColor
        {
            get { return rtbLinedBox.ForeColor; }
            set { rtbLinedBox.ForeColor = value; }
        }
        //BackColor
        public Color MainBackColor
        {
            get { return rtbLinedBox.BackColor; }
            set { rtbLinedBox.BackColor = value; }
        }
        //Gets wether lineNumers is visible or not
        public bool LineNumbers
        {
            get { return rtbLineNums.Visible; }
            set { rtbLineNums.Visible = value; }
        }
        //returns wether softwrap is on or off
        public bool SoftWrap
        {
            get { return softWrap; }
            set { softWrap = rtbLinedBox.WordWrap = value; }
        }
        //Gets wether auto brackets is on or off
        public bool AutoBrackets
        {
            get { return autoBrackets; }
            set { autoBrackets = value; }
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
            stringWidth = TextRenderer.MeasureText(strLine, rtbLinedBox.Font).Width;
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

        #region Increase / Decrease
        //Used to call the Adding method
        public void IncreaseLineNum(int num)
        {
            //Check if the selection start is at 0
            if (rtbLinedBox.SelectionStart == 0)
            {
                //Call GetStringWidth
                GetStringWidth(num);
                //If it does call Adding(num + 2)
                Adding(num + 2);
            }
            else
            {
                //Call GetStringWidth and give it the last line 
                //Which is num - 1
                GetStringWidth(num - 1);
                //Else give Adding(num+1)
                Adding(num + 1);
            }
        }//End IncreaseLineNum

        //Used to call Removing method
        public void DecreaseLineNum(int num)
        {
            //Check to see if there is text selected
            if (rtbLinedBox.SelectionLength > 0)
            {
                //If so redraw the lines
                isChanged = true;
                return;
            }
            else
            {
                //Check if selection start == 0
                if (rtbLinedBox.SelectionStart == 0)
                {
                    //return
                    return;
                }
                //Check if a line string is empty
                if (rtbLinedBox.Lines[num - 1].ToString() == "")
                {
                    //call the removing method
                    Removing(num);
                }
                else
                {
                    //return
                    return;
                }

            }
        }//End DecreaseLineNum
        #endregion

        #region Adding or Removing Line Numbers
        //Method to add lines to the string array rtbLineNums
        private void Adding(int num)
        {
            //Pre: Needs the variable num to be initialized.
            //Purpose: To Add line numbers or spaces depending whether softWrap is on or not
            //to the line numbers text box.
            //Pose: Add the line numbers or spaces in the rtbLineNums box.

            //Create a string array and set it to rtbLineNums.lines
            string[] lineArr = rtbLineNums.Lines;
            //turn the array into a list
            var lineColl = new List<string>(lineArr);
            //Check if softwrap is enabled
            if(softWrap == true)
            {
                //Adds spaces if the SoftWrap is true to the line numbers------------
                //Check that the string width is bigger or equal to the control width
                if (stringWidth + 8 > rtbLinedBox.Width)
                {
                    
                    //While integer StringWidth is greater or equal to the width of the richtextbox
                    while (stringWidth + 8 > rtbLinedBox.Width)
                    {
                        //Change the integer stringWidth to equal itself minus width of the rtbLinedBox
                        stringWidth = (stringWidth + Indentation) - rtbLinedBox.Width;
                        //add a new empty line to the list
                        lineColl.Add(" ");
                    }
                }
            }//End If(softwrap == true)
            //--------------------------------------------------------------------------------
            //add a new line to the list
            lineColl.Add((num).ToString());
            //convert back to an array
            lineArr = lineColl.ToArray();
            //set the array to the rtbLineNums.lines where the line numbers are stored
            rtbLineNums.Lines = lineArr;
            LoadDefaults();
        }//End Adding()

        //Method to remove lines from the string array rtbLineNums
        private void Removing(int num)
        {
            //Pre: Needs the variable num to be initialized.
            //Purpose: To Add line numbers or spaces depending whether softWrap is on or not
            //to the line numbers text box.
            //Pose: Add the line numbers or spaces in the rtbLineNums box.

            //Create a string array and set it to rtbLineNums.lines
            string[] lineArr = rtbLineNums.Lines;
            //turn the array into a list
            var lineColl = new List<string>(lineArr);
            //Check if softwrap is enabled
            if (softWrap == true)
            {
                for (int i = 0; i < lineArr.Count(); i++)
                {
                    lineColl.Remove(" ");
                }
            }//End If(softwrap == true)
            //--------------------------------------------------------------------------------
            //add a new line to the list
            lineColl.Remove((num).ToString());
            //convert back to an array
            lineArr = lineColl.ToArray();
            //set the array to the rtbLineNums.lines where the line numbers are stored
            rtbLineNums.Lines = lineArr;
            LoadDefaults();
        }//End Removing()
        #endregion

        //Use this to redraw the rtbLineNum box using the rtbLinedBox line count
        public void RedrawLines()
        {
            //Pre: Does not need any incoming variables.
            //Purpose: To redraw the lines in the rtbLineNums box where the line numbers are stored.
            //Post: Redraw the lineNumbers on the rtbLineNums box according to the
            //Amount of lines in the rtbLinedBox and whether the softwrap is on.

            //Clear the rtbLineNums box
            rtbLineNums.Clear();
            //Set the text to have "1" 
            rtbLineNums.Text += 1;

            for (int i = 0; i < (rtbLinedBox.Lines.Count() - 1); i++)
            {
                //Get the string width
                GetStringWidth(i);
                //Call adding method
                Adding(i + 2);
            }
            LoadDefaults();
        }//End RedrawLines

        //Holds the default values for the Control
        public void LoadDefaults()
        {
            //Load Default Values
            rtbLinedBox.SelectionIndent = indentation;
            //Set the alignment of the rtbLineNums rich text box
            rtbLineNums.SelectAll();
            rtbLineNums.SelectionAlignment = HorizontalAlignment.Center;
        }
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
