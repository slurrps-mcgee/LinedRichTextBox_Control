//Author: Pritam Zope
//https://www.codeproject.com/Articles/1097171/Creating-AutoComplete-HTML-Tags-in-Csharp

using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LinedRichTextBox
{

    public class IDERTBControl : RichTextBox
    {
        //declare ListBox object AutoCompleteBox
        public ListBox AutoCompleteBox = new ListBox();

        //****************************
        //Holds the Controls Variables
        #region Variables
        //Holds whether the listbox is added
        public static bool isAutoCompleteBoxAdded = false;

        //Holds the htmlstring to be checked to auto close html brackets
        public static string htmlString = "";
        //Holds the autostring to be checked on the list box
        public static string autoString = "";

        //Holds whether < is pressed
        public static bool isLessThanKeyPressed = false;
        //Holds whether > is pressed
        public static bool isGreaterThanKeyPressed = false;
        //Hold whether a bracket was pressed
        public static bool isAutoBracket = false;


        public bool Is_TagClosedKeyPressed = false;

        //Settings-----
        // declare backcolor & forecolor variables
        public static Color backcolor = SystemColors.Window;
        public static Color forecolor = Color.Black;

        #endregion
        //****************************

        //*********************** 
        //RichTextBox Constructor
        #region Constructor
        public IDERTBControl()
        {
            //Properties for ListBox
            AutoCompleteBox.BorderStyle = BorderStyle.FixedSingle;

            //Properties for RTB control
            AcceptsTab = true;
            WordWrap = false;

            ////Dont know if I will use
            AutoCompleteBox.KeyDown += new KeyEventHandler(AutoCompleteBox_KeyDown);
            //AutoCompleteBox.KeyPress += new KeyPressEventHandler(AutoCompleteBox_KeyPress);
            AutoCompleteBox.MouseClick += new MouseEventHandler(AutoCompleteBox_MouseClick);
        }
        #endregion
        //***********************

        //Do Not Touch is working correctly!!!
        //***********************************************************
        //Handles the Position and Sizing of the AutoComplete ListBox
        #region Position and Size
        public Point GetXYPoints()
        {
            //get current caret position point from RTB
            Point pt = GetPositionFromCharIndex(SelectionStart);
            // increase the Y co-ordinate size by 10 & Font size of RTB
            pt.Y = pt.Y + (int)Font.Size + 10;

            //  check Y co-ordinate value is greater than RTB Height - AutoCompleteBox
            //   for add AutoCompleteBox at the Bottom of RTB
            if (pt.Y > Height - AutoCompleteBox.Height)
            {
                //Set Y co-ordinate to Y - ListBox Height - (int) FontSize - 10
                pt.Y = pt.Y - AutoCompleteBox.Height - (int)Font.Size - 10;
            }
            //Return point pt
            return pt;
        }


        public int GetWidth()
        {
            //Create int variable width set it to 100.
            int width = 100;

            //For each loop to go through each item in the list box
            foreach (string item in AutoCompleteBox.Items)
            {
                //if statements to check the item length
                if (item.Length <= 5)
                {
                    //Set width
                    width = 160;
                }
                else if (item.Length <= 10)
                {
                    //Set width
                    width = 200;
                }
                else if (item.Length <= 20)
                {
                    //Set width
                    width += item.Length * 2;
                }
                else
                {
                    //Set width
                    width += item.Length * 4;
                }
            }
            //Return width
            return width;
        }


        public int GetHeight()
        {
            //Create int variable height set it to 10.
            int height = 10;

            //  get Font size of RTB
            int fontsize = (int)Font.Size;

            //  get number of items added to AutoCompleteBox
            int count = AutoCompleteBox.Items.Count;


            //Increase the height of AutoCompleteBox if added items count is 0,1,2,3,4,5
            switch (count)
            {
                case 0:
                    height = fontsize;
                    break;
                case 1:
                    height += 10 + fontsize;
                    break;
                case 2:
                    height += 20 + fontsize;
                    break;
                case 3:
                    height += 30 + fontsize;
                    break;
                case 4:
                    height += 40 + fontsize;
                    break;
                case 5:
                    height += 50 + fontsize;
                    break;
                case 6:
                    height += 60 + fontsize;
                    break;
                case 7:
                    height += 70 + fontsize;
                    break;
                case 8:
                    height += 80 + fontsize;
                    break;
                case 9:
                    height += 90 + fontsize;
                    break;
                case 10:
                    height += 100 + fontsize;
                    break;
                case 11:
                    height += 110 + fontsize;
                    break;
                case 12:
                    height += 120 + fontsize;
                    break;
                case 13:
                    height += 130 + fontsize;
                    break;
                case 14:
                    height += 140 + fontsize;
                    break;
                case 15:
                    height += 150 + fontsize;
                    break;
                default:
                    height += 200 + fontsize;
                    break;

            }

            return height;
        }
        #endregion 
        //***********************************************************

        //*******************************************
        //Handles the Auto Completion of all brackets
        #region AutoBrackets
        public void AutoBrackets(String s)
        {
            int sel = SelectionStart;
            switch (s)
            {
                case "(":
                    Text = Text.Insert(sel, ")");
                    SelectionStart = sel;
                    ScrollToCaret();
                    isAutoBracket = true;
                    break;

                case "[":
                    Text = Text.Insert(sel, "]");
                    SelectionStart = sel;
                    ScrollToCaret();
                    isAutoBracket = true;
                    break;

                case "{":
                    Text = Text.Insert(sel, "}");
                    SelectionStart = sel;
                    ScrollToCaret();
                    isAutoBracket = true;
                    break;

                case "\"":
                    isAutoBracket = true;
                    Text = Text.Insert(sel, "\"");
                    ScrollToCaret();
                    SelectionStart = sel;
                    break;

                case "'":
                    Text = Text.Insert(sel, "'");
                    SelectionStart = sel;
                    ScrollToCaret();
                    isAutoBracket = true;
                    break;
            }
        }
        #endregion
        //*******************************************

        //Would like to edit this to include CSS and a few other languages but will keep simply for now
        //to test
        //******************************
        //Handles Auto Closing HTML Tags
        #region AutoHTML
        public void AutoHTML(String ch)
        {
            //Check if the incoming character is <
            if (ch == "<")
            {
                //Update Modifiers
                isLessThanKeyPressed = true;
                //Set htmlString to empty
                htmlString = "";
            }
            //Check if the incoming character is >
            else if (ch == ">")
            {
                //Update modifier
                isGreaterThanKeyPressed = true;
                //Get selectionStart
                int sel = SelectionStart;

                //foreach loop to go through the tagslist
                foreach(string tag in HTMLTagList.tagsList)
                {
                    //Check if the htmlString == the tag
                    if(htmlString == tag)
                    {
                        //Insert the closing tag
                        Text = Text.Insert(sel, "</" + tag + ">");
                        //Set selectionStart
                        SelectionStart += sel;
                        //Clear the htmlString
                        htmlString = "";
                    }
                }
                //Remove the list box control
                Controls.Remove(AutoCompleteBox);
                //Update modifiers
                isAutoCompleteBoxAdded = false;
                isLessThanKeyPressed = false;
            }
            //Else if closing bracket is not pressed >
            else
            {
                //Check if the opening bracket is pressed <
                if (isLessThanKeyPressed)
                {
                    //Remove any character that is not a through z
                    ch = Regex.Replace(ch, @"[^\w\.@-]", "");
                    //set htmlString to itself + the character
                    htmlString += ch;
                }
            }
        }
        #endregion
        //******************************

        //To Do: Edit the Algorithm to add the items to the list box.
        //****************************************
        //Handles filling the AutoComplete ListBox
        #region AutoComplete Items
        public void AutoComplete()
        {

            //ToDo:
            //TESTING!!!-----
            //add each item to AutoCompleteBox
            foreach (String item in HTMLTagList.keywordsList)
            {
                //check item is starts with EnteredKey or not
                if (item.ToUpper().StartsWith(autoString.ToUpper()))
                {
                    AutoCompleteBox.Items.Add(item);
                }
            }
            
            //Foreach loop will go in here once fully tested
            //Check if the file extension is .html
            if (Properties.Settings.Default.FileExt == ".html")
            {
                
            }
            
            //Check that the autoCompleteBox items is not empty
            if(AutoCompleteBox.Items.Count > 0)
            {
                //  set Default cursor to AutoCompleteBox
                AutoCompleteBox.Cursor = Cursors.Default;

                //  set Size to AutoCompleteBox
                // width=this.getWidth() & height=this.getHeight()+(int)this.Font.Size
                AutoCompleteBox.Size = new Size(GetWidth(), GetHeight() + (int)Font.Size);

                //  set Location to AutoCompleteBox by calling getXYPoints() function
                AutoCompleteBox.Location = GetXYPoints();

                //adding controls of AutoCompleteBox to RTB
                Controls.Add(AutoCompleteBox);

                //  set isAutoCompleteBoxAdded to true
                isAutoCompleteBoxAdded = true;

                //  read each item from AutoCompleteBox to set SelectedItem
                foreach (String item in HTMLTagList.keywordsList)
                {
                    if (item.ToUpper().StartsWith(autoString.ToUpper()))
                    {
                        AutoCompleteBox.SelectedItem = item;

                        break;
                    }
                    else
                    {
                        AutoCompleteBox.SelectedIndex = -1;
                    }
                }
            }
            else
            {
                isAutoCompleteBoxAdded = false;
            }

            
        }
        #endregion
        //****************************************

        //******************************
        //Handles the RichTextBox Events
        #region RTB Events

        //*******************************
        //Handles TextChanged for the RTB
        #region TextChanged
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            
            //Check if the RTB text is empty
            if (!isAutoCompleteBoxAdded)
            {
                //Check if the listbox is added
                if (Text == "")
                {
                    //remove the control
                    Controls.Remove(AutoCompleteBox);
                    //set bool isAutoCompleteBoxAdded to false
                    isAutoCompleteBoxAdded = false;
                }
                //set autoString to empty
                //autoString = "";
            }
        }
        #endregion
        //*******************************

        //*************************
        //Handles KeyUp for the RTB
        #region KeyUP
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            switch (e.KeyCode)
            {
                case Keys.Back:
                    if(autoString == "")
                    {
                        Controls.Remove(AutoCompleteBox);
                        isAutoCompleteBoxAdded = false;
                        autoString = "";
                    }
                    break;
            }
        }
        #endregion
        //*************************

        //***************************
        //Handles KeyDown for the RTB
        #region KeyDown
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            int sel; //Variable to hold the selection start
            string text;//Variable to hold the string

            switch (e.KeyCode)
            {
                //Is outputting a random line sometimes
                #region Space Key
                // if Space key is down then remove AutoCompleteBox control from RTB
                case Keys.Space:
                    //Check if the AutoCompleteBox has been added
                    if (isAutoCompleteBoxAdded)
                    {
                        //Get selectionstart
                        sel = SelectionStart;

                        //Check if AutoCompleteBox is not empty
                        if (AutoCompleteBox.Items.Count != 0)
                        {
                            if(AutoCompleteBox.SelectedIndex != -1)
                            {
                                //Remove the autoString from the RTB
                                Text = Text.Remove(sel - autoString.Length, autoString.Length);
                                //Set text to the selected item in the list
                                text = AutoCompleteBox.SelectedItem.ToString();
                                //Set htmlString to the selected item in the list
                                htmlString = AutoCompleteBox.SelectedItem.ToString();

                                //Insert text to the RTB starting at selectionstart
                                Text = Text.Insert(sel - autoString.Length, text);
                                //Set selection start to sel + the text.length
                                SelectionStart = sel + text.Length - autoString.Length;
                                //-----
                                //--Not sure if autoString to empty should go here
                                //-----
                            }
                        }
                       
                    }
                    // Clear the AutoCompleteBox Items 
                    AutoCompleteBox.Items.Clear();

                    //remove control
                    Controls.Remove(AutoCompleteBox);
                    //set bool isAutoCompleteBoxAdded to false
                    isAutoCompleteBoxAdded = false;
                    //Set autoString to empty
                    autoString = "";
                    break;
                #endregion

                #region Tab Key
                // if Space key is down then remove AutoCompleteBox control from RTB
                case Keys.Tab:
                    //Check if the AutoCompleteBox has been added
                    if (isAutoCompleteBoxAdded)
                    {
                        //Get selectionstart
                        sel = SelectionStart;

                        //Check if AutoCompleteBox is not empty
                        if (AutoCompleteBox.Items.Count != 0)
                        {
                            
                            //Remove the autoString from the RTB
                            Text = Text.Remove(sel - autoString.Length, autoString.Length);
                            //Set text to the selected item in the list
                            text = AutoCompleteBox.SelectedItem.ToString();
                            //Set htmlString to the selected item in the list
                            htmlString = AutoCompleteBox.SelectedItem.ToString();

                            //Insert text to the RTB starting at selectionstart
                            Text = Text.Insert(sel - autoString.Length, text);

                            //Some kind of if statement to check the line

                            //Set selection start to sel + the text.length
                            SelectionStart = sel + text.Length - autoString.Length;

                        }
                    }
                    // Clear the AutoCompleteBox Items 
                    AutoCompleteBox.Items.Clear();
                    
                    //Set autoString to empty
                    autoString = "";
                    break;
                #endregion

                #region Enter Key
                // if Enter key is down then remove AutoCompleteBox control from RTB
                //Set autoString to empty
                case Keys.Enter:
                    if (isAutoCompleteBoxAdded)
                    {
                        //Remove control
                        Controls.Remove(AutoCompleteBox);
                        
                        //set bool isAutoCompleteBoxAdded to false
                        isAutoCompleteBoxAdded = false;
                    }
                    autoString = "";//Set autoString to empty
                    break;
                #endregion

                #region Escape Key
                // if Escape key is down then remove AutoCompleteBox control from RTB
                //Set autoString to empty
                case Keys.Escape:
                    if (isAutoCompleteBoxAdded)
                    {
                        //Remove control
                        Controls.Remove(AutoCompleteBox);
                        //set bool isAutoCompleteBoxAdded to false
                        isAutoCompleteBoxAdded = false;
                        
                    }
                    autoString = "";//Set autoString to empty
                    break;
                #endregion

                #region Back Key
                // if Back key is down then remove AutoCompleteBox control from RTB
                case Keys.Back:
                    //Check if isAutoBracket is true
                    if (isAutoBracket)
                    {
                        if(autoString == "")
                        {
                            //Get selection start
                            sel = SelectionStart;
                            //Remove text starting at selectionstart and for 1 character
                            Text = Text.Remove(sel, 1);
                            //Set the selectionstart
                            SelectionStart = sel;
                            //Set isAutoBracket to false.
                            isAutoBracket = false;
                        }
                    }
                    //Check if the string for auto complete autoString is not empty
                    if (autoString != "")
                    {
                        //set autoString to autoString.Substring(0, autoString.Length - 1)
                        //removing the last character
                        autoString = autoString.Substring(0, autoString.Length - 1);
                    }
                    if(htmlString != "")
                    {
                      htmlString = htmlString.Substring(0, htmlString.Length - 1);
                    }
                    
                    break;
                #endregion

                #region Arrow Keys
                    //Used to give focus to the list box
                case Keys.Up:
                    if (isAutoCompleteBoxAdded)
                    {
                        AutoCompleteBox.Focus();
                    }

                    break;

                case Keys.Down:
                    if (isAutoCompleteBoxAdded)
                    {
                        AutoCompleteBox.Focus();
                    }

                    break;
                    
                    //case Keys.Left:
                    //    if (isAutoBracket == false)
                    //    {
                    //        autoString = "";
                    //        isAutoBracket = false;
                    //    }

                    //    Is_SpaceBarKeyPressed = false;

                    //    break;

                    //case Keys.Right:
                    //    if (isAutoBracket == false)
                    //    {
                    //        autoString = "";
                    //        isAutoBracket = false;
                    //    }

                    //    Is_SpaceBarKeyPressed = false;

                    //    break;
                    #endregion
            }
        }
        #endregion
        //***************************

        //**************************************
        //Handles the PreviewKeyDown for the RTB
        #region PreviewKeyDown
        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            base.OnPreviewKeyDown(e);

        }
        #endregion
        //**************************************

        //**************************************
        //Handles the KeyPress Event for the RTB
        #region KeyPress
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            //Create a string variable keyStroke and grab the key that was pressed
            string keyStroke = e.KeyChar.ToString();

            #region Special Character Handling
            //  check pressed key on AutoCompleteBox is special character or not
            //   if it is a special character then remove AutoCompleteBox from the RTB
            switch (keyStroke.ToString())
            {
                case "~":
                case "`":
                case "!":
                case "@":
                case "#":
                case "$":
                case "%":
                case "^":
                case "&":
                case "*":
                case "-":
                case "_":
                case "+":
                case "=":
                case "(":
                case ")":
                case "[":
                case "]":
                case "{":
                case "}":
                case ":":
                case ";":
                case "\"":
                case "'":
                case "|":
                case "\\":
                case "<":
                case ">":
                case ",":
                case ".":
                case "/":
                case "?":
                    if (isAutoCompleteBoxAdded)
                    {
                        Controls.Remove(AutoCompleteBox);
                        //set bool isAutoCompleteBoxAdded to false
                        isAutoCompleteBoxAdded = false;
                        autoString = "";
                    }
                    break;
            }
            #endregion

            #region AutoBrackets
            //Check if AutoBrackets feature is on
            if (Properties.Settings.Default.AutoBrackets == true)
            {
                //Call Auto Brackets and check the keystroke
                AutoBrackets(keyStroke);
            }
            #endregion

            #region AutoHTML
            //Check if AutoHTML feature is on
            if (Properties.Settings.Default.AutoHTML == true)
            {
                //Call Auto HTML
                AutoHTML(keyStroke);
            }
            #endregion

            #region AutoComplete
            //Call AutoHTML if any of the following keys are Not pressed
            // Space=32, Enter=13, Escape=27, Tab=9 Or if Back=8 is pressed
            if (Convert.ToInt32(e.KeyChar) != 32 && 
                Convert.ToInt32(e.KeyChar) != 13 && 
                Convert.ToInt32(e.KeyChar) != 27 && 
                Convert.ToInt32(e.KeyChar) != 9 || 
                Convert.ToInt32(e.KeyChar) == 8)
            {
                //Check to make sure it is a - Z and not any special character
                //[^\w\.@-] matches any character that is not a word character, a period, an @ symbol, or a hyphen
                keyStroke = Regex.Replace(keyStroke, @"[^\w\.@-]", "");

                //Concat the key & autoString postfix
                autoString += keyStroke;

                //Check if autoString is not empty
                if (autoString != "")
                {
                    //Check if AutoComplete feature is on
                    if (Properties.Settings.Default.AutoComplete == true)
                    {
                        //Call AutoComplete to show the list box
                        AutoComplete();
                    }
                }
            }
            #endregion

            //Check to see if isAutoCompleteBoxAdded is true and if the KeyChar is tab
            if ((isAutoCompleteBoxAdded && Convert.ToInt32(e.KeyChar) == 9))
            {
                //Handles the keystroke stopping the tab from being entered into the RTB text
                e.Handled = true;
                //remove control
                Controls.Remove(AutoCompleteBox);
                //set bool isAutoCompleteBoxAdded to false
                isAutoCompleteBoxAdded = false;
            }
        }
        #endregion
        //**************************************

        //******************************
        //Handles MouseClick for the RTB
        #region MouseClick
        //Handles Mouse clicking in the RTB
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            //Check if the AutoCompleteBox is added to the control
            if (isAutoCompleteBoxAdded)
            {
                //Remove control
                Controls.Remove(AutoCompleteBox);
                //set bool isAutoCompleteBoxAdded to false
                isAutoCompleteBoxAdded = false;
                autoString = "";//Set string to empty
            }
        }
        #endregion //Complete
        //******************************

        //**************************************************************************
        //This is to remove the list box if the main rich text box has been scrolled
        #region Scroll
        protected override void OnVScroll(EventArgs e)
        {
            base.OnVScroll(e);

            // remove AutoCompleteBox when VScroll is changed in RTB
            // before remove check AutoCompleteBox is added to RTB or not
            if (isAutoCompleteBoxAdded)
            {
                //Remove control
                Controls.Remove(AutoCompleteBox);
                //set bool isAutoCompleteBoxAdded to false
                isAutoCompleteBoxAdded = false;
                //Set autoString to empty
                autoString = "";
            }
        }
        #endregion //Complete
        //**************************************************************************
        #endregion
        //******************************

        //To Do: Test!!!
        //May not need these not sure
        #region AutoComplete List events
        //*********************************************************************
        //  AutoCompleteBox KeyDown events function
        //*********************************************************************
        private void AutoCompleteBox_KeyDown(object sender, KeyEventArgs e)
        {
            int sel; //Variable to hold the selection start
            string text;//Variable to hold the string

            switch (e.KeyCode)
            {
                case Keys.Enter:
                    //Check if the AutoCompleteBox has been added
                    if (isAutoCompleteBoxAdded)
                    {
                        //Get selectionstart
                        sel = SelectionStart;

                        //Check if AutoCompleteBox is not empty
                        if (AutoCompleteBox.Items.Count != 0)
                        {
                            //Remove the autoString from the RTB
                            Text = Text.Remove(sel - autoString.Length, autoString.Length);
                            //Set text to the selected item in the list
                            text = AutoCompleteBox.SelectedItem.ToString();
                            //Set htmlString to the selected item in the list
                            htmlString = AutoCompleteBox.SelectedItem.ToString();

                            //Insert text to the RTB starting at selectionstart
                            Text = Text.Insert(sel - autoString.Length, text);
                            //Set selection start to sel + the text.length
                            SelectionStart = sel + text.Length - 1;

                        }
                    }
                    //remove control
                    Controls.Remove(AutoCompleteBox);
                    //set bool isAutoCompleteBoxAdded to false
                    isAutoCompleteBoxAdded = false;
                    //Set autoString to empty
                    autoString = "";
                    break;

                // if Left key is down then remove AutoCompleteBox from this
                case Keys.Left:
                    
                    break;

                // if Right key is down then remove AutoCompleteBox from this
                case Keys.Right:
                    
                    break;
            }
        }

        ////*********************************************************************
        ////  AutoCompleteBox KeyPress events function
        ////*********************************************************************
        private void AutoCompleteBox_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        ////*********************************************************************
        ////  AutoCompleteBox MouseClick events function
        ////*********************************************************************
        private void AutoCompleteBox_MouseClick(object sender, MouseEventArgs e)
        {
            int sel; //Variable to hold the selection start
            string text;//Variable to hold the string

            //Check if the AutoCompleteBox has been added
            if (isAutoCompleteBoxAdded)
            {
                //Get selectionstart
                sel = SelectionStart;

                //Check if AutoCompleteBox is not empty
                if (AutoCompleteBox.Items.Count != 0)
                {
                    //Remove the autoString from the RTB
                    Text = Text.Remove(sel - autoString.Length, autoString.Length);
                    //Set text to the selected item in the list
                    text = AutoCompleteBox.SelectedItem.ToString();
                    //Set htmlString to the selected item in the list
                    htmlString = AutoCompleteBox.SelectedItem.ToString();

                    //Insert text to the RTB starting at selectionstart
                    Text = Text.Insert(sel - autoString.Length, text);
                    //Set selection start to sel + the text.length
                    SelectionStart = sel + text.Length - 1;
                    //-----
                    //--Not sure if autoString to empty should go here
                    //-----
                }
            }
            //remove control
            Controls.Remove(AutoCompleteBox);
            //set bool isAutoCompleteBoxAdded to false
            isAutoCompleteBoxAdded = false;
            //Set autoString to empty
            autoString = "";
        }
        #endregion

    }
}

