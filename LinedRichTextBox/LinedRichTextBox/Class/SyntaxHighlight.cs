using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LinedRichTextBox
{
    class SyntaxHighlight
    {
        public static void HTMLHighlight(SyntaxControl control, string checkString, int insertion, int sel)
        {
            //int StartCursorPosition = control.SelectionStart;

            foreach (string item in HTMLTagList.tagsList)
            {
                if(item == checkString)
                {
                    int startIndex = insertion;
                    int StopIndex = checkString.Length;
                    control.Select(startIndex, StopIndex);
                    control.SelectionColor = Color.Red;
                    control.SelectionStart = sel + checkString.Length - insertion;
                }
                else
                {
                    control.SelectionColor = Color.White;
                }
            }
            
        }

    }
}
