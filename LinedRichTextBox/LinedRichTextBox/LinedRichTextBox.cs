using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

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

        //Variables to calculate Softwrap
        //Holds the string line
        //--private string strLine;
        //Holds the width of the string
        //--private int stringWidth;
        //Holds the current LineNum for rtbLinedBox
        private int carretPos;
        private int intLineNum;

        #endregion

        //Holds the IDERichTextBox Control to the LinedBox
        public readonly IDERTBControl rtbLinedBox = new IDERTBControl();

        #region Control Load
        public LinedRichTextBox()
        {
            InitializeComponent();
            //Load the defaults
            LoadDefaults();

            //---- RTB Events----
            rtbLinedBox.KeyDown += new KeyEventHandler(RtbLinedBox_KeyDown);
            rtbLinedBox.KeyPress += new KeyPressEventHandler(RtbLinedBox_KeyPress);
            rtbLinedBox.VScroll += new EventHandler(RtbLinedBox_VScroll);
            rtbLinedBox.TextChanged += new EventHandler(RtbLinedBox_TextChanged);

            //Add control
            Controls.Add(rtbLinedBox);

            //Bring control to front
            rtbLinedBox.BringToFront();
        }
        #endregion

        #region Events

        #region KeyDown
        //KeyDown Event for rtbLinedBox
        private void RtbLinedBox_KeyDown(object sender, KeyEventArgs e)
        {
            //Code may be useless----------------------------------------
            //Set lineNum variable to equal the rtbLinedBox line count
            intLineNum = rtbLinedBox.Lines.Count();

            switch (e.KeyCode)
            {
                case Keys.Enter:
                    //Increase Line number
                    IncreaseLineNum(intLineNum);
                    break;

                case Keys.Back:
                    //Decrease Line number
                    DecreaseLineNum(intLineNum);
                    
                    break;
            }

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
        private void RtbLinedBox_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        #endregion

        #region Scrolling
        //VScroll Event for rtbLinedBox
        private void RtbLinedBox_VScroll(object sender, EventArgs e)
        {
            //Call SyncScroll()
            SyncScroll();
        }
        #endregion

        #region TextChanged
        //TextChanged Event for rtbLinedBox
        private void RtbLinedBox_TextChanged(object sender, EventArgs e)
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

        //Testing to select a line in the rich text box
        #region MouseEvents
        //MouseClick Event for rtbLineNums
        //In progress to highlight row when line num is clicked
        private void RtbLineNums_MouseClick(object sender, MouseEventArgs e)
        {
            //int firstcharindex = rtbLineNums.GetFirstCharIndexOfCurrentLine();

            //int currentline = rtbLineNums.GetLineFromCharIndex(firstcharindex);

            //string currentlinetext = rtbLinedBox.Lines[currentline];

            //rtbLinedBox.Select(currentline, currentlinetext.Length);

            //HighlightLine(rtbLinedBox, currentline, selection);
        }

        //MouseEnter Event for rtbLineNums
        private void RtbLineNums_MouseEnter(object sender, EventArgs e)
        {
            rtbLineNums.Cursor = Cursors.Arrow;
        }
        #endregion

        #endregion

        #region Settings
        //FileExtension
        public string FileExt
        {
            get { return Properties.Settings.Default.FileExt; }
            set { Properties.Settings.Default.FileExt = value; }
        }
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
        
        //rtbLineNums rich text box settings
        //rtbLineNums ForeColor
        public Color LineNumForeColor
        {
            get { return rtbLineNums.ForeColor; }
            set { rtbLineNums.ForeColor = value; }
        }

        //rtbLinedBox rich text box settings
        //ForeColor
        public Color ControlForeColor
        {
            get { return rtbLinedBox.ForeColor; }
            set { rtbLinedBox.ForeColor = value;
                  rtbLinedBox.AutoCompleteBox.ForeColor = value;
                }
        }
        //BackColor
        public Color ControlBackColor
        {
            get { return rtbLinedBox.BackColor; }
            set { rtbLinedBox.BackColor = value;
                  rtbLineNums.BackColor = value;
                  rtbLinedBox.AutoCompleteBox.BackColor = value;
                }
        }
        
        //Booleans
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

        

        //RTB Auto-----
        //AutoComplete
        public bool AutoComplete
        {
            get { return Properties.Settings.Default.AutoComplete; }
            set { Properties.Settings.Default.AutoComplete = value; }
        }

        //AutoHTML
        public bool AutoHTML
        {
            get { return Properties.Settings.Default.AutoHTML; }
            set { Properties.Settings.Default.AutoHTML = value; }
        }

        //Gets wether auto brackets is on or off
        public bool AutoBrackets
        {
            get { return Properties.Settings.Default.AutoBrackets; }
            set { Properties.Settings.Default.AutoBrackets = value; }
        }

        #endregion

        #region Methods

        #region Get String and GetStringWidth
        //Returns a string at the given line number
        private string GetString(int lineNum)
        {
            //Create a variable to hold the string
            string returnString = "";

            //Try and get the most recent line in the rtbLinedBox
            try
            {
                //Set returnString to rtbLinedBox.Lines[lineCount]
                returnString = rtbLinedBox.Lines[lineNum];
                return returnString;//Return the string
            }
            catch
            {
                //Do nothing
                return returnString;//Return the empty string
            }
        }

        //Returns a given strings width
        private int GetStringWidth(int lineNum)
        {
            //Create a variable to hold the string and call the GetString method giving it the lineNum
            string strInfo = GetString(lineNum);
            //Set the string width (TextRenderer.MeasureText(Send it string line, rtbLinedBox.Font property).Get the Width)
            int strWidth = TextRenderer.MeasureText(strInfo, rtbLinedBox.Font).Width;

            return strWidth;//Return the string width
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
        public void IncreaseLineNum(int lineNum)
        {
            //Check if the selection start is at 0
            if (rtbLinedBox.SelectionStart == 0)
            {
                
                //If it does call Adding(int LineNum, int strWidth)
                Adding(lineNum + 2, GetStringWidth(lineNum));
            }
            else
            {
                //Else give Adding(int LineNum, int strWidth)
                Adding(lineNum + 1, GetStringWidth(lineNum - 1));
            }
        }//End IncreaseLineNum

        //Used to call Removing method
        public void DecreaseLineNum(int lineNum)
        {
            carretPos = rtbLinedBox.SelectionStart;
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
                if (rtbLinedBox.Lines[lineNum - 1].ToString() == "" || 
                    (rtbLinedBox.Lines[lineNum - 1].ToString() != "" && rtbLinedBox.Text[carretPos - 1] == '\n'))
                {
                    //call the removing method
                    Removing(lineNum);
                    
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
        private void Adding(int lineNum, int intStrWidth)
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
                if (intStrWidth + 8 > rtbLinedBox.Width)
                {
                    //While integer StringWidth is greater or equal to the width of the richtextbox
                    while (intStrWidth + 8 > rtbLinedBox.Width)
                    {
                        //Change the integer stringWidth to equal itself minus width of the rtbLinedBox
                        intStrWidth -= rtbLinedBox.Width;
                        //add a new empty line to the list
                        lineColl.Add(" ");
                    }
                }
            }//End If(softwrap == true)
            //--------------------------------------------------------------------------------
            //add a new line to the list
            lineColl.Add((lineNum).ToString());
            //convert back to an array
            lineArr = lineColl.ToArray();
            //set the array to the rtbLineNums.lines where the line numbers are stored
            rtbLineNums.Lines = lineArr;
            LoadDefaults();
        }//End Adding()

        //Method to remove lines from the string array rtbLineNums
        private void Removing(int lineNum)
        {
            //-----When removing a line in softwrap and there is a blank line before it, it does not remove correctly

            //Pre: Needs the variable num to be initialized.
            //Purpose: To Add line numbers or spaces depending whether softWrap is on or not
            //to the line numbers text box.
            //Pose: Add the line numbers or spaces in the rtbLineNums box.

            //Create a string array and set it to rtbLineNums.lines
            string[] lineArr = rtbLineNums.Lines;

            //turn the array into a list
            var lineColl = new List<string>(lineArr);

            //remove lineNum from the list
            lineColl.Remove((lineNum).ToString());

            //Check if softwrap is enabled
            if (softWrap == true)
            {
                //This may not be elegant but it works to remove leading whitespace after a number
                //This will remove all trailing whitespace after the previous num and leave the leading whitespace alone
                //Loop to go through the list starting at the bottom
                for(int i = lineColl.Count - 1; i > 0; i--)
                {
                    //Check if the element contains a space
                    if(lineColl.ElementAt(i).Contains(" "))
                    {
                        //If it does remove it
                        lineColl.RemoveAt(i);
                    }
                    else
                    {
                        break;//Exit the loop
                    }
                }
            }//End If(softwrap == true)
            //--------------------------------------------------------------------------------
           
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
               //Call adding method
                Adding(i + 2, GetStringWidth(i));
            }
            LoadDefaults();
        }//End RedrawLines

        //Holds the default values for the Control
        public void LoadDefaults()
        {
            //Load Default Values
            //-----Variable Properties-----
            softWrap = rtbLinedBox.WordWrap;//set the softWrap to the rtbLinedBox.WordWrap property

            //-----RTBLineNums Properties-----
            LineNumForeColor = Color.LightBlue;//Set the color of the text in the LineNumForeColor setting
            rtbLineNums.SelectionAlignment = HorizontalAlignment.Center;//Set the alignment of the rtbLineNums rich text box

            //-----RTB Properties-----
            rtbLinedBox.BackColor = rtbLineNums.BackColor;
            rtbLinedBox.ForeColor = Color.White;
            rtbLinedBox.Dock = DockStyle.Fill;
            rtbLinedBox.BorderStyle = BorderStyle.None;

            //-----RTBListBox-----
            rtbLinedBox.AutoCompleteBox.ForeColor = rtbLinedBox.ForeColor;
            rtbLinedBox.AutoCompleteBox.BackColor = rtbLinedBox.BackColor;

            //Set the alignment of the rtbLineNums rich text box
            rtbLineNums.SelectAll();
            rtbLineNums.SelectionAlignment = HorizontalAlignment.Center;
        }
        #endregion

        #region Drag And Drop Public method


        //FIXME ADD File handling

        public int GetLineCount()
        {
            int count;
            rtbLinedBox.Refresh();
            count = rtbLinedBox.Lines.Count();

            return count;
        }

        #endregion

        #endregion

    }
}
